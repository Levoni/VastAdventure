using System;
using System.Collections.Generic;
using System.Linq;
using Base.Events;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VastAdventure.Model;
using Action = VastAdventure.Model.Action;
using VastAdventure.Utility;
using VastAdventure.Utility.Services;
using VastAdventure.Screens;
using VastAdventure.Events;

namespace VastAdventureDesktop.Screens
{
   [Serializable]
   public class LocationScreen : BasicScreen
   {
      [NonSerialized]
      Texture2D scroll;
      [NonSerialized]
      Texture2D table;

      LocationScreenModel locationScreenModel;

      Label locationLabel;
      List<Button> buttons;
      Dictionary<string, List<ActionNode>> ButtonActionLink;

      [NonSerialized]
      Random rand;

      public LocationScreen() : base()
      {
         ScreenType = ScreenType.Location;
      }

      public override void Update(int dt)
      {
         locationLabel.Update(dt);
         for (int i = buttons.Count - 1; i >= 0; i--)
         {
            buttons[i].Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(table, viewport.toRectangle(), Color.White);
         sb.Draw(scroll, viewport.toRectangle(), Color.White);
         locationLabel.Render(sb);
         foreach (Button b in buttons)
         {
            b.Render(sb);
         }
      }

      public void Init(int eventId)
      {
         //Setting up information
         base.Init();
         rand = new Random();
         ButtonActionLink = new Dictionary<string, List<ActionNode>>();
         buttons = new List<Button>();
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(true));
         locationScreenModel = VastAdventure.Utility.Services.ScreenService.getLocation(eventId);

         //Createing UI
         table = ContentService.Get2DTexture("table_image");
         scroll = ContentService.Get2DTexture("Scroll");


         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         //Create label informaiton
         locationLabel = new Label("locationLabel", locationScreenModel.locationName, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10 * 1, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
         locationLabel.minFontScale = 0;
         locationLabel.maxFontScale = 2;


         //Create go to change locaiton screen
         //Create Return Button
         Button changeLocation = new Button("btnMoveLocation", "Move Location", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         ButtonActionLink.Add(changeLocation.Name, CreateChangeLocationActionNode());
         changeLocation.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleClick));
         buttons.Add(changeLocation);
         changeLocation.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         changeLocation.padding = new int[] { 0, 30, 0, 30 };
         changeLocation.minFontScale = 0;
         changeLocation.maxFontScale = 1;

         var visibleOptionsCount = 0;
         //Create buttons from options
         for (int i = 0; i < locationScreenModel.Options.Count; i++)
         {
            bool precheck = true;
            foreach (Action action in locationScreenModel.Options[i].PreChecks)
            {
               if (!ActionService.PerformCheck(action))
               {
                  precheck = false;
               }
            }
            //TODO: add actual checking of prechecks
            if (precheck)
            {
               Button tempButton = new Button(i.ToString(), locationScreenModel.Options[i].OptionDescription, new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - visibleOptionsCount - 2)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
               ButtonActionLink.Add(tempButton.Name, locationScreenModel.Options[i].Actions);
               tempButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleClick));
               buttons.Add(tempButton);
               tempButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
               tempButton.padding = new int[] { 0, 30, 0, 30 };
               tempButton.minFontScale = 0;
               tempButton.maxFontScale = 1;
               visibleOptionsCount++;
            }
         }
      }

      public void HandleClick(object sender, OnClick click)
      {
         Button sendingButton = sender as Button;
         if (ButtonActionLink.ContainsKey(sendingButton.Name))
         {
            List<ActionNode> actions = ButtonActionLink[sendingButton.Name];
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
               passed = HandleAction(actions[currentNodeId].Action);
               //passed = HandleAction(actions[currentNodeId].Action);
               currentNodeId = passed ? actions[currentNodeId].PassNextNode : actions[currentNodeId].FailNextNode;
            }
            return;
         }
      }

      public bool HandleAction(VastAdventure.Model.Action action)
      {
         if (action.type == ActionType.ChangeLocationScreen)
         {
            VastAdventure.Utility.Services.ScreenService.replaceWithChangeLocaitonScreen(
               parentScene, locationScreenModel.connectedLocations,
               locationScreenModel.locationName,
               locationScreenModel.locationId);
         }
         else
         {
            return ActionService.HandleAction(action, parentScene);
         }

         return true;
      }

      private List<ActionNode> CreateChangeLocationActionNode()
      {
         Action goToChangeLocationAction = new Action();
         goToChangeLocationAction.type = ActionType.ChangeLocationScreen;
         goToChangeLocationAction.value = "changeLocation";

         ActionNode actionNode = new ActionNode();
         actionNode.Action = goToChangeLocationAction;
         actionNode.FailNextNode = -1;
         actionNode.NodeId = 0;
         actionNode.PassNextNode = -1;

         List<ActionNode> actionNodes = new List<ActionNode>();
         actionNodes.Add(actionNode);
         return actionNodes;
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
            locationLabel.onDeserialized();

            foreach (Button ui in buttons)
            {
               ui.onDeserialized();
            }
         }
      }
      #endregion
   }
}