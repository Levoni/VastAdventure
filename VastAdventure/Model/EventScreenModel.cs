using System;
using System.Collections.Generic;
using System.Text;
using VastAdventure.Utility;

namespace VastAdventure.Model
{
   [Serializable]
   public class EventScreenModel
   {
      public int EventId { get; set; }
      public ScreenType Type {get;set;}
      public string EventLocation { get; set; }
      public string EventDescription { get; set; }
      public string ImageReference { get; set; }
      public List<Option> Options { get; set; }
   }
}
