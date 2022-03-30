using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class BattleScreenModel
   {
      public int battleId;
      public ScreenType screenType;
      public string locationName;
      public int enemySquadId;
      public bool canFlee;
      public Action WinNextSceenAction;
      public Action LoseNextSceenAction;
      public Action FleeScreenAction;
   }
}
