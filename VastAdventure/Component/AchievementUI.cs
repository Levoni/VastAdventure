using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Components;
using VastAdventure.Model;

namespace VastAdventure.Component
{
   [Serializable]
   public class Achievement:Component<Achievement>
   {
      public AchievementModel achievement;
      public int timeLeft;
      public Achievement(AchievementModel achievement, int lifetime)
      {
         this.achievement = achievement;
         this.timeLeft = lifetime;
      }

      public Achievement()
      {
         this.achievement = null;
         this.timeLeft = 0;
      }
   }
}