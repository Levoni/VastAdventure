using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.UI.Mobile;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventureAndriod.VastAdventureUI;
using Base.Utility;
using VastAdventure.Events;
using VastAdventure.Utility.Services;
using Base.Utility.Services;
using Base.Events;

namespace VastAdventureAndriod.Screens
{
   [Serializable]
   public class NewCharacterInfoScreen:BasicScreen
   {
      //background
      [NonSerialized]
      Texture2D underline;

      //idividual character section
      Button classIcon;
      ProgressBar bar;
      Label Title;
      Label name;
      Label level;
      Label maxHealth;
      Label strength;
      Label toughness;
      Label dexterity;
      Label attributePoints;

      //Increase buttons
      //Button increaseMaxHealth;
      Button increaseStrength;
      Button increaseToughness;
      Button increaseDexterity;

      //misc
      Button backButton;
      CharacterModel model;

      public NewCharacterInfoScreen() : base()
      {
         ScreenType = VastAdventure.Utility.ScreenType.CharacterInformation;
      }

      public override void Update(int dt)
      {
         Title.Update(dt);
         name.Update(dt);
         level.Update(dt);
         maxHealth.Update(dt);
         strength.Update(dt);
         toughness.Update(dt);
         dexterity.Update(dt);
         classIcon.Update(dt);
         bar.Update(dt);
         //characterClass.Update();
         attributePoints.Update(dt);
         backButton.Update(dt);
         if (model != null && model.attiributePoints > 0)
         {
            //increaseMaxHealth.Update();
            increaseStrength.Update(dt);
            increaseToughness.Update(dt);
            increaseDexterity.Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(underline, new Rectangle(0,0,(int)viewport.Width, (int)viewport.Height/10), null, Color.White);
         Title.Render(sb);
         name.Render(sb);
         level.Render(sb);
         maxHealth.Render(sb);
         strength.Render(sb);
         toughness.Render(sb);
         dexterity.Render(sb);
         classIcon.Render(sb);
         bar.Render(sb);
         //characterClass.Render(sb);
         attributePoints.Render(sb);
         backButton.Render(sb);
         if (model != null && model.attiributePoints > 0)
         {
            //increaseMaxHealth.Render(sb);
            increaseStrength.Render(sb);
            increaseToughness.Render(sb);
            increaseDexterity.Render(sb);
         }
      }

      public override void Init()
      {
         base.Init();
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         //initialize stuff

         //Creating UI
         underline = ContentService.Get2DTexture("white_bottom_bar");

         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();

         model = CharacterService.getPlayer();

         // Overview section
         Title = new Label("lblTitle", "Character Info", new EngineRectangle(0,0,viewport.Width,viewport.Height / 10), Color.DarkGray);
         Title.textAnchor = Enums.TextAchorLocation.centerLeft;
         Title.padding = new int[] { 25, 100, 25, 50 };
         Title.maxFontScale = 2;
         classIcon = new Button("btnClassIcon", "", new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10, viewport.Height / 5 - 100, viewport.Height / 5 - 100), Color.DarkGray);
         classIcon.setImageReferences(model.characterClass.ToString() + "_icon", model.characterClass.ToString() + "_icon", model.characterClass.ToString() + "_icon", model.characterClass.ToString() + "_icon");
         name = new Label("lblName", "Name: " + model.name, new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 30 * 3 + 50, (viewport.Width / 2) , viewport.Height / 30), Color.DarkGray);
         name.minFontScale = 0;
         level = new Label("lblLevel", "Level: " + model.level.ToString(), new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 30 * 5 + 100, (viewport.Width / 2), (viewport.Height / 30)), Color.DarkGray);
         level.minFontScale = 0;
         bar = new ProgressBar("bar", model.experience + "/" + (model.level * 100).ToString(), new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 30 * 6  + 100, viewport.Width / 2 - 50, 50), Color.White, "dark_gray");
         bar.setImageReferences("button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none");
         if (model.level == 0)
         {
            bar.barPercent = 1;
         }
         else
         {
            bar.barPercent = (float)model.experience / (float)(model.level * 100);
         }


         //Stat section
         maxHealth = new Label("lblMaxHealth", "Max Health: " + model.maxHealth.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 6, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.DarkGray);
         maxHealth.minFontScale = 0;

         strength = new Label("lblStrength", "Strength: " + model.strength.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 7, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.DarkGray);
         strength.minFontScale = 0;

         toughness = new Label("lblToughness", "Toughness: " + model.toughness.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 8, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.DarkGray);
         toughness.minFontScale = 0;

         dexterity = new Label("lblDexterity", "Dexterity: " + model.dexterity.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 9, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.DarkGray);
         dexterity.minFontScale = 0;

         attributePoints = new Label("lblAttributePoints", "Available Attribute Points: " + model.attiributePoints.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 10, (viewport.Width - 100), (viewport.Height / 20)), Color.DarkGray);
         attributePoints.minFontScale = 0;
         attributePoints.textAnchor = Enums.TextAchorLocation.center;

         increaseStrength = new Button("btnIncreaseStrength", "", new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + 55 + viewport.Height / 20 * 7, viewport.Height / 20, (viewport.Height / 20) - 10), Color.DarkGray);
         increaseStrength.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         increaseStrength.setImageReferences("gray_plus_none", "gray_plus_hover", "gray_plus_hover", "gray_plus_hover");
         increaseToughness = new Button("btnIncreaseToughness", "", new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + 55 + viewport.Height / 20 * 8, viewport.Height / 20, (viewport.Height / 20) - 10), Color.DarkGray);
         increaseToughness.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         increaseToughness.setImageReferences("gray_plus_none", "gray_plus_hover", "gray_plus_hover", "gray_plus_hover");
         increaseDexterity = new Button("btnIncreaseDexterity", "", new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + 55 + viewport.Height / 20 * 9, viewport.Height / 20, (viewport.Height / 20) - 10), Color.DarkGray);
         increaseDexterity.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         increaseDexterity.setImageReferences("gray_plus_none", "gray_plus_hover", "gray_plus_hover", "gray_plus_hover");

         backButton = new Button("btnBack", "", new EngineRectangle(viewport.X + viewport.Width / 10 * 9 - 50, viewport.Y + 50, viewport.Width / 10, viewport.Width / 10), Microsoft.Xna.Framework.Color.DarkGray);
         backButton.minFontScale = 0;
         backButton.maxFontScale = 2;
         backButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleBackButtonClick));
         backButton.setImageReferences("gray_exit_none", "gray_exit_hover", "gray_exit_hover", "gray_exit_hover");

         SquadModel sm = SquadService.getAllySquad();
      }

      public void HandleCharacterUIClick(object sender, OnClick clickEvent)
      {
         //foreach (CharacterUI characterUIs in characters)
         //{
         //   characterUIs.selected = false;
         //}
         //CharacterUI ui = sender as CharacterUI;
         //ui.selected = true;
         //CharacterModel cm = CharacterService.getCharacter(ui.characterId);
         //model = cm;
         //name.value = "Name: " + model.name;
         //level.value = "Level: " + model.level.ToString();
         //experience.value = "XP: " + model.experience.ToString();
         //maxHealth.value = "Max Health: " + model.maxHealth.ToString();
         //strength.value = "Strength: " + model.strength.ToString();
         //toughness.value = "Toughness: " + model.toughness.ToString();
         //dexterity.value = "Dexterity: " + model.dexterity.ToString();
         //attributePoints.value = "Available Attribute Points: " + model.attiributePoints.ToString();
      }

      public void HandleIncreaseButtonClick(object sender, OnClick clickEvent)
      {
         var clickedButton = sender as Button;
         if (clickedButton != null)
         {
            if (clickedButton.Name == "btnIncreaseMaxHealth")
            {
               model.maxHealth += 2;
               model.attiributePoints--;
               maxHealth.value = "Max Health: " + model.maxHealth.ToString();
               attributePoints.value = "Available Attribute Points: " + model.attiributePoints.ToString();
            }
            if (clickedButton.Name == "btnIncreaseStrength")
            {
               model.strength += 1;
               model.attiributePoints--;
               strength.value = "Strength: " + model.strength.ToString();
               attributePoints.value = "Available Attribute Points: " + model.attiributePoints.ToString();
            }
            if (clickedButton.Name == "btnIncreaseToughness")
            {
               model.toughness += 1;
               model.attiributePoints--;
               toughness.value = "Toughness: " + model.toughness.ToString();
               attributePoints.value = "Available Attribute Points: " + model.attiributePoints.ToString();
            }
            if (clickedButton.Name == "btnIncreaseDexterity")
            {
               model.dexterity += 1;
               model.attiributePoints--;
               dexterity.value = "Dexterity: " + model.dexterity.ToString();
               attributePoints.value = "Available Attribute Points: " + model.attiributePoints.ToString();
            }
         }
      }

      public void HandleRemoveCharacterClick(object sender, OnClick clickEvent)
      {
         //if (model != null && model.id != 0)
         //{
         //   SquadModel sm = SquadService.getAllySquad();
         //   sm.characterIds.Remove(model.id);

         //   var viewport = ScreenGraphicService.GetViewportBounds();

         //   characters = new List<CharacterUI>();
         //   for (int i = 0; i < sm.characterIds.Count; i++)
         //   {
         //      CharacterModel characterModel = CharacterService.getCharacter(sm.characterIds[i]);
         //      CharacterUI ui = new CharacterUI(characterModel, new EngineRectangle(viewport.X + 50 + viewport.Width / 2, viewport.Y + 50 + viewport.Height / 10 * (i + 1), (viewport.Width / 2 - 100), viewport.Height / 10));
         //      ui.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleCharacterUIClick));
         //      if (sm.characterIds[i] == 0)
         //      {
         //         ui.selected = true;
         //      }
         //      characters.Add(ui);
         //   }

         //   HandleCharacterUIClick(characters[0], new OnClick());
         //}
      }

      public void HandleBackButtonClick(object sender, OnClick clickEvent)
      {
         ScreenService.replaceWithPreviousScreen(parentScene);
      }

      public void HandleMapButtonClick(object sender, OnClick clickEvent)
      {
         string commands = "map:-1";
         ScreenService.replaceScreen(parentScene, commands);
      }

      #region deserialization
      public override void onDeserialized()
      {
         if (ContentService.isInitialized)
         {
            underline = ContentService.Get2DTexture("white_bottom_bar");
            Title.onDeserialized();
            classIcon.onDeserialized();
            name.onDeserialized();
            level.onDeserialized();
            maxHealth.onDeserialized();
            strength.onDeserialized();
            toughness.onDeserialized();
            dexterity.onDeserialized();
            attributePoints.onDeserialized();
            bar.onDeserialized();

            //increaseMaxHealth.onDeserialized();
            increaseStrength.onDeserialized();
            increaseToughness.onDeserialized();
            increaseDexterity.onDeserialized();

            backButton.onDeserialized();
         }
      }
      #endregion
   }
}