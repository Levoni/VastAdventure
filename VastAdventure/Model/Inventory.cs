using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Model
{
   [Serializable]
   public class Inventory
   {
      public List<InventoryItemModel> items;
      public int Gold;
   }
}
