using Base.UI;
using Base.Utility;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VastAdventure.Model;
using Microsoft.Xna.Framework;
using Base.Utility.Services;

namespace VastAdventureDesktop.VastAdventureUI
{
   [Serializable]
   public class InventoryItemUI : Control, IListBoxItem
   {
      InventoryItemModel item;
      Button itemImage;
      Label itemName;
      public object Value { get; set; }
      public bool isSelected { get; set; }
      public float MinFontSize { get; set; }
      public float MaxFontSize { get; set; }

      public InventoryItemUI(InventoryItemModel item, EngineRectangle bounds, Color color) : base(item.itemDescription, "", bounds, color)
      {
         this.item = item;
         Value = item;
         itemImage = new Button(item.itemName + "button", "", new EngineRectangle(), color);
         itemImage.setImageReferences(item.imageReference, item.imageReference, item.imageReference, item.imageReference);
         itemName = new Label("lblItemName", item.itemName, new EngineRectangle(), Color.DarkGray);
         itemName.padding = new int[]
         {
            25,0,25,0
         };
         itemName.textAnchor = Enums.TextAchorLocation.centerLeft;
         isSelected = false;
         setImageReferences("button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none");
         init();
      }

      public override void Render(SpriteBatch sb)
      {
         //TODO: show different image (different state) if achievement has been gotten or not
         base.Render(sb);
         itemName.Render(sb);
         if (item != null)
         {
            sb.Draw(ContentService.Get2DTexture(item.imageReference), new Rectangle((int)bounds.X + 25, (int)bounds.Y + 10, (int)bounds.Height - 35, (int)bounds.Height - 20), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
         }
      }

      public void RenderToBounds(SpriteBatch sb, EngineRectangle renderBounds)
      {
         bounds = renderBounds;
         itemImage.bounds = new EngineRectangle(renderBounds.X + 50, renderBounds.Y + renderBounds.Height / 10, renderBounds.Height / 10 * 8, renderBounds.Height / 10 * 8);
         itemName.bounds = new EngineRectangle(renderBounds.X + renderBounds.Width / 4, renderBounds.Y, renderBounds.Width / 4 * 3, renderBounds.Height);
         Render(sb);
      }

      public override void onDeserialized()
      {
         base.onDeserialized();
         itemName.onDeserialized();
         itemImage.onDeserialized();
      }
   }
}