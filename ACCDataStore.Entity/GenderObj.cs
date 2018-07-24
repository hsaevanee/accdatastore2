using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class GenderObj: BaseEntity
    {
        public string id { get; set; }
        public string abbName { get; set; }
        public string fullName { get; set; }
        public bool selected { get; set; }

        public GenderObj(string id, string abbName, string fullName)
        {
            this.id = id;
            this.abbName = abbName;
            this.fullName = fullName;
            this.selected = true;
        }

    }
}
