using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Base.UI.Mobile;
using Base.Utility;
using Microsoft.Xna.Framework;

namespace VastAdventureAndriod.VastAdventureUI
{
   [Serializable]
   public class uiTypeListBoxItem:ListBoxItem
   {
      public uiTypeListBoxItem():base()
      {

      }
      public uiTypeListBoxItem(object item):base(item)
      {

      }

      public override void RenderToBounds(SpriteBatch sb, EngineRectangle renderBounds)
      {
         if (isSelected)
         {
            Vector2 fontSize = font.MeasureString(Value.ToString());
            EngineVector2 vectorScaling = RenderUtil.CalculateFontScaling(Value.ToString(), renderBounds, font);
            float scaling = Math.Min(vectorScaling.X, vectorScaling.Y);
            sb.DrawString(font, Value.ToString(), new Vector2(renderBounds.X, renderBounds.Y + renderBounds.Height / 2), Color.White, 0, new Vector2(0, fontSize.Y / 2), scaling, SpriteEffects.None, 0);
         }
         else
         {
            Vector2 fontSize = font.MeasureString(Value.ToString());
            EngineVector2 vectorScaling = RenderUtil.CalculateFontScaling(Value.ToString(), renderBounds, font);
            float scaling = Math.Min(vectorScaling.X, vectorScaling.Y);
            sb.DrawString(font, Value.ToString(), new Vector2(renderBounds.X, renderBounds.Y + renderBounds.Height / 2), Color.DarkGray, 0, new Vector2(0, fontSize.Y / 2), scaling, SpriteEffects.None, 0);
         }
      }
   }
}