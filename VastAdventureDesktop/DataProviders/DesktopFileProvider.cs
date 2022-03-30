using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using VastAdventure.DataProviders;

namespace VastAdventureDesktop.DataProviders
{
   public class DesktopFileProvider : IFileProvider
   {
      public Stream GetFileStream(string filename)
      {
         string path =  Path.Combine(GetDirectoryPath(),filename);
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
         return Directory.GetCurrentDirectory();
      }

      public string GetExportDirectoryPath()
      {
         return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"My games", "VastAdventure");
      }
   }
}