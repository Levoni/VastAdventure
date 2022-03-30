using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VastAdventure.DataProviders;

namespace VastAdventureAndriod.DataProviders
{
   public class AndriodAssetDataProvider : IAssetDataProvider
   {
      AssetManager assetManager;
      public AndriodAssetDataProvider(AssetManager asset)
      {
         this.assetManager = asset;
      }

      public Stream GetAssetStream(string AssetName, string AssetPath)
      {
         try
         {
            return assetManager.Open($"{AssetPath}{AssetName}");
         }
         catch (Exception ex)
         {
            return null;
         }
      }
   }
}