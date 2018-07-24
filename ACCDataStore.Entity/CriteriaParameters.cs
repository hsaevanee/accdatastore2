using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class CriteriaParameters
    {
        public List<string> ListConditionYear { get; set; }
        public List<string> ListConditionGender { get; set; }
        public List<string> ListConditionNationality { get; set; }
        public List<string> ListConditionDecile { get; set; }
    }
}
