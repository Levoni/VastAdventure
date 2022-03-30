using Base.Utility;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Model;

namespace VastAdventure.UiInterfaces
{
   public interface AchievementUIInterface
   {
      AchievementModel Model { get; set; }
      EngineRectangle bounds { get; set; }
      void SetNewBounds(EngineRectangle newBounds);
      void Render(SpriteBatch sb);
      void SetNewAchievement(AchievementModel newModel);
      void onDeserialized();
   }
}
