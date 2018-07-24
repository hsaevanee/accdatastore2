using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class ChartData
    {
        public BaseRenderObject ChartNationalityIdentity { get; set; }
        public BaseRenderObject ChartLevelOfEnglish { get; set; }
        public BaseRenderObject ChartLevelOfEnglishByCatagories { get; set; }
        public BaseRenderObject ChartEthnicbackground { get; set; }
        public BaseRenderObject ChartStudentStage { get; set; }
        public BaseRenderObject ChartPIPSReading { get; set; }
        public BaseRenderObject ChartPIPSMath { get; set; }
        public BaseRenderObject ChartPIPSPhonics { get; set; }
        public BaseRenderObject ChartPIPSTotal { get; set; }
        public BaseRenderObject ChartSIMD { get; set; }
        public BaseRenderObject CartSchoolRollForecast { get; set; }
        public BaseRenderObject ChartIEP { get; set; }
        public BaseRenderObject ChartCSP { get; set; }
        public BaseRenderObject ChartLookedAfter { get; set; }
        public BaseRenderObject ChartAttendance { get; set; }
        public BaseRenderObject ChartAuthorisedAbsence  { get; set; }
        public BaseRenderObject ChartUnauthorisedAbsence { get; set; }
        public BaseRenderObject ChartTotalAbsence{ get; set; }
        public BaseRenderObject ChartNumberofDaysLostExclusion { get; set; }
        public BaseRenderObject ChartNumberofExclusionRFR { get; set; }
        public BaseRenderObject ChartNumberofExclusionTemporary { get; set; }


        public BaseRenderObject ChartFSM { get; set; }



        public BaseRenderObject ChartTimelineDestinations { get; set; }

    }
}
