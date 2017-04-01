using System.Collections.Generic;

namespace TagItDatabaseModels.Tables
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            HotspotTags = new List<HotspotTag>();
        }

        public int TagId { get; set; }
        public string HashTag { get; set; }
        public User User { get; set; }

        public virtual List<HotspotTag> HotspotTags { get; set; }
    }
}
