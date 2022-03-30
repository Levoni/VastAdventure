using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class CharacterClassModel
   {
      public ClassType classType;
      public string readableClassName;
      public int MaxHealthLvlUpIncrease;
      public int StrengthLvlUpIncrease;
      public int ToughnessLvlUpIncrease;
      public int DexterityLvlUpIncrease;

      public int MaxHealthStartingAmount;
      public int StrengthStartingAmount;
      public int ToughnessStartingAmount;
      public int DexterityStartingAmount;
   }
}
