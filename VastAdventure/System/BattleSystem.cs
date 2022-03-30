using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using System;
using System.Collections.Generic;
using VastAdventure.Component;
using VastAdventure.Events;
using VastAdventure.Utility.Services;

namespace VastAdventure.System
{
   [Serializable]
   //TODO: base system of squad components with character models inside of them. everything (attack flee pass turn) 
   // will be based on character id
   public class BattleSystem:EngineSystem
   {
      private List<Character> Characters;
      [NonSerialized]
      Random rand;
      int baseFightSpeed;

      EHandler<BasicAttackEvent> onBasicAttack;
      EHandler<PassBattleTurnEvent> onPassBattleTurn;
      EHandler<ClearBattleEntities> onClearBattleEntities;
      EHandler<SelectTaticsEvent> onSelectTatics;
      EHandler<BattleStartEvent> onBattleStart;

      public BattleSystem(Scene s)
      {
         Characters = new List<Character>();
         systemSignature = (uint)(1 << Squad.GetFamily());
         RegisterScene(s);
         registeredEntities = new List<Entity>();
         rand = new Random();
         baseFightSpeed = 0;
         Init(s);
      }

      public BattleSystem()
      {
         systemSignature = (uint)(1 << Squad.GetFamily());
         registeredEntities = new List<Entity>();
         Characters = new List<Character>();
         rand = new Random();
         baseFightSpeed = 0;
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);

         onBasicAttack = new EHandler<BasicAttackEvent>(new Action<object, BasicAttackEvent>(HandleAttack));
         onPassBattleTurn = new EHandler<PassBattleTurnEvent>(new Action<object, PassBattleTurnEvent>(HandleTimePass));
         onClearBattleEntities = new EHandler<ClearBattleEntities>(new Action<object, ClearBattleEntities>(HandleClearEntitesFromSystem));
         onSelectTatics = new EHandler<SelectTaticsEvent>(new Action<object, SelectTaticsEvent>(HandleTaticChange));
         onBattleStart = new EHandler<BattleStartEvent>(new Action<object, BattleStartEvent>(HandleBattleStart));

         parentScene.bus.Subscribe(onBasicAttack);
         parentScene.bus.Subscribe(onPassBattleTurn);
         parentScene.bus.Subscribe(onClearBattleEntities);
         parentScene.bus.Subscribe(onSelectTatics);
         parentScene.bus.Subscribe(onBattleStart);
      }

      public override void Terminate()
      {
         base.Terminate();
         parentScene.bus.Unsubscribe(onBasicAttack);
         parentScene.bus.Unsubscribe(onPassBattleTurn);
         parentScene.bus.Unsubscribe(onClearBattleEntities);
         parentScene.bus.Unsubscribe(onSelectTatics);
         parentScene.bus.Unsubscribe(onBattleStart);
      }

      public override void RegisterEnitity(Entity entity)
      {
         base.RegisterEnitity(entity);
         foreach(Character c in parentScene.GetComponent<Squad>(entity).characters)
         {
            Characters.Add(c);
         }
      }

      public override void DeregisterEntity(Entity entity)
      {
         foreach (Entity e in registeredEntities)
         {
            if (e.id == entity.id)
            {
               foreach (Character c in parentScene.GetComponent<Squad>(entity).characters)
               {
                  Characters.Remove(c);
               }
               registeredEntities.Remove(entity);
               return;
            }
         }
      }

      //TODO: make way to determine which character is the player (put flag in character component) and
      //      use that to do player checks.
      //TODO: split the time ticking and aiattcking part into its own function
      //TODO; call sepearte time ticking function at start of fight progress time till player can attack
      public void HandleAttack(object sender, BasicAttackEvent attackEvent)
      {
         // Setup variables
         string actionLog = string.Empty;
         bool playersTurn = false;
         int winningTeam;
         List<Character>  aliveCharacters = new List<Character>();

         Character attacker = null;
         Character defender = null;

         foreach(Character c in Characters)
         {
            if(c.model.id == attackEvent.attackerId)
            {
               attacker = c;
            }
            if(c.model.id == attackEvent.defenderId)
            {
               defender = c;
            }
         }

         // Get info from scene
         foreach (Character c in Characters)
         {
            if (c.health > 0)
            {
               aliveCharacters.Add(c);
            }
         }
         if(attacker == null || defender == null)
         {
            publishBattleInformation("Attacker or Defender doesn't exist", Characters);
            return;
         }

         //Perform player attack
         actionLog += calculateAttack(attacker, defender);
         if (checkForDeath(defender))
         {
            actionLog += $"\n{defender.model.name} died";
            aliveCharacters.Remove(defender);
         }
         if (EndFightCheck(aliveCharacters, attacker, out winningTeam))
         {
            publishBattleInformation(actionLog, Characters);
            PublishBattleOverEvent(new BattleOverEvent(winningTeam));
            return;
         }
         attacker.ticksTillNextAttack = (int)(100 * ((float)baseFightSpeed / (float)attacker.model.dexterity));

         PassTurn(attacker, out string passActionLog);
         actionLog += passActionLog;

         //TODO: pssibly add check to do this only if battle isn't finished
         publishBattleInformation(actionLog, Characters);
      }

      public void HandleTimePass(object sender, PassBattleTurnEvent passBattleTurnEvent)
      {
         Character character = null;
         foreach (Character c in Characters)
         {
            if (c.model.id == passBattleTurnEvent.characterId)
            {
               character = c;
            }
         }
         List<Character> allCharacters = new List<Character>();
         
         character.ticksTillNextAttack = (int)(100 * ((float)baseFightSpeed / (float)character.model.dexterity));

         PassTurn(character, out string actionLog);
         publishBattleInformation(actionLog, Characters);
      }

      //TODO: remove enemy from call by making the updateBattleInfo event take all the 
      // characters and then have the battle sceen sort out the actual updating
      private bool PassTurn(Character player, out string actionLog)
      {
         actionLog = string.Empty;
         bool playersTurn = false;
         int winningTeam;

         List<Character>  aliveCharacters = new List<Character>();


         // Get info from scene
         foreach (Character c in Characters)
         {
            if (c.health > 0)
            {
               aliveCharacters.Add(c);
            }
         }

         while (!playersTurn)
         {
            foreach (Character c in Characters)
            {
               if (c.health > 0)
               {
                  c.ticksTillNextAttack--;
                  if (c.ticksTillNextAttack <= 0)
                  {
                     if (c == player)
                     {
                        return false;
                     }
                     else
                     {
                        c.ticksTillNextAttack = (int)(100 * ((float)baseFightSpeed / (float)c.model.dexterity));
                        Character characterToAttack = determineCharacterTooAttack(c, aliveCharacters);
                        actionLog += calculateAttack(c, characterToAttack);
                        if (checkForDeath(characterToAttack))
                        {
                           actionLog += $"\n{characterToAttack.model.name} died";
                           aliveCharacters.Remove(characterToAttack);
                        }
                        if (EndFightCheck(aliveCharacters, player, out winningTeam))
                        {
                           publishBattleInformation(actionLog, Characters);
                           PublishBattleOverEvent(new BattleOverEvent(winningTeam));
                           return true;
                        }
                     }
                  }
               }
            }
         }
         return false;
      }

      private bool EndFightCheck(List<Character> aliveCharacters, Character player, out int teamWin)
      {
         bool playerDied = true;
         uint onlyOneTeamLeft = 0;
         teamWin = -1;

         foreach (Character c in aliveCharacters)
         {
            onlyOneTeamLeft |= (uint)(1 << c.team);
            if(c.model.id == player.model.id)
            {
               playerDied = false;
            }
         }
         //Three means there are people from team 0 and team 1 alive
         if(playerDied || onlyOneTeamLeft < 3)
         {
            if (onlyOneTeamLeft == 1)
               teamWin = 0;
            else if (onlyOneTeamLeft == 2)
               teamWin = 1;

            if (playerDied)
               teamWin = 1;
            return true;
         }
         return false;
      }

      private Character determineCharacterTooAttack(Character character, List<Character> possibleCharacters)
      {
         int attackerTeam = character.team;

         Random rand = new Random();

         List<Character> possibleTargests = new List<Character>();

         foreach(Character c in possibleCharacters)
         {
            if(c.team != attackerTeam)
            {
               possibleTargests.Add(c);
            }
         }

         if (possibleCharacters.Count > 0)
         {
            Character characterToAttack = null;
            if (character.tatic == Utility.TaticsType.random)
            {
               int index = rand.Next() % possibleTargests.Count;

               characterToAttack = possibleTargests[index];
            }
            else if(character.tatic == Utility.TaticsType.Strongest)
            {
               int highestStrength = 0;
               foreach(var c in possibleTargests)
               {
                  if(c.model.strength > highestStrength)
                  {
                     highestStrength = c.model.strength;
                  }
               }
               for(int i = possibleTargests.Count - 1; i >= 0; i--)
               {
                  if(possibleTargests[i].model.strength < highestStrength)
                  {
                     possibleTargests.RemoveAt(i);
                  }
               }
               int index = rand.Next() % possibleTargests.Count;
               characterToAttack = possibleTargests[index];
            }
            else if(character.tatic == Utility.TaticsType.MostVaunerable)
            {
               int lowestToughness = 9999;
               foreach (var c in possibleTargests)
               {
                  if (c.model.toughness < lowestToughness)
                  {
                     lowestToughness = c.model.toughness;
                  }
               }
               for (int i = possibleTargests.Count - 1; i >= 0; i--)
               {
                  if (possibleTargests[i].model.toughness > lowestToughness)
                  {
                     possibleTargests.RemoveAt(i);
                  }
               }
               int index = rand.Next() % possibleTargests.Count;
               characterToAttack = possibleTargests[index];
            }
            else if (character.tatic == Utility.TaticsType.Fastest)
            {
               int highestDexterity = 0;
               foreach (var c in possibleTargests)
               {
                  if (c.model.dexterity > highestDexterity)
                  {
                     highestDexterity = c.model.dexterity;
                  }
               }
               for (int i = possibleTargests.Count - 1; i >= 0; i--)
               {
                  if (possibleTargests[i].model.dexterity < highestDexterity)
                  {
                     possibleTargests.RemoveAt(i);
                  }
               }
               int index = rand.Next() % possibleTargests.Count;
               characterToAttack = possibleTargests[index];
            }
            else if (character.tatic == Utility.TaticsType.LeastHealth)
            {
               int lowestHealth = 9999;
               foreach (var c in possibleTargests)
               {
                  if (c.health < lowestHealth)
                  {
                     lowestHealth = c.health;
                  }
               }
               for (int i = possibleTargests.Count - 1; i >= 0; i--)
               {
                  if (possibleTargests[i].health > lowestHealth)
                  {
                     possibleTargests.RemoveAt(i);
                  }
               }
               int index = rand.Next() % possibleTargests.Count;
               characterToAttack = possibleTargests[index];
            }
            return characterToAttack;
         }

         return default;
      }

      private string calculateAttack(Character attacker, Character defender)
      {
         string turnInfo = string.Empty;

         float chance = 100 * ((float)attacker.model.dexterity / (float)defender.model.dexterity);
         if(chance < 20)
         {
            chance = 20;
         }
         if(chance > 90)
         {
            chance = 90;
         }

         if (chance >= rand.Next() % 100)
         {
            int damage = (attacker.model.strength * attacker.model.strength) / (attacker.model.strength + (defender.model.toughness * 2));
            if(rand.Next() % 100 < 10)
            {
               damage = (int)(damage * 1.5);
            }
            if (damage < 1)
            {
               damage = 1;
            }

            defender.health -= damage;
            if (defender.health < 0)
            {
               defender.health = 0;
            }

            turnInfo = $"\n{attacker.model.name} deals {damage} damage to {defender.model.name}";
         }
         else
         {
            turnInfo = $"\n{attacker.model.name} missed {defender.model.name}";
         }

         return turnInfo;
      }

      private bool checkForDeath(Character characterToCheck)
      {
         if(characterToCheck.health == 0)
         {
            CharacterService.getCharacter(characterToCheck.model.id).hasDied = true;
            parentScene.bus.Publish(this, new CharacterDiedEvent(characterToCheck.model.id));
            if (characterToCheck.team == 0)
            {
               SquadService.getAllySquad().characterIds.Remove(characterToCheck.model.id);
            }
            return true;
         }
         return false;
      }
      
      public void publishBattleInformation(string log, List<Character> characters)
      {
         parentScene.bus.Publish(this, new UpdateBattleHistoryEvent(log, characters));
      }

      public void PublishBattleOverEvent(BattleOverEvent battleOverEvent)
      {
         parentScene.bus.Publish(this, battleOverEvent);
      }

      public void HandleClearEntitesFromSystem(object sender, ClearBattleEntities clearBattleEntitiesEvnet)
      {
         List<Entity> entitiesToRemove = new List<Entity>();
         foreach (Entity e in registeredEntities)
         {
            entitiesToRemove.Add(e);
         }

         foreach (Entity e in entitiesToRemove)
         {
            parentScene.DestroyEntity(e);
         }
      }

      public void HandleTaticChange(object sender, SelectTaticsEvent changeSelectedTatic)
      {
         foreach(Character c in Characters)
         {
            if(c.team == 0)
            {
               c.tatic = changeSelectedTatic.Tatic;
            }
         }
      }

      public void HandleBattleStart(object sender, BattleStartEvent battleStartEvent)
      {
         int slowestDexterity = 9999;
         foreach(Character c in Characters)
         {
            if(c.model.dexterity < slowestDexterity)
            {
               slowestDexterity = c.model.dexterity;
            }
         }
         baseFightSpeed = slowestDexterity;
      }

      #region deserialization
      public override void onDeserialized()
      {
         rand = new Random();
      }
      #endregion
   }
}
