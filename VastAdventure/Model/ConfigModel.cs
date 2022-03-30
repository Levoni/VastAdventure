using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VastAdventure.Model
{
   [Serializable]
   public class ConfigModel
   {
      public float SoundVolume;
      public string UIType;

      public ConfigModel()
      {
         SoundVolume = .5f;
         UIType = "dark theme";
      }
   }
}
