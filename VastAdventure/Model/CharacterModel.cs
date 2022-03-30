using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class CharacterModel
   {
      public string name { get; set; }
      public int id { get; set; }
      public int level { get; set; }
      public int experience { get; set; }
      public int attiributePoints { get; set; }
      public int maxHealth { get; set; }
      public int strength { get; set; }
      public int toughness { get; set; }
      public int dexterity { get; set; }
      public bool hasDied { get; set; }
      public ClassType characterClass { get; set; }

      public CharacterModel()
      {
         name = "";
         id = -1;
         level = 0;
         experience = 0;
         attiributePoints = 0;
         maxHealth = 0;
         strength = 0;
         toughness = 0;
         dexterity = 0;
         hasDied = false;
         characterClass = ClassType.Fighter;
      }
      
   }
}
