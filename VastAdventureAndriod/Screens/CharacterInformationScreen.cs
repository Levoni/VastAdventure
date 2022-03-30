using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Events;
using Base.UI.Mobile;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.Events;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility;
using VastAdventure.Utility.Services;
using VastAdventureAndriod.VastAdventureUI;

namespace VastAdventureAndriod.Screens
{
   [Serializable]
   //TODO: make a back button that returns to the previous screen
   public class CharacterInformationScreen: BasicScreen
   {
      //background
      [NonSerialized]
      Texture2D scroll;
      [NonSerialized]
      Texture2D table;

      //idividual character section
      Label name;
      Label level;
      Label experience;
      Label maxHealth;
      Label strength;
      Label toughness;
      Label dexterity;
      Label attributePoints;
      Label squadTatics;
      Button removeCharacter;

      //Increase buttons
      //Button increaseMaxHealth;
      Button increaseStrength;
      Button increaseToughness;
      Button increaseDexterity;

      List<CharacterUI> characters;
      Inventory inventory;

      //misc
      Button backButton;
      CharacterModel model;

      //inventory section
      Label inventoryHeader;
      List<Control> inventoryControls;

      public CharacterInformationScreen():base()
      {
         ScreenType = VastAdventure.Utility.ScreenType.CharacterInformation;
      }

      public override void Update(int dt)
      {
         name.Update(dt);
         level.Update(dt);
         experience.Update(dt);
         maxHealth.Update(dt);
         strength.Update(dt);
         toughness.Update(dt);
         dexterity.Update(dt);
         //characterClass.Update();
         attributePoints.Update(dt);
         squadTatics.Update(dt);
         backButton.Update(dt);
         if(model != null && model.id != 0)
         {
            removeCharacter.Update(dt);
         }
         if (model != null && model.attiributePoints > 0)
         {
            //increaseMaxHealth.Update();
            increaseStrength.Update(dt);
            increaseToughness.Update(dt);
            increaseDexterity.Update(dt);
         }
         for(int i = characters.Count - 1; i >= 0; i--)
         {
            characters[i].Update(dt);
         }
         foreach(Control c in inventoryControls)
         {
            c.Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(table, viewport.toRectangle(), Color.White);
         sb.Draw(scroll, viewport.toRectangle(), Color.White);
         name.Render(sb);
         level.Render(sb);
         experience.Render(sb);
         maxHealth.Render(sb);
         strength.Render(sb);
         toughness.Render(sb);
         dexterity.Render(sb);
         //characterClass.Render(sb);
         attributePoints.Render(sb);
         squadTatics.Render(sb);
         backButton.Render(sb);
         if (model != null && model.id != 0)
         {
            removeCharacter.Render(sb);
         }
         if (model != null && model.attiributePoints > 0)
         {
            //increaseMaxHealth.Render(sb);
            increaseStrength.Render(sb);
            increaseToughness.Render(sb);
            increaseDexterity.Render(sb);
         }
         for (int i = characters.Count - 1; i >= 0; i--)
         {
            characters[i].Render(sb);
         }
         foreach (Control c in inventoryControls)
         {
            c.Render(sb);
         }
      }

      public override void Init()
      {
         base.Init();
         characters = new List<CharacterUI>();
         inventoryControls = new List<Control>();
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         //initialize stuff

         //Creating UI
         table = ContentService.Get2DTexture("table_image");
         scroll = ContentService.Get2DTexture("Scroll");

         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();

         model = CharacterService.getPlayer();

         name = new Label("lblName","Name: " + model.name, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 2, (viewport.Width - 50) / 8 * 3, viewport.Height / 20),Color.Black);
         name.minFontScale = 0;

         level = new Label("lblLevel","Level: " + model.level.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 3, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         level.minFontScale = 0;

         experience = new Label("lblExperience", "XP: " + model.experience.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 4, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         experience.minFontScale = 0;

         maxHealth = new Label("lblMaxHealth", "Max Health: " + model.maxHealth.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 5, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         maxHealth.minFontScale = 0;

         strength = new Label("lblStrength", "Strength: " + model.strength.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 6, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         strength.minFontScale = 0;

         toughness = new Label("lblToughness","Toughness: " + model.toughness.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 7, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         toughness.minFontScale = 0;

         dexterity = new Label("lblDexterity", "Dexterity: " + model.dexterity.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 8, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         dexterity.minFontScale = 0;

         attributePoints = new Label("lblAttributePoints", "Available Attribute Points: " + model.attiributePoints.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 9, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black);
         attributePoints.minFontScale = 0;

         squadTatics = new Label("lblTatic", "Squad Tatic: " + SquadService.getAllySquad().tatic.ToString(), new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 10, (viewport.Width - 50) / 8 * 3, (viewport.Height / 20)), Color.Black); ;
         squadTatics.minFontScale = 0;

         //increaseMaxHealth = new Button("btnIncreaseMaxHealth", "", new EngineRectangle(viewport.X + (viewport.Width - 50) / 8 * 3 + 55, viewport.Y + 50 + viewport.Height / 20 * 5, viewport.Height / 20, (viewport.Height / 20)), Color.Black);
         //increaseMaxHealth.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         //increaseMaxHealth.setImageReferences("plus_button_none", "plus_button_hover", "plus_button_hover", "plus_button_none");
         increaseStrength = new Button("btnIncreaseStrength", "", new EngineRectangle(viewport.X + (viewport.Width - 50) / 8 * 3 + 55, viewport.Y + 50 + viewport.Height / 20 * 6, viewport.Height / 20, (viewport.Height / 20)), Color.Black);
         increaseStrength.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         increaseStrength.setImageReferences("plus_button_none", "plus_button_hover", "plus_button_hover", "plus_button_none");
         increaseToughness = new Button("btnIncreaseToughness", "", new EngineRectangle(viewport.X + (viewport.Width - 50) / 8 * 3 + 55, viewport.Y + 50 + viewport.Height / 20 * 7, viewport.Height / 20, (viewport.Height / 20)), Color.Black);
         increaseToughness.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         increaseToughness.setImageReferences("plus_button_none", "plus_button_hover", "plus_button_hover", "plus_button_none");
         increaseDexterity = new Button("btnIncreaseDexterity", "", new EngineRectangle(viewport.X + (viewport.Width - 50) / 8 * 3 + 55, viewport.Y + 50 + viewport.Height / 20 * 8, viewport.Height / 20, (viewport.Height / 20)), Color.Black);
         increaseDexterity.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleIncreaseButtonClick));
         increaseDexterity.setImageReferences("plus_button_none", "plus_button_hover", "plus_button_hover", "plus_button_none");

         removeCharacter = new Button("lblRemoveCharacter", "", new EngineRectangle(viewport.X + 100 + ((viewport.Width - 50) / 8 * 3), viewport.Y + 50 + viewport.Height / 20 * 2, viewport.Height / 20, viewport.Height / 20), Color.Black);
         removeCharacter.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleRemoveCharacterClick));
         removeCharacter.minFontScale = 0;
         removeCharacter.maxFontScale = 1;
         removeCharacter.setImageReferences("cancel_button_none", "cancel_button_hover", "cancel_button_hover","cancel_button_none");

         backButton = new Button("btnBack", "", new EngineRectangle(viewport.X + 50, viewport.Y + 50, viewport.Width / 10, viewport.Width / 10), Microsoft.Xna.Framework.Color.Black);
         backButton.minFontScale = 0;
         backButton.maxFontScale = 2;
         backButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleBackButtonClick));
         backButton.setImageReferences("wax_seal_back_none", "wax_seal_back_hover", "wax_seal_back_hover", "wax_seal_back_none");

         SquadModel sm = SquadService.getAllySquad();

         for(int i = 0; i < sm.characterIds.Count; i++)
         {
            CharacterModel characterModel = CharacterService.getCharacter(sm.characterIds[i]);
            CharacterUI ui = new CharacterUI(characterModel, new EngineRectangle(viewport.X + 50 + viewport.Width / 2, viewport.Y + 50 + viewport.Height / 10 * (i + 1), (viewport.Width / 2 - 100), viewport.Height / 10), Color.Black, "button_outline_none");
            ui.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleCharacterUIClick));
            if(sm.characterIds[i] == 0)
            {
               ui.selected = true;
            }
            characters.Add(ui);
         }

         //Initialize inventory seciton
         inventoryHeader = new Label("lblInventoryHeader", "Inventory - Gold: ", new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * 11, (viewport.Width - 50) / 8 * 3, viewport.Height / 20), Color.Black);
         inventory = InventoryService.inventory;
         inventoryHeader.value += inventory.Gold;
         inventoryControls.Add(inventoryHeader);

         for(int i = 0; i < inventory.items.Count; i++)
         {
            if (inventory.items[i].itemName == "map")
            {
               Button newButton = new Button("Map", inventory.items[i].itemName, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * (12 + i), (viewport.Width - 50) / 8, viewport.Height / 20), Color.Black);
               newButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleMapButtonClick));
               newButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
               newButton.minFontScale = 0;
               newButton.padding = new int[]
               {
                  0,10,0,10
               };
               inventoryControls.Add(newButton);
            }
            else
            {
               Label newLabel = new Label("Item:" + i.ToString(), "- " + inventory.items[i].itemName, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 20 * (12 + i), (viewport.Width - 50) / 8, viewport.Height / 20), Color.Black);
               inventoryControls.Add(newLabel);
            }
         }

      }

      public void HandleCharacterUIClick(object sender, OnClick clickEvent)
      {
         foreach(CharacterUI characterUIs in characters)
         {
            characterUIs.selected = false;
         }
         CharacterUI ui = sender as CharacterUI;
         ui.selected = true;
         CharacterModel cm = CharacterService.getCharacter(ui.characterId);
         model = cm;
         name.value = "Name: " + model.name;
         level.value = "Level: " + model.level.ToString();
         experience.value = "XP: " + model.experience.ToString();
         maxHealth.value = "Max Health: " + model.maxHealth.ToString();
         strength.value = "Strength: " + model.strength.ToString();
         toughness.value = "Toughness: " + model.toughness.ToString();
         dexterity.value = "Dexterity: " + model.dexterity.ToString();
         attributePoints.value = "Available Attribute Points: " + model.attiributePoints.ToString();
      }

      public void HandleIncreaseButtonClick(object sender, OnClick clickEvent)
      {
         var clickedButton = sender as Button;
         if(clickedButton != null)
         {
            if(clickedButton.Name == "btnIncreaseMaxHealth")
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
         if(model != null && model.id != 0)
         {
            SquadModel sm = SquadService.getAllySquad();
            sm.characterIds.Remove(model.id);

            var viewport = ScreenGraphicService.GetViewportBounds();

            characters = new List<CharacterUI>();
            for (int i = 0; i < sm.characterIds.Count; i++)
            {
               CharacterModel characterModel = CharacterService.getCharacter(sm.characterIds[i]);
               CharacterUI ui = new CharacterUI(characterModel, new EngineRectangle(viewport.X + 50 + viewport.Width / 2, viewport.Y + 50 + viewport.Height / 10 * (i + 1), (viewport.Width / 2 - 100), viewport.Height / 10), Color.Black, "button_outline_none");
               ui.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleCharacterUIClick));
               if (sm.characterIds[i] == 0)
               {
                  ui.selected = true;
               }
               characters.Add(ui);
            }

            HandleCharacterUIClick(characters[0], new OnClick());
         }
      }

      public void HandleBackButtonClick(object sender, OnClick clickEvent)
      {
         parentScene.bus.Publish(this, new SetCharacterInfoButtonVisabilityEvent(true));
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
            table = ContentService.Get2DTexture("table_image");
            scroll = ContentService.Get2DTexture("Scroll");
            name.onDeserialized();
            level.onDeserialized();
            experience.onDeserialized();
            maxHealth.onDeserialized();
            strength.onDeserialized();
            toughness.onDeserialized();
            dexterity.onDeserialized();
            attributePoints.onDeserialized();

            //increaseMaxHealth.onDeserialized();
            increaseStrength.onDeserialized();
            increaseToughness.onDeserialized();
            increaseDexterity.onDeserialized();

            squadTatics.onDeserialized();

            foreach (CharacterUI ui in characters)
            {
               ui.onDeserialized();
            }

            inventoryHeader.onDeserialized();
            foreach(Control c in inventoryControls)
            {
               c.onDeserialized();
            }

            backButton.onDeserialized();
         }
      }
      #endregion
   }
}