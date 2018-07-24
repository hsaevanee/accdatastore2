using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class ChartData1
    {
        public ChartData1(string sName, List<double> listData)
        {
            this.name = sName;
            this.data = listData;
        }
        public string name { get; set; }
        public List<double> data { get; set; }
    }
}
