using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class CitySchool:BaseSPDataModel
    {
        public SPCfElevel SPCfElevel_NCfElevel { get; set; }
        public List<FreeSchoolMeal> listFSM_National { get; set; }
        public FreeSchoolMeal FSM_National { get; set; }

    }
}
