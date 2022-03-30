using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using VastAdventure.DataProviders;

namespace VastAdventureDesktop.DataProviders
{
   public class DesktopAssetDataProvider : IAssetDataProvider
   {
      public DesktopAssetDataProvider()
      {
      }

      public Stream GetAssetStream(string AssetName, string AssetPath)
      {
         try
         {
            return File.OpenRead("./Assets/" + AssetPath + AssetName);
         }
         catch (Exception ex)
         {
            return null;
         }
      }
   }
}