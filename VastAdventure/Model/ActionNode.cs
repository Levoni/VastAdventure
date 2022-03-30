using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Model
{
   [Serializable]
   public class ActionNode
   {
      public int NodeId { get; set; }
      public int PassNextNode { get; set; }
      public int FailNextNode { get; set; }
      public Action Action { get; set; }
   }
}
