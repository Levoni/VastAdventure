using Base.Components;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Model;
using VastAdventure.Utility;

namespace VastAdventure.Component
{
   [Serializable]
   public class Character : Component<Character>
   {
      public CharacterModel model {get; set;}
      public int health { get; set; }
      public int team { get; set; }
      public int ticksTillNextAttack { get; set; }
      public TaticsType tatic { get; set; }

      public Character()
      {

      }

      public  Character(CharacterModel model, int team, TaticsType taticsType)
      {
         this.model = model;
         this.health = model.maxHealth;
         this.team = team;
         this.ticksTillNextAttack = 100 - ((model.dexterity + 1) / 2);
         this.tatic = taticsType;
      }
   }
}
