using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VastAdventure.DataProviders;

namespace VastAdventureAndriod.DataProviders
{
   public class AndriodFileProvider : IFileProvider
   {
      public Stream GetFileStream(string filename)
      {
         string path = Path.Combine(GetDirectoryPath(), filename);
         if(File.Exists(path))
         {
            return File.Open(path, FileMode.Open,FileAccess.ReadWrite);
         }
         else
         {
            return File.Open(path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
         }
      }

      public Stream GetExportFileStream(string filename)
      {
         string path = Path.Combine(GetExportDirectoryPath(), filename);
         if (File.Exists(path))
         {
            return File.Open(path, FileMode.Open, FileAccess.ReadWrite);
         }
         else
         {
            return File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
         }
      }

      public string GetDirectoryPath()
      {
         return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
      }

      public string GetExportDirectoryPath()
      {
         return Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory,"VastAdventure");
      }
   }
}