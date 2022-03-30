using System;
using System.Collections.Generic;
using System.Text;
using Base.Events;
using VastAdventure.Component;
using VastAdventure.Model;

namespace VastAdventure.Events
{
   [Serializable]
   public class UpdateBattleHistoryEvent:Event
   {
      public string actionHistory { get; set; }
      public List<Character> CharactersToUpdate;

      public UpdateBattleHistoryEvent()
      {

      }

      public UpdateBattleHistoryEvent(string history, List<Character> characters)
      {
         this.actionHistory = history;
         this.CharactersToUpdate = characters;
      }

   }
}
