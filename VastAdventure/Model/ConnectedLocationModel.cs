using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Model
{
   [Serializable]
   public class ConnectedLocationModel
   {
      public int LocationId { get; set; }
      public string connectionDescription { get; set; }
      public List<ActionNode> Actions { get; set; }
      public List<Action> PreChecks { get; set; }
   }
}