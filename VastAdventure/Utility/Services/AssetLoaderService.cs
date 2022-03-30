using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using VastAdventure.DataProviders;

namespace VastAdventure.Utility.Services
{
   public static class AssetLoaderService
   {
      static IAssetDataProvider assetProvider;
      static public bool isInitialized = false;

      public static void Initialize(IAssetDataProvider provider)
      {
         assetProvider = provider;
         isInitialized = true;
      }

      public static Stream GetAssetStream(string AssetName, string AssetPath)
      {
         try
         {
            return assetProvider.GetAssetStream(AssetName, AssetPath);
         }
         catch (Exception ex)
         {
            return null;
         }
      }

      public static T GetXmlSerializedAsset<T>(string AssetName, string AssetPath)
      {
         try
         {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(assetProvider.GetAssetStream(AssetName, AssetPath)))
            {
               return (T)serializer.Deserialize(reader);
            }
         }
         catch (Exception ex)
         {
            return default;
         }
      }
   }
}
