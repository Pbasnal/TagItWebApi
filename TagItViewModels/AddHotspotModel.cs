using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagItViewModels
{
    public class AddHotspotModel
    {
        public string Name { get; set; }
        public PositionModel Location { get; set; }
        public string Portal { get; set; }
        public string Tags { get; set; }
        public string Comments { get; set; }
        public string Images { get; set; }
    }
}
