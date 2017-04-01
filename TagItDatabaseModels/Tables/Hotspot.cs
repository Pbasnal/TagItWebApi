using System.Collections.Generic;

namespace TagItDatabaseModels.Tables
{
    public class Hotspot : BaseEntity
    {
        public Hotspot()
        {
            HotspotTags = new List<HotspotTag>();
            HotspotComments = new List<Comment>();
            HotspotImages = new List<Image>();
        }

        public int HotspotId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Portal { get; set; }
        public bool IsActive { get; set; }

        public virtual List<HotspotTag> HotspotTags { get; set; }
        public virtual List<Comment> HotspotComments { get; set; }
        public virtual List<Image> HotspotImages { get; set; }
        public virtual User User { get; set; }
        
    }
}
