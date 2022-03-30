using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.UI.Mobile;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.DataProviders;
using VastAdventure.factories;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility.Services;
using VastAdventureAndriod.VastAdventureUI;

namespace VastAdventureAndriod.Screens
{
   [Serializable]
   public class MainMenu : BasicScreen
   {
      #region variables
      Entity menuEntity;

      //Main Menu UI
      Label titleLineOne;
      Label titleLineTwo;
      Button newGame;
      Button continueGame;
      Button options;
      Button graveyard;
      Button achievement;


      //Graveyard UI
      Texture2D GraveBackground;
      GraveListBox graveyardListUI;
      GraveyardModelDetailsUI characterDetailsUI;

      Entity torchOne;
      Entity torchTwo;

      //Name selection UI
      Label selectionTitle;
      Label txtboxVolumeLabel;
      Textbox nameInput;
      Button selectionStartButton;

      //Option UI
      Label OptionTitle;
      Textbox txtboxBackgroundMusicVolume;
      Button btnDelData;
      Button btnExportData;
      Button btnImportData;
      Label uiTypeLabel;
      ListBox uiTypeList;

      //AchievementUI
      Label AchievementTitle;
      ListBox achievementList;


      //Shared UI
      Button returnToMainMenu;

      VastAdventureSaveFile currentSave;
      string currentMenu;

      #endregion

      #region basic screen methods

      public MainMenu() : base()
      {
         ScreenType = VastAdventure.Utility.ScreenType.MainMenu;
      }

      public override void Update(int dt)
      {
         if (currentMenu == "Main Menu")
         {
            UpdateMainMenu(dt);
         }
         else if (currentMenu == "Graveyard")
         {
            UpdateGraveyard(dt);
         }
         else if (currentMenu == "Name Selection")
         {
            UpdateNameSelection(dt);
         }
         else if (currentMenu == "Acheivement Menu")
         {
            UpdateAcheivementmenu(dt);
         }
         else if (currentMenu == "Options")
         {
            UpdateOptionMenu(dt);
         }
      }
      public override void Render(SpriteBatch sb)
      {
         if (currentMenu == "Main Menu")
         {
            RenderMainMenu(sb);
         }
         else if (currentMenu == "Graveyard")
         {
            RenderGraveyard(sb);
         }
         else if (currentMenu == "Name Selection")
         {
            RenderNameSelection(sb);
         }
         else if (currentMenu == "Acheivement Menu")
         {
            RenderAcheivementmenu(sb);
         }
         else if (currentMenu == "Options")
         {
            RenderOptionMenu(sb);
         }
      }

      public void Init(Entity menuEntity)
      {
         base.Init();
         this.menuEntity = menuEntity;
         InitializeMainMenu();
      }
      #endregion

      #region Main Menu
      private void InitializeMainMenu()
      {
         var viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         currentMenu = "Main Menu";

         titleLineOne = new Label("btnTitle", "The", new EngineRectangle(viewport.Width / 8 * 3, 50, viewport.Width / 4, viewport.Height / 8), Color.DarkRed);
         titleLineOne.minFontScale = .2f;
         titleLineOne.maxFontScale = 2;
         //Title.isMultiLine = true;
         titleLineOne.textAnchor = Enums.TextAchorLocation.center;
         titleLineOne.fontName = "old_font";
         titleLineOne.init();

         titleLineTwo = new Label("btnTitle", "Dungeon", new EngineRectangle(viewport.Width / 8 * 3, 150, viewport.Width / 4, viewport.Height / 8), Color.DarkRed);
         titleLineTwo.minFontScale = .2f;
         titleLineTwo.maxFontScale = 5;
         //Title.isMultiLine = true;
         titleLineTwo.textAnchor = Enums.TextAchorLocation.center;
         titleLineTwo.fontName = "old_font";
         titleLineTwo.init();

         currentSave = SavesService.saveFile;
         if (currentSave.isCurrentSave)
         {
            newGame = new Button("btnNewGame", "New Game", new EngineRectangle(viewport.Width / 16, viewport.Height / 4, viewport.Width / 8 * 3, viewport.Height / 8), Color.Black);
            newGame.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
            newGame.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleNewGameClick));
            newGame.minFontScale = .5f;
            newGame.isDragable = true;
            newGame.DragDelayInMS = 200;

            continueGame = new Button("btnContinueGame", "Continue Game", new EngineRectangle(viewport.Width / 16 * 9, viewport.Height / 4, viewport.Width / 8 * 3, viewport.Height / 8), Color.Black);
            continueGame.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleContinueGameClick));
            continueGame.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
            continueGame.minFontScale = .5f;
         }
         else
         {
            newGame = new Button("btnNewGame", "New Game", new EngineRectangle(viewport.Width / 4, viewport.Height / 4, viewport.Width / 2, viewport.Height / 8), Color.Black);
            newGame.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
            newGame.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleNewGameClick));
            newGame.minFontScale = .5f;
            newGame.isDragable = true;
            newGame.DragDelayInMS = 200;
         }

         options = new Button("btnOptions", "Options", new EngineRectangle(viewport.Width / 4, viewport.Height / 16 * 7, viewport.Width / 2, viewport.Height / 8), Color.Black);
         options.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         options.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleOptionClick));
         options.minFontScale = .5f;

         graveyard = new Button("btnGraveyard", "Graveyard", new EngineRectangle(viewport.Width / 4, viewport.Height / 16 * 10, viewport.Width / 2, viewport.Height / 8), Color.Black);
         graveyard.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleGraveyardClick));
         graveyard.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         graveyard.minFontScale = .5f;

         achievement = new Button("btnAchievement", "Achievements", new EngineRectangle(viewport.Width / 4, viewport.Height / 16 * 13, viewport.Width / 2, viewport.Height / 8), Color.Black);
         achievement.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         achievement.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleAchievementClick));
         achievement.minFontScale = .5f;

         torchOne = animationFactory.createTorchAnimation(parentScene, new EngineRectangle(0, 0, viewport.Width / 2, viewport.Width / 2));
         torchTwo = animationFactory.createTorchAnimation(parentScene, new EngineRectangle(viewport.Width / 2, 0, viewport.Width / 2, viewport.Width / 2));
      }

      private void UnloadMainMenu()
      {
         parentScene.DestroyEntity(torchOne);
         parentScene.DestroyEntity(torchTwo);
      }

      private void UpdateMainMenu(int dt)
      {
         titleLineOne.Update(dt);
         titleLineTwo.Update(dt);
         newGame.Update(dt);
         options.Update(dt);
         graveyard.Update(dt);
         achievement.Update(dt);
         if (currentSave.isCurrentSave)
         {
            continueGame.Update(dt);
         }
      }

      private void RenderMainMenu(SpriteBatch sb)
      {
         titleLineOne.Render(sb);
         titleLineTwo.Render(sb);
         newGame.Render(sb);
         options.Render(sb);
         graveyard.Render(sb);
         achievement.Render(sb);
         if (currentSave.isCurrentSave)
         {
            continueGame.Render(sb);
         }
      }
      #endregion

      #region Graveyard Menu
      private void InitializeGraveyard()
      {
         currentMenu = "Graveyard";
         GraveBackground = ContentService.Get2DTexture("graveyard");
         List<GraveyardModel> graveyardModels = GraveyardService.GetGraveyard();
         var viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         graveyardListUI = new GraveListBox("LstBoxGraveyard", "", new EngineRectangle(viewport.Width / 100 * 14, viewport.Height / 100 * 13, viewport.Width / 100 * 72, viewport.Height / 100 * 35), Color.Black);
         graveyardListUI.itemBaseHeight = 100;
         graveyardListUI.padding[1] += 20;
         graveyardListUI.padding[3] = 20;
         graveyardListUI.DragDelayInMS = 100;
         //TODO: have it sorted by copletion then level
         foreach (var model in graveyardModels)
         {
            ListBoxItem item = new ListBoxItem(model);
            graveyardListUI.Items.Add(item);
         }
         graveyardListUI.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleGraveyardModelChange));

         if (graveyardListUI.Items.Count > 0)
         {
            characterDetailsUI = new GraveyardModelDetailsUI(((GraveyardModel)graveyardListUI.Items[graveyardListUI.selectedIndex].Value), new EngineRectangle(viewport.Width / 100 * 5, viewport.Height / 100 * 50, viewport.Width / 100 * 91, viewport.Height / 100 * 44));
         }
         else
         {
            characterDetailsUI = new GraveyardModelDetailsUI();
         }

         returnToMainMenu = new Button("backButton", "", new EngineRectangle(0, 0, 100, 100), Color.Black);
         returnToMainMenu.setImageReferences("arrow_button", "arrow_button_hover", "arrow_button_hover", "arrow_button");
         returnToMainMenu.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleReturnToMainMenu));
      }

      private void UnloadGraveyard()
      {

      }

      private void UpdateGraveyard(int dt)
      {
         graveyardListUI.Update(dt);
         characterDetailsUI.Update(dt);
         returnToMainMenu.Update(dt);
      }

      private void RenderGraveyard(SpriteBatch sb)
      {
         sb.Draw(GraveBackground, ScreenGraphicService.GetViewportBounds().toRectangle(), Color.White);
         graveyardListUI.Render(sb);
         characterDetailsUI.Render(sb);
         returnToMainMenu.Render(sb);
      }
      #endregion

      #region Name Selection Menu

      private void InitializeNameSelection()
      {
         var viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         currentMenu = "Name Selection";

         selectionTitle = new Label("btnTitle", "Enter Your Name", new EngineRectangle(viewport.Width / 4, viewport.Height / 4, viewport.Width / 2, viewport.Height / 8), Color.DarkRed);
         selectionTitle.minFontScale = .2f;
         selectionTitle.maxFontScale = 5;
         selectionTitle.textAnchor = Enums.TextAchorLocation.center;
         selectionTitle.fontName = "old_font";
         selectionTitle.init();

         nameInput = new Textbox("btnNewGame", "", new EngineRectangle(viewport.Width / 4, viewport.Height / 8 * 3, viewport.Width / 2, viewport.Height / 8), Color.Black, 8, Textbox.defaultCharSet);

         selectionStartButton = new Button("btnStart", "Start Game", new EngineRectangle(viewport.Width / 4, viewport.Height / 8 * 5, viewport.Width / 2, viewport.Height / 8), Color.Black);
         selectionStartButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleNameSelectionContinue));
         selectionStartButton.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         selectionStartButton.padding = new int[]
         {
            0,25,0,25
         };

         returnToMainMenu = new Button("backButton", "", new EngineRectangle(0, 0, 100, 100), Color.Black);
         returnToMainMenu.setImageReferences("arrow_button", "arrow_button_hover", "arrow_button_hover", "arrow_button");
         returnToMainMenu.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleReturnToMainMenu));

         torchOne = animationFactory.createTorchAnimation(parentScene, new EngineRectangle(0, 0, viewport.Width / 2, viewport.Width / 2));
         torchTwo = animationFactory.createTorchAnimation(parentScene, new EngineRectangle(viewport.Width / 2, 0, viewport.Width / 2, viewport.Width / 2));
      }

      private void UnloadNameSelection()
      {
         parentScene.DestroyEntity(torchOne);
         parentScene.DestroyEntity(torchTwo);

      }

      private void UpdateNameSelection(int dt)
      {
         selectionTitle.Update(dt);
         nameInput.Update(dt);
         selectionStartButton.Update(dt);
         returnToMainMenu.Update(dt);
      }

      private void RenderNameSelection(SpriteBatch sb)
      {
         selectionTitle.Render(sb);
         nameInput.Render(sb);
         selectionStartButton.Render(sb);
         returnToMainMenu.Render(sb);
      }

      #endregion

      #region Opiton Menu

      public void InitializeOptionMenu()
      {
         var viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         currentMenu = "Options";

         OptionTitle = new Label("btnTitle", "Options", new EngineRectangle(viewport.Width / 8, viewport.Height / 16, viewport.Width / 4 * 3, viewport.Height / 8), Color.DarkRed);
         OptionTitle.minFontScale = 0;
         OptionTitle.maxFontScale = 5;
         OptionTitle.textAnchor = Enums.TextAchorLocation.center;
         OptionTitle.fontName = "old_font";
         OptionTitle.init();

         txtboxVolumeLabel = new Label("lblVolumeLabel", "Master Volume", new EngineRectangle(viewport.Width / 4, -50 + viewport.Height / 8 * 2, viewport.Width / 2, 40), Color.White);
         txtboxVolumeLabel.minFontScale = .5f;

         string curVolume = (ConfigService.config.SoundVolume * 100).ToString();
         txtboxBackgroundMusicVolume = new Textbox("txtboxVolume", curVolume, new EngineRectangle(viewport.Width / 4, viewport.Height / 8 * 2, viewport.Width / 2, viewport.Height / 8), Color.Black, 3, new HashSet<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
         txtboxBackgroundMusicVolume.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleTxtboxVolumeChange));

         uiTypeLabel = new Label("lbluiType", "SpriteSet", new EngineRectangle(viewport.Width / 4, viewport.Height / 16 * 7 - 50, viewport.Width / 2, 40), Color.White);
         uiTypeLabel.minFontScale = .5f;

         uiTypeList = new ListBox("lstboxUiType", "",new EngineRectangle(viewport.Width / 4, viewport.Height / 16 * 7, viewport.Width / 2, viewport.Height / 8), Color.White);
         uiTypeList.itemBaseHeight = (int)(uiTypeList.bounds.Height / 2 - 40);
         uiTypeList.isEditing = true;
         uiTypeList.isFocused = true;
         uiTypeList.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleUiTypeChange));
         uiTypeList.setImageReferences("button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none", "button_gray_outline_none");
         uiTypeList.Items.Add(new uiTypeListBoxItem("dark theme"));
         uiTypeList.Items.Add(new uiTypeListBoxItem("scroll theme"));
         if (ConfigService.config.UIType == "dark theme")
         {
            uiTypeList.selectedIndex = 0;
         }
         else
         {
            uiTypeList.selectedIndex = 1;
         }
         uiTypeList.padding = new int[] {20,20,20,20 };
         uiTypeList.isDragable = false;
         


         btnExportData = new Button("btnExport", "Export Data", new EngineRectangle(viewport.Width / 8, viewport.Height / 8 * 5, viewport.Width / 4, viewport.Height / 8), Color.Black);
         btnExportData.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleExportClick));
         btnExportData.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         btnExportData.minFontScale = .25f;

         btnImportData = new Button("btnImport", "Import Data", new EngineRectangle(viewport.Width / 8 * 5, viewport.Height / 8 * 5, viewport.Width / 4, viewport.Height / 8), Color.Black);
         btnImportData.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleImportClick));
         btnImportData.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         btnImportData.minFontScale = .25f;

         btnDelData = new Button("btnDelete", "Delete All Data", new EngineRectangle(viewport.Width / 4, viewport.Height / 8 * 6, viewport.Width / 2, viewport.Height / 8), Color.Black);
         btnDelData.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleDeleteDataClick));
         btnDelData.setImageReferences("banner", "banner_shadow", "banner_shadow", "banner");
         btnDelData.minFontScale = .5f;

         returnToMainMenu = new Button("backButton", "", new EngineRectangle(0, 0, 100, 100), Color.Black);
         returnToMainMenu.setImageReferences("arrow_button", "arrow_button_hover", "arrow_button_hover", "arrow_button");
         returnToMainMenu.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleReturnToMainMenu));
      }

      public void UnloadOptionMenu()
      {

      }

      public void UpdateOptionMenu(int dt)
      {
         OptionTitle.Update(dt);
         txtboxVolumeLabel.Update(dt);
         txtboxBackgroundMusicVolume.Update(dt);
         btnDelData.Update(dt);
         btnExportData.Update(dt);
         btnImportData.Update(dt);
         returnToMainMenu.Update(dt);
         uiTypeLabel.Update(dt);
         uiTypeList.Update(dt);
      }

      public void RenderOptionMenu(SpriteBatch sb)
      {
         OptionTitle.Render(sb);
         txtboxVolumeLabel.Render(sb);
         txtboxBackgroundMusicVolume.Render(sb);
         btnDelData.Render(sb);
         btnExportData.Render(sb);
         btnImportData.Render(sb);
         returnToMainMenu.Render(sb);
         uiTypeLabel.Render(sb);
         uiTypeList.Render(sb);
      }

      #endregion

      #region Acheivement Menu

      private void InitializeAcheivementMenu()
      {
         var viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         currentMenu = "Acheivement Menu";


         AchievementTitle = new Label("lblAchievementTitle", "Achievements", new EngineRectangle(viewport.Width / 8, viewport.Height / 20, viewport.Width / 4 * 3, viewport.Height / 20 * 4), Color.DarkRed);
         AchievementTitle.textAnchor = Enums.TextAchorLocation.center;
         AchievementTitle.maxFontScale = 5;
         AchievementTitle.minFontScale = 0;
         AchievementTitle.fontName = "old_font";
         AchievementTitle.init();

         achievementList = new ListBox("achievementList", "", new EngineRectangle(0, viewport.Height / 10 * 2, viewport.Width, viewport.Height / 10 * 8), Color.Black);
         achievementList.itemBaseHeight = (int)(viewport.Height / 10);
         achievementList.padding = new int[]
         {
            0,(int)(achievementList.bounds.Width / (float)20) ,0,10
         };
         achievementList.setImageReferences("none", "none", "none", "none");
         achievementList.bar.setImageReferences("gold_bar", "gold_bar", "gold_bar", "gold_bar");
         achievementList.isFocused = true;
         achievementList.isEditing = true;

         AchievementUI Cartographer = new AchievementUI(AchievementService.achievements["Cartographer"], new EngineRectangle(10, viewport.Height / 10 * 2, (viewport.Width - 20), viewport.Height / 10));
         Cartographer.minFontScale = 0;
         achievementList.AddItem(Cartographer);

         AchievementUI CultStopper = new AchievementUI(AchievementService.achievements["Cult Stopper"], new EngineRectangle(10, viewport.Height / 10 * 3, (viewport.Width - 20), viewport.Height / 10));
         CultStopper.minFontScale = 0;
         achievementList.AddItem(CultStopper);

         AchievementUI Social = new AchievementUI(AchievementService.achievements["Social"], new EngineRectangle(10, viewport.Height / 10 * 4, (viewport.Width - 20), viewport.Height / 10));
         Social.minFontScale = 0;
         achievementList.AddItem(Social);

         AchievementUI OneManArmy = new AchievementUI(AchievementService.achievements["One Man Army"], new EngineRectangle(10, viewport.Height / 10 * 5, (viewport.Width - 20), viewport.Height / 10));
         OneManArmy.minFontScale = 0;
         achievementList.AddItem(OneManArmy);

         AchievementUI MasterNegotiator = new AchievementUI(AchievementService.achievements["Master Negotiator"], new EngineRectangle(10, viewport.Height / 10 * 6, (viewport.Width - 20), viewport.Height / 10));
         MasterNegotiator.minFontScale = 0;
         achievementList.AddItem(MasterNegotiator);

         AchievementUI GentlemanInmate = new AchievementUI(AchievementService.achievements["Gentleman Inmate"], new EngineRectangle(10, viewport.Height / 10 * 7, (viewport.Width - 20), viewport.Height / 10));
         GentlemanInmate.minFontScale = 0;
         achievementList.AddItem(GentlemanInmate);

         AchievementUI Bloodthirsty = new AchievementUI(AchievementService.achievements["Bloodthirsty"], new EngineRectangle(10, viewport.Height / 10 * 8, (viewport.Width - 20), viewport.Height / 10));
         Bloodthirsty.minFontScale = 0;
         achievementList.AddItem(Bloodthirsty);

         AchievementUI DarkPower = new AchievementUI(AchievementService.achievements["Dark Power"], new EngineRectangle(10, viewport.Height / 10 * 9, (viewport.Width - 20), viewport.Height / 10));
         DarkPower.minFontScale = 0;
         achievementList.AddItem(DarkPower);

         AchievementUI BookSmart = new AchievementUI(AchievementService.achievements["Book Smart"], new EngineRectangle(10, viewport.Height / 10 * 9, (viewport.Width - 20), viewport.Height / 10));
         BookSmart.minFontScale = 0;
         achievementList.AddItem(BookSmart);
         
         AchievementUI OpenDoors = new AchievementUI(AchievementService.achievements["Knowledge Opens Doors"], new EngineRectangle(10, viewport.Height / 10 * 9, (viewport.Width - 20), viewport.Height / 10));
         BookSmart.minFontScale = 0;
         achievementList.AddItem(OpenDoors);

         
         returnToMainMenu = new Button("backButton", "", new EngineRectangle(0, 0, 100, 100), Color.Black);
         returnToMainMenu.setImageReferences("arrow_button", "arrow_button_hover", "arrow_button_hover", "arrow_button");
         returnToMainMenu.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleReturnToMainMenu));
      }

      private void UnloadAcheivementmenu()
      {

      }

      private void UpdateAcheivementmenu(int dt)
      {
         AchievementTitle.Update(dt);
         achievementList.Update(dt);
         returnToMainMenu.Update(dt);
      }

      private void RenderAcheivementmenu(SpriteBatch sb)
      {
         AchievementTitle.Render(sb);
         achievementList.Render(sb);
         returnToMainMenu.Render(sb);
      }


      #endregion

      #region Click Handlers

      public void HandleNewGameClick(object sender, OnClick onClick)
      {
         UnloadMainMenu();
         InitializeNameSelection();
      }

      public void HandleNameSelectionContinue(object sender, OnClick onClick)
      {
         //Reset stuff
         UnloadNameSelection();
         CharacterService.Initialize();
         SquadService.Initialize();
         ScreenService.Init((IScreenProvider)new VastAdventureAndriod.DataProviders.AndriodScreenProvider());
         ScreenService.navigationModel.ScreenHistory = new Stack<BasicScreen>();

         //Creating character
         //CharacterInfoOverlay cOverlay = new CharacterInfoOverlay();
         //cOverlay.Init();
         //parentScene.AddComponent(parentScene.CreateEntity(), cOverlay);
         parentScene.DestroyEntity(menuEntity);
         CharacterService.GenerateAndAddNewPlayerCharacter();
         CharacterService.player.name = nameInput.value.Trim('_');

         //Load Defaults
         SquadService.GenerateAndAddNewPlayerSquad();
         FlagService.LoadDefaultFlagCollection();
         InventoryService.LoadDefaultInventory();
         AudioService.PlaySong("Medieval_Theme_2");
         ScreenService.replaceScreen(parentScene, "event:0");
      }

      public void HandleContinueGameClick(object sender, OnClick onClick)
      {
         UnloadMainMenu();
         Scene s = SavesService.LoadGame();
         s.parentWorld = parentScene.parentWorld;
         s.onDeserialized();
         AudioService.PlaySong("Medieval_Theme_2");
         parentScene.parentWorld.scenes[0] = s;
      }

      public void HandleGraveyardClick(object sender, OnClick onClick)
      {
         UnloadMainMenu();
         InitializeGraveyard();
      }

      public void HandleAchievementClick(object sender, OnClick onClick)
      {
         UnloadMainMenu();
         InitializeAcheivementMenu();
      }

      public void HandleOptionClick(object sender, OnClick onClick)
      {
         UnloadMainMenu();
         InitializeOptionMenu();
      }

      public void HandleDeleteDataClick(object sender, OnClick onClick)
      {
         SavesService.ClearSave();
         GraveyardService.ClearGraveyard();
         AchievementService.ClearAchievements();

         ConfigService.ClearConfig();
         txtboxBackgroundMusicVolume.isFocused = false;
         VirtualKeyboardService.HideKeyboard();
         txtboxBackgroundMusicVolume.isEditing = false;
         txtboxBackgroundMusicVolume.value = (ConfigService.config.SoundVolume * 100).ToString();
      }

      public void HandleReturnToMainMenu(object sender, OnClick onClick)
      {
         if (currentMenu == "Name Selection")
         {
            UnloadNameSelection();
         }
         InitializeMainMenu();
      }
      #endregion

      #region value change handlers

      public void HandleGraveyardModelChange(object sender, OnChange change)
      {
         int newIndex = (int)change.newValue;
         ListBox graves = (ListBox)sender;
         if (newIndex != -1)
         {
            characterDetailsUI.UpdateCharacter(((GraveyardModel)graves.Items[newIndex + graves.startingIndex].Value));
         }
      }

      public void HandleUiTypeChange(object sender, OnChange change)
      {
         int newIndex = (int)change.newValue;
         ListBox uiTypes = (ListBox)sender;
         if (newIndex != -1)
         {
            var newType = (IListBoxItem)uiTypes.GetSelectedItem();
            ConfigService.config.UIType = newType.Value.ToString();
            ConfigService.SaveConfig();
         }
      }

      public void HandleTxtboxVolumeChange(object sender, OnChange onChange)
      {
         var trimedString = onChange.newValue.ToString().TrimEnd('_');
         if (int.TryParse(trimedString, out int newIntValue))
         {
            if (newIntValue > 100)
            {
               newIntValue = 100;
               ((Textbox)sender).value = "100_";
            }
            ConfigService.config.SoundVolume = (float)newIntValue / 100f;
            ConfigService.SaveConfig();
            AudioService.SetBackgroundVolume((float)newIntValue / 100f);
         }
      }

      public void HandleExportClick(object sender, OnClick onClick)
      {
         SavesService.exportSave();
      }

      public void handleImportClick(object sender, OnClick onClick)
      {
         SavesService.importSave();
         if (txtboxBackgroundMusicVolume != null)
         {
            txtboxBackgroundMusicVolume.value = (ConfigService.config.SoundVolume * 100).ToString();
            AudioService.SetBackgroundVolume(ConfigService.config.SoundVolume);
            if (ConfigService.config.UIType == "dark theme")
            {
               uiTypeList.selectedIndex = 0;
            }
            else
            {
               uiTypeList.selectedIndex = 1;
            }
         }
      }

      #endregion

      #region Deserialization
      public override void onDeserialized()
      {
         //Method not being used, and won't work as is if ever acutlly needed to be called
         newGame.onDeserialized();
         continueGame.onDeserialized();
         graveyard.onDeserialized();
      }
      #endregion
   }
}