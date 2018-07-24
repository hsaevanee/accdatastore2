using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class NationalityObj : BaseEntity
    {
        public string IdentityCode { get; set; }
        public string IdentityName { get; set; }
        public double PercentageFemaleInSchool { get; set; }
        public double PercentageFemaleAllSchool { get; set; }
        public double PercentageInSchool { get; set; }
        public double PercentageAllSchool { get; set; }
        public double PercentageMaleInSchool { get; set; }
        public double PercentageMaleAllSchool { get; set; }

        public NationalityObj(string setIdentitycode,string setIdentityname)
        {
            this.IdentityCode = setIdentitycode;
            this.IdentityName = setIdentityname;
        }

        public NationalityObj()
        {
            this.IdentityCode = null;
            this.IdentityName = null;       
            this.PercentageFemaleInSchool = 0;
            this.PercentageFemaleAllSchool = 0;
            this.PercentageMaleInSchool = 0;
            this.PercentageMaleAllSchool = 0;
            this.PercentageInSchool = 0;
            this.PercentageAllSchool = 0;
        }
    }
}
