using Base.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Events
{
   [Serializable]
   public class CharacterDiedEvent:Event
   {
      public int characterWhoDiedId { get; set; }

      public CharacterDiedEvent()
      {

      }

      public CharacterDiedEvent(int id)
      {
         this.characterWhoDiedId = id;
      }
   }
}
