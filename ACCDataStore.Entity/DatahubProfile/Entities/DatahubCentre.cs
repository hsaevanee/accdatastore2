using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class DatahubCentre : BaseEntity
    {
        public string seedcode { get; set; }
        public string name { get; set; }
        public string centretype { get; set; } //1 is school, 2 is neighbourhood

        public DatahubCentre() { }

        public DatahubCentre(string seedcode, string name)
        {
            this.seedcode = seedcode;
            this.name = name;
        }


        public DatahubCentre(string seedcode, string name, string centretype)
        {
            this.seedcode = seedcode;
            this.name = name;
            this.centretype = centretype;
        }

         
        public object GetJson()
        {
            return new
            {
                SeedCode = this.seedcode,
                Name = this.name,
                CentreType = this.centretype
            };
        }

       
    }
}
