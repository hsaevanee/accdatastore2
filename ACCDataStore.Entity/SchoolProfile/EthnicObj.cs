using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class EthnicObj : BaseEntity
    {
        public EthnicObj(string sethiniccode,string sethinicname)
        {
            this.EthinicCode = sethiniccode;
            this.EthinicName = sethinicname;
        }

        public EthnicObj()
        {
            this.EthinicCode = null;
            this.EthinicName = null;
            //this.EthnicGender = null;
            //this.EthnicGenderTotal = -1;           
            this.PercentageFemaleInSchool = 0;
            this.PercentageFemaleAllSchool = 0;
            this.PercentageMaleInSchool = 0;
            this.PercentageMaleAllSchool = 0;
            this.PercentageInSchool = 0;
            this.PercentageAllSchool = 0;
        }

        public string EthinicCode { get; set; }
        public string EthinicName { get; set; }
       // public string EthnicGender { get; set; }        
        //public double EthnicGenderTotal { get; set; }
        public double PercentageFemaleInSchool { get; set; }
        public double PercentageFemaleAllSchool { get; set; }
        public double PercentageInSchool { get; set; }
        public double PercentageAllSchool { get; set; }
        public double PercentageMaleInSchool { get; set; }
        public double PercentageMaleAllSchool { get; set; }
    }
}
