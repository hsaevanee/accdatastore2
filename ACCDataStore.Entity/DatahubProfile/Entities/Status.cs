using ACCDataStore.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class Status
    {
        public string code { get; set; }
        public string name { get; set; }

        public Status(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
    }
}
