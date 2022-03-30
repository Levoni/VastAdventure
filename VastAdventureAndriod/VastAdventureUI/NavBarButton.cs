using Base.UI.Mobile;
using Base.Serialization;
using Base.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Base.Utility.Services;

namespace VastAdventureAndriod.VastAdventureUI
{
   [Serializable]
   public class NavBarButton:Button
   {
      public string iconReference;
      public NavBarButton() : base()
      {
         Name = "button";
         value = "Click";
         textAnchor = Enums.TextAchorLocation.center;
         init();
      }

      public NavBarButton(string name, string value, EngineRectangle bounds, Color color, string iconReference) : base(name, value, bounds, color)
      {
         textAnchor = Enums.TextAchorLocation.center;
         setImageReferences("button_gray_outline_none", "button_gray_outline_hover", "button_gray_outline_pressed", "button_gray_outline_released");
         minFontScale = .5f;
         padding = new int[] { (int)bounds.Height / 2, 10, 0, 10 };
         this.iconReference = iconReference;
         init();
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         if(clickState == Enums.cState.none)
         {
            sb.Draw(ContentService.Get2DTexture(iconReference + "_none"), new Rectangle((int)bounds.X + ((int)bounds.Width - (int)(bounds.Height / 8 * 3)) / 2, (int)bounds.Y + (int)bounds.Height / 8, (int)bounds.Height / 8 * 3, (int)bounds.Height / 8 * 3), Color.DarkGray);
         }
         else
         {
            sb.Draw(ContentService.Get2DTexture(iconReference + "_hover"), new Rectangle((int)bounds.X + ((int)bounds.Width - (int)(bounds.Height / 8 * 3)) / 2, (int)bounds.Y + (int)bounds.Height / 8, (int)bounds.Height / 8 * 3, (int)bounds.Height / 8 * 3), Color.DarkGray);
         }

      }
   }
}