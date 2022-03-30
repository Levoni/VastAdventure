using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VastAdventure.Utility
{
   [Serializable]
   public enum ActionType
   {
      ChangeScreen,                 //screentype : screen ID
      ChangeLocationScreen,
      PreviousScreen,               //None
      CheckRequirement,             //Requirement type : thing to check : expected value(minimum or id or bool)
      SetRequirement,               //Requirement type : thing to set : value to set to
      CheckRequirementWithChance,   //requirement type: thing to check : minimum value : percent decrease per point off
      CheckRandomChance,            //percent chance out of 100 percent
      ChangeCharacterInfo,          //CharacterId : InfoType:  how to change : What to change : changeValue
      GenerateRandomEncounter,      //classtypes ',' seperated : levelOffset : number of enemies (0 means equal to squad amount): locationName : winNextScreenType : winNextScreenId : loseNextScreenType: loseNextScreenId : canFlee : fleeNextScreenType : fleeNextScreenId
      AddCharacterToSquad,          //characterId to add
      RemoveCharacterFromSquad,     //characterId to add
      AddItem,                      //item description: item imageReference
      RemoveItem,                   //item description
      ChangeMoney,                  //Amount
      RemoveOption,
      custom,
      changeText,
      EndGame
   }
   [Serializable]
   public enum ScreenType
   {
      Event,
      Battle,
      Location,
      Map,
      ChangeLocation,
      CharacterInformation,
      MainMenu,
      Squad
   }
   [Serializable]
   public enum ClassType
   {
      Fighter,
      Noble,
      Brawler,
      Thief,
      Guard,
      GuardCaptain,
      DogOfTheDark,
      FrailHuman,
      Cultist,
      Tessa,
      Fredrick,
      Roland,
      Demon
   }
   [Serializable]
   public enum TaticsType
   { 
      random,
      LeastHealth,
      Strongest,
      Fastest,
      MostVaunerable,
   }
}
