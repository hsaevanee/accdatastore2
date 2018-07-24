using ACCDataStore.Entity.DatahubProfile;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore
{
    public class SummaryGeneratorHelper
    {

        protected static dynamic CalculateSummaryData<T>(T entity, IList<DatahubDataObj> studentDataForPeriod, string datacode, string name, string type, int month, int year) where T : SummaryData
        {
            // TODO: remove year month and work only with subsetStudent for a given year and month
            // studentDataForPeriod = studentDataForPeriod.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();

            T summaryData = entity;

            //string unspecified = "information not yet obtained";
            string unspecified = "prefer not to say";


            summaryData.name = name;
            summaryData.dataCode = datacode;
            summaryData.dataMonth = month;
            summaryData.dataYear = year;
            summaryData.type = type;

            summaryData.movedOutScotlandFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("female"));
            summaryData.movedOutScotlandMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("male"));
            summaryData.movedOutScotlandUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals(unspecified));

            studentDataForPeriod = studentDataForPeriod.Where(x => !(x.Current_Status.ToLower().Equals("moved outwith scotland"))).ToList();

            summaryData.allFemale = studentDataForPeriod.Count(x => x.Gender.ToLower().Equals("female"));
            summaryData.allMale = studentDataForPeriod.Count(x => x.Gender.ToLower().Equals("male"));
            summaryData.allUnspecified = studentDataForPeriod.Count(x => x.Gender.ToLower().Equals(unspecified));

            summaryData.all15Female = studentDataForPeriod.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("female"));
            summaryData.all15Male = studentDataForPeriod.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("male"));
            summaryData.all15Unspecified = studentDataForPeriod.Count(x => x.Age == 15 && x.Gender.ToLower().Equals(unspecified));

            summaryData.all16Female = studentDataForPeriod.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("female"));
            summaryData.all16Male = studentDataForPeriod.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("male"));
            summaryData.all16Unspecified = studentDataForPeriod.Count(x => x.Age == 16 && x.Gender.ToLower().Equals(unspecified));

            summaryData.all17Female = studentDataForPeriod.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("female"));
            summaryData.all17Male = studentDataForPeriod.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("male"));
            summaryData.all17Unspecified = studentDataForPeriod.Count(x => x.Age == 17 && x.Gender.ToLower().Equals(unspecified));

            summaryData.all18Female = studentDataForPeriod.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("female"));
            summaryData.all18Male = studentDataForPeriod.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("male"));
            summaryData.all18Unspecified = studentDataForPeriod.Count(x => x.Age == 18 && x.Gender.ToLower().Equals(unspecified));

            summaryData.all19Female = studentDataForPeriod.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("female"));
            summaryData.all19Male = studentDataForPeriod.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("male"));
            summaryData.all19Unspecified = studentDataForPeriod.Count(x => x.Age == 19 && x.Gender.ToLower().Equals(unspecified));

            summaryData.activityAgreementFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals(unspecified));

            summaryData.schoolFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals(unspecified));

            summaryData.schoolInTransitionFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolInTransitionMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolInTransitionUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals(unspecified));

            summaryData.activityAgreementFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals(unspecified));

            summaryData.fundStage2Female = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage2Male = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage2Unspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals(unspecified));

            summaryData.fundStage3Female = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage3Male = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage3Unspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals(unspecified));

            summaryData.fundStage4Female = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage4Male = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage4Unspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals(unspecified));

            //summaryData.fullTimeEmploymentFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("female"));
            //summaryData.fullTimeEmploymentMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("male"));
            //summaryData.fullTimeEmploymentUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals(unspecified));

            summaryData.fullTimeEmploymentFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employment") && x.Gender.ToLower().Equals("female"));
            summaryData.fullTimeEmploymentMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employment") && x.Gender.ToLower().Equals("male"));
            summaryData.fullTimeEmploymentUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("employment") && x.Gender.ToLower().Equals(unspecified));

            summaryData.furtherEducationFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("female"));
            summaryData.furtherEducationMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("male"));
            summaryData.furtherEducationUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals(unspecified));

            summaryData.higherEducationFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("female"));
            summaryData.higherEducationMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("male"));
            summaryData.higherEducationUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals(unspecified));

            summaryData.modernApprenticeshipFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("female"));
            summaryData.modernApprenticeshipMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("male"));
            summaryData.modernApprenticeshipUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals(unspecified));

            summaryData.parttimeEmploymentFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.parttimeEmploymentMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.parttimeEmploymentUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals(unspecified));

            summaryData.personalDevelopmentFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("female"));
            summaryData.personalDevelopmentMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("male"));
            summaryData.personalDevelopmentUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals(unspecified));

            summaryData.selfEmployedFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("female"));
            summaryData.selfEmployedMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("male"));
            summaryData.selfEmployedUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals(unspecified));

            summaryData.trainingFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("other formal training") && x.Gender.ToLower().Equals("female"));
            summaryData.trainingMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("other formal training") && x.Gender.ToLower().Equals("male"));
            summaryData.trainingUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("other formal training") && x.Gender.ToLower().Equals(unspecified));

            summaryData.voluntaryWorkFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("female"));
            summaryData.voluntaryWorkMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("male"));
            summaryData.voluntaryWorkUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals(unspecified));

            summaryData.custodyFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("female"));
            summaryData.custodyMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("male"));
            summaryData.custodyUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals(unspecified));

            summaryData.economicallyInactiveFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("female"));
            summaryData.economicallyInactiveMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("male"));
            summaryData.economicallyInactiveUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals(unspecified));

            summaryData.illHealthFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("female"));
            summaryData.illHealthMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("male"));
            summaryData.illHealthUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals(unspecified));

            summaryData.unemployedFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("female"));
            summaryData.unemployedMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("male"));
            summaryData.unemployedUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals(unspecified));

            // Aberdeen, Glasgow
            //summaryData.unknownFemale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("female"));
            //summaryData.unknownMale = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("male"));
            //summaryData.unknownUnspecified = studentDataForPeriod.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals(unspecified));

            // Aberdeenshire
            summaryData.unknownFemale = studentDataForPeriod.Count(x => x.Current_Status == null && x.Gender.ToLower().Equals("female"));
            summaryData.unknownMale = studentDataForPeriod.Count(x => x.Current_Status == null && x.Gender.ToLower().Equals("male"));
            summaryData.unknownUnspecified = studentDataForPeriod.Count(x => x.Current_Status == null && x.Gender.ToLower().Equals(unspecified));

            //Aberdeen only
            //var temp = studentDataForPeriod.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().ToList();
            //summaryData.AvgWeek = temp.First() == null ? 0.0 : temp.Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status ?? "0"));

            // Glasgo w and aberdeenshire avg
            summaryData.AvgWeek = 0.0;

            return summaryData;
        }

        public static IList<DatahubDataObj> getSubsetStudentsThisPeriod(IList<DatahubDataObj> allStudents, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
            return subsetStudents;
        }

        public static IList<DatahubDataObj> getSubsetStudentsBySchool(IList<DatahubDataObj> allStudentsThisPeriod, string seedCode)
        {
            IList<DatahubDataObj> subsetStudents = allStudentsThisPeriod.Where(x => x.SEED_Code != null && x.SEED_Code.Equals(seedCode)).ToList();
            return subsetStudents;
        }

        public static IList<DatahubDataObj> getSubsetStudentsByZone(ISession session, IList<DatahubDataObj> allStudentsThisPeriod, string zonetype, string zonecode)
        {
            // This method retrieves a subset of Datahub Student Data by their corresponding neighbourhood or data zone and a specific period of time
            // We first fetch all the post codes that correspond to that neighbourhood or data zone
            // Then we filther the Datahub Student Data list by the specific period (month, year)
            // Finally we join the two lists and return a list of Datahub Student Data matching the criteria

            IList<NeighbourhoodObj> postCodes = new Collection<NeighbourhoodObj>();
            IList<DatahubDataObj> subsetStudents = new Collection<DatahubDataObj>();

            switch (zonetype.ToLower())
            {
                case "intermediate zone":
                    postCodes = session.QueryOver<NeighbourhoodObj>().Where(x => x.IntDataZone == zonecode).List();
                    break;
                case "data zone":
                    postCodes = session.QueryOver<NeighbourhoodObj>().Where(x => x.DataZone == zonecode).List();
                    break;
                default:
                    postCodes = null;
                    break;
            }

            try
            {
                subsetStudents = (from s in allStudentsThisPeriod
                                  join p in postCodes
                                  on s.CSS_Postcode equals p.CSS_Postcode
                                  select s).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return subsetStudents;
        }

        public static void populateForSinglePeriod<T, U>(ISession session, IList<DatahubDataObj> studentDataThisPeriod, Council council, int currMonth, int currYear, IList<IntermediateZoneObj> intermediateZonesList, IList<string> dataZonesList, IList<AllSchools> schoolsList)
            where T : DatahubDataObj
            where U : SummaryData, new()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Populating data for {0}: Current period: {1},{2}", council.name, currMonth, currYear);
            Console.WriteLine("Generating summary data and saving to session for whole council: Started", council.name);
            U entity = CalculateSummaryData<U>(new U(),studentDataThisPeriod, council.referenceCode, council.name, "Council", currMonth, currYear);
            session.Save(entity);

            stopwatch.Stop();
            Console.WriteLine("Generating summary data and saving to session for whole council: Completed");
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            stopwatch.Reset();

            // Calculate intermediate zones summary
            stopwatch.Start();
            Console.WriteLine("Generating summary data and saving to session for {0} intermediate zones: Started", intermediateZonesList.Count());
            foreach (IntermediateZoneObj intZone in intermediateZonesList)
            {
                var allIntermediateZoneStudentsThisPeriod = getSubsetStudentsByZone(session, studentDataThisPeriod, "Intermediate Zone", intZone.Reference);
                U currSummary = CalculateSummaryData<U>(new U(),allIntermediateZoneStudentsThisPeriod, intZone.Reference, intZone.Name, "Intermediate Zone", currMonth, currYear);
                session.Save(currSummary);
            }

            stopwatch.Stop();
            Console.WriteLine("Generating summary data and saving to session for intermediate zones: Completed");
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            stopwatch.Reset();

            // Calculate data zones summary
            stopwatch.Start();
            Console.WriteLine("Generating summary data and saving to session for {0} data zones: Started", dataZonesList.Count);
            foreach (string zonecode in dataZonesList)
            {
                var allIntermediateZoneStudentsThisPeriod = getSubsetStudentsByZone(session, studentDataThisPeriod, "Data Zone", zonecode);
                U currSummary = CalculateSummaryData<U>(new U(),allIntermediateZoneStudentsThisPeriod, zonecode, zonecode, "Data Zone", currMonth, currYear);
                session.Save(currSummary);
            }

            stopwatch.Stop();
            Console.WriteLine("Generating summary data and saving to session for data zones: Completed");
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            stopwatch.Reset();

            // Calculate all schools summary
            stopwatch.Start();
            Console.WriteLine("Generating summary data and saving to session for {0} schools: Started", schoolsList.Count());
            foreach (AllSchools school in schoolsList)
            {
                var allSchoolStudentsThisPeriod = getSubsetStudentsBySchool(studentDataThisPeriod, school.seedCode);
                U currSummary = CalculateSummaryData<U>(new U(),allSchoolStudentsThisPeriod, school.seedCode, school.name, "School", currMonth, currYear);
                session.Save(currSummary);
            }
            //    stopwatch.Stop();
            //    Console.WriteLine("Generating summary data and saving to session for schools: Completed");
            //    Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            //    stopwatch.Reset();
        }


        public static void initialPopulationForCouncil<T>(ISession session) where T : DatahubDataObj
        {

            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatch_g = new Stopwatch();
            stopwatch.Start();
            stopwatch_g.Start();

            Council council = getCouncil<T>();
            Console.WriteLine("Initial population for {0}: Started", council.name);

            

            Console.WriteLine("Preparing data for all periods: Started");
            // Aberdeenshire

            IList<DatahubDataObj> studentDataAllPeriods = session.QueryOver<T>().Where(x => x.Current_Status != "Time Out to Travel"
                    && x.Current_Status != "PSD (Social & Health)" && x.Current_Status != "PSD (Employability)" && x.Current_Status != "Get Ready for Work").List<DatahubDataObj>();
            // Aberdeen glasgow
            //IList<DatahubDataObj> studentDataAllPeriods = session.QueryOver<T>().List<DatahubDataObj>();

            var monthYearList = studentDataAllPeriods.Select(x => new { x.Data_Month, x.Data_Year }).ToList().Distinct().ToList();
            IList<string> dataZonesList = session.QueryOver<DataZoneObj>().Where(x => x.Reference_Council == council.referenceCode).List().Select(x => x.Reference).ToList();
            IList<IntermediateZoneObj> intermediateZonesList = session.QueryOver<IntermediateZoneObj>().Where(x => x.Reference_Council == council.referenceCode).List();
            IList<AllSchools> allSchoolsList = session.QueryOver<AllSchools>().Where(x => x.referenceCouncil == council.referenceCode).List<AllSchools>();

            stopwatch.Stop();
            Console.WriteLine("Preparing data for all periods: Completed");
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            stopwatch.Reset();


            foreach (var period in monthYearList)
            {
                int currMonth = period.Data_Month;
                int currYear = period.Data_Year;
                var studentDataThisPeriod = getSubsetStudentsThisPeriod(studentDataAllPeriods, currMonth, currYear);
                
                switch(council.name)
                {
                    case "Aberdeen City":
                        populateForSinglePeriod<T,AberdeenSummary>(session, studentDataThisPeriod, council, currMonth, currYear, intermediateZonesList, dataZonesList, allSchoolsList);
                        break;
                    case "Glasgow City":
                        populateForSinglePeriod<T,GlasgowSummary>(session, studentDataThisPeriod, council, currMonth, currYear, intermediateZonesList, dataZonesList, allSchoolsList);
                        break;
                    case "Aberdeenshire":
                        populateForSinglePeriod<T, AberdeenshireSummary>(session, studentDataThisPeriod, council, currMonth, currYear, intermediateZonesList, dataZonesList, allSchoolsList);
                        break;
                }
                
            }

            stopwatch_g.Stop();
            Console.WriteLine("Initial population for {0}: Finalized", council.name);
            Console.WriteLine("Total time elapsed: {0}", stopwatch_g.Elapsed);
        }

        private static Council getCouncil<T>() where T : DatahubDataObj
        {
            string name = typeof(T).Name;
            Council council = new Council();
            
            switch (name)
            {
                case "DatahubDataAberdeen":
                    council.referenceCode = "S12000033";
                    council.name = "Aberdeen City";
                    council.entity  = new AberdeenSummary();
                    break;
                case "DatahubDataGlasgow":
                    council.referenceCode = "S12000046";
                    council.name = "Glasgow City";
                    council.entity = new GlasgowSummary();
                    break;
                case "DatahubDataAberdeenshire":
                    council.referenceCode = "S12000034";
                    council.name = "Aberdeenshire";
                    council.entity = new AberdeenshireSummary();
                    break;
                default:
                    council = null;
                    break;
            }
            return council;
        }

        public class Council
        {
            public string name { get; set; }
            public string referenceCode { get; set; }
            public SummaryData entity { get; set; }
            //public SummaryData summaryDataType { get; set; }
        }
    }
}
