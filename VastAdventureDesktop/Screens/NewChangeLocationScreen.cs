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
using VastAdventureDesktop.VastAdventureUI;

namespace VastAdventureDesktop.Screens
{
   [Serializable]
   public class NewChangeLocationScreen : BasicScreen
   {

      [NonSerialized]
      Texture2D underline;

      Label currentLocation;
      List<Button> buttons;
      BottomNavBar navBar;
      Dictionary<string, List<ActionNode>> ButtonActionLink;

      List<ConnectedLocationModel> connectedLocations;
      int returnScreenId;

      public NewChangeLocationScreen() : base()
      {
         ScreenType = ScreenType.ChangeLocation;
      }

      public override void Update(int dt)
      {
         for (int i = buttons.Count - 1; i >= 0; i--)
         {
            buttons[i].Update(dt);
         }
         navBar.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         sb.Draw(underline, new Rectangle(0, 0, (int)viewport.Width, (int)viewport.Height / 10), null, Color.White);
         currentLocation.Render(sb);
         foreach (Button b in buttons)
         {
            b.Render(sb);
         }
         navBar.Render(sb);
      }

      public void Init(List<ConnectedLocationModel> connectedLocations, string curLocation, int returnScreenId)
      {
         base.Init();
         ButtonActionLink = new Dictionary<string, List<ActionNode>>();
         buttons = new List<Button>();
         this.connectedLocations = connectedLocations;
         this.returnScreenId = returnScreenId;

         underline = ContentService.Get2DTexture("white_bottom_bar");

         EngineRectangle viewport = Base.Utility.Services.ScreenGraphicService.GetViewportBounds();
         //create location information label
         currentLocation = new Label("locationLabel", curLocation, new EngineRectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height / 10), Color.DarkGray);
         currentLocation.minFontScale = 0;
         currentLocation.maxFontScale = 2;
         currentLocation.padding = new int[] { 25, 100, 25, 50 };

         //Create Return Button
         Button returnButton = new Button("buttonReturn", "Return", new EngineRectangle(viewport.X, (viewport.Height / 10 * (9 - 1)) - 50, viewport.Width, viewport.Height / 10), Color.DarkGray);
         ButtonActionLink.Add(returnButton.Name, CreateReturnActionNode());
         returnButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleReturnClick));
         buttons.Add(returnButton);
         returnButton.setImageReferences("button_gray_outline_none", "button_gray_outline_hover", "button_gray_outline_pressed", "button_gray_outline_released");
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
               if (connectedLocations[i].connectionDescription == string.Empty || connectedLocations[i].connectionDescription == null)
               {
                  buttonValue = "Go To " + VastAdventure.Utility.Services.ScreenService.getLocation(connectedLocations[i].LocationId).locationName;
               }
               else
               {
                  buttonValue = connectedLocations[i].connectionDescription;
               }
               Button tempButton = new Button("button" + i, buttonValue, new EngineRectangle(viewport.X, (viewport.Height / 10 * (9 - visibleLocation - 2)) - 50, viewport.Width, viewport.Height / 10), Color.DarkGray);
               ButtonActionLink.Add(tempButton.Name, connectedLocations[i].Actions);
               tempButton.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleClick));
               buttons.Add(tempButton);
               tempButton.setImageReferences("button_gray_outline_none", "button_gray_outline_hover", "button_gray_outline_pressed", "button_gray_outline_released");
               tempButton.padding = new int[] { 0, 30, 0, 30 };
               tempButton.minFontScale = 0;
               tempButton.maxFontScale = 1;
               visibleLocation++;
            }
         }

         navBar = new BottomNavBar("NavBar", "", new EngineRectangle(0, viewport.Height / 10 * 9 - 50, viewport.Width, viewport.Height / 10), Color.DarkGray);
         navBar.parentScene = parentScene;
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
               //HandleAction(actions[currentNodeId].Action);
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
            underline = ContentService.Get2DTexture("white_bottom_bar");
            currentLocation.onDeserialized();
            navBar.onDeserialized();
            navBar.parentScene = parentScene;

            foreach (Button b in buttons)
            {
               b.onDeserialized();
            }
         }
      }
      #endregion
   }
}