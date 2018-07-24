using ACCDataStore.Entity.DatahubProfile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class BaseSPDataModel
    {
        public string SeedCode { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCostperPupil { get; set; }
        public SchoolRoll  SchoolRoll { get; set; }
        public List<NationalityIdentity> listNationalityIdentity { get; set; }
        public NationalityIdentity NationalityIdentity { get; set; }
        public List<LevelOfEnglish> listLevelOfEnglish { get; set; }
        public LevelOfEnglish LevelOfEnglish { get; set; }
        public List<Ethnicbackground> listEthnicbackground { get; set; }
        public Ethnicbackground Ethnicbackground { get; set; }
        public List<StudentStage> listStudentStage { get; set; }
        public StudentStage StudentStage { get; set; }
        public List<LookedAfter> listLookedAfter { get; set; }
        public LookedAfter  LookedAfter { get; set; }
        public List<SPPIPS> listPIPS { get; set; }
        public SPPIPS PIPS { get; set; }
        public SPSchoolRollForecast SchoolRollForecast { get; set; }
        public List<SPInCAS> listInCASP2 { get; set; }
        public List<SPInCAS> listInCASP3 { get; set; }
        public List<SPInCAS> listInCASP4 { get; set; }
        public List<SPInCAS> listInCASP5 { get; set; }
        public List<SPInCAS> listInCASP6 { get; set; }
        public SPInCAS InCASP2 { get; set; }
        public SPInCAS InCASP3 { get; set; }
        public SPInCAS InCASP4 { get; set; }
        public SPInCAS InCASP5 { get; set; }
        public SPInCAS InCASP6 { get; set; }
        public List<SPSIMD> listSIMD { get; set; }
        public SPSIMD SIMD { get; set; }
        public List<FreeSchoolMeal> listFSM { get; set; }
        public FreeSchoolMeal FSM { get; set; }
        public StudentNeed StudentNeed { get; set; }
        public List<StudentNeed> listStudentNeed { get; set; }
        public List<SPAttendance> listAttendance { get; set; }
        public SPAttendance SPAttendance { get; set; }
        public List<SPExclusion> listExclusion { get; set; }
        public SPExclusion SPExclusion { get; set; }
        public List<SPCfElevel> listSPCfElevel { get; set; }
        public SPCfElevel SPCfElevel { get; set; }
        public List<SPCfElevel> listSPCfElevel2 { get; set; }
        public SPCfElevel SPCfElevel2 { get; set; }
        public List<SPMiDYIS> listMiDYIS { get; set; }
        public SPMiDYIS SPMiDYIS { get; set; }
        public List<SPPIPS> listSOSCA { get; set; }
        public SPPIPS SOSCA { get; set; }
        public List<SummaryDHdata> listdestinations { get; set; }
  }
}
