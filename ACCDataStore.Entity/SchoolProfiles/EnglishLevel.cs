using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class EnglishLevel:BaseEntity
    {
        public virtual string ScotXedCode { get; set; }
        public virtual string Description { get; set; }
        // attribute for MySql 
        //public virtual int ID { get; set; }
        //public virtual string Code { get; set; }
        //public virtual string Description { get; set; }
    }
}
