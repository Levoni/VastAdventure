using Base.Entities;
using Base.UI;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Screens;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class ScreenNavigationStatusModel
   {
      public Entity currentScreenEntity;
      public Stack<BasicScreen> ScreenHistory;
   }
}
