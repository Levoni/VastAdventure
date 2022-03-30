using Base.Events;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Events
{
   [Serializable]
   public class InventoryEvent:Event
   {
      public ActionType actionType { get; set; }
      public string actionValue { get; set; }

      public InventoryEvent() {}
      public InventoryEvent(ActionType type, string value)
      {
         this.actionType = type;
         this.actionValue = value;
      }
   }
}
