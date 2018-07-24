using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class NationalIdentity:BaseEntity
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual string scotXedcode { get; set; }
    }
}
