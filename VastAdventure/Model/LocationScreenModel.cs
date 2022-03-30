using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class LocationScreenModel
   {
      public int locationId;
      public ScreenType screenType;
      public string locationName;
      public List<ConnectedLocationModel> connectedLocations;
      public List<Option> Options;
   }
}
