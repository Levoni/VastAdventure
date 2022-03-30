using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class SquadModel
   {
      public List<int> characterIds { get; set; }
      public int teamId { get; set; }
      public int id { get; set; }
      public TaticsType tatic { get; set; }

      public SquadModel()
      {
         characterIds = new List<int>();
         teamId = -1;
         id = -1;
         tatic = TaticsType.random;
      }

      public SquadModel(List<int> characterIds, int id, int team, TaticsType taticsType)
      {
         this.characterIds = characterIds;
         this.id = id;
         this.teamId = team;
         this.tatic = taticsType;
      }

      public SquadModel(int characterId, int id, int team, TaticsType taticsType)
      {
         this.characterIds = new List<int>();
         characterIds.Add(characterId);
         this.id = id;
         this.teamId = team;
         this.tatic = taticsType;
      }
   }
}
