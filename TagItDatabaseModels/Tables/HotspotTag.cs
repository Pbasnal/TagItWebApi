namespace TagItDatabaseModels.Tables
{
    public class HotspotTag:BaseEntity
    {
        public int HotspotTagId { get; set; }
        public bool IsActive { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Hotspot Hotspot { get; set; }
    }
}
