using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class SchoolNew
    {
        public virtual int ID { get; set; }
        public virtual string LaCode { get; set; }
        public virtual string SeedCode { get; set; }
        public virtual int SchoolFundingType { get; set; }
        public virtual string Name { get; set; }
    }
}
