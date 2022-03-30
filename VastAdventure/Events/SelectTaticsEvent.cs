using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Events
{
   [Serializable]
   public class SelectTaticsEvent
   {
      public TaticsType Tatic { get; set; }
      public int SquadId { get; set; }

      public SelectTaticsEvent()
      {

      }
      
      public SelectTaticsEvent(TaticsType taticType, int SquadNumber)
      {
         this.Tatic = taticType;
         this.SquadId = SquadNumber;
      }
   }
}
