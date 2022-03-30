using System;
using System.Collections.Generic;
using System.Text;
using Base.Entities;
using Base.Events;

namespace VastAdventure.Events
{
   [Serializable]
   public class BasicAttackEvent:Event
   {
      public int defenderId { get; set; }
      public int attackerId { get; set; }

      public BasicAttackEvent(int attackerId, int defenderId)
      {
         this.defenderId = defenderId;
         this.attackerId = attackerId;
      }
   }
}
