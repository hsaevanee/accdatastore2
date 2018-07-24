using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace ACCDataStore.Entity
{
    public class CheckModel : BaseEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isselected { get; set; }
        public CheckModel(int id, string name) {
            this.id = id;
            this.name = name;
        }
    }
}
