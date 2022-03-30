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
   public class newSquadScreen:BasicScreen
   {
      [NonSerialized]
      Texture2D underline;

      List<NewCharacterUI> squadMembers;
      CharacterModel currentCharacter;

      Button classIcon;
      ProgressBar bar;
      Label lblTitle;
      Label lblTatics;
      Label lblName;
      Label lblLevel;
      Label lblMaxHealth;
      Label lblStrength;
      Label lblToughness;
      Label lblDexterity;

      Button backButton;

      public newSquadScreen() : base()
      {
         ScreenType = VastAdventure.Utility.ScreenType.Squad;
      }

      public override void Update(int dt)
      {
         lblTitle.Update(dt);
         lblName.Update(dt);
         lblLevel.Update(dt);
         lblMaxHealth.Update(dt);
         lblStrength.Update(dt);
         lblToughness.Update(dt);
         lblDexterity.Update(dt);
         lblTatics.Update(dt);
         foreach (var character in squadMembers)
         {
            character.Update(dt);
         }
         bar.Update(dt);
         backButton.Update(dt);
         classIcon.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(underline, new Rectangle(0, 0, (int)viewport.Width, (int)viewport.Height / 10), null, Color.White);
         lblTitle.Render(sb);
         lblName.Render(sb);
         lblLevel.Render(sb);
         lblMaxHealth.Render(sb);
         lblStrength.Render(sb);
         lblToughness.Render(sb);
         lblDexterity.Render(sb);
         lblTatics.Render(sb);
         foreach (var character in squadMembers)
         {
            character.Render(sb);
         }
         bar.Render(sb);
         backButton.Render(sb);
         classIcon.Render(sb);
      }

      public override void Init()
      {
         base.Init();
         squadMembers = new List<NewCharacterUI>();
         //initialize stuff

         //Creating UI
         underline = ContentService.Get2DTexture("white_bottom_bar");

         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();

         SquadModel sm = SquadService.getAllySquad();
         for (int i = 0; i < sm.characterIds.Count; i++)
         {
            CharacterModel characterModel = CharacterService.getCharacter(sm.characterIds[i]);
            NewCharacterUI ui = new NewCharacterUI(characterModel, new EngineRectangle(viewport.X, viewport.Y + viewport.Height / 10 * (i + 6), (viewport.Width), viewport.Height / 10), Color.DarkGray, "button_gray_outline_none");
            ui.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleCharacterUIClick));
            if (sm.characterIds[i] == 0)
            {
               ui.selected = true;
            }
            squadMembers.Add(ui);
         }

         currentCharacter = CharacterService.getPlayer();



         // Overview section
         lblTitle = new Label("lblTitle", "Squad Info", new EngineRectangle(0, 0, viewport.Width, viewport.Height / 10), Color.DarkGray);
         lblTitle.textAnchor = Enums.TextAchorLocation.centerLeft;
         lblTitle.padding = new int[] { 25, 100, 25, 50 };
         lblTitle.maxFontScale = 2;

         classIcon = new Button("btnClassIcon", "", new EngineRectangle(viewport.X + 50, viewport.Y + viewport.Height / 30 * 3 + 50, viewport.Height / 5 - 100, viewport.Height / 5 - 100), Color.DarkGray);
         classIcon.setImageReferences(currentCharacter.characterClass.ToString() + "_icon", currentCharacter.characterClass.ToString() + "_icon", currentCharacter.characterClass.ToString() + "_icon", currentCharacter.characterClass.ToString() + "_icon");
         lblName = new Label("lblName", "Name: " + currentCharacter.name, new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 30 * 3 + 50, (viewport.Width / 2), viewport.Height / 30), Color.DarkGray);
         lblName.minFontScale = 0;
         lblLevel = new Label("lblLevel", "Level: " + currentCharacter.level.ToString(), new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 30 * 5 + 100, (viewport.Width / 2), (viewport.Height / 30)), Color.DarkGray);
         lblLevel.minFontScale = 0;
         bar = new ProgressBar("bar", currentCharacter.experience + "/" + (currentCharacter.level * 100).ToString(), new EngineRectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 30 * 6 + 100, viewport.Width / 2 - 50, 50), Color.White, "dark_gray");
         bar.setImageReferences("button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none");
         
         if (currentCharacter.level == 0)
         {
            bar.barPercent = 1;
         }
         else
         {
            bar.barPercent = (float)currentCharacter.experience / (float)(currentCharacter.level * 100);
         }


         //Stat section
         lblMaxHealth = new Label("lblMaxHealth", "Max Health: " + currentCharacter.maxHealth.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 30 * 9, (viewport.Width - 100) /  2, (viewport.Height / 30 * 2)), Color.DarkGray);
         lblMaxHealth.minFontScale = 0;

         lblStrength = new Label("lblStrength", "Strength: " + currentCharacter.strength.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 30 * 11, (viewport.Width - 100) / 2, (viewport.Height / 30 * 2)), Color.DarkGray);
         lblStrength.minFontScale = 0;

         lblToughness = new Label("lblToughness", "Toughness: " + currentCharacter.toughness.ToString(), new EngineRectangle(viewport.X + viewport.Width / 2 + 50, viewport.Y + 50 + viewport.Height / 30 * 9, (viewport.Width - 100) / 2, (viewport.Height / 30 * 2)), Color.DarkGray);
         lblToughness.minFontScale = 0;

         lblDexterity = new Label("lblDexterity", "Dexterity: " + currentCharacter.dexterity.ToString(), new EngineRectangle(viewport.X + viewport.Width / 2 + 50, viewport.Y + 50 + viewport.Height / 30 * 11, (viewport.Width - 100) /  2, (viewport.Height / 30 * 2)), Color.DarkGray);
         lblDexterity.minFontScale = 0;

         lblTatics = new Label("lblDexterity", "Selected Squad Tatic: " + SquadService.getAllySquad().tatic.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 30 * 13, viewport.Width - 100, (viewport.Height / 20)), Color.DarkGray);
         lblTatics.textAnchor = Enums.TextAchorLocation.center;

         backButton = new Button("btnBack", "", new EngineRectangle(viewport.X + viewport.Width / 10 * 9 - 50, viewport.Y + viewport.Height / 40, viewport.Height / 20 * 1, viewport.Height / 20 * 1), Microsoft.Xna.Framework.Color.DarkGray);
         backButton.minFontScale = 0;
         backButton.maxFontScale = 2;
         backButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleBackButtonClick));
         backButton.setImageReferences("gray_exit_none", "gray_exit_hover", "gray_exit_hover", "gray_exit_hover");
      }
      public void HandleCharacterUIClick(object sender, OnClick clickEvent)
      {
         foreach (CharacterUI characterUIs in squadMembers)
         {
            characterUIs.selected = false;
         }
         CharacterUI ui = sender as CharacterUI;
         ui.selected = true;
         CharacterModel cm = CharacterService.getCharacter(ui.characterId);
         currentCharacter = cm;
         lblName.value = "Name: " + currentCharacter.name;
         lblLevel.value = "Level: " + currentCharacter.level.ToString();
         lblMaxHealth.value = "Max Health: " + currentCharacter.maxHealth.ToString();
         lblStrength.value = "Strength: " + currentCharacter.strength.ToString();
         lblToughness.value = "Toughness: " + currentCharacter.toughness.ToString();
         lblDexterity.value = "Dexterity: " + currentCharacter.dexterity.ToString();
         classIcon.setImageReferences(currentCharacter.characterClass.ToString() + "_icon", currentCharacter.characterClass.ToString() + "_icon", currentCharacter.characterClass.ToString() + "_icon", currentCharacter.characterClass.ToString() + "_icon");
         classIcon.init();
         if (currentCharacter.level == 0)
         {
            bar.barPercent = 1;
         }
         else
         {
            bar.barPercent = (float)currentCharacter.experience / (float)(currentCharacter.level * 100);
         }
         bar.value = currentCharacter.experience + "/" + (currentCharacter.level * 100).ToString();
      }
      public void HandleBackButtonClick(object sender, OnClick clickEvent)
      {
         parentScene.bus.Publish(this, new SetCharacterInfoButtonVisabilityEvent(true));
         ScreenService.replaceWithPreviousScreen(parentScene);
      }

      public override void onDeserialized()
      {
         if (ContentService.isInitialized)
         {
            underline = ContentService.Get2DTexture("white_bottom_bar");
            lblTitle.onDeserialized();
            lblName.onDeserialized();
            lblLevel.onDeserialized();
            lblMaxHealth.onDeserialized();
            lblStrength.onDeserialized();
            lblToughness.onDeserialized();
            lblDexterity.onDeserialized();
            bar.onDeserialized();

            backButton.onDeserialized();
         }
      }
   }
}