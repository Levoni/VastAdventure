using Base.UI;
using Base.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VastAdventure.Model;

namespace VastAdventureDesktop.VastAdventureUI
{
   [Serializable]
   public class GraveyardModelDetailsUI: Control
   {
      public CharacterModel character;

      //Character
      Label name;
      Label level;
      Label experience;
      Label maxHealth;
      Label strength;
      Label toughness;
      Label dexterity;
      Label attributePoints;

      Label inventoryHeader;
      List<Control> inventoryControls;

      public GraveyardModelDetailsUI()
      {
         base.init();
      }

      public GraveyardModelDetailsUI(GraveyardModel graveyardModel, EngineRectangle bounds) : base()
      {
         base.init();
         inventoryControls = new List<Control>();
         this.character = graveyardModel.player;

         // Left Side (Character)
         name = new Label("lblName", "Name: " + character.name, new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 1, bounds.Width / 2, bounds.Height / 9), Color.Black);
         name.padding[3] = name.padding[1] = 25;
         name.minFontScale = 0;

         level = new Label("lblLevel", "Level: " + character.level.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 2, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         level.padding[3] = level.padding[1] = 25;
         level.minFontScale = 0;

         experience = new Label("lblExperience", "XP: " + character.experience.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 3, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         experience.padding[3] = experience.padding[1] = 25;
         experience.minFontScale = 0;

         maxHealth = new Label("lblMaxHealth", "Max Health: " + character.maxHealth.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 4, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         maxHealth.padding[3] = maxHealth.padding[1] = 25;
         maxHealth.minFontScale = 0;

         strength = new Label("lblStrength", "Strength: " + character.strength.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 5, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         strength.padding[3] = strength.padding[1] = 25;
         strength.minFontScale = 0;

         toughness = new Label("lblToughness", "Toughness: " + character.toughness.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 6, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         toughness.padding[3] = toughness.padding[1] = 25;
         toughness.minFontScale = 0;

         dexterity = new Label("lblDexterity", "Dexterity: " + character.dexterity.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 7, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         dexterity.padding[3] = dexterity.padding[1] = 25;
         dexterity.minFontScale = 0;

         attributePoints = new Label("lblAttributePoints", "Left Over Attribute Points: " + character.attiributePoints.ToString(), new EngineRectangle(bounds.X, bounds.Y + bounds.Height / 9 * 8, bounds.Width / 2, (bounds.Height / 9)), Color.Black);
         attributePoints.padding[3] = attributePoints.padding[1] = 25;
         attributePoints.minFontScale = 0;

         // Right Side (Inventory)
         inventoryHeader = new Label("lblInventoryHeader", "Inventory - Gold: ", new EngineRectangle(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 9 * 1, bounds.Width / 2, bounds.Height / 9), Color.Black);
         inventoryHeader.padding[3] = inventoryHeader.padding[1] = 25;
         inventoryHeader.minFontScale = 0;
         Inventory inventory = graveyardModel.inventory;
         inventoryHeader.value += inventory.Gold.ToString();
         inventoryControls.Add(inventoryHeader);

         for (int i = 0; i < inventory.items.Count; i++)
         {
            Label newLabel = new Label("Item:" + i.ToString(), "- " + inventory.items[i].itemDescription, new EngineRectangle(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 9 * (1 * i + 2), bounds.Width / 2, bounds.Height / 9), Color.Black);
            newLabel.padding[3] = newLabel.padding[1] = 25;
            newLabel.minFontScale = 0;
            inventoryControls.Add(newLabel);
         }

      }

      public override void Update(int dt)
      {
         if (character != null)
         {
            name.Update(dt);
            level.Update(dt);
            experience.Update(dt);
            maxHealth.Update(dt);
            strength.Update(dt);
            toughness.Update(dt);
            dexterity.Update(dt);
            attributePoints.Update(dt);
            foreach(Control c in inventoryControls)
            {
               c.Update(dt);
            }
         }
      }

      public override void Render(SpriteBatch sb)
      {
         if (character != null)
         {
            name.Render(sb);
            level.Render(sb);
            experience.Render(sb);
            maxHealth.Render(sb);
            strength.Render(sb);
            toughness.Render(sb);
            dexterity.Render(sb);
            attributePoints.Render(sb);
            foreach (Control c in inventoryControls)
            {
               c.Render(sb);
            }
         }
      }

      public void UpdateCharacter(GraveyardModel graveyardModel)
      {
         character = graveyardModel.player;
         name.value = "Name: " + graveyardModel.player.name.ToString();
         level.value = "Level: " + graveyardModel.player.level.ToString();
         experience.value = "XP: " + graveyardModel.player.experience.ToString();
         maxHealth.value = "Max Health: " + graveyardModel.player.maxHealth.ToString();
         strength.value = "Strength: " + graveyardModel.player.strength.ToString();
         toughness.value = "Toughness: " + graveyardModel.player.toughness.ToString();
         dexterity.value = "Dexterity: " + graveyardModel.player.dexterity.ToString();
         attributePoints.value = "Left Over Attribute Points: " + graveyardModel.player.attiributePoints.ToString();



         // Inventory
         inventoryControls.Clear();
         inventoryHeader.value = "Inventory - Gold: ";
         inventoryHeader.value += graveyardModel.inventory.Gold.ToString();
         inventoryControls.Add(inventoryHeader);

         for (int i = 0; i < graveyardModel.inventory.items.Count; i++)
         {
            Label newLabel = new Label("Item:" + i.ToString(), "- " + graveyardModel.inventory.items[i].itemDescription, new EngineRectangle(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 9 * (1 * i + 2), bounds.Width / 2, bounds.Height / 9), Color.White);
            newLabel.padding[3] = newLabel.padding[1] = 25;
            inventoryControls.Add(newLabel);
         }
      }

      public override void onDeserialized()
      {
         base.onDeserialized();
         //Isn't called right now. If it ever needs to be updated, update it so it works.
      }
   }
}