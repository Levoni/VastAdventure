using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VastAdventure.Model;

namespace VastAdventure.Utility.Services
{
   public static class AchievementService
   {
      public static bool isInitialized = false;

      public static Dictionary<string, AchievementModel> achievements;

      public static void Initialize()
      {
         LoadDefaultAchievements();
         LoadAchievements();
         SavesService.SaveObjectToFile("Achievement.achievement", achievements);
      }

      public static void LoadDefaultAchievements()
      {
         achievements = new Dictionary<string, AchievementModel>();
         List<AchievementModel> tempList = AssetLoaderService.GetXmlSerializedAsset<List<AchievementModel>>("default.achieve", "defaults/");
         foreach (AchievementModel model in tempList)
         {
            achievements.Add(model.Name, model);
         }
      }

      public static void LoadAchievements()
      {
         Dictionary<string, AchievementModel> tempAchievements = SavesService.GetObjectFromFile<Dictionary<string, AchievementModel>>("Achievement.achievement");
         if (tempAchievements != null)
         {
            foreach (var achiemventPair in tempAchievements)
            {
               achievements[achiemventPair.Key] = achiemventPair.Value;
            }
         }
      }

      public static void SaveAchievements()
      {
         SavesService.SaveObjectToFile("Achievement.achievement", achievements);
      }

      public static void ClearAchievements()
      {
         SavesService.DeleteFile("Achievement.achievement");
         LoadDefaultAchievements();
      }
   }
}
