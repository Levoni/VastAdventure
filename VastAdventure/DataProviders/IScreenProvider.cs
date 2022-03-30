using Base.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility;

namespace VastAdventure.DataProviders
{
   public interface IScreenProvider
   {
      BasicScreen CreateNewEventScreen(ChangeScreenCommand screenCommand, Scene scene);

      BasicScreen CreateNewLocationScreen(ChangeScreenCommand screenCommand, Scene scene);

      BasicScreen CreateNewBattleScreen(ChangeScreenCommand screenCommand, Scene scene);

      BasicScreen CreateNewCharacterInformationScreen(ChangeScreenCommand screenCommand, Scene scene);

      BasicScreen CreateNewChangeLocationScreen(Scene scene, List<ConnectedLocationModel> connectedLocations, string curLocation, int returnSceneId);

      BasicScreen CreateNewMapScreen(Scene scene);

      BasicScreen CreateNewSquadScreen(Scene scene);

      BasicScreen CreateNewInventoryScreen(Scene scene);

      BasicScreen GenerateBattle(Scene scene, string generateCommandValue);

      EventScreenModel getEvent(int id);

      LocationScreenModel getLocation(int id);


      BattleScreenModel getBattle(int id);

   }
}
