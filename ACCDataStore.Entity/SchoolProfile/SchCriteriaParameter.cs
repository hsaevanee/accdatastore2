using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class SchCriteriaParameter
    {
        public List<string> ListConditionYear { get; set; }
        public List<string> ListConditionGender { get; set; }
        public List<string> ListConditionNationality { get; set; }
        public List<string> ListConditionDecile { get; set; }
        public List<string> ListConditionEthnicbg { get; set; }
        public string selectedschoolname { get; set; }
    }
}
