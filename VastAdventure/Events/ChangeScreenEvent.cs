using Base.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Events
{
   [Serializable]
   public class ChangeScreenEvent:Event
   {
      public string screenType { get; set; }
      public int id { get; set; }

      public ChangeScreenEvent() {}

      public ChangeScreenEvent(string type, int id)
      {
         this.screenType = type;
         this.id = id;
      }
   }
}
