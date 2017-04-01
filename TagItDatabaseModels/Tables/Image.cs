using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagItDatabaseModels.Tables
{
    public class Image : BaseEntity
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
        public virtual Hotspot Hotspot { get; set; }
    }
}
