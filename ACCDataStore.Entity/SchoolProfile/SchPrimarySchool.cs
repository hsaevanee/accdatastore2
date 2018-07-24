using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class SchPrimarySchool : BaseEntity
    {
        public virtual int RecordId { get; set; }
        public virtual string LaCode { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string SchoolFundingType { get; set; }
        public virtual string Name { get; set; }
        public virtual int OpeningsPerWeek { get; set; }
        public virtual string UsingBS7666AddressFormat { get; set; }
        public virtual string SchoolBoardType { get; set; }
        public virtual DateTime DateOfLastElection { get; set; }

    }
}
