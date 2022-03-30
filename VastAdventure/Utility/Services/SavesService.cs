using Base.Scenes;
using Base.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using VastAdventure.DataProviders;
using VastAdventure.Model;

namespace VastAdventure.Utility.Services
{
   public static class SavesService
   {

      static IFileProvider fileProvider;
      static public VastAdventureSaveFile saveFile;
      const string SaveVersion = "1.4";
      const string fileName = "save.save";
      static public bool isInitialized = false;

      public static void Initialize(IFileProvider provider)
      {
         fileProvider = provider;
         isInitialized = true;
         saveFile = GetObjectFromFile<VastAdventureSaveFile>(fileName);
         if (saveFile == null)
         {
            saveFile = CreateBasicSave();
         }
      }

      public static T GetObjectFromFile<T>(string fileName)
      {
         if (isInitialized)
         {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream fileStream = null;
            try
            {
               fileStream = fileProvider.GetFileStream(fileName);
               T obj = (T)formatter.Deserialize(fileStream);
               fileStream.Dispose();
               fileStream.Close();
               return obj;
            }
            catch (Exception ex)
            {
               return default;
            }
            finally
            {
               if (fileStream != null)
               {
                  fileStream.Dispose();
                  fileStream.Close();
               }
            }
         }
         // TODO: change to throw exception
         return default;
      }

      public static bool SaveObjectToFile(string fileName, object objectToSerialize)
      {
         if (isInitialized)
         {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream fileStream = null;
            try
            {
               fileStream = fileProvider.GetFileStream(fileName);
               formatter.Serialize(fileStream, objectToSerialize);
               return true;
            }
            catch (Exception ex)
            {
               return false;
            }
            finally
            {
               if (fileStream != null)
               {
                  fileStream.Dispose();
                  fileStream.Close();
               }
            }
         }
         else
         {
            return false;
         }
      }

      public static T GetObjectFromImportFile<T>(string fileName)
      {
         if (isInitialized)
         {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream fileStream = null;
            try
            {
               fileStream = fileProvider.GetExportFileStream(fileName);
               T obj = (T)formatter.Deserialize(fileStream);
               fileStream.Dispose();
               fileStream.Close();
               return obj;
            }
            catch (Exception ex)
            {
               return default;
            }
            finally
            {
               if (fileStream != null)
               {
                  fileStream.Dispose();
                  fileStream.Close();
               }
            }
         }
         // TODO: change to throw exception
         return default;
      }

      public static bool SaveObjectToExportFile(string fileName, object objectToSerialize)
      {
         if (isInitialized)
         {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream fileStream = null;
            try
            {
               fileStream = fileProvider.GetExportFileStream(fileName);
               formatter.Serialize(fileStream, objectToSerialize);
               fileStream.Dispose();
               fileStream.Close();
               return true;
            }
            catch (Exception ex)
            {
               return false;
            }
            finally
            {
               if (fileStream != null)
               {
                  fileStream.Dispose();
                  fileStream.Close();
               }
            }
         }
         else
         {
            return false;
         }
      }

      public static bool DeleteFile(string filename)
      {
         if (isInitialized && File.Exists(Path.Combine(fileProvider.GetDirectoryPath(), filename)))
         {
            try
            {

               File.Delete(Path.Combine(fileProvider.GetDirectoryPath(), filename));
               return true;
            }
            catch (Exception ex)
            {
               return false;
            }
         }
         else
         {
            return false;
         }
      }

      //public static bool IsCurrentSave()
      //{
      //   if (isInitialized)
      //   {
      //      string path = fileProvider.GetDirectoryPath() + Path.DirectorySeparatorChar + fileName;
      //      if (!File.Exists(path))
      //      {
      //         bool currentSave = false;
      //         SaveObjectToFile(path, currentSave);
      //         return false;
      //      }
      //      BinaryFormatter formatter = new BinaryFormatter();
      //      Stream fileStream = fileProvider.GetFileStream(fileName);
      //      bool isSave = (bool)formatter.Deserialize(fileStream);
      //      fileStream.Dispose();
      //      fileStream.Close();
      //      return isSave;
      //   }
      //   return false;
      //}

      public static void SaveGame(Scene sceneToSave)
      {
         saveFile.isCurrentSave = true;
         saveFile.SaveVersion = SaveVersion;
         SaveObjectToFile(saveFile.FileName, saveFile);

         SquadService.SavePlayerSquadToFile();

         CharacterService.SaveCharactersToFile();

         FlagService.saveFlagCollectionToFile();

         InventoryService.saveInventory();

         ScreenService.SaveNavigationInformation();

         SaveObjectToFile("scene.scene", sceneToSave);
      }

      public static Scene LoadGame()
      {
         saveFile = GetObjectFromFile<VastAdventureSaveFile>(fileName);

         SquadService.LoadPlayerSquadFromSave();

         CharacterService.LoadPlayerFromSave();

         FlagService.LoadFlagCollection();

         InventoryService.LoadInventory();

         ScreenService.LoadnavigationInformation();

         Scene scene = GetObjectFromFile<Scene>("scene.scene");

         saveFile.SaveVersion = SaveVersion;

         return scene;
      }

      public static void ClearSave()
      {
         saveFile.isCurrentSave = false;
         SaveObjectToFile(saveFile.FileName, saveFile);
      }

      public static VastAdventureSaveFile CreateBasicSave()
      {
         VastAdventureSaveFile saveFile = new VastAdventureSaveFile();
         saveFile.currentLevel = 0;
         saveFile.FileName = fileName;
         saveFile.SaveVersion = SaveVersion;
         saveFile.isCurrentSave = false;
         return saveFile;
      }

      public static void exportSave()
      {
         if (!Directory.Exists(fileProvider.GetExportDirectoryPath()))
         {
            Directory.CreateDirectory(fileProvider.GetExportDirectoryPath());
         }
         var tempCurrentSave = saveFile.isCurrentSave;
         saveFile.isCurrentSave = false;

         //Export code
         SaveObjectToExportFile(saveFile.FileName, saveFile);
         SaveObjectToExportFile("graveyard.graveyard", GraveyardService.GetGraveyard());
         SaveObjectToExportFile("achievements.achievements", AchievementService.achievements);
         SaveObjectToExportFile("settings.config", ConfigService.config);

         saveFile.isCurrentSave = tempCurrentSave;
      }

      public static void importSave()
      {
         if (Directory.Exists(fileProvider.GetExportDirectoryPath()) &&
            File.Exists(Path.Combine(fileProvider.GetExportDirectoryPath(),saveFile.FileName)) &&
            File.Exists(Path.Combine(fileProvider.GetExportDirectoryPath(), "graveyard.graveyard")) &&
            File.Exists(Path.Combine(fileProvider.GetExportDirectoryPath(), "achievements.achievements")) &&
            File.Exists(Path.Combine(fileProvider.GetExportDirectoryPath(), "settings.config")))
         {
            bool isCurrentSave = saveFile.isCurrentSave;
            saveFile = GetObjectFromImportFile<VastAdventureSaveFile>(saveFile.FileName);
            saveFile.isCurrentSave = isCurrentSave;
            GraveyardService.graveyard = GetObjectFromImportFile<List<GraveyardModel>>("graveyard.graveyard");
            SaveObjectToFile("Graveyard.graveyard", GraveyardService.graveyard);
            AchievementService.achievements = GetObjectFromImportFile<Dictionary<string, AchievementModel>>("achievements.achievements");
            AchievementService.SaveAchievements();
            ConfigService.config = GetObjectFromImportFile<ConfigModel>("settings.config");
            ConfigService.SaveConfig();

            if (saveFile.SaveVersion == "1.3")
            {
               if (ConfigService.config.UIType == null)
               {
                  ConfigService.config.UIType = "dark theme";
               }
            }
            saveFile.SaveVersion = SaveVersion;
         }
      }
   }
}
