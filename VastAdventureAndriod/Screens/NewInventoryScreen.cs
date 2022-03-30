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
   public class NewInventoryScreen : BasicScreen
   {
      [NonSerialized]
      public Texture2D underline;
      public ListBox items;
      public Label lblTitle;
      public Button backButton;
      public Button itemPicture;
      public Label itemName;
      public Label itemDescription;
      public Label lblGoldAmount;

      public NewInventoryScreen()
      {
         EngineRectangle viewport = ScreenGraphicService.GetViewportBounds();

         lblGoldAmount = new Label("lblGoldAmount", "Gold: " + InventoryService.inventory.Gold.ToString(), new EngineRectangle(viewport.X, viewport.Y + viewport.Height / 10, viewport.Width, viewport.Height/ 20), Color.DarkGray);
         lblGoldAmount.padding = new int[] { 50, 0, 0, 0 };
         lblGoldAmount.textAnchor = Enums.TextAchorLocation.center;


         items = new ListBox("lstboxInventory", "", new EngineRectangle(viewport.X, viewport.Y + viewport.Height / 10 * 5, viewport.Width, viewport.Height / 10 * 5), Color.DarkGray);
         items.itemBaseHeight = (int)(viewport.Height / 10);
         items.padding = new int[]
         {
            0,(int)(items.bounds.Width / (float)20) ,0,10
         };
         //items.setImageReferences("none", "none", "none", "none");
         items.isFocused = true;
         items.isEditing = true;
         items.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleItemSelectionChange));
         items.setImageReferences("none", "none", "none", "none");
         items.bar.setImageReferences("dark_gray", "dark_gray", "dark_gray", "dark_gray");
         items.DragDelayInMS = 100;
         foreach (var i in InventoryService.inventory.items)
         {
            items.AddItem(new InventoryItemUI(i, new EngineRectangle(), Color.DarkGray));
         }
         items.textAnchor = Enums.TextAchorLocation.center;
         if(items.Items.Count == 0)
         {
            items.value = "Your inventory is empty";
         }

         underline = ContentService.Get2DTexture("white_bottom_bar");
         lblTitle = new Label("lblTitle", "Inventory", new EngineRectangle(0, 0, viewport.Width, viewport.Height / 10), Color.DarkGray);
         lblTitle.textAnchor = Enums.TextAchorLocation.centerLeft;
         lblTitle.padding = new int[] { 25, 100, 25, 50 };
         lblTitle.maxFontScale = 2;
         backButton = new Button("btnBack", "", new EngineRectangle(viewport.X + viewport.Width / 10 * 9 - 50, viewport.Y + viewport.Height / 40, viewport.Height / 20 * 1, viewport.Height / 20 * 1), Microsoft.Xna.Framework.Color.DarkGray);
         backButton.minFontScale = 0;
         backButton.maxFontScale = 2;
         backButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleBackButtonClick));
         backButton.setImageReferences("gray_exit_none", "gray_exit_hover", "gray_exit_hover", "gray_exit_hover");

         itemPicture = new Button("itemPictureButton", "", new EngineRectangle(viewport.X + viewport.Width / 8, viewport.Y + viewport.Height / 10 * 2, viewport.Width / 4, viewport.Width / 4), Color.DarkGray);
         itemPicture.setImageReferences("none", "none", "none", "none");
         itemPicture.init();
         itemName = new Label("lblItemName", "", new EngineRectangle(viewport.X + viewport.Width / 8 * 3, viewport.Y + viewport.Height / 10 * 2, viewport.Width / 8 * 5, viewport.Width / 4), Color.DarkGray);
         itemName.textAnchor = Enums.TextAchorLocation.center;
         itemName.padding = new int[]
         {
            0,25,0,25
         };
         itemDescription = new Label("lblItemDescription", "", new EngineRectangle(viewport.X + 50, viewport.Y + viewport.Height / 10 * 2 + viewport.Width / 4, viewport.Width - 100, viewport.Height / 10), Color.DarkGray);
         itemDescription.isMultiLine = true;
         itemDescription.minFontScale = 0;
         itemDescription.maxFontScale = 2;
         itemDescription.padding = new int[]
         {
            50,0,0,0
         };
         if (items.Items.Count > 0)
         {
            HandleItemSelectionChange(items, new OnChange("",0));
         }
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         lblGoldAmount.Update(dt);
         items.Update(dt);
         lblTitle.Update(dt);
         backButton.Update(dt);
         itemPicture.Update(dt);
         itemName.Update(dt);
         itemDescription.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = ScreenGraphicService.GetViewportBounds();
         base.Render(sb);
         lblGoldAmount.Render(sb);
         items.Render(sb);
         lblTitle.Render(sb);
         backButton.Render(sb);
         itemPicture.Render(sb);
         itemName.Render(sb);
         itemDescription.Render(sb);
         sb.Draw(underline, new Rectangle((int)viewport.X, (int)viewport.Y, (int)viewport.Width, (int)viewport.Height / 10), null, Color.White);
      }

      public void HandleBackButtonClick(object sender, OnClick clickEvent)
      {
         ScreenService.replaceWithPreviousScreen(parentScene);
      }

      public void HandleItemSelectionChange(object sender, OnChange change)
      {
         int newIndex = (int)change.newValue;
         ListBox listBox = (ListBox)sender;
         if (newIndex != -1)
         {
            InventoryItemModel tempModel = (InventoryItemModel)((IListBoxItem)listBox.GetSelectedItem()).Value;
            itemPicture.setImageReferences(tempModel.imageReference, tempModel.imageReference, tempModel.imageReference, tempModel.imageReference);
            itemPicture.init();
            itemName.value = tempModel.itemName;
            itemDescription.value = tempModel.itemDescription;
         }
      }

      public override void Init()
      {
         base.Init();
      }

      public override void UnInitialize()
      {
         base.UnInitialize();
      }

      public override void onDeserialized()
      {
         items.onDeserialized();
         underline = ContentService.Get2DTexture("white_bottom_bar");
         lblTitle.onDeserialized();
         backButton.onDeserialized();
         base.onDeserialized();
      }

   }
}