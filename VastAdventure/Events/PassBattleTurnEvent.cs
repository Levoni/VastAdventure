using System;
using System.Collections.Generic;
using System.Text;
using Base.Entities;
using Base.Events;

namespace VastAdventure.Events
{
   [Serializable]
   public class PassBattleTurnEvent:Event
   {
      public int characterId { get; set; }
      public PassBattleTurnEvent()
      {

      }

      public PassBattleTurnEvent(int characterId)
      {
         this.characterId = characterId;
      }
   }
}