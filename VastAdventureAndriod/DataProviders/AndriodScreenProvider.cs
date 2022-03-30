using Base.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Entities;
using VastAdventure.Component;
using VastAdventure.DataProviders;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility;
using VastAdventure.Utility.Services;
using VastAdventureAndriod.Screens;
using VastAdventure.Events;

namespace VastAdventureAndriod.DataProviders
{
   public class AndriodScreenProvider : IScreenProvider
   {

      public static Dictionary<int, EventScreenModel> eventScreens = new Dictionary<int, EventScreenModel>();
      public static Dictionary<int, LocationScreenModel> locationScreens = new Dictionary<int, LocationScreenModel>();
      public static Dictionary<int, BattleScreenModel> battleScreens = new Dictionary<int, BattleScreenModel>();


      public AndriodScreenProvider()
      {
         eventScreens = new Dictionary<int, EventScreenModel>();
         List<EventScreenModel> eScreens = new List<EventScreenModel>();
         eScreens = AssetLoaderService.GetXmlSerializedAsset<List<EventScreenModel>>("events.screen", "screens/");
         foreach (EventScreenModel s in eScreens)
         {
            eventScreens.Add(s.EventId, s);
         }

         locationScreens = new Dictionary<int, LocationScreenModel>();
         List<LocationScreenModel> lScreens = new List<LocationScreenModel>();
         lScreens = AssetLoaderService.GetXmlSerializedAsset<List<LocationScreenModel>>("locations.screen", "screens/");
         foreach (LocationScreenModel s in lScreens)
         {
            locationScreens.Add(s.locationId, s);
         }

         battleScreens = new Dictionary<int, BattleScreenModel>();
         List<BattleScreenModel> BScreens = new List<BattleScreenModel>();
         BScreens = AssetLoaderService.GetXmlSerializedAsset<List<BattleScreenModel>>("battles.screen", "screens/");
         foreach (BattleScreenModel s in BScreens)
         {
            battleScreens.Add(s.battleId, s);
         }
      }

      public BasicScreen CreateNewEventScreen(ChangeScreenCommand screenCommand, Scene scene)
      {
         if (ConfigService.config.UIType == "dark theme")
         {
            NewEventScreen screen = new NewEventScreen();
            screen.Init(scene.bus, scene);
            screen.Init(int.Parse(screenCommand.screenId));
            return screen;
         }
         else
         {
            EventScreen screen = new EventScreen();
            screen.Init(scene.bus, scene);
            screen.Init(int.Parse(screenCommand.screenId));
            return screen;
         }
      }

      public BasicScreen CreateNewLocationScreen(ChangeScreenCommand screenCommand, Scene scene)
      {
         if (ConfigService.config.UIType == "dark theme")
         {
            NewLocationScreen screen = new NewLocationScreen();
            screen.Init(scene.bus, scene);
            screen.Init(int.Parse(screenCommand.screenId));
            return screen;
         }
         else
         {
            LocationScreen screen = new LocationScreen();
            screen.Init(scene.bus, scene);
            screen.Init(int.Parse(screenCommand.screenId));
            return screen;
         }
      }

      public BasicScreen CreateNewBattleScreen(ChangeScreenCommand screenCommand, Scene scene)
      {
         if (ConfigService.config.UIType == "dark theme")
         {
            NewBattleScreen screen = new NewBattleScreen();

            BattleScreenModel battleScreenModel = getBattle(int.Parse(screenCommand.screenId));

            //Create enemy squad
            SquadModel enemySquadModel = SquadService.GetSquad(battleScreenModel.enemySquadId);
            Squad enemySquad = new Squad();
            enemySquad.model = enemySquadModel;
            foreach (int CharacterId in enemySquadModel.characterIds)
            {
               CharacterModel cm = CharacterService.getCharacter(CharacterId);
               enemySquad.characters.Add(new Character(cm, enemySquad.model.teamId, TaticsType.random));
            }

            //Create ally squad info
            SquadModel allySquadModel = SquadService.getAllySquad();
            Squad allySquad = new Squad();
            allySquad.model = allySquadModel;
            foreach (int CharacterId in allySquadModel.characterIds)
            {
               CharacterModel cm = CharacterService.getCharacter(CharacterId);
               allySquad.characters.Add(new Character(cm, allySquad.model.teamId, allySquad.model.tatic));
            }

            Entity allyEntity = scene.CreateEntity();
            Entity enemyEntity = scene.CreateEntity();
            scene.AddComponent(allyEntity, allySquad);
            scene.AddComponent(enemyEntity, enemySquad);

            screen.enemySquad = enemySquadModel;
            screen.allySquad = allySquadModel;
            screen.Init(scene.bus, scene);
            screen.Init(int.Parse(screenCommand.screenId));
            scene.bus.Publish(null, new UpdateBattleHistoryEvent($"You encounter an enemy", new List<Character>()));
            return screen;
         }
         else
         {
            BattleScreen screen = new BattleScreen();
            BattleScreenModel battleScreenModel = getBattle(int.Parse(screenCommand.screenId));

            //Create enemy squad
            SquadModel enemySquadModel = SquadService.GetSquad(battleScreenModel.enemySquadId);
            Squad enemySquad = new Squad();
            enemySquad.model = enemySquadModel;
            foreach (int CharacterId in enemySquadModel.characterIds)
            {
               CharacterModel cm = CharacterService.getCharacter(CharacterId);
               enemySquad.characters.Add(new Character(cm, enemySquad.model.teamId, TaticsType.random));
            }

            //Create ally squad info
            SquadModel allySquadModel = SquadService.getAllySquad();
            Squad allySquad = new Squad();
            allySquad.model = allySquadModel;
            foreach (int CharacterId in allySquadModel.characterIds)
            {
               CharacterModel cm = CharacterService.getCharacter(CharacterId);
               allySquad.characters.Add(new Character(cm, allySquad.model.teamId, allySquad.model.tatic));
            }

            Entity allyEntity = scene.CreateEntity();
            Entity enemyEntity = scene.CreateEntity();
            scene.AddComponent(allyEntity, allySquad);
            scene.AddComponent(enemyEntity, enemySquad);

            screen.enemySquad = enemySquadModel;
            screen.allySquad = allySquadModel;
            screen.Init(scene.bus, scene);
            screen.Init(int.Parse(screenCommand.screenId));
            scene.bus.Publish(null, new UpdateBattleHistoryEvent($"You encounter an enemy", new List<Character>()));
            return screen;
         }

      }

      public BasicScreen GenerateBattle(Scene scene, string generateCommandValue)
      {

         string[] parsedValue = generateCommandValue.Split(':');
         string[] classTypeStrings = parsedValue[0].Split(',');
         string levelOffset = parsedValue[1];
         string enemyCount = parsedValue[2];
         string location = parsedValue[3];
         string winNextScreenType = parsedValue[4];
         string winNextScreenId = parsedValue[5];
         string loseNextScreenType = parsedValue[6];
         string loseNextScreenId = parsedValue[7];
         string canFlee = parsedValue[8];
         string fleeNextScreenType = parsedValue[9];
         string fleeNextScreenId = parsedValue[10];

         List<ClassType> classTypeList = new List<ClassType>();

         foreach (string classType in classTypeStrings)
         {
            if (Enum.TryParse(classType, out ClassType cType))
            {
               classTypeList.Add(cType);
            }
         }

         //Determine Enemy level
         SquadModel allySquadModel = SquadService.getAllySquad();
         int calculatedEnemyLevel = 0;
         int calculatedExtraLevels = 0;
         int totalAllySquadLevel = 0;
         int.TryParse(levelOffset, out int levelOffsetInt);
         foreach (var characterId in allySquadModel.characterIds)
         {
            totalAllySquadLevel += CharacterService.getCharacter(characterId).level;
         }
         calculatedEnemyLevel = (totalAllySquadLevel / allySquadModel.characterIds.Count) + levelOffsetInt;
         calculatedExtraLevels = totalAllySquadLevel % allySquadModel.characterIds.Count;

         int.TryParse(enemyCount, out int enemyIntCount);
         if (enemyIntCount == 0)
         {
            enemyIntCount = SquadService.getAllySquad().characterIds.Count;
         }

         SquadModel generatedSquad = SquadService.GenerateSquad(classTypeList, calculatedEnemyLevel, enemyIntCount, calculatedExtraLevels);

         BattleScreenModel model = new BattleScreenModel();
         model.battleId = 1000;
         try
         {
            model.canFlee = bool.Parse(canFlee);
         }
         catch
         {
            model.canFlee = false;
         }
         model.screenType = ScreenType.Battle;
         model.enemySquadId = generatedSquad.id;

         model.LoseNextSceenAction = new VastAdventure.Model.Action(ActionType.ChangeScreen, loseNextScreenType + ':' + loseNextScreenId);
         model.WinNextSceenAction = new VastAdventure.Model.Action(ActionType.ChangeScreen, winNextScreenType + ':' + winNextScreenId);
         model.FleeScreenAction = new VastAdventure.Model.Action(ActionType.ChangeScreen, fleeNextScreenType + ':' + fleeNextScreenId);
         model.locationName = location;

         //Create enemy squad
         Squad enemySquad = new Squad();
         enemySquad.model = generatedSquad;
         foreach (int CharacterId in generatedSquad.characterIds)
         {
            CharacterModel cm = CharacterService.getCharacter(CharacterId);
            enemySquad.characters.Add(new Character(cm, enemySquad.model.teamId, TaticsType.random));
         }

         //Get ally squad info
         Squad allySquad = new Squad();
         allySquad.model = allySquadModel;
         foreach (int CharacterId in allySquadModel.characterIds)
         {
            CharacterModel cm = CharacterService.getCharacter(CharacterId);
            allySquad.characters.Add(new Character(cm, allySquad.model.teamId, allySquad.model.tatic));
         }

         Entity allyEntity = scene.CreateEntity();
         Entity enemyEntity = scene.CreateEntity();
         scene.AddComponent(allyEntity, allySquad);
         scene.AddComponent(enemyEntity, enemySquad);

         if (ConfigService.config.UIType == "dark theme")
         {
            NewBattleScreen screen = new NewBattleScreen();
            screen.enemySquad = generatedSquad;
            screen.allySquad = allySquadModel;
            screen.Init(scene.bus, scene);
            screen.Init(model);
            scene.bus.Publish(null, new UpdateBattleHistoryEvent($"You encounter an enemy", new List<Character>()));
            return screen;
         }
         else
         {
            BattleScreen screen = new BattleScreen();
            screen.enemySquad = generatedSquad;
            screen.allySquad = allySquadModel;
            screen.Init(scene.bus, scene);
            screen.Init(model);
            scene.bus.Publish(null, new UpdateBattleHistoryEvent($"You encounter an enemy", new List<Character>()));
            return screen;
         }
      }

      public BasicScreen CreateNewCharacterInformationScreen(ChangeScreenCommand screenCommand, Scene scene)
      {
         if (ConfigService.config.UIType == "dark theme")
         {
            NewCharacterInfoScreen screen = new NewCharacterInfoScreen();
            screen.Init(scene.bus, scene);
            screen.Init();
            return screen;
         }
         else
         {
            CharacterInformationScreen screen = new CharacterInformationScreen();
            screen.Init(scene.bus, scene);
            screen.Init();
            return screen;
         }
      }

      public BasicScreen CreateNewMapScreen(Scene scene)
      {
         if (ConfigService.config.UIType == "dark theme")
         {
            NewMapScreen screen = new NewMapScreen();
            screen.Init(scene.bus, scene);
            screen.Init();
            return screen;
         }
         else
         {
            MapScreen screen = new MapScreen();
            screen.Init(scene.bus, scene);
            screen.Init();
            return screen;
         }
      }

      public BasicScreen CreateNewChangeLocationScreen(Scene scene, List<ConnectedLocationModel> connectedLocations, string curLocation, int returnSceneId)
      {
         if (ConfigService.config.UIType == "dark theme")
         {
            NewChangeLocationScreen screen = new NewChangeLocationScreen();
            screen.Init(scene.bus, scene);
            screen.Init(connectedLocations, curLocation, returnSceneId);
            return screen;
         }
         else
         {
            ChangeLocationScreen screen = new ChangeLocationScreen();
            screen.Init(scene.bus, scene);
            screen.Init(connectedLocations, curLocation, returnSceneId);
            return screen;
         }
      }

      public BasicScreen CreateNewSquadScreen(Scene scene)
      {
         newSquadScreen screen = new newSquadScreen();
         screen.Init(scene.bus, scene);
         screen.Init();
         return screen;
      }

      public BasicScreen CreateNewInventoryScreen(Scene scene)
      {
         NewInventoryScreen screen = new NewInventoryScreen();
         screen.Init(scene.bus, scene);
         screen.Init();
         return screen;
      }

      public EventScreenModel getEvent(int id)
      {
         if (eventScreens.ContainsKey(id))
            return eventScreens[id];
         else
            return null;
      }

      public LocationScreenModel getLocation(int id)
      {
         if (locationScreens.ContainsKey(id))
            return locationScreens[id];
         else
            return null;
      }

      public BattleScreenModel getBattle(int id)
      {
         if (battleScreens.ContainsKey(id))
            return battleScreens[id];
         else
            return null;
      }
   }
}