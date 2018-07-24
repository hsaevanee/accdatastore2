using ACCDataStore.Entity.SchoolProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class ObjectDetail: BaseEntity

    {
        public List<StudentObj> liststudents { get; set; }
        public string itemcode { get; set; } // Code 
        public int count { get; set; } //length of pupils list
        public double percentage { get; set; }
        public int schoolroll { get; set; }
        public double percentageFemale { get; set; }
        public double percentageMale { get; set; }

        public ObjectDetail(string itemcode, int count)
        {
            this.itemcode = itemcode;
            this.count = 0;

        }

        public ObjectDetail()
        {
            this.itemcode = itemcode;
            this.count = 0;

        }
    }



}
