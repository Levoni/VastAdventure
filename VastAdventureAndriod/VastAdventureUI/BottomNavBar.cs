using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Events;
using Base.Scenes;
using Base.UI.Mobile;
using Base.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.Utility.Services;

namespace VastAdventureAndriod.VastAdventureUI
{
   [Serializable]
   public class BottomNavBar : Control
   {
      NavBarButton character;
      NavBarButton squad;
      NavBarButton map;
      NavBarButton inventory;
      NavBarButton menu;
      public Scene parentScene;

      public BottomNavBar(string name, string value, EngineRectangle bounds, Color color) : base(name, value, bounds, color)
      {
         character = new NavBarButton("btnCharacter", "Character", new EngineRectangle(bounds.X, bounds.Y, bounds.Width / 5, bounds.Height), color, "character_icon");
         //character.setImageReferences("button_character_gray_outline_none", "button_character_gray_outline_hover", "button_character_gray_outline_pressed", "button_character_gray_outline_released");
         character.minFontScale = .5f;
         character.padding = new int[] { (int)bounds.Height / 2, 10, 0, 10 };
         character.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleCharacterButtonPress));
         squad = new NavBarButton("btnSquad", "Squad", new EngineRectangle(bounds.X + bounds.Width / 5, bounds.Y, bounds.Width / 5, bounds.Height), color, "squad_icon");
         //squad.setImageReferences("button_squad_gray_outline_none", "button_squad_gray_outline_hover", "button_squad_gray_outline_pressed", "button_squad_gray_outline_released");
         squad.minFontScale = .5f;
         squad.padding = new int[] { (int)bounds.Height / 2, 10, 0, 10 };
         squad.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleSquadClick));
         map = new NavBarButton("btnMap", "Map", new EngineRectangle(bounds.X + bounds.Width / 5 * 2, bounds.Y, bounds.Width / 5, bounds.Height), color, "map_icon");
         //map.setImageReferences("button_map_gray_outline_none", "button_map_gray_outline_hover", "button_map_gray_outline_pressed", "button_map_gray_outline_released");
         map.minFontScale = .5f;
         map.padding = new int[] { (int)bounds.Height / 2, 10, 0, 10 };
         map.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleMapClick));
         inventory = new NavBarButton("btnMenu", "Inventory", new EngineRectangle(bounds.X + bounds.Width / 5 * 3, bounds.Y, bounds.Width / 5, bounds.Height), color, "inventory_icon");
         //inventory.setImageReferences("button_menu_gray_outline_none", "button_menu_gray_outline_hover", "button_menu_gray_outline_pressed", "button_menu_gray_outline_released");
         inventory.minFontScale = .5f;
         inventory.padding = new int[] { (int)bounds.Height / 2, 10, 0, 10 };
         inventory.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleInventoryClick));
         menu = new NavBarButton("btnMenu", "Menu", new EngineRectangle(bounds.X + bounds.Width / 5 * 4, bounds.Y, bounds.Width / 5, bounds.Height), color, "menu_icon");
         //menu.setImageReferences("button_menu_gray_outline_none", "button_menu_gray_outline_hover", "button_menu_gray_outline_pressed", "button_menu_gray_outline_released");
         menu.minFontScale = .5f;
         menu.padding = new int[] { (int)bounds.Height / 2, 10, 0, 10 };
         menu.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleMenuClick));
         setImageReferences("none", "none", "none", "none");
         init();
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         character.Update(dt);
         squad.Update(dt);
         if (InventoryService.HaveItem("map"))
         {
            map.Update(dt);
         }
         inventory.Update(dt);
         menu.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         character.Render(sb);
         squad.Render(sb);
         if (!InventoryService.HaveItem("map"))
         {
            map.iconReference = "map_dark_icon";
         }
         else
         {
            map.iconReference = "map_icon";
         }
         map.Render(sb);
         inventory.Render(sb);
         menu.Render(sb);
      }

      public override void onDeserialized()
      {
         base.onDeserialized();
         character.onDeserialized();
         squad.onDeserialized();
         map.onDeserialized();
         inventory.onDeserialized();
         menu.onDeserialized();
      }

      public void handleMenuClick(object sender, OnClick click)
      {
         if (parentScene != null)
         {
            SavesService.SaveGame(parentScene);
            Base.System.GUISystem guiSystem = parentScene.GetSystem<Base.System.GUISystem>();
            guiSystem.ClearRegisteredEntities();

            //Setup Main Menu
            Screens.MainMenu mm = new Screens.MainMenu();
            Base.Entities.Entity entity = parentScene.CreateEntity();
            mm.Init(parentScene.bus, parentScene);
            mm.Init(entity);
            parentScene.AddComponent(entity, mm);
         }
      }

      public void handleMapClick(object sender, OnClick click)
      {
         if (parentScene != null && InventoryService.HaveItem("map"))
         {
            string commands = "map:-1";
            ScreenService.replaceScreen(parentScene, commands);
         }
      }

      public void handleSquadClick(object sender, OnClick click)
      {
         string commands = "squad:-1";
         ScreenService.replaceScreen(parentScene, commands);
      }

      public void handleInventoryClick(object sender, OnClick click)
      {
         string commands = "inventory:-1";
         ScreenService.replaceScreen(parentScene, commands);
      }

      private void handleCharacterButtonPress(object sender, OnClick clickEvent)
      {
         ScreenService.replaceScreen(parentScene, "characterInfo:-1");
      }
   }
}