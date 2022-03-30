using Base.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VastAdventure.Model;

namespace VastAdventure.Utility.Services
{
   public static class GraveyardService
   {
      public static  bool isInitialized;
      public static List<GraveyardModel> graveyard;

      public static void Initialize()
      {
         graveyard =  SavesService.GetObjectFromFile<List<GraveyardModel>>("Graveyard.graveyard");
         if(graveyard == null)
         {
            CreateDefaultGraveyard();
         }
       
         isInitialized = true;
      }

      public static List<GraveyardModel> GetGraveyard()
      {
         return graveyard;
      }

      public static void CreateDefaultGraveyard()
      {
         graveyard = new List<GraveyardModel>();
      }

      public static void AddCurrentPlayerToGraveyard()
      {
         GraveyardModel model = new GraveyardModel()
         {
            player = CharacterService.getPlayer(),
            charactersInSquad = new List<CharacterModel>(),
            playerSquad = SquadService.getAllySquad(),
            inventory = InventoryService.inventory,
            collection = FlagService.GetFlagCollection()
         };
         foreach(int id in model.playerSquad.characterIds)
         {
            if(id != 0)
            {
               model.charactersInSquad.Add(CharacterService.getCharacter(id));
            }
         }
         graveyard.Add(model);
         SavesService.SaveObjectToFile("Graveyard.graveyard",graveyard);
      }

      public static void ClearGraveyard()
      {
         SavesService.DeleteFile("Graveyard.graveyard");
         CreateDefaultGraveyard();
      }

   }
}
