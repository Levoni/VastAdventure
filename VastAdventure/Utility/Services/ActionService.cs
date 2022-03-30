//Testing: comment out below line whenn running program
//#undef ANDROID
//#define LINUX
using Base.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.DataProviders;
using VastAdventure.Events;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility;
using Action = VastAdventure.Model.Action;

namespace VastAdventure.Utility.Services
{
   public static class ActionService
   {
      private static Random rand = new Random();
      public static bool HandleAction(VastAdventure.Model.Action action, Scene parentScene)
      {
         if (action.type == VastAdventure.Utility.ActionType.ChangeScreen)
         {
            VastAdventure.Utility.Services.ScreenService.replaceScreen(parentScene, action.value);
            parentScene.bus.Publish(null, new ChangeScreenEvent(action.value.Split(':')[0], int.Parse(action.value.Split(':')[1])));
         }
         else if (action.type == VastAdventure.Utility.ActionType.EndGame)
         {

#if ANDROID
            //Send Character to gravetyardd
            GraveyardService.AddCurrentPlayerToGraveyard();

            //Reset Services
            ScreenService.Init(new VastAdventureAndriod.DataProviders.AndriodScreenProvider());
            CharacterService.Initialize();
            SquadService.Initialize();
            ScreenService.navigationModel.ScreenHistory = new Stack<BasicScreen>();

            //Clear GUI
            Base.System.GUISystem guiSystem = parentScene.GetSystem<Base.System.GUISystem>();
            guiSystem.ClearRegisteredEntities();
            VastAdventureAndriod.VastAdventureUI.CharacterInfoOverlay cOverlay = new VastAdventureAndriod.VastAdventureUI.CharacterInfoOverlay();
            cOverlay.Init();
            parentScene.AddComponent(parentScene.CreateEntity(), cOverlay);

            SavesService.ClearSave();

            //Setup Main Menu
            VastAdventureAndriod.Screens.MainMenu mm = new VastAdventureAndriod.Screens.MainMenu();
            Base.Entities.Entity entity = parentScene.CreateEntity();
            mm.Init(parentScene.bus, parentScene);
            mm.Init(entity);
            parentScene.AddComponent(entity, mm);
#elif LINUX
            //Send Character to gravetyardd
            GraveyardService.AddCurrentPlayerToGraveyard();

            //Reset Services
            ScreenService.Init((IScreenProvider)new VastAdventureDesktop.DataProviders.DesktopScreenProvider());
            CharacterService.Initialize();
            SquadService.Initialize();
            ScreenService.navigationModel.ScreenHistory = new Stack<BasicScreen>();

            //Clear GUI
            Base.System.GUISystem guiSystem = parentScene.GetSystem<Base.System.GUISystem>();
            guiSystem.ClearRegisteredEntities();
            VastAdventureDesktop.VastAdventureUI.CharacterInfoOverlay cOverlay = new VastAdventureDesktop.VastAdventureUI.CharacterInfoOverlay();
            cOverlay.Init();
            parentScene.AddComponent(parentScene.CreateEntity(), cOverlay);


            SavesService.ClearSave();

            //Setup Main Menu
            VastAdventureDesktop.Screens.MainMenu mm = new VastAdventureDesktop.Screens.MainMenu();
            Base.Entities.Entity entity = parentScene.CreateEntity();
            mm.Init(parentScene.bus, parentScene);
            mm.Init(entity);
            parentScene.AddComponent(entity, mm);
#endif
         }
         else if (action.type == VastAdventure.Utility.ActionType.GenerateRandomEncounter)
         {
            ScreenService.replaceWithGeneratedBattle(parentScene, action.value);
         }
         else if (action.type == VastAdventure.Utility.ActionType.ChangeCharacterInfo)
         {
            string[] commands = action.value.Split(':');
            int characterId = int.Parse(commands[0]);
            if (commands[1] == "stat")
            {
               int amountChange = int.Parse(commands[4]);
               if (commands[3] == "decrease")
               {
                  amountChange *= -1;
               }
               CharacterService.ChangeCharacterStat(commands[2], characterId, amountChange);
            }
            else if (commands[1] == "nonStat")
            {
               if (commands[3] == "hasDied")
               {
                  bool hasDied = bool.Parse(commands[4]);
                  CharacterService.getCharacter(characterId).hasDied = hasDied;
               }
            }
            else if(commands[1] == "class")
            {
               ClassType type = (ClassType)Enum.Parse(typeof(ClassType), commands[2], true);
               CharacterService.getCharacter(characterId).characterClass = type;
            }
         }
         else if (action.type == ActionType.CheckRandomChance)
         {
            int randomNumber = rand.Next() % 100;
            return int.Parse(action.value) >= randomNumber;
         }
         else if (ActionType.CheckRequirement == action.type)
         {
            var parameters = action.value.Split(':');
            if (parameters[0] == "flag")
            {
               if (bool.TryParse(parameters[2], out bool flagResult))
               {
                  return FlagService.CheckFlagValue(parameters[1]) == flagResult;
               }
               else
               {
                  return false;
               }
            }
            else if (parameters[0] == "squad")
            {
               if (parameters[1] == "hasCharacter")
               {
                  return SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
               }
               else if (parameters[1] == "doesNotHaveCharacter")
               {
                  return !SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
               }
               else if (parameters[1] == "size")
               {
                  return SquadService.getAllySquad().characterIds.Count >= int.Parse(parameters[2]);
               }
               else if (parameters[1] == "minAverageLevel")
               {
                  int level = 0;
                  foreach(var character in SquadService.getAllySquad().characterIds)
                  {
                     level += CharacterService.getCharacter(character).level;
                  }
                  level = level / SquadService.getAllySquad().characterIds.Count;
                  return level >= int.Parse(parameters[2]);
               }
            }
            else if (parameters[0] == "character")
            {
               if (parameters[1] == "hasDied")
               {
                  int characterId = int.Parse(parameters[2]);
                  return CharacterService.getCharacter(characterId).hasDied;
               }
               if (parameters[1] == "isAlive")
               {
                  int characterId = int.Parse(parameters[2]);
                  return !CharacterService.getCharacter(characterId).hasDied;
               }
            }
            else if (parameters[0] == "stat")
            {
               int expectedMinimum = int.Parse(parameters[2]);
               int statValue = 0;
               if (parameters[1] == "maxHealth")
               {
                  statValue = CharacterService.getPlayer().maxHealth;
               }
               else if (parameters[1] == "strength")
               {
                  statValue = CharacterService.getPlayer().strength;
               }
               else if (parameters[1] == "toughness")
               {
                  statValue = CharacterService.getPlayer().toughness;
               }
               else if (parameters[1] == "dexterity")
               {
                  statValue = CharacterService.getPlayer().dexterity;
               }
               else if(parameters[1] == "level")
               {
                  statValue = CharacterService.getPlayer().level;
               }
               return statValue >= expectedMinimum;
            }
            else if (parameters[0] == "inventory")
            {
               if (parameters[1] == "haveItem")
               {
                  string itemDescription = parameters[2];
                  return InventoryService.HaveItem(itemDescription);
               }
               else if (parameters[1] == "haveMoney")
               {
                  int expectedMinimum = int.Parse(parameters[2]);
                  return InventoryService.GetGoldTotal() >= expectedMinimum;
               }
            }
         }
         else if (ActionType.CheckRequirementWithChance == action.type)
         {
            var parameters = action.value.Split(':');
            if (parameters[0] == "stat")
            {
               int minimumValue = int.Parse(parameters[2]);
               int percentChangePerPoint = int.Parse(parameters[3]);
               int randomValue = rand.Next() % 100;
               int statValue = 0;
               if (parameters[1] == "maxHealth")
               {
                  statValue = CharacterService.getPlayer().maxHealth;
               }
               else if (parameters[1] == "strength")
               {
                  statValue = CharacterService.getPlayer().strength;
               }
               else if (parameters[1] == "toughness")
               {
                  statValue = CharacterService.getPlayer().toughness;
               }
               else if (parameters[1] == "dexterity")
               {
                  statValue = CharacterService.getPlayer().dexterity;
               }
               int calculatedChanceDifference = 100 + ((statValue - minimumValue) * percentChangePerPoint);
               return calculatedChanceDifference >= randomValue;
            }
         }
         else if (ActionType.AddCharacterToSquad == action.type)
         {
            int characterId = int.Parse(action.value);
            SquadService.getAllySquad().characterIds.Add(characterId);
            parentScene.bus.Publish(null, new AddCharacterEvent(characterId));
         }
         else if (ActionType.RemoveCharacterFromSquad == action.type)
         {
            int characterId = int.Parse(action.value);
            SquadService.getAllySquad().characterIds.Remove(characterId);
         }
         else if (action.type == VastAdventure.Utility.ActionType.SetRequirement)
         {
            //TODO: split requiremnt types out into flag, etc...
            string[] commands = action.value.Split(':');
            FlagService.SetFlagValue(commands[1], bool.Parse(commands[2]));
            parentScene.bus.Publish(null,new FlagSetEvent(commands[1], bool.Parse(commands[2])));
         }
         else if (action.type == ActionType.AddItem)
         {
            var parameters = action.value.Split(':');
            InventoryItemModel item = new InventoryItemModel();
            item.itemName = parameters[0];
            item.itemDescription = parameters[1];
            item.imageReference = parameters[2]; 
            InventoryService.AddItemToInventory(item);
            parentScene.bus.Publish(null, new InventoryEvent(action.type, parameters[0]));
         }
         else if (action.type == ActionType.RemoveItem)
         {
            InventoryService.RemoveItemToInventory(action.value);
         }
         else if (action.type == ActionType.ChangeMoney)
         {
            InventoryService.ChangeMoneyAmount(int.Parse(action.value));
         }
         else if (action.type == ActionType.PreviousScreen)
         {
            ScreenService.replaceWithPreviousScreen(parentScene);
         }
         else if (action.type == ActionType.custom)
         {
            var parameters = action.value.Split(':');
            if(parameters[0] == "Bloodthirsty")
            {
               return CharacterService.getCharacter(20).hasDied &&
               CharacterService.getCharacter(21).hasDied &&
               CharacterService.getCharacter(22).hasDied &&
               CharacterService.getCharacter(25).hasDied &&
               CharacterService.getCharacter(26).hasDied &&
               CharacterService.getCharacter(27).hasDied &&
               CharacterService.getCharacter(30).hasDied &&
               CharacterService.getCharacter(35).hasDied &&
               CharacterService.getCharacter(36).hasDied &&
               CharacterService.getCharacter(37).hasDied;
            }
            if(parameters[0] == "Gentleman Inmate")
            {
               return !CharacterService.getCharacter(20).hasDied &&
               !CharacterService.getCharacter(21).hasDied &&
               !CharacterService.getCharacter(22).hasDied &&
               !CharacterService.getCharacter(25).hasDied &&
               !CharacterService.getCharacter(26).hasDied &&
               !CharacterService.getCharacter(27).hasDied &&
               !CharacterService.getCharacter(30).hasDied &&
               !CharacterService.getCharacter(35).hasDied &&
               !CharacterService.getCharacter(36).hasDied &&
               !CharacterService.getCharacter(37).hasDied;
            }
         }

         return true;
      }

      public static bool PerformCheck(Action action)
      {
         var parameters = action.value.Split(':');
         if (parameters[0] == "flag")
         {
            if (bool.TryParse(parameters[2], out bool flagResult))
            {
               return FlagService.CheckFlagValue(parameters[1]) == flagResult;
            }
            else
            {
               return false;
            }
         }
         else if (parameters[0] == "squad")
         {
            if (parameters[1] == "hasCharacter")
            {
               return SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
            }
            if (parameters[1] == "doesNotHaveCharacter")
            {
               return !SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
            }
            if (parameters[1] == "size")
            {
               return SquadService.getAllySquad().characterIds.Count >= int.Parse(parameters[2]);
            }
            if (parameters[1] == "sizeLessThen")
            {
               return SquadService.getAllySquad().characterIds.Count < int.Parse(parameters[2]);
            }
         }
         else if (parameters[0] == "character")
         {
            if (parameters[1] == "hasDied")
            {
               int characterId = int.Parse(parameters[2]);
               return CharacterService.getCharacter(characterId).hasDied;
            }
            if (parameters[1] == "isAlive")
            {
               int characterId = int.Parse(parameters[2]);
               return !CharacterService.getCharacter(characterId).hasDied;
            }
         }
         else if (parameters[0] == "stat")
         {
            int expectedMinimum = int.Parse(parameters[2]);
            int statValue = 0;
            if (parameters[1] == "maxHealth")
            {
               statValue = CharacterService.getPlayer().maxHealth;
            }
            else if (parameters[1] == "strength")
            {
               statValue = CharacterService.getPlayer().strength;
            }
            else if (parameters[1] == "toughness")
            {
               statValue = CharacterService.getPlayer().toughness;
            }
            else if (parameters[1] == "dexterity")
            {
               statValue = CharacterService.getPlayer().dexterity;
            }
            return statValue >= expectedMinimum;
         }
         else if (parameters[0] == "inventory")
         {
            if (parameters[1] == "haveItem")
            {
               string itemDescription = parameters[2];
               return InventoryService.HaveItem(itemDescription);
            }
            else if (parameters[1] == "haveMoney")
            {
               int expectedMinimum = int.Parse(parameters[2]);
               return InventoryService.GetGoldTotal() >= expectedMinimum;
            }
         }
         return true;
      }
   }
}
