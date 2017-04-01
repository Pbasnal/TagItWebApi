using System.Collections.Generic;

namespace TagItViewModels
{
    public class HotspotModel
    {
        public string Name { get; set; }
        public PositionModel Location { get; set; }
        public string Portal { get; set; }
        public List<string> Tags { get; set; }
        public string Comments { get; set; }
        public string Images { get; set; }
    }
}