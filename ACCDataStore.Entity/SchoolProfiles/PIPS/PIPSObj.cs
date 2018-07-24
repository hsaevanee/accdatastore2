using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class PIPSObj : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual int DfES { get; set; }
        public virtual string classRoom { get; set; }
        public virtual string pfname { get; set; }
        public virtual string plname { get; set; }
        public virtual DateTime dob { get; set; }
        public virtual string sex { get; set; }
        public virtual string UPN { get; set; }
        public virtual int intake { get; set; }
        public virtual int matched { get; set; }
        public virtual DateTime Sassdate { get; set; }
        public virtual int SisCD { get; set; }
        public virtual int Srm { get; set; }
        public virtual int Srr { get; set; }
        public virtual int Srp { get; set; }
        public virtual int Srt { get; set; }
        public virtual double? Szm { get; set; }
        public virtual double? Szr { get; set; }
        public virtual double? Szp { get; set; }
        public virtual double? Szt { get; set; }
        public virtual DateTime Eassdate { get; set; }
        public virtual int EisCD { get; set; }
        public virtual int Erm { get; set; }
        public virtual int Err { get; set; }
        public virtual int Erp { get; set; }
        public virtual int Ert { get; set; }
        public virtual double? Ezm { get; set; }
        public virtual double? Ezr { get; set; }
        public virtual double? Ezp { get; set; }
        public virtual double? Ezt { get; set; }
        public virtual int mGrade { get; set; }
        public virtual int rGrade { get; set; }
        public virtual double mResid { get; set; }
        public virtual double rResid { get; set; }
        public virtual string KSMW { get; set; }
        public virtual string KSM1 { get; set; }
        public virtual string KSM2C { get; set; }
        public virtual string KSM2B { get; set; }
        public virtual string KSM2A { get; set; }
        public virtual string KSM3 { get; set; }
        public virtual string KSRW { get; set; }
        public virtual string KSR1 { get; set; }
        public virtual string KSR2C { get; set; }
        public virtual string KSR2B { get; set; }
        public virtual string KSR2A { get; set; }
        public virtual string KSR3 { get; set; }
        public virtual string KSWW { get; set; }
        public virtual string KSW1 { get; set; }
        public virtual string KSW2C { get; set; }
        public virtual string KSW2B { get; set; }
        public virtual string KSW2A { get; set; }
        public virtual string KSW3 { get; set; }
        public virtual string cemID { get; set; }
        public virtual string year { get; set; }
    }

    public class PIPS : PIPSObj
    {
    }
}
