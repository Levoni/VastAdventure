using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class GraveyardModel
   {
      public CharacterModel player;
      public List<CharacterModel> charactersInSquad;
      public SquadModel playerSquad;
      public Inventory inventory;
      public FlagCollection collection;

      public override string ToString()
      {
         string objectString = "Name: " + player.name + ", Level: " + player.level;
         if(collection.CheckFlag("escaped"))
         {
            objectString += " Escaped";
         }
         return objectString;
      }

      public GraveyardModel()
      {

      }

      public GraveyardModel(CharacterModel character)
      {
         player = character;
         charactersInSquad = new List<CharacterModel>();
         charactersInSquad.Add(player);
         playerSquad = new SquadModel(player.id, 5000, 0, TaticsType.random);
         inventory = new Inventory();
         inventory.items = new List<InventoryItemModel>();
         collection = new FlagCollection();
      }
   }
}
