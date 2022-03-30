using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Model;

namespace VastAdventure.Utility.Services
{
   public static class InventoryService
   {
      public static bool isInitialized;
      public static Inventory inventory;

      public static void Initialize()
      {
         LoadDefaultInventory();
         isInitialized = true;
      }

      public static void LoadDefaultInventory()
      {
         Inventory i = new Inventory();
         i.Gold = 0;
         i.items = new List<InventoryItemModel>();
         inventory = i;
      }

      public static void saveInventory()
      {
         SavesService.SaveObjectToFile("Inventory.inventory", inventory);
      }

      public static void LoadInventory()
      {
         inventory = SavesService.GetObjectFromFile<Inventory>("Inventory.inventory");
      }
      public static void AddItemToInventory(InventoryItemModel item)
      {
         inventory.items.Add(item);
      }
      public static void RemoveItemToInventory(InventoryItemModel item)
      {
         inventory.items.Remove(item);
      }
      public static void RemoveItemToInventory(string Name)
      {
         int index = inventory.items.FindIndex(item => item.itemName == Name);
         if(index != -1)
         {
            inventory.items.RemoveAt(index);
         }
      }
      public static void ChangeMoneyAmount(int amount)
      {
         inventory.Gold += amount;
      }
      public static bool HaveItem(string itemName)
      {
         int index = inventory.items.FindIndex(item => item.itemName == itemName);
         if (index != -1)
         {
            return true;
         }
         return false;
      }
      public static int GetGoldTotal()
      {
         return inventory.Gold;
      }
   }
}
