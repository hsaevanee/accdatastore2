using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class CensusObj
    {
        Year year { get; set; }
        List<School> listSchooldata { get; set; }
    }

    public class SchoolData
    {
        Year year { get; set; }
        List<School> listSchooldata { get; set; }
    }

    public class DataObj
    {

        public string code { get; set; }
        public int count { get; set; }
        public int schoolroll { get; set; }
        public float percentage { get; set; }
    }

}
