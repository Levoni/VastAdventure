using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VastAdventure.Model;
using VastAdventure.Utility.Services;

namespace VastAdventure.Utility.Services
{
   public static class CharacterService
   {
      public static CharacterModel player;

      public static Dictionary<int, CharacterModel> baseCharacters;

      public static Dictionary<ClassType, CharacterClassModel> baseClasses;



      private static bool initialized = false;

      private static int nextAvailableCharacterId;

      public static void Initialize()
      {
         initialized = true;
         baseCharacters = new Dictionary<int, CharacterModel>();
         baseClasses = new Dictionary<ClassType, CharacterClassModel>();
         nextAvailableCharacterId = 0;

         List<CharacterModel> characters = AssetLoaderService.GetXmlSerializedAsset<List<CharacterModel>>("enemys.characters", "characters/");
         foreach(CharacterModel model in characters)
         {
            baseCharacters.Add(model.id, model);
            if (model.id >= nextAvailableCharacterId)
               nextAvailableCharacterId = model.id + 1;
         }

         List<CharacterClassModel> classes = AssetLoaderService.GetXmlSerializedAsset<List<CharacterClassModel>>("classes.classtypes", "characters/");
         foreach(CharacterClassModel model in classes)
         {
            baseClasses.Add(model.classType, model);
         }
      }
      public static void GenerateAndAddNewPlayerCharacter()
      {
         player = new CharacterModel();

         player.name = "";
         player.level = 1;
         player.id = 0;
         player.maxHealth = 138; //138
         player.strength = 8; //8
         player.toughness = 8; //8
         player.dexterity = 8; //8
         player.attiributePoints = 0;
         player.characterClass = VastAdventure.Utility.ClassType.Fighter;

         baseCharacters.Add(0, player);
      }

      public static void SaveCharactersToFile()
      {
         SavesService.SaveObjectToFile("all.characters", baseCharacters);
      }

      public static void LoadPlayerFromSave()
      {
         baseCharacters = SavesService.GetObjectFromFile<Dictionary<int, CharacterModel>>("all.characters");
         nextAvailableCharacterId = 0;
         
         foreach(KeyValuePair<int,CharacterModel> KVP in baseCharacters)
         {
            if (KVP.Value.id == 0)
            {
               player = KVP.Value;
            }
            if (KVP.Value.id >= nextAvailableCharacterId)
               nextAvailableCharacterId = KVP.Value.id + 1;
         }
      }
      public static CharacterModel getCharacter(int id)
      {
         if(baseCharacters.ContainsKey(id))
         {
            return baseCharacters[id];
         }
         return null;
      }
      public static CharacterModel getPlayer()
      {
         return player;
      }

      public static CharacterModel GenerateNewCharacter(ClassType classType)
      {
         CharacterClassModel classModel = baseClasses[classType];

         CharacterModel newCharacter = new CharacterModel();
         newCharacter.strength += classModel.StrengthStartingAmount;
         newCharacter.maxHealth += classModel.MaxHealthStartingAmount;
         newCharacter.toughness += classModel.ToughnessStartingAmount;
         newCharacter.dexterity += classModel.DexterityStartingAmount;
         newCharacter.id = nextAvailableCharacterId;

         baseCharacters.Add(newCharacter.id, newCharacter);
         nextAvailableCharacterId++;
         return newCharacter;
      }

      public static CharacterModel GenerateNewCharacter(ClassType classType, int level)
      {
         CharacterClassModel classModel = baseClasses[classType];

         CharacterModel newCharacter = new CharacterModel();
         newCharacter.id = nextAvailableCharacterId;
         baseCharacters.Add(newCharacter.id, newCharacter);
         nextAvailableCharacterId++;


         newCharacter.level = 1;
         newCharacter.name = classModel.readableClassName;
         newCharacter.strength += classModel.StrengthStartingAmount;
         newCharacter.maxHealth += classModel.MaxHealthStartingAmount;
         newCharacter.toughness += classModel.ToughnessStartingAmount;
         newCharacter.dexterity += classModel.DexterityStartingAmount;
         newCharacter.characterClass = classModel.classType;

         //Level up character to desired level
         int currentLvl = 1;
         while (currentLvl < level)
         {
            LevelUp(newCharacter.id);
            currentLvl++;
         }
         return newCharacter;
      }

      public static void LevelUp(int characterId)
      {
         CharacterModel characterToLvlup = getCharacter(characterId);
         if (characterToLvlup != null)
         {
            CharacterClassModel classModel = baseClasses[characterToLvlup.characterClass];
            characterToLvlup.level++;
            characterToLvlup.strength += classModel.StrengthLvlUpIncrease;
            characterToLvlup.maxHealth += classModel.MaxHealthLvlUpIncrease;
            characterToLvlup.toughness += classModel.ToughnessLvlUpIncrease;
            characterToLvlup.dexterity += classModel.DexterityLvlUpIncrease;
         }
      }

      public static void increaseCharacterXP(int characterId, int amount)
      {
         if (!baseCharacters.ContainsKey(characterId))
         {
            return;
         }
         CharacterModel cm = baseCharacters[characterId];
         cm.experience += amount;

         while(cm.experience >= cm.level * 100)
         {
            cm.experience -= cm.level * 100;
            if(cm.id == 0)
            {
               cm.level++;
               cm.attiributePoints += 10;
               cm.maxHealth += 6;
            }
            else
            {
               LevelUp(characterId);
            }
         }
      }

      public static void increaseCharacterLevel(int characterId, int amount)
      {
         CharacterModel cm = baseCharacters[characterId];
         cm.level += amount;
         cm.attiributePoints += 10*amount;
      }

      public static void ChangeCharacterStat(string stat, int characterId, int amount)
      {
         if(!baseCharacters.ContainsKey(characterId))
         {
            return;
         }
         CharacterModel cm = baseCharacters[characterId];
         if(stat == "experience")
         {
            increaseCharacterXP(cm.id, amount);
         }
         if(stat == "level")
         {
            increaseCharacterLevel(characterId, amount);
         }
         if(stat == "maxHealth")
         {
            cm.maxHealth += amount;
         }
         if(stat == "strength")
         {
            cm.strength += amount;
         }
         if(stat == "toughness")
         {
            cm.toughness += amount;
         }
         if(stat == "dexterity")
         {
            cm.dexterity += amount;
         }
      }
   }
}