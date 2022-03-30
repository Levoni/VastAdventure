using Android;
using Android.Content.Res;
using Android.Support.V4.App;
using Android.Views.InputMethods;
using Base;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VastAdventure.DataProviders;
using VastAdventure.Model;
using VastAdventure.System;
using VastAdventure.Utility.Services;
using VastAdventureAndriod.DataProviders;
using VastAdventureAndriod.Screens;
using VastAdventureAndriod.VastAdventureUI;

namespace VastAdventureAndriod
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

      public AssetManager Assets { get; set; }

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         graphics.IsFullScreen = true;
         graphics.SupportedOrientations = DisplayOrientation.Portrait;
         //ScreenGraphicService.graphics.GraphicsDevice.Viewport = new Viewport(0, 0, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
         graphics.ApplyChanges();
      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         // TODO: Add your initialization logic here
         base.Initialize();
      }

      public void InitializeAssetLoader(AssetManager asset, Activity1 app)
      {
         this.Assets = asset;
         IAssetDataProvider assetProvider = new AndriodAssetDataProvider(asset);
         AssetLoaderService.Initialize(assetProvider);
         IScreenProvider screenProvider = new AndriodScreenProvider();
         VastAdventure.Utility.Services.ScreenService.Init(screenProvider);
         SavesService.Initialize(new AndriodFileProvider());
         FlagService.Initialize();
         CharacterService.Initialize();
         InventoryService.Initialize();
         SquadService.Initialize();
         GraveyardService.Initialize();
         AchievementService.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch(GraphicsDevice);
         graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
         graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
         graphics.ApplyChanges();

         // TODO: use this.Content to load your game content here
         ScreenGraphicService.InitializeService(graphics);
         ScreenGraphicService.graphics.GraphicsDevice.Viewport = new Viewport(0, 0, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
         ContentService.InitService(Content);
         ContentService.usesCache = true;
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

         //string[] temparray = Assets.List("");
         //string[] temparray26 = Assets.List("screens");

         //using (Stream sw = Assets.Open("screens/events.screen"))
         //{
         //   XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<EventScreen>));
         //   var temp = xmlSerializer.Deserialize(sw);
         //}

         w = new World();
         s = new Scene(w);

         //EventScreen ds = new EventScreen();
         //ds.Init(0);
         //CharacterInformationScreen cis = new CharacterInformationScreen();
         //cis.Init();

         CharacterInfoOverlay cOverlay = new CharacterInfoOverlay();
         cOverlay.Init();
         s.AddComponent(s.CreateEntity(), cOverlay);
         //ScreenServiceAndriod.replaceScreen(s, "event:0");

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

         w.AddScene(s);
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
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();

         // TODO: Add your update logic here

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
