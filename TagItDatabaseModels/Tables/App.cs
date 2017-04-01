using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagItDatabaseModels.Tables
{
    public class App : BaseEntity
    {
        public Guid AppId { get; set; }
        public string AppName { get; set; }
    }
}
