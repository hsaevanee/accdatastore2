using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class CurrentCouncil
    {
        public  CurrentCouncil(string name, IList<School> schools, IList<School> intermediateZones, IList<School> dataZones)
        {
            this.name = name;
            this.schools = schools;
            this.intermediateZones = intermediateZones;
            this.dataZones = dataZones;
        }
        public string name { get; set; }
        public IList<School> schools { get; set; }
        public IList<School> intermediateZones { get; set; }
        public IList<School> dataZones { get; set; }
    }
}