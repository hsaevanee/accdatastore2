using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class PIPS2008Map : ClassMap<PIPS2008>
    {
        public PIPS2008Map() {
            Table("pips_p1_2008");
            Id(x => x.ID).Column("ID");
            Map(x => x.DfES);
            Map(x => x.classRoom);
            Map(x => x.pfname);
            Map(x => x.plname);
            Map(x => x.dob);
            Map(x => x.sex);
            Map(x => x.UPN);
            Map(x => x.intake);
            Map(x => x.matched);
            Map(x => x.Sassdate);
            Map(x => x.SisCD);
            Map(x => x.Srm);
            Map(x => x.Srr);
            Map(x => x.Srp);
            Map(x => x.Srt);
            Map(x => x.Szm);
            Map(x => x.Szr);
            Map(x => x.Szp);
            Map(x => x.Szt);
            Map(x => x.Eassdate);
            Map(x => x.EisCD);
            Map(x => x.Erm);
            Map(x => x.Err);
            Map(x => x.Erp);
            Map(x => x.Ert);
            Map(x => x.Ezm);
            Map(x => x.Ezr);
            Map(x => x.Ezp);
            Map(x => x.Ezt);
            Map(x => x.mGrade);
            Map(x => x.rGrade);
            Map(x => x.mResid);
            Map(x => x.rResid);
            Map(x => x.KSMW);
            Map(x => x.KSM1);
            Map(x => x.KSM2C);
            Map(x => x.KSM2B);
            Map(x => x.KSM2A);
            Map(x => x.KSM3);
            Map(x => x.KSRW);
            Map(x => x.KSR1);
            Map(x => x.KSR2C);
            Map(x => x.KSR2B);
            Map(x => x.KSR2A);
            Map(x => x.KSR3);
            Map(x => x.KSWW);
            Map(x => x.KSW1);
            Map(x => x.KSW2C);
            Map(x => x.KSW2B);
            Map(x => x.KSW2A);
            Map(x => x.KSW3);
            Map(x => x.cemID);
        
        }
    }
}
