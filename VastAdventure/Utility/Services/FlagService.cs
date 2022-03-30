using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using VastAdventure.Utility;

namespace VastAdventure.Utility.Services
{
   public static class FlagService
   {
      private static FlagCollection flags;
      private static bool isInitialized = false;

      public static void Initialize()
      {
         LoadDefaultFlagCollection();
         isInitialized = true;
      }

      public static bool CheckFlagValue(string flag)
      {
         if(isInitialized)
         {
            return flags.CheckFlag(flag);
         }
         return false;
      }

      public static void SetFlagValue(string flag, bool value)
      {
         if(isInitialized)
         {
            flags.SetFlag(flag, value);
         }
      }

      public static void saveFlagCollectionToFile()
      {
         SavesService.SaveObjectToFile("flags.flagCollection", flags);
      }

      public static void LoadFlagCollection()
      {
         FlagCollection tempCollection = SavesService.GetObjectFromFile<FlagCollection>("flags.flagCollection");
         foreach(var flag in tempCollection.Flags)
         {
            flags.SetFlag(flag.Key, flag.Value);
         }
      }

      public static void LoadDefaultFlagCollection()
      {
         FlagCollection tempCollection = new FlagCollection();
         using (StreamReader reader = new StreamReader(AssetLoaderService.GetAssetStream("default.flags", "defaults/")))
         {
            while (!reader.EndOfStream)
            {
               string flag = reader.ReadLine();
               string[] splitFlag = flag.Split(':');
               tempCollection.AddFlag(splitFlag[0], bool.Parse(splitFlag[1]));
            }
         }
         flags = tempCollection;
      }

      public static FlagCollection GetFlagCollection()
      {
         return flags;
      }
   }
}