using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.UI.Mobile;
using Base.Utility;
using VastAdventure.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.Model;
using Base.Events;
using Base.Utility.Services;

namespace VastAdventureAndriod.VastAdventureUI
{
   [Serializable]
   //TODO: make border (create image control)
   public class CharacterUI:Control
   {
      //EngineRectangle bounds;
      public int characterId { get; set; }
      public bool selected;


      protected Label lblName;
      protected Label lblLevel;
      protected Label lblHealth;
      [NonSerialized]
      public Texture2D Outline;
      public string outlineReference;

      public CharacterUI()
      {
         selected = false;
         Outline = ContentService.Get2DTexture("button_outline_none");
         lblName = new Label();
         lblLevel = new Label();
         lblHealth = new Label();
      }

      public CharacterUI(CharacterModel character, EngineRectangle bounds, Color color, string outlineReference):base()
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
         lblName = new Label("lblName", character.name, new EngineRectangle(safeBounds.X,safeBounds.Y, (safeBounds.Width / 4) * 3,safeBounds.Height / 3), textColor);
         lblName.maxFontScale = 1;
         lblName.minFontScale = .2f;
         lblName.textAnchor = Enums.TextAchorLocation.centerLeft;
         lblLevel = new Label("lblLevel", "Lvl: " + character.level.ToString(), new EngineRectangle(safeBounds.X, safeBounds.Y + ((safeBounds.Height / 3) * 1), safeBounds.Width / 4, safeBounds.Height / 3), textColor) ;
         lblLevel.maxFontScale = 1;
         lblLevel.minFontScale = .2f;
         lblLevel.textAnchor = Enums.TextAchorLocation.centerLeft;
         lblHealth = new Label("lblHealth", "Health: " +  character.maxHealth.ToString() + " / " + character.maxHealth, new EngineRectangle(safeBounds.X, safeBounds.Y + ((safeBounds.Height / 3) * 2), safeBounds.Width, safeBounds.Height / 3), textColor);
         lblHealth.maxFontScale = 1;
         lblHealth.minFontScale = .2f;
         lblHealth.textAnchor = Enums.TextAchorLocation.centerLeft;
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         lblName.Update(dt);
         lblLevel.Update(dt);
         lblHealth.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         if(selected)
         {
            sb.Draw(Outline, bounds.toRectangle(), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
         }
         lblName.Render(sb);
         lblLevel.Render(sb);
         lblHealth.Render(sb);
      }

      public void UpdateCharacterInfo(Character character)
      {
         lblName.value = character.model.name;
         lblLevel.value = "Lvl: " + character.model.level.ToString();
         lblHealth.value = "Health: " + character.health + " / " + character.model.maxHealth;
         if(character.health <= 0)
         {
            selected = false;
         }
      }

      public override void onDeserialized()
      {
         lblName.onDeserialized();
         lblLevel.onDeserialized();
         lblHealth.onDeserialized();
         if (ContentService.isInitialized)
         {
            Outline = ContentService.Get2DTexture(outlineReference);
            init();
         }
      }
   }
}