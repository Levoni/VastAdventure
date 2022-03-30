using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class Action
   {
      public ActionType type { get; set; }
      public string value { get; set; }


      public Action(ActionType type, string value)
      {
         this.type = type;
         this.value = value;
      }

      public Action()
      {
      }
   }
}
