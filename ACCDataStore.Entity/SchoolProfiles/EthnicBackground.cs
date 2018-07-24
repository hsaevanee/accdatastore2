using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class EthnicBackground:BaseEntity
    {
        //public virtual int id { get; set; }
        public virtual string code { get; set; }
        public virtual string value { get; set; }
        public virtual string ScotXedcode { get; set; }
        // attribute for MySql 
        //public virtual int ID { get; set; }
        //public virtual string Code { get; set; }
        //public virtual string Description { get; set; }
    }
}
