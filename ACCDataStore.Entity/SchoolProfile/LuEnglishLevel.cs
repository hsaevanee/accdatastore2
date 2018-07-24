using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class LuEnglishLevel: BaseEntity
    {
        public virtual string ScotXedCode { get; set; }
        public virtual string Description { get; set; }
    }
}
