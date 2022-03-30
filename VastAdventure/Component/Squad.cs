using Base.Components;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Model;

namespace VastAdventure.Component
{
   [Serializable]
   public class Squad : Component<Squad>
   {
      public SquadModel model { get; set; }
      public List<Character> characters { get; set; }

      public Squad()
      {
         this.characters = new List<Character>();
      }

      public Squad(SquadModel squad, List<Character> characters)
      {
         this.model = squad;
         this.characters = characters;
      }
   }
}
