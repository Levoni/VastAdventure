using Base.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Events
{
   [Serializable]
   public class FlagSetEvent:Event
   {
      public string flag { get; set; }
      public bool value { get; set; }

      public FlagSetEvent()
      {

      }

      public FlagSetEvent(string flagSet, bool valueSetTo)
      {
         this.flag = flagSet;
         this.value = valueSetTo;
      }
   }
}
