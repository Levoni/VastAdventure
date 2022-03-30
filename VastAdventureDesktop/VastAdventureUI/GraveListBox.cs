using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Base.UI;
using Base.Utility;
using VastAdventure.Model;

namespace VastAdventureDesktop.VastAdventureUI
{
   [Serializable]
   public class GraveListBox:ListBox
   {
      public GraveListBox(string name, string value, EngineRectangle Rectangle, Microsoft.Xna.Framework.Color color) : base(name, value, Rectangle, color)
      {
         //Items = new List<IListBoxItem>();

         //CharacterModel cm = new CharacterModel();
         //ListBoxItem i= new ListBoxItem();
         //i.Value = new GraveyardModel(cm);
         //Items.Add(i);
         //CharacterModel cm1 = new CharacterModel();
         //cm1.level = 1;
         //ListBoxItem i1 = new ListBoxItem();
         //i1.Value = new GraveyardModel(cm1);
         //Items.Add(i1);
         //CharacterModel cm2 = new CharacterModel();
         //cm2.level = 2;
         //ListBoxItem i2 = new ListBoxItem();
         //i2.Value = new GraveyardModel(cm2);
         //Items.Add(i2);
         //CharacterModel cm3 = new CharacterModel();
         //cm3.level = 3;
         //ListBoxItem i3 = new ListBoxItem();
         //i3.Value = new GraveyardModel(cm3);
         //Items.Add(i3);
         //CharacterModel cm4 = new CharacterModel();
         //cm4.level = 4;
         //ListBoxItem i4 = new ListBoxItem();
         //i4.Value = new GraveyardModel(cm4);
         //Items.Add(i4);
         //CharacterModel cm5 = new CharacterModel();
         //cm5.level = 5;
         //ListBoxItem i5 = new ListBoxItem();
         //i5.Value = new GraveyardModel(cm5);
         //Items.Add(i5);
         //CharacterModel cm6 = new CharacterModel();
         //cm6.level = 6;
         //ListBoxItem i6 = new ListBoxItem();
         //i6.Value = new GraveyardModel(cm6);
         //Items.Add(i6);
         //CharacterModel cm7 = new CharacterModel();
         //cm7.level = 7;
         //ListBoxItem i7 = new ListBoxItem();
         //i7.Value = new GraveyardModel(cm7);
         //Items.Add(i7);
         //CharacterModel cm8 = new CharacterModel();
         //cm8.level = 8;
         //ListBoxItem i8 = new ListBoxItem();
         //i8.Value = new GraveyardModel(cm8);
         //Items.Add(i8);
         //CharacterModel cm9 = new CharacterModel();
         //cm9.level = 9;
         //ListBoxItem i9 = new ListBoxItem();
         //i9.Value = new GraveyardModel(cm9);
         //Items.Add(i9);
         //CharacterModel cm10 = new CharacterModel();
         //cm10.level = 10;
         //ListBoxItem i10 = new ListBoxItem();
         //i10.Value = new GraveyardModel(cm10);
         //Items.Add(i10);


         this.value = value;
         selectedIndex = 0;
         startingIndex = 0;
         isEditable = true;
         isEditing = true;
         isFocused = true;
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