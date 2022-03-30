using System;
using System.Collections.Generic;
using System.Linq;
using Base.Events;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VastAdventure.Model;
using VastAdventure.Screens;
using VastAdventure.Utility.Services;
using Action = VastAdventure.Model.Action;

namespace VastAdventureDesktop.Screens
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
            foreach (Action action in eventScreenModel.Options[i].PreChecks)
            {
               if (!ActionService.PerformCheck(action))
               {
                  precheck = false;
               }
            }
            //TODO: add actual checking of prechecks
            if (precheck)
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

         descriptionLabel = new Label("descriptionLabel", eventScreenModel.EventDescription, new EngineRectangle(viewport.X + 50, viewport.Y + viewport.Height / 10 * 2, viewport.Width - 100, ((viewport.Height / 10 * (9 - (visibleOptions - 1) - 1)) - 50) - (viewport.Y + viewport.Height / 10 * 2)), Color.Black);
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
               passed = ActionService.HandleAction(actions[currentNodeId].Action, parentScene);
               currentNodeId = passed ? actions[currentNodeId].PassNextNode : actions[currentNodeId].FailNextNode;
            }
            return;
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