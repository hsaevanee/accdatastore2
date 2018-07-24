using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPSchoolRollForecast 
    {
        public List<GenericSchoolData> ListActualSchoolRoll { get; set; }
        public List<GenericSchoolData> ListForecastSchoolRoll { get; set; }
    }
}
