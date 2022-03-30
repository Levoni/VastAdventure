using Base.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace VastAdventure.Model
{
   [Serializable]
   public class VastAdventureSaveFile:SaveFile
   {
      public bool isCurrentSave;
   }
}
