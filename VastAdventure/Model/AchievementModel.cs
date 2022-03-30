using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Model
{
   [Serializable]
   public class AchievementModel
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public string ImageReference { get; set; }
      public bool hasAchievement { get; set; }
   }
}
