using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACCDataStore.Entity
{
    public class RightsCode : BaseEntity
    {
        public virtual int RightCode { get; set; }
        public virtual string RightDesc { get; set; }
        public virtual int Ord { get; set; }

        public virtual string DisplayText
        {
            get 
            {
                return this.RightDesc;
            }
        }

        public virtual int Value
        {
            get
            {
                return this.RightCode;
            }
        }
    }
}
