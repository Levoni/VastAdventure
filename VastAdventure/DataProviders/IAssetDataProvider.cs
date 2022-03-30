using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VastAdventure.DataProviders
{
   public interface IAssetDataProvider
   {
      Stream GetAssetStream(string AssetName, string AssetPath);
   }
}
