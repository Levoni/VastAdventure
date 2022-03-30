using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Base.UI.Mobile;
using Base.Utility;
using VastAdventure.Model;

namespace VastAdventureAndriod.VastAdventureUI
{
   [Serializable]
   public class GraveListBox:ListBox
   {
      public GraveListBox(string name, string value, EngineRectangle Rectangle, Microsoft.Xna.Framework.Color color) : base(name, value, Rectangle, color)
      {
         //Items = new List<object>();
         //CharacterModel cm = new CharacterModel();
         //cm.id = 5000;
         //Items.Add(new GraveyardModel(cm));
         //CharacterModel cm1 = new CharacterModel();
         //cm1.level = 1;
         //Items.Add(new GraveyardModel(cm1));
         //CharacterModel cm2 = new CharacterModel();
         //cm2.level = 2;
         //Items.Add(new GraveyardModel(cm2));
         //CharacterModel cm3 = new CharacterModel();
         //cm3.level = 3;
         //Items.Add(new GraveyardModel(cm3));
         //CharacterModel cm4 = new CharacterModel();
         //cm4.level = 4;
         //Items.Add(new GraveyardModel(cm4));
         //CharacterModel cm5 = new CharacterModel();
         //cm5.level = 5;
         //Items.Add(new GraveyardModel(cm5));
         //CharacterModel cm6 = new CharacterModel();
         //cm6.level = 6;
         //Items.Add(new GraveyardModel(cm6));
         //CharacterModel cm7 = new CharacterModel();
         //cm7.level = 7;
         //Items.Add(new GraveyardModel(cm7));
         //CharacterModel cm8 = new CharacterModel();
         //cm8.level = 8;
         //Items.Add(new GraveyardModel(cm8));
         //CharacterModel cm9 = new CharacterModel();
         //cm9.level = 9;
         //Items.Add(new GraveyardModel(cm9));
         //CharacterModel cm10 = new CharacterModel();
         //cm10.level = 10;
         //Items.Add(new GraveyardModel(cm10));

         this.value = value;
         selectedIndex = 0;
         startingIndex = 0;
         isEditable = false;
         //upButton = new Button("upButton", "", new EngineRectangle(Rectangle.X, Rectangle.Y + (Rectangle.Height / 10 * 9), Rectangle.Width / 2, Rectangle.Height / 10), Color.Black);
         //upButton.setImageReferences("button_arrow_left_default_none", "button_arrow_left_default_hover", "button_arrow_left_default_pressed", "button_arrow_left_default_released");
         //downButton = new Button("downButton", "", new EngineRectangle(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + (Rectangle.Height/ 10 * 9), Rectangle.Width / 2, Rectangle.Height / 10), Color.Black);
         //downButton.setImageReferences("button_arrow_right_default_none", "button_arrow_right_default_hover", "button_arrow_right_default_pressed", "button_arrow_right_default_released");
         padding = new int[]
         {
            0,
            (int)Rectangle.Width / 10,
            0,
            0
         };
         setImageReferences("none", "none", "none", "none");
         init();
      }
   }
}