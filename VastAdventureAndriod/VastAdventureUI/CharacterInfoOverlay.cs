using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Events;
using Base.Scenes;
using Base.UI.Mobile;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.Events;
using VastAdventure.Utility.Services;

namespace VastAdventureAndriod.VastAdventureUI
{
   [Serializable]
   public class CharacterInfoOverlay:Base.UI.GUI
   {
      Button goToCharacterInfo;
      EHandler<SetCharacterInfoButtonVisabilityEvent> visibilityEvent;

      public CharacterInfoOverlay():base()
      {
        
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         if (ConfigService.config.UIType != "dark theme")
         {
            goToCharacterInfo.Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         //TODO: possibly use a flag/stored value to check if button should be updated/rendered (isViewingCharacterInfo flag)
         if (ConfigService.config.UIType != "dark theme")
         {
            goToCharacterInfo.Render(sb);
         }
      }

      public override void Init(EventBus ebus, Scene parentScene)
      {
         base.Init(ebus, parentScene);
         EngineRectangle viewport = ScreenGraphicService.GetViewportBounds();
         goToCharacterInfo = new Button("btnGoToCharacterInfo", "", new Base.Utility.EngineRectangle(50,viewport.Y + 50, viewport.Width / 10, viewport.Width / 10), Microsoft.Xna.Framework.Color.Black);
         goToCharacterInfo.minFontScale = 0;
         goToCharacterInfo.maxFontScale = 2;
         goToCharacterInfo.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleButtonPress));
         goToCharacterInfo.setImageReferences("wax_seal_squad_none", "wax_seal_squad_hover", "wax_seal_squad_hover", "wax_seal_squad_none");
         goToCharacterInfo.isEnabled = false;
         visibilityEvent = new EHandler<SetCharacterInfoButtonVisabilityEvent>(new Action<object, SetCharacterInfoButtonVisabilityEvent>(handleVisibilityChange));
         parentScene.bus.Subscribe(visibilityEvent);
      }

      private void handleButtonPress(object sender, OnClick clickEvent)
      {
         goToCharacterInfo.isEnabled = false;
         VastAdventure.Utility.Services.ScreenService.replaceScreen(parentScene, "characterInfo:-1");
      }

      public void handleVisibilityChange(object sender, SetCharacterInfoButtonVisabilityEvent visibilityEvent)
      {
         goToCharacterInfo.isEnabled = visibilityEvent.isVisible;
      }

      #region deserialization
      public override void onDeserialized()
      {
         if (ContentService.isInitialized)
         {
            goToCharacterInfo.onDeserialized();
         }
      }
      #endregion
   }
}