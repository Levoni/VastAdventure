using System;
using System.Collections.Generic;
using System.Text;
using Base.Entities;
using Base.Events;

namespace VastAdventure.Events
{
   [Serializable]
   public class BattleOverEvent:Event
   {
      public int winningTeam { get; set; }

      public BattleOverEvent()
      {

      }

      public BattleOverEvent(int winningTeam)
      {
         this.winningTeam = winningTeam;
      }
   }
}
