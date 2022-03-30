using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Utility
{
   [Serializable]
   public class FlagCollection
   {
      public Dictionary<string, bool> Flags { get; set; }

      public FlagCollection()
      {
         Flags = new Dictionary<string, bool>();
      }

      public bool CheckFlag(string flag)
      {
         if (Flags.ContainsKey(flag))
         {
            return Flags[flag];
         }
         return false;
      }

      public void SetFlag(string flag, bool value)
      { 
         Flags[flag] = value;
      }

      public bool AddFlag(string flag, bool value)
      {
         if (!Flags.ContainsKey(flag))
         {
            Flags[flag] = value;
            return true;
         }
         return false;
      }

      public void SetAllFlagsToFalse()
      {
         Dictionary<string,bool> TempFlagDictionary = new Dictionary<string, bool>();
         foreach(KeyValuePair<string,bool> KVP in Flags)
         {
            TempFlagDictionary.Add(KVP.Key, false);
         }
         Flags = TempFlagDictionary;
      }
      
      public void SetAllFlagsToTrue()
      {
         Dictionary<string, bool> TempFlagDictionary = new Dictionary<string, bool>();
         foreach (KeyValuePair<string, bool> KVP in Flags)
         {
            TempFlagDictionary.Add(KVP.Key, true);
         }
         Flags = TempFlagDictionary;
      }

      public void ClearFlagCollection()
      {
         Flags = new Dictionary<string, bool>();
      }
   }
}
