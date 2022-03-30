using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Model
{
   [Serializable]
   public class Option
   {
      public int OptionId { get; set; }
      public string OptionDescription { get; set; }
      public List<ActionNode> Actions { get; set; }
      //List of precheck actions
      public List<Action> PreChecks { get; set; }
   }
}
