using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VastAdventure.DataProviders
{
   public interface IFileProvider
   {
      Stream GetFileStream(string filename);

      Stream GetExportFileStream(string filename);

      string GetDirectoryPath();

      string GetExportDirectoryPath();
   }
}
