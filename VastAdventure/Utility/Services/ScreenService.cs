using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Base.Entities;
using Base.Scenes;
using Base.System;
using Base.UI;
using VastAdventure.Component;
using VastAdventure.DataProviders;
using VastAdventure.Events;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.System;
using VastAdventure.Utility;
//using VastAdventureAndriod.Screens;

namespace VastAdventure.Utility.Services
{
   //TODO: make a parent class for all the sceens (Maybe?)
   //TODO: Make a way to handle storing and using past screen history (stack, prev/cur screen, etc)
   public static class ScreenService
   {
      public static bool initialized = false;

      public static ScreenNavigationStatusModel navigationModel;

      //public static BasicScreen CurrentScreen;
      //public static BasicScreen PreviousScreen;
      //public static int currentScreenId;
      //public static Entity currentScreenEntity;
      //public static Stack<ChangeScreenCommand> ChangeScreeenCommands;

      static IScreenProvider screenProvider;



      //public static Dictionary<int, EventScreenModel> eventScreens = new Dictionary<int, EventScreenModel>();
      //public static Dictionary<int, LocationScreenModel> locationScreens = new Dictionary<int, LocationScreenModel>();
      //public static Dictionary<int, BattleScreenModel> battleScreens = new Dictionary<int, BattleScreenModel>();


      public static void Init(IScreenProvider provider)
      {
         initialized = true;
         screenProvider = provider;

         navigationModel = new ScreenNavigationStatusModel()
         {
            ScreenHistory = new Stack<BasicScreen>(),
            currentScreenEntity = null,
         };
      }

      public static EventScreenModel getEvent(int id)
      {
         if (initialized)
         {
            return screenProvider.getEvent(id);
         }
         return null;
      }

      public static LocationScreenModel getLocation(int id)
      {
         if (initialized)
         {
            return screenProvider.getLocation(id);
         }
         return null;
      }

      public static BattleScreenModel getBattle(int id)
      {
         if (initialized)
         {
            return screenProvider.getBattle(id);
         }
         return null;
      }

      //TODO: send visibility event with off to character info overlay
      //TODO: break off logic for each screen into functions or break off full chunch into its own method that takes a command and scene
      public static void replaceScreen(Scene scene, string changeScreenInfo)
      {
         string[] changeScreenParams = changeScreenInfo.Split(':');
         ChangeScreenCommand newChangeScreenCommand = new ChangeScreenCommand(changeScreenParams[0], changeScreenParams[1]);

         if (navigationModel.currentScreenEntity != null)
         {
            BasicScreen tempScreen = (BasicScreen)scene.GetComponent<GUI>(navigationModel.currentScreenEntity);
            navigationModel.ScreenHistory.Pop();
            navigationModel.ScreenHistory.Push(tempScreen);
            scene.DestroyEntity(navigationModel.currentScreenEntity);
         }

         navigationModel.currentScreenEntity = scene.CreateEntity();

         scene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(true));
         BasicScreen screen = new BasicScreen();

         if (newChangeScreenCommand.screenType == "event")
         {
            screen = screenProvider.CreateNewEventScreen(newChangeScreenCommand, scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
         }
         else if (newChangeScreenCommand.screenType == "location")
         {
            screen = screenProvider.CreateNewLocationScreen(newChangeScreenCommand, scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
         }
         else if (newChangeScreenCommand.screenType == "battle")
         {
            screen = screenProvider.CreateNewBattleScreen(newChangeScreenCommand, scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
            //scene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         }
         else if (newChangeScreenCommand.screenType == "characterInfo")
         {
            screen = screenProvider.CreateNewCharacterInformationScreen(newChangeScreenCommand, scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
            //scene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         }
         else if (newChangeScreenCommand.screenType == "map")
         {
            screen = screenProvider.CreateNewMapScreen(scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
            //scene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         }
         else if (newChangeScreenCommand.screenType == "squad")
         {
            screen = screenProvider.CreateNewSquadScreen(scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
         }
         else if(newChangeScreenCommand.screenType == "inventory")
         {
            screen = screenProvider.CreateNewInventoryScreen(scene);
            scene.AddComponent(navigationModel.currentScreenEntity, screen);
         }
         navigationModel.ScreenHistory.Push(screen);
         if(screen.ScreenType == ScreenType.Location)
         {
            FlagService.SetFlagValue("onLocationScreen", true);
         }
         else if(screen.ScreenType == ScreenType.Battle || screen.ScreenType == ScreenType.Event)
         {
            FlagService.SetFlagValue("onLocationScreen", false);
         }

         MaintainStackSize();

         SavesService.SaveGame(scene);
      }

      //TODO: clean up
      //TODO: have logic broken up into methods
      //TODO: track changes into current screen
      public static void replaceWithPreviousScreen(Scene scene)
      {
         navigationModel.ScreenHistory.Pop();
         BasicScreen previousScreen = navigationModel.ScreenHistory.Peek();
         previousScreen.Init(scene.bus, scene);
         previousScreen.onDeserialized();
         if (navigationModel.currentScreenEntity != null)
         {
            scene.DestroyEntity(navigationModel.currentScreenEntity);
         }
         navigationModel.currentScreenEntity = scene.CreateEntity();
         scene.AddComponent(navigationModel.currentScreenEntity, previousScreen);

         if (previousScreen.ScreenType == ScreenType.Location)
         {
            FlagService.SetFlagValue("onLocationScreen", true);
         }
         else if (previousScreen.ScreenType == ScreenType.Battle || previousScreen.ScreenType == ScreenType.Event)
         {
            FlagService.SetFlagValue("onLocationScreen", false);
         }

         SavesService.SaveGame(scene);
      }

      public static void replaceWithGeneratedBattle(Scene scene, string generateCommandValue)
      {
         if (navigationModel.currentScreenEntity != null)
         {
            scene.DestroyEntity(navigationModel.currentScreenEntity);
         }

         navigationModel.currentScreenEntity = scene.CreateEntity();

         BasicScreen screen = screenProvider.GenerateBattle(scene, generateCommandValue);
         scene.AddComponent(navigationModel.currentScreenEntity, screen);

         navigationModel.ScreenHistory.Push(screen);
         MaintainStackSize();

         FlagService.SetFlagValue("onLocationScreen", false);

         SavesService.SaveGame(scene);
      }

      public static void replaceWithChangeLocaitonScreen(Scene scene, List<ConnectedLocationModel> connectedLocations, string curLocation, int returnSceneId)
      {
         if (navigationModel.currentScreenEntity != null)
         {
            scene.DestroyEntity(navigationModel.currentScreenEntity);
         }

         ChangeScreenCommand changeScreenCommand = new ChangeScreenCommand("changeLocation", string.Empty);

         navigationModel.currentScreenEntity = scene.CreateEntity();

         BasicScreen screen = screenProvider.CreateNewChangeLocationScreen(scene, connectedLocations, curLocation, returnSceneId);
         scene.AddComponent(navigationModel.currentScreenEntity, screen);
         scene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         FlagService.SetFlagValue("onLocationScreen", false);
         navigationModel.ScreenHistory.Push(screen);
         MaintainStackSize();

         SavesService.SaveGame(scene);
      }

      public static void removeScreen(Scene scene)
      {
         if (navigationModel.currentScreenEntity != null)
         {
            scene.DestroyEntity(navigationModel.currentScreenEntity);

            navigationModel.currentScreenEntity = null;
         }
      }

      public static void SaveNavigationInformation()
      {
         SavesService.SaveObjectToFile("info.navigation", navigationModel);
      }

      public static void LoadnavigationInformation()
      {
         navigationModel = SavesService.GetObjectFromFile<ScreenNavigationStatusModel>("info.navigation");
         if (navigationModel != null)
         {
            foreach (var screen in navigationModel.ScreenHistory)
            {
               screen.onDeserialized();
            }
         }
      }

      //TODO: make stack into a linked list that has a max count of 3 (may have to manually remove last item if size is == 5)
      private static void MaintainStackSize()
      {
         if (navigationModel.ScreenHistory.Count > 3)
         {
            var tempStack = new Stack<BasicScreen>();
            tempStack.Push(navigationModel.ScreenHistory.Pop());
            tempStack.Push(navigationModel.ScreenHistory.Pop());
            tempStack.Push(navigationModel.ScreenHistory.Pop());
            navigationModel.ScreenHistory.Clear();
            navigationModel.ScreenHistory.Push(tempStack.Pop());
            navigationModel.ScreenHistory.Push(tempStack.Pop());
            navigationModel.ScreenHistory.Push(tempStack.Pop());
         }
      }
   }
}