using System;
using System.Collections.Generic;
using System.Linq;
using Base.Events;
using Base.UI.Mobile;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility.Services;
using Action = VastAdventure.Model.Action;

namespace VastAdventureAndriod.Screens
{
   [Serializable]
   public class EventScreen : BasicScreen
   {
      EventScreenModel eventScreenModel;

      //TODO: create a button to Option Action List dictionary

      //UI
      [NonSerialized]
      Texture2D scroll;
      [NonSerialized]
      Texture2D table;

      Label locationLabel;
      Label descriptionLabel;
      //TODO: Need to create PictureBox, PictureBox Image;
      List<Button> options;
      Dictionary<string, List<ActionNode>> OptionActionlink;

      [NonSerialized]
      Random rand;
      public EventScreen() : base()
      {
         ScreenType = VastAdventure.Utility.ScreenType.Event;
      }

      public override void Update(int dt)
      {
         //locationLabel.Update();
         //descriptionLabel.Update();
         for (int i = options.Count - 1; i >= 0; i--)
         {
            options[i].Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(table, viewport.toRectangle(), Color.White);
         sb.Draw(scroll, viewport.toRectangle(), Color.White);
         locationLabel.Render(sb);
         descriptionLabel.Render(sb);
         foreach (Button b in options)
         {
            b.Render(sb);
         }
      }

      public override void Init()
      {
         base.Init();
         // Get Screens information
         table = ContentService.Get2DTexture("table_image");
         scroll = ContentService.Get2DTexture("Scroll");

         options = new List<Button>();
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         locationLabel = new Label("locationLabel", eventScreenModel.EventLocation, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10 * 1, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
         locationLabel.minFontScale = 0;
         locationLabel.maxFontScale = 2;
         descriptionLabel = new Label("descriptionLabel", eventScreenModel.EventDescription, new EngineRectangle(viewport.X + 50, viewport.Y + viewport.Height / 10 * 2, viewport.Width - 100, viewport.Height / 10 * 4), Color.Black);
         descriptionLabel.minFontScale = 0;
         descriptionLabel.maxFontScale = 1;
         descriptionLabel.isMultiLine = true;
         for (int i = 0; i < eventScreenModel.Options.Count; i++)
         {
            Button tempButton = new Button("button" + i, eventScreenModel.Options[i].OptionDescription, new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - i - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
            tempButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleClick));
            options.Add(tempButton);
            tempButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
            tempButton.padding = new int[] { 0, 30, 0, 30 };
            tempButton.minFontScale = 0;
            tempButton.maxFontScale = 1;
         }
      }

      public void Init(int eventId)
      {
         base.Init();
         rand = new Random();
         OptionActionlink = new Dictionary<string, List<ActionNode>>();
         // Get Screens information
         eventScreenModel = VastAdventure.Utility.Services.ScreenService.getEvent(eventId);

         options = new List<Button>();
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();

         table = ContentService.Get2DTexture("table_image");
         scroll = ContentService.Get2DTexture("Scroll");
         //Create label informaiton
         locationLabel = new Label("locationLabel", eventScreenModel.EventLocation, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10 * 1, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
         locationLabel.minFontScale = 0;
         locationLabel.maxFontScale = 2;

         //TODO: create Image if there is an image in the model

         var visibleOptions = 0;
         //Create buttons from options
         for (int i = 0; i < eventScreenModel.Options.Count; i++)
         {
            bool precheck = true;
            foreach(Action action in eventScreenModel.Options[i].PreChecks)
            {
               if(!ActionService.PerformCheck(action))
               {
                  precheck = false;
               }
            }
            //TODO: add actual checking of prechecks
            if(precheck)
            {
               Button tempButton = new Button(i.ToString(), eventScreenModel.Options[i].OptionDescription, new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - visibleOptions - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
               OptionActionlink.Add(tempButton.Name, eventScreenModel.Options[i].Actions);
               tempButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleClick));
               options.Add(tempButton);
               tempButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
               tempButton.padding = new int[] { 0, 30, 0, 30 };
               tempButton.minFontScale = 0;
               tempButton.maxFontScale = 1;
               visibleOptions++;
            }
         }

         descriptionLabel = new Label("descriptionLabel", eventScreenModel.EventDescription, new EngineRectangle(viewport.X + 50, viewport.Y + viewport.Height / 10 * 2, viewport.Width - 100,((viewport.Height / 10 * (9 - (visibleOptions - 1) - 1)) - 50) - (viewport.Y + viewport.Height / 10 * 2)), Color.Black);
         descriptionLabel.minFontScale = .5f;
         descriptionLabel.maxFontScale = 1;
         descriptionLabel.isMultiLine = true;

      }


      public void HandleClick(object sender, OnClick click)
      {
         Button sendingButton = sender as Button;
         if (OptionActionlink.ContainsKey(sendingButton.Name))
         {
            List<ActionNode> actions = OptionActionlink[sendingButton.Name];
            int currentNodeId = 0;
            if (actions.Count > 0)
            {
               currentNodeId = actions[0].NodeId;
            }
            else
            {
               return;
            }
            while (currentNodeId != -1 && currentNodeId < actions.Count())
            {
               bool passed = true;
               passed = ActionService.HandleAction(actions[currentNodeId].Action,parentScene);
               currentNodeId = passed ? actions[currentNodeId].PassNextNode : actions[currentNodeId].FailNextNode;
            }
            return;
         }
      }

      //public bool HandleAction(VastAdventure.Model.Action action)
      //{
      //   if (action.type == VastAdventure.Utility.ActionType.ChangeScreen)
      //   {
      //      VastAdventure.Utility.Services.ScreenService.replaceScreen(parentScene, action.value);
      //   }
      //   else if (action.type == VastAdventure.Utility.ActionType.RemoveOption)
      //   {
      //      var optionButton = options.Find(x => x.Name == action.value);
      //      options.Remove(optionButton);
      //   }
      //   else if (action.type == VastAdventure.Utility.ActionType.changeText)
      //   {
      //      string[] parsedValue = action.value.Split(':');
      //      if (parsedValue[0] == "ScreenDescription")
      //      {
      //         //eventScreenModel.EventDescription = parsedValue[1];
      //         descriptionLabel.value = parsedValue[1];
      //      }
      //      else if (parsedValue[0] == "ScreenLocation")
      //      {
      //         //eventScreenModel.EventLocation = parsedValue[1];
      //         locationLabel.value = parsedValue[1];
      //      }
      //   }
      //   else if (action.type == VastAdventure.Utility.ActionType.EndGame)
      //   {
      //      //Send Character to gravetyardd
      //      GraveyardService.AddCurrentPlayerToGraveyard();

      //      //Reset Services
      //      ScreenService.Init(new AndriodScreenProvider());
      //      CharacterService.Initialize();
      //      SquadService.Initialize();
      //      ScreenService.navigationModel.ScreenHistory = new Stack<BasicScreen>();

      //      //Clear GUI
      //      Base.System.GUISystem guiSystem = parentScene.GetSystem<Base.System.GUISystem>();
      //      guiSystem.ClearRegisteredEntities();

            
      //      SavesService.ClearSave();

      //      //Setup Main Menu
      //      MainMenu mm = new MainMenu();
      //      Base.Entities.Entity entity = parentScene.CreateEntity();
      //      mm.Init(parentScene.bus, parentScene);
      //      mm.Init(entity);
      //      parentScene.AddComponent(entity, mm);
      //   }
      //   else if (action.type == VastAdventure.Utility.ActionType.GenerateRandomEncounter)
      //   {
      //      ScreenService.replaceWithGeneratedBattle(this.parentScene, action.value);
      //   }
      //   else if (action.type == VastAdventure.Utility.ActionType.ChangeCharacterInfo)
      //   {
      //      string[] commands = action.value.Split(':');
      //      int characterId = int.Parse(commands[0]);
      //      if (commands[1] == "stat")
      //      {
      //         int amountChange = int.Parse(commands[4]); 
      //         if(commands[3] == "decrease")
      //         {
      //            amountChange *= -1;
      //         }
      //         CharacterService.ChangeCharacterStat(commands[2], characterId, amountChange);
      //      }
      //      else if (commands[1] == "nonStat")
      //      {
      //         if (commands[3] == "hasDied")
      //         {
      //            bool hasDied = bool.Parse(commands[4]);
      //            CharacterService.getCharacter(characterId).hasDied = hasDied;
      //         }
      //      }
      //   }
      //   else if (action.type == ActionType.CheckRandomChance)
      //   {
      //      int randomNumber = rand.Next() % 100;
      //      if (int.Parse(action.value) > randomNumber)
      //      {
      //         return false;
      //      }
      //   }
      //   else if (ActionType.CheckRequirement == action.type)
      //   {
      //      var parameters = action.value.Split(':');
      //      if (parameters[0] == "flag")
      //      {
      //         if (bool.TryParse(parameters[2], out bool flagResult))
      //         {
      //            return FlagService.CheckFlagValue(parameters[1]) == flagResult;
      //         }
      //         else
      //         {
      //            return false;
      //         }
      //      }
      //      else if (parameters[0] == "squad")
      //      {
      //         if(parameters[1] == "hasCharacter")
      //         {
      //            return SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
      //         }
      //         if (parameters[1] == "doesNotHaveCharacter")
      //         {
      //            return !SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
      //         }
      //         if (parameters[1] == "size")
      //         {
      //            return SquadService.getAllySquad().characterIds.Count >= int.Parse(parameters[2]);
      //         }
      //      }
      //      else if(parameters[0] == "character")
      //      {
      //         if(parameters[1] == "hasDied")
      //         {
      //            int characterId = int.Parse(parameters[2]);
      //            return CharacterService.getCharacter(characterId).hasDied;
      //         }
      //         if (parameters[1] == "isAlive")
      //         {
      //            int characterId = int.Parse(parameters[2]);
      //            return !CharacterService.getCharacter(characterId).hasDied;
      //         }
      //      }
      //      else if (parameters[0] == "stat")
      //      {
      //         int expectedMinimum = int.Parse(parameters[2]);
      //         int statValue = 0;
      //         if (parameters[1] == "maxHealth")
      //         {
      //            statValue = CharacterService.getPlayer().maxHealth;
      //         }
      //         else if (parameters[1] == "strength")
      //         {
      //            statValue = CharacterService.getPlayer().strength;
      //         }
      //         else if (parameters[1] == "toughness")
      //         {
      //            statValue = CharacterService.getPlayer().toughness;
      //         }
      //         else if (parameters[1] == "dexterity")
      //         {
      //            statValue = CharacterService.getPlayer().dexterity;
      //         }
      //         return statValue >= expectedMinimum;
      //      }
      //      else if (parameters[0] == "inventory")
      //      {
      //         if (parameters[1] == "haveItem")
      //         {
      //            string itemDescription = parameters[2];
      //            return InventoryService.HaveItem(itemDescription);
      //         }
      //         else if (parameters[1] == "haveMoney")
      //         {
      //            int expectedMinimum = int.Parse(parameters[2]);
      //            return InventoryService.GetGoldTotal() >= expectedMinimum;
      //         }
      //      }
      //   }
      //   else if (ActionType.CheckRequirementWithChance == action.type)
      //   {
      //      var parameters = action.value.Split(':');
      //      if (parameters[0] == "stat")
      //      {
      //         int minimumValue = int.Parse(parameters[2]);
      //         int percentChangePerPoint = int.Parse(parameters[3]);
      //         int randomValue = rand.Next() % 100;
      //         int statValue = 0;
      //         if (parameters[1] == "maxHealth")
      //         {
      //            statValue = CharacterService.getPlayer().maxHealth;
      //         }
      //         else if (parameters[1] == "strength")
      //         {
      //            statValue = CharacterService.getPlayer().strength;
      //         }
      //         else if (parameters[1] == "toughness")
      //         {
      //            statValue = CharacterService.getPlayer().toughness;
      //         }
      //         else if (parameters[1] == "dexterity")
      //         {
      //            statValue = CharacterService.getPlayer().dexterity;
      //         }
      //         int calculatedChanceDifference = 100 + ((statValue - minimumValue) * percentChangePerPoint);
      //         return calculatedChanceDifference >= randomValue;
      //      }
      //   }
      //   else if(ActionType.AddCharacterToSquad == action.type)
      //   {
      //      int characterId = int.Parse(action.value);
      //      SquadService.getAllySquad().characterIds.Add(characterId);
      //   }
      //   else if (ActionType.RemoveCharacterFromSquad == action.type)
      //   {
      //      int characterId = int.Parse(action.value);
      //      SquadService.getAllySquad().characterIds.Remove(characterId);
      //   }
      //   else if (action.type == VastAdventure.Utility.ActionType.SetRequirement)
      //   {
      //      //TODO: split requiremnt types out into flag, etc...
      //      string[] commands = action.value.Split(':');
      //      FlagService.SetFlagValue(commands[1], bool.Parse(commands[2]));
      //   }
      //   else if (action.type == ActionType.AddItem)
      //   {
      //      var parameters = action.value.Split(':');
      //      InventoryItemModel item = new InventoryItemModel();
      //      item.itemDescription = parameters[0];
      //      item.imageReference = parameters[1];
      //      InventoryService.AddItemToInventory(item);
      //   }
      //   else if(action.type == ActionType.RemoveItem)
      //   {
      //      InventoryService.RemoveItemToInventory(action.value);
      //   }
      //   else if (action.type == ActionType.ChangeMoney)
      //   {
      //      InventoryService.ChangeMoneyAmount(int.Parse(action.value));
      //   }
      //   else if (action.type == ActionType.PreviousScreen)
      //   {
      //      ScreenService.replaceWithPreviousScreen(parentScene);
      //   }

      //   return true;
      //}

      //public bool PerformCheck(Action action)
      //{
      //   var parameters = action.value.Split(':');
      //   if (parameters[0] == "flag")
      //   {
      //      if (bool.TryParse(parameters[2], out bool flagResult))
      //      {
      //         return FlagService.CheckFlagValue(parameters[1]) == flagResult;
      //      }
      //      else
      //      {
      //         return false;
      //      }
      //   }
      //   else if (parameters[0] == "squad")
      //   {
      //      if (parameters[1] == "hasCharacter")
      //      {
      //         return SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
      //      }
      //      if (parameters[1] == "doesNotHaveCharacter")
      //      {
      //         return !SquadService.getAllySquad().characterIds.Contains(int.Parse(parameters[2]));
      //      }
      //      if (parameters[1] == "size")
      //      {
      //         return SquadService.getAllySquad().characterIds.Count >= int.Parse(parameters[2]);
      //      }
      //   }
      //   else if (parameters[0] == "character")
      //   {
      //      if (parameters[1] == "hasDied")
      //      {
      //         int characterId = int.Parse(parameters[2]);
      //         return CharacterService.getCharacter(characterId).hasDied;
      //      }
      //      if (parameters[1] == "isAlive")
      //      {
      //         int characterId = int.Parse(parameters[2]);
      //         return !CharacterService.getCharacter(characterId).hasDied;
      //      }
      //   }
      //   else if (parameters[0] == "stat")
      //   {
      //      int expectedMinimum = int.Parse(parameters[2]);
      //      int statValue = 0;
      //      if (parameters[1] == "maxHealth")
      //      {
      //         statValue = CharacterService.getPlayer().maxHealth;
      //      }
      //      else if (parameters[1] == "strength")
      //      {
      //         statValue = CharacterService.getPlayer().strength;
      //      }
      //      else if (parameters[1] == "toughness")
      //      {
      //         statValue = CharacterService.getPlayer().toughness;
      //      }
      //      else if (parameters[1] == "dexterity")
      //      {
      //         statValue = CharacterService.getPlayer().dexterity;
      //      }
      //      return statValue >= expectedMinimum;
      //   }
      //   else if (parameters[0] == "inventory")
      //   {
      //      if (parameters[1] == "haveItem")
      //      {
      //         string itemDescription = parameters[2];
      //         return InventoryService.HaveItem(itemDescription);
      //      }
      //      else if (parameters[1] == "haveMoney")
      //      {
      //         int expectedMinimum = int.Parse(parameters[2]);
      //         return InventoryService.GetGoldTotal() >= expectedMinimum;
      //      }
      //   }
      //   return true;
      //}

      #region deserialization
      public override void onDeserialized()
      {
         rand = new Random();
         if (ContentService.isInitialized)
         {
            table = ContentService.Get2DTexture("table_image");
            scroll = ContentService.Get2DTexture("Scroll");
            locationLabel.onDeserialized();
            descriptionLabel.onDeserialized();
           
            foreach (Button ui in options)
            {
               ui.onDeserialized();
            }
         }
      }
      #endregion
   }
}