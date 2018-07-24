using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class PosNegSchoolList
    {
        public string name { get; set; }
        public double participating { get; set; }
        public double notParticipating { get; set; }
        public double unknown { get; set; }
    }
}