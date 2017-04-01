using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagItDatabaseModels.Tables
{
    public class Comment : BaseEntity
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }

        public virtual CommentType CommentType { get; set; }
        public virtual User User { get; set; }
        public virtual Hotspot Hotspot { get; set; }
    }
}
