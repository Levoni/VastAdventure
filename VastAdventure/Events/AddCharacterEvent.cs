using System;
using System.Collections.Generic;
using System.Text;
using Base.Events;

namespace VastAdventure.Events
{
   public class AddCharacterEvent:Event
   {
      public int CharacterId;

      public AddCharacterEvent()
      {

      }

      public AddCharacterEvent(int id)
      {
         this.CharacterId = id;
      }
   }
}
