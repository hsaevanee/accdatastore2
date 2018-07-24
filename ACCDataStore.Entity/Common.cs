using ACCDataStore.Entity.SchoolProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class Common
    {
        public School school;
        public List<PupilObj> listpupils;
        public int count; //length of pupils list
        public double percentage;
        public double percentageFemale;
        public double percentageMale;
    }
}
