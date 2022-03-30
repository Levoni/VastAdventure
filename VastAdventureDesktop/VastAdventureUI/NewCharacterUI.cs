using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Base.Utility;
using VastAdventure.Model;
using Base.UI;
using Base.Utility.Services;
using Microsoft.Xna.Framework.Graphics;

namespace VastAdventureDesktop.VastAdventureUI
{
   [Serializable]
   public class NewCharacterUI : CharacterUI
   {
      Button classIcon;
      public NewCharacterUI() : base()
      {
         classIcon = new Button();
      }

      public NewCharacterUI(CharacterModel character, EngineRectangle bounds, Color color, string outlineReference) : base(character, bounds, color, outlineReference)
      {
         EngineRectangle safeBounds = new EngineRectangle(bounds.X + 25, bounds.Y + 25, bounds.Width - 50, bounds.Height - 50);
         this.textColor = color;
         this.textColorUint = color.PackedValue;
         selected = false;
         this.outlineReference = outlineReference;
         Outline = ContentService.Get2DTexture(outlineReference);
         value = "";
         imageReference = new Base.Serialization.SerializableDictionary<string, string>();
         this.bounds = bounds;
         base.init();

         characterId = character.id;
         this.bounds = bounds;
         lblName = new Label("lblName", character.name, new EngineRectangle(safeBounds.X + safeBounds.Width / 4, safeBounds.Y, (safeBounds.Width / 4) * 3, safeBounds.Height / 3), textColor);
         lblName.maxFontScale = 1;
         lblName.minFontScale = .2f;
         lblName.textAnchor = Enums.TextAchorLocation.centerLeft;
         lblLevel = new Label("lblLevel", "Lvl: " + character.level.ToString(), new EngineRectangle(safeBounds.X + safeBounds.Width / 4, safeBounds.Y + ((safeBounds.Height / 3) * 1), (safeBounds.Width / 4) * 3, safeBounds.Height / 3), textColor);
         lblLevel.maxFontScale = 1;
         lblLevel.minFontScale = .2f;
         lblLevel.textAnchor = Enums.TextAchorLocation.centerLeft;
         lblHealth = new Label("lblHealth", "Health: " + character.maxHealth.ToString() + " / " + character.maxHealth, new EngineRectangle(safeBounds.X + safeBounds.Width / 4, safeBounds.Y + ((safeBounds.Height / 3) * 2), (safeBounds.Width / 4) * 3, safeBounds.Height / 3), textColor);
         lblHealth.maxFontScale = 1;
         lblHealth.minFontScale = .2f;
         lblHealth.textAnchor = Enums.TextAchorLocation.centerLeft;

         if (safeBounds.Width / 4 > safeBounds.Height)
         {
            classIcon = new Button("btnIcon", "", new EngineRectangle(safeBounds.X, safeBounds.Y, safeBounds.Height, safeBounds.Height), Color.White);
            classIcon.setImageReferences(character.characterClass.ToString() + "_icon", character.characterClass.ToString() + "_icon", character.characterClass.ToString() + "_icon", character.characterClass.ToString() + "_icon");
         }
         else
         {
            classIcon = new Button("btnIcon", "", new EngineRectangle(safeBounds.X, safeBounds.Y + safeBounds.Width / 8, safeBounds.Width / 4, safeBounds.Width / 4), Color.White);
            classIcon.setImageReferences(character.characterClass.ToString() + "_icon", character.characterClass.ToString() + "_icon", character.characterClass.ToString() + "_icon", character.characterClass.ToString() + "_icon");
         }
      }

      public override void Update(int dt)
      {
         base.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         classIcon.Render(sb);
      }

      public override void onDeserialized()
      {
         base.onDeserialized();
         classIcon.onDeserialized();
      }
   }
}