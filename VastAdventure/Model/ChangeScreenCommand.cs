using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VastAdventure.Utility
{
   [Serializable]
   public class ChangeScreenCommand
   {
      public string screenType;
      public string screenId;

      public ChangeScreenCommand()
      {
         this.screenType = string.Empty;
         this.screenId = string.Empty;
      }

      public ChangeScreenCommand(string screenType, string screenId)
      {
         this.screenType = screenType;
         this.screenId = screenId;
      }
   }
}