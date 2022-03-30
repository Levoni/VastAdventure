using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Screens
{
   [Serializable]
   public class BasicScreen: Base.UI.GUI
   {
      public ScreenType ScreenType { get; set; }
   }
}
