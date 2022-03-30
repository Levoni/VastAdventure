using Base;
using Base.Entities;
using Base.Scenes;
using Base.System;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using VastAdventure.DataProviders;
using VastAdventure.Model;
using VastAdventure.System;
using VastAdventure.Utility.Services;
using VastAdventureDesktop.DataProviders;
using VastAdventureDesktop.Screens;
using VastAdventureDesktop.VastAdventureUI;

namespace VastAdventureDesktop
{
   /// <summary>
   /// This is the main type for your game.
   /// </summary>
   public class Game1 : Game
   {
      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;
      World w;
      Scene s;


      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";

         graphics.IsFullScreen = false;
         graphics.PreferredBackBufferWidth = ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100) / 16) * 9;
         graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
         IsMouseVisible = true;
        
      }

      //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         // TODO: Add your initialization logic here


         //deserializeTest test = new deserializeTest();

         //ScreenModel eventScreenModel;
         //eventScreenModel = new ScreenModel();
         //eventScreenModel.EventId = 1;
         //eventScreenModel.EventLocation = "Forest";
         //eventScreenModel.EventDescription = "You awake in as forest, What do you do?";
         //eventScreenModel.ImageReference = null;
         //eventScreenModel.Options = new List<Option>();

         //Option eOne = new Option();
         //eOne.Actions = new List<ActionNode>();
         //ActionNode node = new ActionNode();
         //node.Action = new Action();
         //node.Action.type = VastAdventure.Utility.ActionType.ChangeScreen;
         //node.Action.value = "3";

         //eOne.Actions.Add(node);

         //eventScreenModel.Options.Add(eOne);

         //List<ScreenModel> list = new List<ScreenModel>();
         //list.Add(eventScreenModel);


         //Base.Serialization.Serializer<List<ScreenModel>>.SerializeToFile(list,"test.screen");

         //var test = Base.Serialization.Serializer<List<ScreenModel>>.DeserializeFromFile("test.screen");



         base.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
         graphics.ApplyChanges();
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch(GraphicsDevice);

         IAssetDataProvider assetProvider = new DesktopAssetDataProvider();
         AssetLoaderService.Initialize(assetProvider);
         IScreenProvider screenProvider = new DesktopScreenProvider();
         VastAdventure.Utility.Services.ScreenService.Init(screenProvider);
         SavesService.Initialize(new DesktopFileProvider());
         FlagService.Initialize();
         CharacterService.Initialize();
         InventoryService.Initialize();
         SquadService.Initialize();
         GraveyardService.Initialize();
         AchievementService.Initialize();
         


         KeyboardService.InitService();
         MouseService.InitService();

         Base.Utility.Services.ScreenGraphicService.InitializeService(graphics);
         ContentService.InitService(Content);
         CameraService.InitSystem();
         CameraService.camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
         CameraService.camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
         CameraService.camera.ClampBounds = new EngineRectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
         CameraService.camera.MinZoom = 1;
         CameraService.camera.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
         AudioService.Initialize(Content);
         AudioService.SetBackgroundVolume(.5f);
         AudioService.AddSong("Battle_Loop_1");
         AudioService.AddSong("Battle_Loop_4");
         AudioService.AddSong("Medieval_Theme_1");
         AudioService.AddSong("Medieval_Theme_2");
         AudioService.AddSong("LastCallForAHero");
         AudioService.PlaySong("Medieval_Theme_1");
         ConfigService.Initialize();

         w = new World();
         s = new Scene(w);

         MainMenu mm = new MainMenu();
         Entity menuEntity = s.CreateEntity();
         mm.Init(s.bus, s);
         mm.Init(menuEntity);
         s.AddComponent(menuEntity, mm);


         GUISystem gs = new GUISystem(s);
         BattleSystem BS = new BattleSystem();
         BS.Init(s);

         AnimationSystem animationSystem = new AnimationSystem();
         AchievementSystem achievementSystem = new AchievementSystem();
         achievementSystem.UiInterface = new AchievementUI(new AchievementModel(), new EngineRectangle());

         s.AddSystem(animationSystem);
         s.AddSystem(BS);
         s.AddSystem(gs);
         s.AddSystem(achievementSystem);

         CharacterInfoOverlay cOverlay = new CharacterInfoOverlay();
         cOverlay.Init();
         s.AddComponent(s.CreateEntity(), cOverlay);



         w.AddScene(s);

         //SavesService.exportSave();
         //SavesService.importSave();
         // TODO: use this.Content to load your game content here
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// game-specific content.
      /// </summary>
      protected override void UnloadContent()
      {
         // TODO: Unload any non ContentManager content here
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update(GameTime gameTime)
      {
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

         // TODO: Add your update logic here
         KeyboardService.UpdateKeyboardState();
         MouseService.UpdateMouseState();
         w.UpdateScenes(gameTime);

         base.Update(gameTime);
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.Black);

         // TODO: Add your drawing code here

         w.RenderScenes(spriteBatch);

         base.Draw(gameTime);
      }
   }
}
