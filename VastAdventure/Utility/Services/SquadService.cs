using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VastAdventure.Model;
using VastAdventure.Utility.Services;

namespace VastAdventure.Utility.Services
{
   public static class SquadService
   {
      private static SquadModel AllySquad;

      public static Dictionary<int, SquadModel> baseSquads;

      private static bool initialized = false;

      private static int nextAvailableSquadId;

      public static void Initialize()
      {
         initialized = true;
         baseSquads = new Dictionary<int, SquadModel>();

         List<SquadModel> squads = AssetLoaderService.GetXmlSerializedAsset<List<SquadModel>>("squads.squad", "squads/");
         foreach (SquadModel model in squads)
         {
            baseSquads.Add(model.id, model);
            if (model.id >= nextAvailableSquadId)
               nextAvailableSquadId = model.id + 1;
         }


      }

      public static void GenerateAndAddNewPlayerSquad()
      {
         AllySquad = new SquadModel(new List<int>() { 0 }, 0, 0, TaticsType.random);
         baseSquads.Add(0, AllySquad);
      }

      public static void SavePlayerSquadToFile()
      {
         SavesService.SaveObjectToFile("all.squad", baseSquads);
      }

      public static SquadModel GenerateSquad(List<ClassType> possibleTypes, int levelBase, int enemyAmountBase, int extraLevelAmount)
      {
         //TODO: add level modifier and enemy modifier to generation
         if (possibleTypes.Count() > 0)
         {
            Random rand = new Random();

            //int enemyModifier = (rand.Next() % 3) - 1;

            SquadModel newSquad = new SquadModel();
            newSquad.id = nextAvailableSquadId;
            nextAvailableSquadId++;
            newSquad.teamId = 1;

            for (int i = 0; i < enemyAmountBase; i++)
            {
               int typeIndex = rand.Next() % possibleTypes.Count();

               CharacterModel newCharacter = null;
               if (extraLevelAmount > 0)
               {
                  newCharacter = CharacterService.GenerateNewCharacter(possibleTypes[typeIndex], levelBase + 1);
                  extraLevelAmount--;
               }
               else
               {
                  newCharacter = CharacterService.GenerateNewCharacter(possibleTypes[typeIndex], levelBase);
               }
               newSquad.characterIds.Add(newCharacter.id);
            }
            return newSquad;
         }
         return null;
      }

      public static SquadModel GenerateSquad(List<int> charactersInSquad, int squadTeamId)
      {
         SquadModel newSquad = new SquadModel();
         newSquad.teamId = squadTeamId;

         foreach(int i in charactersInSquad)
         {
            newSquad.characterIds.Add(i);
         }
         newSquad.id = nextAvailableSquadId;
         nextAvailableSquadId++;
         return newSquad;
      }

      public static void LoadPlayerSquadFromSave()
      {
         baseSquads = SavesService.GetObjectFromFile<Dictionary<int,SquadModel>>("all.squad");

         foreach (KeyValuePair<int,SquadModel> KVP in baseSquads)
         {
            if(KVP.Value.id == 0)
            {
               AllySquad = KVP.Value;
            }
            if (KVP.Value.id >= nextAvailableSquadId)
               nextAvailableSquadId = KVP.Value.id + 1;
         }
      }

      public static SquadModel GetSquad(int id)
      {
         if (baseSquads.ContainsKey(id))
         {
            return baseSquads[id];
         }
         return null;
      }

      public static SquadModel getAllySquad()
      {
         return AllySquad;
      }
   }
}