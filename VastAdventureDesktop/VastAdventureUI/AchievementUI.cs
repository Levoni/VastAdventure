using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Base.UI;
using Base.Utility;
using VastAdventure.Model;
using VastAdventure.UiInterfaces;
using Microsoft.Xna.Framework.Graphics;
using Base.Utility.Services;

namespace VastAdventureDesktop.VastAdventureUI
{
   [Serializable]
   public class AchievementUI : Control, AchievementUIInterface, IListBoxItem
   {
      public AchievementModel Model { get; set; }
      public object Value { get; set; }
      public bool isSelected { get; set; }
      Label lblName;
      Label lblDescription;
      public string AchievementIconReference = "defaultTexture";
      public float MinFontSize { get; set; }
      public float MaxFontSize { get; set; }

      public AchievementUI(AchievementModel achievement, EngineRectangle bounds) : base(achievement.Name, "", bounds, Color.White)
      {
         Model = achievement;
         Value = Model;
         isSelected = false;
         AchievementIconReference = achievement.ImageReference;
         //TODO: add achivement texture than set it as the referenece
         imageReference.Add(Enums.cState.none.ToString(), "defaultTexture");
         imageReference.Add(Enums.cState.hover.ToString(), "defaultTexture");
         imageReference.Add(Enums.cState.pressed.ToString(), "defaultTexture");
         imageReference.Add(Enums.cState.released.ToString(), "defaultTexture");
         padding[3] = 50;
         init();
         lblName = new Label("lblName", achievement.Name, new EngineRectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height / 2), Color.White);
         lblName.padding = new int[]
         {
            5,25,5,(int)bounds.Height
         };
         lblName.minFontScale = .5f;
         lblName.maxFontScale = 1;
         lblDescription = new Label("lblDescription", achievement.Description, new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 2, bounds.Width, bounds.Height / 2), Color.White);
         lblDescription.padding = new int[]
         {
            5,25,5,(int)bounds.Height
         };
         lblDescription.minFontScale = .25f;
         lblDescription.maxFontScale = .5f;

         if (!Model.hasAchievement)
         {
            setImageReferences("board_gold_outline_disabled", "board_gold_outline_disabled", "board_gold_outline_disabled", "board_gold_outline_disabled");
         }
         else
         {
            setImageReferences("board_gold_outline", "board_gold_outline", "board_gold_outline", "board_gold_outline");
         }
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         lblName.Render(sb);
         lblDescription.Render(sb);
         if (Model.hasAchievement)
         {
            sb.Draw(ContentService.Get2DTexture(AchievementIconReference), new Rectangle((int)bounds.X + 25, (int)bounds.Y + 10, (int)bounds.Height - 35, (int)bounds.Height - 20), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
         }
      }

      public void SetNewAchievement(AchievementModel newAchievement)
      {
         AchievementModel oldAchievement = Model;

         Model = newAchievement;
         Name = newAchievement.Name;
         lblName.value = newAchievement.Name;
         lblDescription.value = newAchievement.Description;
         AchievementIconReference = newAchievement.ImageReference;

         if (Model.hasAchievement != oldAchievement.hasAchievement)
         {
            if (!Model.hasAchievement)
            {
               setImageReferences("board_gold_outline_disabled", "board_gold_outline_disabled", "board_gold_outline_disabled", "board_gold_outline_disabled");
            }
            else
            {
               setImageReferences("board_gold_outline", "board_gold_outline", "board_gold_outline", "board_gold_outline");
            }
         }
      }

      public void SetNewBounds(EngineRectangle newBounds)
      {
         bounds = newBounds;
         lblName.bounds = new EngineRectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height / 2);
         lblName.padding = new int[]
         {
            5,25,5,(int)bounds.Height
         };
         lblDescription.bounds = new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 2, bounds.Width, bounds.Height / 2);
         lblDescription.padding = new int[]
         {
            5,25,5,(int)bounds.Height
         };
      }

      public void RenderToBounds(SpriteBatch sb, EngineRectangle renderBounds)
      {
         bounds = renderBounds;
         lblName.bounds = new EngineRectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height / 2);
         lblName.padding = new int[]
         {
            5,25,5,(int)bounds.Height
         };
         lblDescription.bounds = new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 2, bounds.Width, bounds.Height / 2);
         lblDescription.padding = new int[]
         {
            5,25,5,(int)bounds.Height
         };
         Render(sb);
      }

      public override void onDeserialized()
      {
         base.onDeserialized();
         lblName.onDeserialized();
         lblDescription.onDeserialized();
      }
   }
}