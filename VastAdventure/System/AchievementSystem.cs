using Base.Scenes;
using Base.System;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Base.Entities;
using VastAdventure.Events;
using VastAdventure.Model;
using VastAdventure.Utility;
using VastAdventure.Utility.Services;
using VastAdventure.Component;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Base.Utility;
using VastAdventure.UiInterfaces;
using Base.Events;

namespace VastAdventure.System
{
   //TODO: when acheivment is gained show popup for a few seconds
   //TODO: Save/Load achievments 
   [Serializable]
   public class AchievementSystem: EngineSystem
   {
      EHandler<InventoryEvent> onInventory;
      EHandler<AddCharacterEvent> onAddCharacter;
      EHandler<ChangeScreenEvent> onChangeScreen;
      EHandler<CharacterDiedEvent> onCharacterDied;
      EHandler<FlagSetEvent> onFlagSet;

      public AchievementUIInterface UiInterface { get; set; }
      bool hasAddedCharacter;
      //Achievments
      // - (done-tested) cartogropher (obtain a map) -  (item-based:adding map to inventory)
      // - (done-tested) cult stopper (stop a dark ritural) - (flag-based: saved tessa)
      // - (done-tested) social (Talk to all the inmates) - (multi-flag-based: have all talked to flags set to true) - need to keep track of these flags in this system
      // - (done) One man Army (beat the game never having a person join your squad) - (screen-change-id and system tracked based: get to final screen and never have a character add to the squad based on info stored in system)
      // - (done) Master Nagotiatitor (Get past the last checkpoint without fighting) = (screen-change-id: get to screen that explies how you coinved to guards to let you pass)
      // - (done) Gentalman Inmate (beat the game never having killed a guard) - (scree-change-id and system tracked based: get to final screen not having any guard id stored in list of killed characters)
      // - (done) BloodThirsty (beat the game killing every guard possible) - (screen-change-id and system tracked based: get to final screen and have every guard id stored in list of killed characters)
      // - (done-tested) Dark Power (Gain power from a dark ritual) - (screen-change-id based: get to screen (106) where you recieve xp for completing the ritual
      public AchievementSystem(Scene s)
      {
         systemSignature = (uint)(1 << Achievement.GetFamily());
         RegisterScene(s);
         registeredEntities = new List<Base.Entities.Entity>();
         hasAddedCharacter = false;
         UiInterface = null;
         Init(s);
      }

      public AchievementSystem()
      {
         systemSignature = (uint)(1 << Achievement.GetFamily());
         registeredEntities = new List<Base.Entities.Entity>();
         hasAddedCharacter = false;
         UiInterface = null;
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         for(int i = registeredEntities.Count - 1; i >= 0; i--)
         {
            var achievement = parentScene.GetComponent<Achievement>(registeredEntities[i]);
            achievement.timeLeft -= dt;
            if(achievement.timeLeft <= 0)
            {
               parentScene.DestroyEntity(registeredEntities[i]);
            }
         }
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         for(int i = 0; i < registeredEntities.Count; i++)
         {
            var achievement = parentScene.GetComponent<Achievement>(registeredEntities[i]);
            var viewport = ScreenGraphicService.GetViewportBounds();
            if (UiInterface != null)
            {
               UiInterface.SetNewAchievement(achievement.achievement);
               UiInterface.SetNewBounds(new EngineRectangle(10, viewport.Height / 10 * (9 - i), (viewport.Width - 20), viewport.Height / 10));
               //UiInterface.SetNewBounds(new EngineRectangle((int)viewport.X + (int)viewport.Width / 2, (int)viewport.Y + (int)viewport.Height / 10 * (8 - 2 * i), (int)viewport.Width / 2, (int)viewport.Height / 10 * 2));
               sb.Begin();
               UiInterface.Render(sb);
               sb.End();
            }
         }
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         onInventory = new EHandler<InventoryEvent>(new Action<object, InventoryEvent>(HandleInventoryEvent));
         onAddCharacter = new EHandler<AddCharacterEvent>(new Action<object, AddCharacterEvent>(HandleAddCharacterEvent));
         onChangeScreen = new EHandler<ChangeScreenEvent>(new Action<object, ChangeScreenEvent>(HandleChangeScreenEvent));
         onCharacterDied = new EHandler<CharacterDiedEvent>(new Action<object, CharacterDiedEvent>(HandleCharacterDiedEvent));
         onFlagSet = new EHandler<FlagSetEvent>(new Action<object, FlagSetEvent>(HandleFlagSetEvent));

         parentScene.bus.Subscribe(onInventory);
         parentScene.bus.Subscribe(onAddCharacter);
         parentScene.bus.Subscribe(onChangeScreen);
         parentScene.bus.Subscribe(onCharacterDied);
         parentScene.bus.Subscribe(onFlagSet);
      }

      public override void Terminate()
      {
         base.Terminate();
         parentScene.bus.Unsubscribe(onInventory);
         parentScene.bus.Unsubscribe(onAddCharacter);
         parentScene.bus.Unsubscribe(onChangeScreen);
         parentScene.bus.Unsubscribe(onCharacterDied);
         parentScene.bus.Unsubscribe(onFlagSet);
      }

      public void HandleCharacterDiedEvent(object sender, CharacterDiedEvent e)
      {

      }

      public void HandleFlagSetEvent(object sender, FlagSetEvent e)
      {
         if(!AchievementService.achievements["Cult Stopper"].hasAchievement && e.flag == "savedTessa" && e.value)
         {
            AchievementModel tempModel = AchievementService.achievements["Cult Stopper"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }
         if(!AchievementService.achievements["Social"].hasAchievement &&
            FlagService.CheckFlagValue("talkedToFredrick") &&
            FlagService.CheckFlagValue("talkedToRoland") &&
            FlagService.CheckFlagValue("talkedToNobleInmate") &&
            FlagService.CheckFlagValue("talkedToThiefMaster") &&
            FlagService.CheckFlagValue("talkedToBrawlerMaster") &&
            FlagService.CheckFlagValue("talkedTodocumentsMan"))
         {
            AchievementModel tempModel = AchievementService.achievements["Social"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }
      }

      public void HandleInventoryEvent(object sender, InventoryEvent e)
      {
         if(!AchievementService.achievements["Cartographer"].hasAchievement && e.actionType == ActionType.AddItem && e.actionValue == "map")
         {
            AchievementModel tempModel = AchievementService.achievements["Cartographer"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }
      }

      public void HandleAddCharacterEvent(object sender, AddCharacterEvent e)
      {
         hasAddedCharacter = true;
      }
      //TODO: handle getting multple achievments at once
      public void HandleChangeScreenEvent(object sender, ChangeScreenEvent e)
      {
         if(!AchievementService.achievements["Dark Power"].hasAchievement && e.screenType == "event" && e.id == 106)
         {
            AchievementModel tempModel = AchievementService.achievements["Dark Power"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }
         if (!AchievementService.achievements["Master Negotiator"].hasAchievement && e.screenType == "event" && e.id == 825)
         {
            AchievementModel tempModel = AchievementService.achievements["Master Negotiator"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }
         if(e.screenType == "event" && e.id == 830)
         {
            if(!AchievementService.achievements["One Man Army"].hasAchievement && !hasAddedCharacter)
            {
               AchievementModel tempModel = AchievementService.achievements["One Man Army"];
               tempModel.hasAchievement = true;
               ShowAchievement(tempModel);
            }
            if (!AchievementService.achievements["Bloodthirsty"].hasAchievement &&
               CharacterService.getCharacter(20).hasDied &&
               CharacterService.getCharacter(21).hasDied &&
               CharacterService.getCharacter(22).hasDied &&
               CharacterService.getCharacter(25).hasDied &&
               CharacterService.getCharacter(26).hasDied &&
               CharacterService.getCharacter(27).hasDied &&
               CharacterService.getCharacter(30).hasDied &&
               CharacterService.getCharacter(35).hasDied &&
               CharacterService.getCharacter(36).hasDied &&
               CharacterService.getCharacter(37).hasDied)
            {
               AchievementModel tempModel = AchievementService.achievements["Bloodthirsty"];
               tempModel.hasAchievement = true;
               ShowAchievement(tempModel);
            }
            if (!AchievementService.achievements["Gentleman Inmate"].hasAchievement &&
               !CharacterService.getCharacter(20).hasDied &&
               !CharacterService.getCharacter(21).hasDied &&
               !CharacterService.getCharacter(22).hasDied &&
               !CharacterService.getCharacter(25).hasDied &&
               !CharacterService.getCharacter(26).hasDied &&
               !CharacterService.getCharacter(27).hasDied &&
               !CharacterService.getCharacter(30).hasDied &&
               !CharacterService.getCharacter(35).hasDied &&
               !CharacterService.getCharacter(36).hasDied &&
               !CharacterService.getCharacter(37).hasDied)
            {
               AchievementModel tempModel = AchievementService.achievements["Gentleman Inmate"];
               tempModel.hasAchievement = true;
               ShowAchievement(tempModel);
            }
         }
         if (!AchievementService.achievements["Book Smart"].hasAchievement &&
             FlagService.CheckFlagValue("readTheForgottenDungeon") &&
             FlagService.CheckFlagValue("readInfluentialFamilies") &&
             FlagService.CheckFlagValue("readPrisonerProcedure") &&
             FlagService.CheckFlagValue("readDarkSuspicions"))
         {
            AchievementModel tempModel = AchievementService.achievements["Book Smart"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }
         if(e.screenType == "event" && e.id == 723)
         {
            AchievementModel tempModel = AchievementService.achievements["Knowledge Opens Doors"];
            tempModel.hasAchievement = true;
            ShowAchievement(tempModel);
         }

      }

      private void ShowAchievement(AchievementModel achievement)
      {
         Entity newEntity = parentScene.CreateEntity();
         Achievement achievementUIComponent = new Achievement(achievement, 5000);
         parentScene.AddComponent(newEntity, achievementUIComponent);
         achievement.hasAchievement = true;
         AchievementService.SaveAchievements();
      }

      public override void onDeserialized()
      {
         base.onDeserialized();
         UiInterface.onDeserialized();
      }
   }
}
