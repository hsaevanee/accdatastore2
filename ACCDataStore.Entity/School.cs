using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class School:BaseEntity
    {
        public string seedcode { get; set; }
        public string name { get; set; }
        public string schooltype { get; set; }
        public string hmie_report { get; set; }
        public string website_link { get; set; }
        public double costperpupil { get; set; }
        public Nullable<DateTime> hmieLastReport { get; set; }
        public int schoolCapacity { get; set; }

        public School() { }

        public School(string seedcode, string name)
        {
            this.seedcode = seedcode;
            this.name = name;
        }


        public School(string seedcode, string name, string schooltype)
        {
            this.seedcode = seedcode;
            this.name = name;
            this.schooltype = schooltype;
        }

        public string getHimeReportDate() {

            if (this.hmieLastReport.HasValue)
            {
                return this.hmieLastReport.Value.Day + "/" + this.hmieLastReport.Value.Month + "/" + this.hmieLastReport.Value.Year;
            }
            else { 
            
                return "N/A";
            
            }
        
        }

        public object GetJson()
        {
            return new
            {
                SeedCode = this.seedcode,
                Name = this.name
            };
        }

        public object GetSchoolDetailJson()
        {
            return new
            {
                SeedCode = this.seedcode,
                Name = this.name,
                Hmiereport = this.hmie_report,
                website = this.website_link,
                Costperpupil = this.costperpupil,
                hmieLastReport = getHimeReportDate()
            };
        }

    }


}
