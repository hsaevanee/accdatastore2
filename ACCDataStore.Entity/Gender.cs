using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Gender : BaseEntity
    {
        private int _gendercode;
        public int gendercode
        {
            get
            {
                return this._gendercode;
            }
            set
            {
                this._gendercode = value;
                this.name = value == 0 ? "ALL" : value == 1 ? "Male" : "Female";
            }
        }
        public string name { get; set; }
        public bool isSelected { get; set; }

        public Gender(int gendercode)
        {
            this.gendercode = gendercode;
            this.isSelected = true;
        }


        public Gender()
        {
        }
    }
}
