using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagItDatabaseModels.Tables
{
    public class User : BaseEntity
    {
        public User()
        {
            UserHotspots = new List<Hotspot>();
            UserTags = new List<Tag>();
            UserComments = new List<Comment>();
            UserImages = new List<Image>();
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public long PhoneNumber { get; set; }

        public virtual List<Hotspot> UserHotspots { get; set; }
        public virtual List<Tag> UserTags { get; set; }
        public virtual List<Comment> UserComments { get; set; }
        public virtual List<Image> UserImages { get; set; }
    }
}
