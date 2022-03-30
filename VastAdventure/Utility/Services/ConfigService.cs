using Base.Serialization;
using Base.Utility.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VastAdventure.Model;

namespace VastAdventure.Utility.Services
{
   public static class ConfigService
   {
      public static bool isInitialized = false;
      public static ConfigModel config;


      public static void Initialize()
      {
         LoadConfig();
      }

      public static void LoadConfig()
      {
         config = SavesService.GetObjectFromFile<ConfigModel>("settings.config");
         if (config == null)
         {
            config = new ConfigModel();
            SavesService.SaveObjectToFile("settings.config", config);
         }
         if(SavesService.saveFile.SaveVersion == "1.3")
         {
            if (config.UIType == null)
            {
               config.UIType = "dark theme";
            }
         }
         AudioService.SetBackgroundVolume(config.SoundVolume);
      }

      public static void SaveConfig()
      {
         if (config != null)
         {
            SavesService.SaveObjectToFile("settings.config", config);
         }
      }

      public static void ClearConfig()
      {
         SavesService.DeleteFile("settings.config");
         LoadConfig();
      }
   }
}
