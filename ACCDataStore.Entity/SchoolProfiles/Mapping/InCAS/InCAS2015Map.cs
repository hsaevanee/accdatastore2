using ACCDataStore.Entity.SchoolProfiles.InCAS;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class InCAS2015Map : ClassMap<InCAS2015>
    {
        public InCAS2015Map()
        {
            Table("incas_2015");
            Id(x => x.ID).Column("ID");
            Map(x => x.SchoolId);
            Map(x => x.SoftwareVersion);
            Map(x => x.UPN);
            Map(x => x.YearGroup);
            Map(x => x.ClassName);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.DateOfBirth);
            Map(x => x.Gender);
            Map(x => x.AgeAtTest_DevAbil);
            Map(x => x.AgeEquiv_DevAbil);
            Map(x => x.AgeDiff_DevAbil);
            Map(x => x.Standardised_DevAbil);
            Map(x => x.AgeAtTest_Reading);
            Map(x => x.AgeEquiv_Reading);
            Map(x => x.AgeDiff_Reading);
            Map(x => x.Standardised_Reading);
            Map(x => x.AgeAtTest_Spelling);
            Map(x => x.AgeEquiv_Spelling);
            Map(x => x.AgeDiff_Spelling);
            Map(x => x.AgeAtTest_MentArith);
            Map(x => x.AgeEquiv_MentArith);
            Map(x => x.AgeDiff_MentArith);
            Map(x => x.Standardised_MentArith);
            Map(x => x.AgeAtTest_GenMaths);
            Map(x => x.AgeEquiv_GenMaths);
            Map(x => x.AgeDiff_GenMaths);
            Map(x => x.Standardised_GenMaths);

 
        }


    }
}
