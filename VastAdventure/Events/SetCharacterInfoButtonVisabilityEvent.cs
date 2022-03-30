using System;
using System.Collections.Generic;
using System.Text;
using Base.Events;

namespace VastAdventure.Events
{
   [Serializable]
   public class SetCharacterInfoButtonVisabilityEvent:Event
   {
      public bool isVisible;

      public SetCharacterInfoButtonVisabilityEvent()
      {

      }

      public SetCharacterInfoButtonVisabilityEvent(bool isVisible)
      {
         this.isVisible = isVisible;
      }
   }
}
