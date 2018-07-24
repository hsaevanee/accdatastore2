using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class DataSeries: ObjectDetail
    {
        public string dataSeriesNames;
        public School school;
        public Year year;
        public List<ObjectDetail> listdataitems;
        public List<PIPSObjDetail> listPIPSdataitems;
        public double checkSumPercentage;
        public int checkSumCount;
    }
}
