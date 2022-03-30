using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Events;

using Base.UI.Mobile;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Base.Entities;
using VastAdventure.Events;
using VastAdventure.Model;
using VastAdventure.Utility;
using Action = VastAdventure.Model.Action;
using VastAdventure.Component;
using VastAdventureAndriod.VastAdventureUI;
using VastAdventure.Utility.Services;
using VastAdventure.Screens;
using System.Runtime.Serialization;
using Base.Scenes;

namespace VastAdventureAndriod.Screens
{
   [Serializable]
   public class BattleScreen: BasicScreen
   {
      BattleScreenModel Model;
      public SquadModel allySquad;
      public SquadModel enemySquad;

      EHandler<UpdateBattleHistoryEvent> updateBattleEHandler;
      EHandler<BattleOverEvent> battleOverEHandler;
      [NonSerialized]
      Random rand;

      List<CharacterUI> CharacterUIs;

      [NonSerialized]
      Texture2D scroll;
      [NonSerialized]
      Texture2D table;

      //Both UI
      Label lblLocation;
      Label lblFightDescription;
      string currentScreen;

      //Battle UI
      Button fleeButton;
      Button taticsSelectionButton;
      Button attackButton;
      List<Button> Skills;

      //Tatic selection UI
      Button RandomTatic;
      Button MostHealthTatic;
      Button StrongestTatic;
      Button FastestTatic;
      Button MostVaunerableTatic;

      public BattleScreen() : base()
      {
         ScreenType = ScreenType.Battle;
      }

      public override void Update(int dt)
      {
         if (currentScreen == "battle")
         {
            lblLocation.Update(dt);
            lblFightDescription.Update(dt);
            attackButton.Update(dt);
            taticsSelectionButton.Update(dt);
            foreach (CharacterUI ui in CharacterUIs)
            {
               ui.Update(dt);
            }
            for (int i = Skills.Count - 1; i >= 0; i--)
            {
               Skills[i].Update(dt);
            }
            if (Model.canFlee)
            {
               fleeButton.Update(dt);
            }
         }
         else
         {
            lblLocation.Update(dt);
            RandomTatic.Update(dt);
            MostHealthTatic.Update(dt);
            StrongestTatic.Update(dt);
            FastestTatic.Update(dt);
            MostVaunerableTatic.Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         if (currentScreen == "battle")
         {
            EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
            sb.Draw(table, viewport.toRectangle(), Color.White);
            sb.Draw(scroll, viewport.toRectangle(), Color.White);
            lblLocation.Render(sb);
            lblFightDescription.Render(sb);
            attackButton.Render(sb);
            taticsSelectionButton.Render(sb);
            foreach (CharacterUI ui in CharacterUIs)
            {
               ui.Render(sb);
            }
            foreach (Button b in Skills)
            {
               b.Render(sb);
            }
            if (Model.canFlee)
            {
               fleeButton.Render(sb);
            }
         }
         else
         {
            EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
            sb.Draw(table, viewport.toRectangle(), Color.White);
            sb.Draw(scroll, viewport.toRectangle(), Color.White);
            lblLocation.Render(sb);
            RandomTatic.Render(sb);
            MostHealthTatic.Render(sb);
            StrongestTatic.Render(sb);
            FastestTatic.Render(sb);
            MostVaunerableTatic.Render(sb);
         }
      }


      private void initializeUI()
      {
         //Creating UI
         table = ContentService.Get2DTexture("table_image");
         scroll = ContentService.Get2DTexture("Scroll");
         currentScreen = "battle";

         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         //Create label informaiton
         lblLocation = new Label("locationLabel", Model.locationName, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10 * 1, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
         lblLocation.minFontScale = 0;
         lblLocation.maxFontScale = 5;

         var maxSquadDiplayYLocation = 0;
         for (int i = 0; i < allySquad.characterIds.Count; i++)
         {
            CharacterModel character = CharacterService.getCharacter(allySquad.characterIds[i]);
            CharacterUI ui = new CharacterUI(character, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10 * (2 + i), (viewport.Width / 2 - 100), viewport.Height / 10), Color.Black, "button_outline_none");
            CharacterUIs.Add(ui);
            if ((viewport.Y + 50 + viewport.Height / 10 * (2 + i)) + viewport.Height / 10 > maxSquadDiplayYLocation)
            {
               maxSquadDiplayYLocation = (int)((viewport.Y + 50 + viewport.Height / 10 * (2 + i)) + viewport.Height / 10);
            }
         }
         for (int i = 0; i < enemySquad.characterIds.Count; i++)
         {
            CharacterModel character = CharacterService.getCharacter(enemySquad.characterIds[i]);
            CharacterUI ui = new CharacterUI(character, new EngineRectangle(viewport.Width / 2, viewport.Y + 50 + viewport.Height / 10 * (2 + i), (viewport.Width / 2 - 100), viewport.Height / 10), Color.Black, "button_outline_none");
            ui.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleCharacterUiClick));
            CharacterUIs.Add(ui);
            if ((viewport.Y + 50 + viewport.Height / 10 * (2 + i)) + viewport.Height / 10 > maxSquadDiplayYLocation)
            {
               maxSquadDiplayYLocation = (int)((viewport.Y + 50 + viewport.Height / 10 * (2 + i)) + viewport.Height / 10);
            }
         }

         var minButtonYLocation = 0;
         //Create Battle action buttons
         if (Model.canFlee)
         {
            //flee button
            fleeButton = new Button("flee", "Flee from the battle", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 1)) - 50, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
            fleeButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(Flee));
            fleeButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
            fleeButton.padding = new int[] { 0, 30, 0, 30 };
            fleeButton.minFontScale = 0;
            fleeButton.maxFontScale = 1;

            taticsSelectionButton = new Button("Tatics", "Select squad tatics", new EngineRectangle(viewport.X + (viewport.Width - 100) / 2 + 50, (viewport.Height / 10 * (9 - 1)) - 50, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
            taticsSelectionButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(StartSelectTatic));
            taticsSelectionButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
            taticsSelectionButton.padding = new int[] { 0, 30, 0, 30 };
            taticsSelectionButton.minFontScale = 0;
            taticsSelectionButton.maxFontScale = 1;

            //basic attack button
            attackButton = new Button("attack", "Attack", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 2)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
            attackButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(Attack));
            attackButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
            attackButton.padding = new int[] { 0, 30, 0, 30 };
            attackButton.minFontScale = 0;
            attackButton.maxFontScale = 1;

            minButtonYLocation = (int)((viewport.Height / 10 * (9 - 2)) - 50);
         }
         else
         {
            //basic attack button
            attackButton = new Button("attack", "Attack", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 2)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
            attackButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(Attack));
            attackButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
            attackButton.padding = new int[] { 0, 30, 0, 30 };
            attackButton.minFontScale = 0;
            attackButton.maxFontScale = 1;

            taticsSelectionButton = new Button("Tatics", "Select squad tatics", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
            taticsSelectionButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(StartSelectTatic));
            taticsSelectionButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
            taticsSelectionButton.padding = new int[] { 0, 30, 0, 30 };
            taticsSelectionButton.minFontScale = 0;
            taticsSelectionButton.maxFontScale = 1;


            minButtonYLocation = (int)((viewport.Height / 10 * (9 - 1)) - 50);
         }


         lblFightDescription = new Label("fightInformation", "You encounter a enemy", new EngineRectangle(viewport.X + 50, maxSquadDiplayYLocation, (viewport.Width - 100), minButtonYLocation - maxSquadDiplayYLocation), Color.Black);
         lblFightDescription.minFontScale = 0;
         lblFightDescription.maxFontScale = 1;

         //Tatics section
         RandomTatic = new Button("random", "Attack random unit", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         RandomTatic.onClick = new EHandler<OnClick>(new Action<object, OnClick>(SelectTatic));
         RandomTatic.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         RandomTatic.padding = new int[] { 0, 30, 0, 30 };
         RandomTatic.minFontScale = 0;
         RandomTatic.maxFontScale = 1;

         MostHealthTatic = new Button("LeastHealth", "Attack unit with the least health", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 1 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         MostHealthTatic.onClick = new EHandler<OnClick>(new Action<object, OnClick>(SelectTatic));
         MostHealthTatic.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         MostHealthTatic.padding = new int[] { 0, 30, 0, 30 };
         MostHealthTatic.minFontScale = 0;
         MostHealthTatic.maxFontScale = 1;

         StrongestTatic = new Button("Strongest", "Attack Strongest unit", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 2 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         StrongestTatic.onClick = new EHandler<OnClick>(new Action<object, OnClick>(SelectTatic));
         StrongestTatic.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         StrongestTatic.padding = new int[] { 0, 30, 0, 30 };
         StrongestTatic.minFontScale = 0;
         StrongestTatic.maxFontScale = 1;

         FastestTatic = new Button("Fastest", "Attack Fastest unit", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 3 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         FastestTatic.onClick = new EHandler<OnClick>(new Action<object, OnClick>(SelectTatic));
         FastestTatic.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         FastestTatic.padding = new int[] { 0, 30, 0, 30 };
         FastestTatic.minFontScale = 0;
         FastestTatic.maxFontScale = 1;

         MostVaunerableTatic = new Button("MostVaunerable", "Attack Most vulnerable unit", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 4 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         MostVaunerableTatic.onClick = new EHandler<OnClick>(new Action<object, OnClick>(SelectTatic));
         MostVaunerableTatic.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         MostVaunerableTatic.padding = new int[] { 0, 30, 0, 30 };
         MostVaunerableTatic.minFontScale = 0;
         MostVaunerableTatic.maxFontScale = 1;

         parentScene.bus.Publish(this, new BattleStartEvent());
      }

      public void Init(int screenId)
      {
         //Setting up information
         base.Init();
         rand = new Random();
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         //ButtonActionLink = new Dictionary<string, List<ActionNode>>();
         Skills = new List<Button>();
         CharacterUIs = new List<CharacterUI>();
         updateBattleEHandler = new EHandler<UpdateBattleHistoryEvent>(new Action<object, UpdateBattleHistoryEvent>(HandleBattleInformationUpdate));
         battleOverEHandler = new EHandler<BattleOverEvent>(new Action<object, BattleOverEvent>(HandleBattleOverEvent));
         parentScene.bus.Subscribe(updateBattleEHandler);
         parentScene.bus.Subscribe(battleOverEHandler);
         Model = VastAdventure.Utility.Services.ScreenService.getBattle(screenId);

         if(Model.battleId == 12 || Model.battleId == 13)
         {
            AudioService.PlaySong("Battle_Loop_1");
         }
         else
         {
            AudioService.PlaySong("Battle_Loop_4");
         }

         initializeUI();
         
         //TODO: impliment skills and add button for them
      }

      public void Init(BattleScreenModel model)
      {
         //Setting up information
         base.Init();
         rand = new Random();
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(false));
         //ButtonActionLink = new Dictionary<string, List<ActionNode>>();
         Skills = new List<Button>();
         CharacterUIs = new List<CharacterUI>();
         updateBattleEHandler = new EHandler<UpdateBattleHistoryEvent>(new Action<object, UpdateBattleHistoryEvent>(HandleBattleInformationUpdate));
         battleOverEHandler = new EHandler<BattleOverEvent>(new Action<object, BattleOverEvent>(HandleBattleOverEvent));
         parentScene.bus.Subscribe(updateBattleEHandler);
         parentScene.bus.Subscribe(battleOverEHandler);
         this.Model = model;

         if (model.battleId == 12 || model.battleId == 13)
         {
            AudioService.PlaySong("Battle_Loop_1");
         }
         else
         {
            AudioService.PlaySong("Battle_Loop_4");
         }

         initializeUI();

         //TODO: impliment skills and add button for them
      }

      public override void Init(EventBus ebus, Scene parentScene)
      {
         if(this.parentScene != null)
         {
            this.parentScene.bus.Unsubscribe(updateBattleEHandler);
            this.parentScene.bus.Unsubscribe(battleOverEHandler);
         }

         base.Init(ebus, parentScene);
         if(updateBattleEHandler != null)
         {
            updateBattleEHandler = new EHandler<UpdateBattleHistoryEvent>(new Action<object, UpdateBattleHistoryEvent>(HandleBattleInformationUpdate));
            battleOverEHandler = new EHandler<BattleOverEvent>(new Action<object, BattleOverEvent>(HandleBattleOverEvent));
            parentScene.bus.Subscribe(updateBattleEHandler);
            parentScene.bus.Subscribe(battleOverEHandler);
         }
      }

      private void Attack(object sender, OnClick clickEvent)
      {
         //TODO: get player and the person the palyer is attacking
         int characterToAttackId = -1;

         foreach(CharacterUI cUi in CharacterUIs)
         {
            if(cUi.selected)
            {
               characterToAttackId = cUi.characterId;
            }
         }
         if (characterToAttackId != -1)
         {
            parentScene.bus.Publish(this, new BasicAttackEvent(0, characterToAttackId));
         }
         else
         {
            lblFightDescription.value += "\nNeed to select character to attack";
         }
      }

      private void Flee(object sender, OnClick clickEvent)
      {
         int randomNumber = rand.Next() % 100;
         int highestEnemyDexterity = 0;
         foreach(int id in enemySquad.characterIds)
         {
            CharacterModel m = CharacterService.getCharacter(id);
            if(m.dexterity > highestEnemyDexterity)
            {
               highestEnemyDexterity = m.dexterity;
            }
         }

         float fleeChance = 100 * ((float)CharacterService.getPlayer().dexterity / (float)highestEnemyDexterity);
         fleeChance += 20;
         if (fleeChance < 20)
         {
            fleeChance = 20;
         }

         if (randomNumber <= fleeChance)
         {
            parentScene.bus.Publish(this,new ClearBattleEntities());
            AudioService.PlaySong("Medieval_Theme_2");
            ActionService.HandleAction(Model.FleeScreenAction, parentScene);
         }
         else
         {
            //TODO: change 0 to where you get the player Id
            parentScene.bus.Publish(this, new PassBattleTurnEvent(0));
            lblFightDescription.value += "\nYou falied to flee";
         }
      }

      private void StartSelectTatic(object sender, OnClick onClick)
      {
         currentScreen = "tatics";
      }

      private void SelectTatic(object sender, OnClick selectTaticClick)
      {
         string taticName = ((Control)sender).Name;
         TaticsType tatic = Enum.Parse<TaticsType>(taticName);
         SquadService.getAllySquad().tatic = tatic;
         parentScene.bus.Publish(this, new SelectTaticsEvent(tatic, allySquad.id));
         currentScreen = "battle";
      }

      public void HandleCharacterUiClick(object sender, OnClick clickEvent)
      {
         CharacterUI selectedUi = sender as CharacterUI;
         if (selectedUi != null)
         {
            foreach(CharacterUI ui in CharacterUIs)
            {
               if(selectedUi == ui)
               {
                  ui.selected = true;
               }
               else
               {
                  ui.selected = false;
               }
            }
         }
      }

      public void HandleBattleInformationUpdate(object sender, UpdateBattleHistoryEvent updateEvent)
      {
         //foreach(CharacterUI ui in CharacterUIs)
         foreach (Character c in updateEvent.CharactersToUpdate)
         {
            for (int i = CharacterUIs.Count - 1; i >= 0; i--)
            {
               if (CharacterUIs[i].characterId == c.model.id)
               {
                  CharacterUIs[i].UpdateCharacterInfo(c);
                  if (c.health <= 0)
                  {
                     CharacterUIs.RemoveAt(i);
                  }
               }
            }
         }
         lblFightDescription.value = updateEvent.actionHistory;
      }

      public void HandleBattleOverEvent(object sender, BattleOverEvent battleOverEvent)
      {
         parentScene.bus.Unsubscribe(updateBattleEHandler);
         parentScene.bus.Unsubscribe(battleOverEHandler);
         if(battleOverEvent.winningTeam == 0)
         {
            parentScene.bus.Publish(this, new ClearBattleEntities());
            foreach(int id in allySquad.characterIds)
            {
               int totalEnemyLevel = 0;
               foreach(var characterId in enemySquad.characterIds)
               {
                  totalEnemyLevel += CharacterService.getCharacter(characterId).level;
               }
               CharacterService.increaseCharacterXP(id, 100 * enemySquad.characterIds.Count + (totalEnemyLevel * 25));
            }
            ActionService.HandleAction(Model.WinNextSceenAction, parentScene);
            //HandleAction(Model.WinNextSceenAction);
            AudioService.PlaySong("Medieval_Theme_2");
         }
         else
         {
            parentScene.bus.Publish(this, new ClearBattleEntities());
            ActionService.HandleAction(Model.LoseNextSceenAction, parentScene);
            //HandleAction(Model.LoseNextSceenAction);
            //TODO: change to sad song
            AudioService.PlaySong("LastCallForAHero");
         }
      }

      #region deserialization
      public override void onDeserialized()
      {
         rand = new Random();
         if (ContentService.isInitialized)
         {
            table = ContentService.Get2DTexture("table_image");
            scroll = ContentService.Get2DTexture("Scroll");


            lblLocation.onDeserialized();
            lblFightDescription.onDeserialized();

            attackButton.onDeserialized();
            taticsSelectionButton.onDeserialized();
            RandomTatic.onDeserialized();
            MostHealthTatic.onDeserialized();
            MostHealthTatic.onDeserialized();
            StrongestTatic.onDeserialized();
            FastestTatic.onDeserialized();
            MostVaunerableTatic.onDeserialized();

            if (fleeButton != null)
            {
               fleeButton.onDeserialized();
            }

            foreach (CharacterUI UI in CharacterUIs)
            {
               UI.onDeserialized();
            }

            foreach (Button b in Skills)
            {
               b.onDeserialized();
            }
         }
      }
      #endregion

   }
}