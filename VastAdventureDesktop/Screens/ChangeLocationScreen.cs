using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Events;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VastAdventure.Model;
using VastAdventure.Utility;
using Action = VastAdventure.Model.Action;
using VastAdventure.Screens;
using VastAdventure.Utility.Services;
using System.Runtime.Serialization;
using VastAdventure.Events;

namespace VastAdventureDesktop.Screens
{
   [Serializable]
   public class ChangeLocationScreen : BasicScreen
   {
      [NonSerialized]
      Texture2D scroll;
      [NonSerialized]
      Texture2D table;

      Label currentLocation;
      List<Button> buttons;
      Dictionary<string, List<ActionNode>> ButtonActionLink;

      List<ConnectedLocationModel> connectedLocations;
      int returnScreenId;

      public ChangeLocationScreen() : base()
      {
         ScreenType = ScreenType.ChangeLocation;
      }

      public override void Update(int dt)
      {
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
         currentLocation.Render(sb);
         foreach (Button b in buttons)
         {
            b.Render(sb);
         }
      }

      public void Init(List<ConnectedLocationModel> connectedLocations, string curLocation, int returnScreenId)
      {
         base.Init();
         ButtonActionLink = new Dictionary<string, List<ActionNode>>();
         buttons = new List<Button>();
         this.connectedLocations = connectedLocations;
         this.returnScreenId = returnScreenId;

         table = ContentService.Get2DTexture("table_image");
         scroll = ContentService.Get2DTexture("Scroll");

         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         //create location information label
         currentLocation = new Label("locationLabel", curLocation, new EngineRectangle(viewport.X + 50, viewport.Y + 50 + viewport.Height / 10 * 1, (viewport.Width - 100) / 2, viewport.Height / 10), Color.Black);
         currentLocation.minFontScale = 0;
         currentLocation.maxFontScale = 2;

         //Create Return Button
         Button returnButton = new Button("buttonReturn", "Return", new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - 1)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
         ButtonActionLink.Add(returnButton.Name, CreateReturnActionNode());
         returnButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleReturnClick));
         buttons.Add(returnButton);
         returnButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
         returnButton.padding = new int[] { 0, 30, 0, 30 };
         returnButton.minFontScale = 0;
         returnButton.maxFontScale = 1;

         var visibleLocation = 0;
         //Create all the buttons for changing locations
         for (int i = 0; i < connectedLocations.Count; i++)
         {
            bool precheckPassed = true;
            foreach (Action a in connectedLocations[i].PreChecks)
            {
               if (!ActionService.PerformCheck(a))
               {
                  precheckPassed = false;
               }
            }
            if (precheckPassed)
            {
               string buttonValue = string.Empty;
               if(connectedLocations[i].connectionDescription == string.Empty || connectedLocations[i].connectionDescription == null)
               {
                  buttonValue = "Go To " + VastAdventure.Utility.Services.ScreenService.getLocation(connectedLocations[i].LocationId).locationName;
               }
               else
               {
                  buttonValue = connectedLocations[i].connectionDescription;
               }
               Button tempButton = new Button("button" + i, buttonValue, new EngineRectangle(viewport.X + 50, (viewport.Height / 10 * (9 - visibleLocation - 2)) - 50, viewport.Width - 100, viewport.Height / 10), Color.Black);
               ButtonActionLink.Add(tempButton.Name, connectedLocations[i].Actions);
               tempButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleClick));
               buttons.Add(tempButton);
               tempButton.setImageReferences("button_outline_none", "button_outline_hover", "button_outline_pressed", "button_outline_released");
               tempButton.padding = new int[] { 0, 30, 0, 30 };
               tempButton.minFontScale = 0;
               tempButton.maxFontScale = 1;
               visibleLocation++;
            }
         }
      }

      public void handleReturnClick(object sender, OnClick click)
      {
         parentScene.bus.Publish(null, new SetCharacterInfoButtonVisabilityEvent(true));
         HandleClick(sender, click);
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
               passed = ActionService.HandleAction(actions[currentNodeId].Action, parentScene);
               currentNodeId = passed ? actions[currentNodeId].PassNextNode : actions[currentNodeId].FailNextNode;
            }
            return;
         }
      }

      private List<ActionNode> CreateReturnActionNode()
      {
         //Make this a go to previous screen action not a change screen action
         Action returnAction = new Action();
         returnAction.type = ActionType.PreviousScreen;
         returnAction.value = string.Empty;

         ActionNode actionNode = new ActionNode();
         actionNode.Action = returnAction;
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
         if (ContentService.isInitialized)
         {
            table = ContentService.Get2DTexture("table_image");
            scroll = ContentService.Get2DTexture("Scroll");
            currentLocation.onDeserialized();

            foreach (Button b in buttons)
            {
               b.onDeserialized();
            }
         }
      }
      #endregion
   }
}