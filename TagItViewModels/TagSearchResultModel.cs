using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagItViewModels
{
    public class TagSearchResultModel
    {
        public string Name { get; set; }
        public HotspotInformation Info { get; set; }
        public PositionModel Location { get; set; }
        public string Portal { get; set; }
    }
}
