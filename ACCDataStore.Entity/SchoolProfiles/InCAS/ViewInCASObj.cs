using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.InCAS
{
    public class ViewInCASObj: BaseEntity
    {
        public Year year { get; set; }
        public string seedcode { get; set; }
        public string YearGroup { get; set; }
        public double? AgeDiff_DevAbil { get; set; }
        public double? AgeDiff_GenMaths { get; set; }
        public double? AgeDiff_MentArith { get; set; }
        public double? AgeDiff_Reading { get; set; }
        public double? AgeDiff_Spelling { get; set; }
        public double? Standardised_DevAbil { get; set; }
        public double? Standardised_GenMaths { get; set; }
        public double? Standardised_MentArith { get; set; }
        public double? Standardised_Reading { get; set; }
    }

}
