using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Views.InputMethods;
using Base.Utility.Services;

namespace VastAdventureAndriod
{
   [Activity(Label = "Vast Adventure"
       , MainLauncher = true
       , Icon = "@drawable/Icon"
       , Theme = "@style/Theme.Splash"
       , AlwaysRetainTaskState = true
       , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
       , ScreenOrientation = ScreenOrientation.Portrait
       , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
   public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
   {
      public static View view;
      public static Game1 g;
      protected override void OnCreate(Bundle bundle)
      {
         //Andriod specific setup stuff
         base.OnCreate(bundle);
         this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
         if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted ||
            ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
         {
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 1);
         }

         //Create Game
         g = new Game1();
         g.InitializeAssetLoader(this.Assets,this);
         
         //Create Keyboard handler stuff
         view = (View)g.Services.GetService(typeof(View));
         var inputMethodManager = Application.GetSystemService(InputMethodService) as InputMethodManager;
         VirtualKeyboardService.initialize(view, inputMethodManager);
         SetContentView(view);

         //Andriod specific setup stuff
         SetImmersive();

         //Run
         g.Run();
      }

      private void SetImmersive()
      {
         if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
         {
            view.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutHideNavigation | SystemUiFlags.LayoutFullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky);
         }
      }

      public override void OnWindowFocusChanged(bool hasFocus)
      {
         base.OnWindowFocusChanged(hasFocus);

         if (hasFocus)
            SetImmersive();

         if (ScreenGraphicService.checkInitialized())
         {
            ScreenGraphicService.graphics.GraphicsDevice.Viewport = new Microsoft.Xna.Framework.Graphics.Viewport(0, 0, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
         }
      }

      protected override void OnResume()
      {
         base.OnResume();
         if (ScreenGraphicService.checkInitialized())
         {
            ScreenGraphicService.graphics.GraphicsDevice.Viewport = new Microsoft.Xna.Framework.Graphics.Viewport(0, 0, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, ScreenGraphicService.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
         }
      }

      public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
      {
         base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      }
   }
}

