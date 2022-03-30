using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Events;
using Base.UI.Mobile;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VastAdventure.Model;
using VastAdventure.Utility;
using Action = VastAdventure.Model.Action;
using VastAdventure.Screens;
using VastAdventure.Utility.Services;
using System.Runtime.Serialization;
using VastAdventure.Events;
using VastAdventureAndriod.VastAdventureUI;

namespace VastAdventureAndriod.Screens
{
   [Serializable]
   public class NewMapScreen:BasicScreen
   {
      //background
      [NonSerialized]
      Texture2D scroll;
      [NonSerialized]
      Texture2D underline;
      [NonSerialized]
      Texture2D map;

      Label Title;
      EngineRectangle mapBounds;

      Button back;
      List<Button> fastTravelButtons;

      public NewMapScreen() : base()
      {
         ScreenType = VastAdventure.Utility.ScreenType.Map;
      }

      public override void Update(int dt)
      {
         foreach (Button b in fastTravelButtons)
         {
            b.Update(dt);
         }
         Title.Update(dt);
         back.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(scroll, new Rectangle(0, (int)viewport.Height / 10, (int)viewport.Width, (int)viewport.Height / 10 * 9), Color.White);
         sb.Draw(underline, new Rectangle((int)viewport.X, (int)viewport.Y, (int)viewport.Width, (int)viewport.Height / 10), null, Color.White);
         sb.Draw(map, mapBounds.toRectangle(), Color.White);
         Title.Render(sb);

         foreach (Button b in fastTravelButtons)
         {
            b.Render(sb);
         }

         back.Render(sb);
      }

      public override void Init()
      {
         base.Init();
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         fastTravelButtons = new List<Button>();
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));

         //Creating UI
         scroll = ContentService.Get2DTexture("Scroll");
         map = ContentService.Get2DTexture("map");
         underline = ContentService.Get2DTexture("white_bottom_bar");

         back = new Button("btnBack", "", new EngineRectangle(viewport.X + viewport.Width / 10 * 9 - 50, viewport.Y + viewport.Height / 40, viewport.Height / 20 * 1, viewport.Height / 20 * 1), Microsoft.Xna.Framework.Color.DarkGray);
         back.minFontScale = 0;
         back.maxFontScale = 2;
         back.onClick = new EHandler<OnClick>(new Action<object, OnClick>(goBack));
         back.setImageReferences("gray_exit_none", "gray_exit_hover", "gray_exit_hover", "gray_exit_hover");

         mapBounds = new EngineRectangle(viewport.X + 50, viewport.Y + viewport.Height / 10 * 2, viewport.Width - 100, viewport.Height / 10 * 7);

         //Dimensions
         //Total Dimensions: 1000,1000
         //Room Dimensions: 149,100
         //Cell Block 1 Dimensions: 28,420
         //Cell Block 2 Dimensions; 44,274
         //Dungeon Entrance: 410,70
         //Guard Room 1  748,160
         //Guard Room 2: 151,298
         //Cell BLock 2: 171,395
         //Cell Block 1: 782, 255
         //Black Market: 712,674
         //Cataccomb Entrance: 111,666
         //Dark Room: 384,827

         Title = new Label("lblTitle", "Map", new EngineRectangle(0, 0, viewport.Width, viewport.Height / 10), Color.DarkGray);
         Title.minFontScale = 0;
         Title.maxFontScale = 2;
         Title.padding = new int[] { 25, 100, 25, 50 };

         if (FlagService.CheckFlagValue("onLocationScreen"))
         {
            Button DarkRoomFastTravel = new Button("0", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 384 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 827 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
            DarkRoomFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
            DarkRoomFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
            fastTravelButtons.Add(DarkRoomFastTravel);

            if (FlagService.CheckFlagValue("visitedCatacombs"))
            {
               Button CatacombsFastTravel = new Button("1", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 111 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 666 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               CatacombsFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               CatacombsFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(CatacombsFastTravel);
            }
            else
            {
               Button CatacombsFastTravel = new Button("1", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 111 / 1000, mapBounds.Y + mapBounds.Height * 666 / 1000, mapBounds.Width * 149 / 1000, mapBounds.Height * 100 / 1000), Color.Black);
               CatacombsFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(CatacombsFastTravel);
            }

            if (FlagService.CheckFlagValue("visitedBlackMarket"))
            {
               Button BlackMarketFastTravel = new Button("2", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 712 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 674 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               BlackMarketFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               BlackMarketFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(BlackMarketFastTravel);
            }
            else
            {
               Button BlackMarketFastTravel = new Button("2", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 712 / 1000, mapBounds.Y + mapBounds.Height * 674 / 1000, mapBounds.Width * 149 / 1000, mapBounds.Height * 100 / 1000), Color.Black);
               BlackMarketFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(BlackMarketFastTravel);
            }

            if (FlagService.CheckFlagValue("visitedCellBlockOne"))
            {
               Button CellBlockOneFastTravel = new Button("3", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 834 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 538 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               CellBlockOneFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               CellBlockOneFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(CellBlockOneFastTravel);
            }
            else
            {
               Button CellBlockOneFastTravel = new Button("3", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 782 / 1000, mapBounds.Y + mapBounds.Height * 255 / 1000, mapBounds.Width * 28 / 1000, mapBounds.Height * 420 / 1000), Color.Black);
               CellBlockOneFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(CellBlockOneFastTravel);
            }

            if (FlagService.CheckFlagValue("visitedCellBlockTwo"))
            {
               Button CellBlockTwoFastTravel = new Button("4", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 14 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 512 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               CellBlockTwoFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               CellBlockTwoFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(CellBlockTwoFastTravel);
            }
            else
            {
               Button CellBlockTwoFastTravel = new Button("4", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 171 / 1000, mapBounds.Y + mapBounds.Height * 395 / 1000, mapBounds.Width * 44 / 1000, mapBounds.Height * 274 / 1000), Color.Black);
               CellBlockTwoFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(CellBlockTwoFastTravel);
            }

            if (FlagService.CheckFlagValue("visitedGaurdHouseOne"))
            {
               Button GuardRoomOneFastTravel = new Button("5", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 748 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 160 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               GuardRoomOneFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               GuardRoomOneFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(GuardRoomOneFastTravel);
            }
            else
            {
               Button GuardRoomOneFastTravel = new Button("5", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 748 / 1000, mapBounds.Y + mapBounds.Height * 160 / 1000, mapBounds.Width * 149 / 1000, mapBounds.Height * 100 / 1000), Color.Black);
               GuardRoomOneFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(GuardRoomOneFastTravel);
            }

            if (FlagService.CheckFlagValue("visitedGaurdHouseTwo"))
            {
               Button GuardRoomTwoFastTravel = new Button("6", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 151 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 298 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               GuardRoomTwoFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               GuardRoomTwoFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(GuardRoomTwoFastTravel);
            }
            else
            {
               Button GuardRoomTwoFastTravel = new Button("6", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 151 / 1000, mapBounds.Y + mapBounds.Height * 298 / 1000, mapBounds.Width * 149 / 1000, mapBounds.Height * 100 / 1000), Color.Black);
               GuardRoomTwoFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(GuardRoomTwoFastTravel);
            }

            if (FlagService.CheckFlagValue("visitedGaurdHouseTwo"))
            {
               Button DungeonEntranceFastTravel = new Button("7", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 149 / 1000 / 4 + mapBounds.Width * 410 / 1000, mapBounds.Y + mapBounds.Height * 100 / 1000 / 4 + mapBounds.Height * 70 / 1000, mapBounds.Width * 149 / 1000 / 2, mapBounds.Height * 100 / 1000 / 2), Color.Black);
               DungeonEntranceFastTravel.setImageReferences("fast_travel", "fast_travel", "fast_travel", "fast_travel");
               DungeonEntranceFastTravel.onClick = new EHandler<OnClick>(new Action<object, OnClick>(FastTravelClick));
               fastTravelButtons.Add(DungeonEntranceFastTravel);
            }
            else
            {
               Button DungeonEntranceFastTravel = new Button("7", "", new EngineRectangle(mapBounds.X + mapBounds.Width * 410 / 1000, mapBounds.Y + mapBounds.Height * 70 / 1000, mapBounds.Width * 149 / 1000, mapBounds.Height * 100 / 1000), Color.Black);
               DungeonEntranceFastTravel.setImageReferences("shading", "shading", "shading", "shading");
               fastTravelButtons.Add(DungeonEntranceFastTravel);
            }
         }
      }

      public void goBack(object sender, OnClick click)
      {
         ScreenService.replaceWithPreviousScreen(parentScene);
      }

      public void FastTravelClick(object sender, OnClick click)
      {
         foreach (Button button in fastTravelButtons)
         {
            if (button.Name == ((Button)sender).Name)
            {
               ScreenService.replaceScreen(parentScene, "location:" + button.Name);
            }
         }

      }

      #region deserialization
      public override void onDeserialized()
      {
         if (ContentService.isInitialized)
         {
            scroll = ContentService.Get2DTexture("Scroll");
            map = ContentService.Get2DTexture("map");
            underline = ContentService.Get2DTexture("white_bottom_bar");
            Title.onDeserialized();
            back.onDeserialized();
            foreach (var button in fastTravelButtons)
            {
               button.onDeserialized();
            }
         }
      }
      #endregion
   }
}