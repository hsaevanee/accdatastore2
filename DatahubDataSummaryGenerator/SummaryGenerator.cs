using ACCDataStore;
using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatahubDataSummaryGenerator
{
    class SummaryGenerator
    {
        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(
                        c => c.FromConnectionStringWithKey("ConnectionString")
                    )
                )
                .Mappings(m =>
                
                    m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.DatahubProfile.SummaryData>()
                  
                    )
                .BuildSessionFactory();
        }


        protected static AberdeenSummary CalculateSummaryData(IList<DatahubDataObj> studentData, string datacode, string name, string type, int month, int year)
        {
            // TODO: remove year month and work only with subsetStudent for a given year and month
            // studentData = studentData.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
            
            AberdeenSummary summaryData = new AberdeenSummary();
            
            summaryData.name = name;
            summaryData.dataCode = datacode;
            summaryData.dataMonth = month;
            summaryData.dataYear = year;
            summaryData.type = type;

            summaryData.movedOutScotlandFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("female"));
            summaryData.movedOutScotlandMale = studentData.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("male"));
            summaryData.movedOutScotlandUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("information not yet obtained"));

            studentData = studentData.Where(x => !(x.Current_Status.ToLower().Equals("moved outwith scotland"))).ToList();

            summaryData.allFemale = studentData.Count(x => x.Gender.ToLower().Equals("female"));
            summaryData.allMale = studentData.Count(x => x.Gender.ToLower().Equals("male"));
            summaryData.allUnspecified = studentData.Count(x => x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all15Female = studentData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("female"));
            summaryData.all15Male = studentData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("male"));
            summaryData.all15Unspecified = studentData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all16Female = studentData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("female"));
            summaryData.all16Male = studentData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("male"));
            summaryData.all16Unspecified = studentData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all17Female = studentData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("female"));
            summaryData.all17Male = studentData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("male"));
            summaryData.all17Unspecified = studentData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all18Female = studentData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("female"));
            summaryData.all18Male = studentData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("male"));
            summaryData.all18Unspecified = studentData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all19Female = studentData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("female"));
            summaryData.all19Male = studentData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("male"));
            summaryData.all19Unspecified = studentData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.activityAgreementFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.schoolFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolMale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.schoolInTransitionFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolInTransitionMale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolInTransitionUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.activityAgreementFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage2Female = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage2Male = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage2Unspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage3Female = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage3Male = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage3Unspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage4Female = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage4Male = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage4Unspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fullTimeEmploymentFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.fullTimeEmploymentMale = studentData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.fullTimeEmploymentUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.furtherEducationFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("female"));
            summaryData.furtherEducationMale = studentData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("male"));
            summaryData.furtherEducationUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.higherEducationFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("female"));
            summaryData.higherEducationMale = studentData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("male"));
            summaryData.higherEducationUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.modernApprenticeshipFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("female"));
            summaryData.modernApprenticeshipMale = studentData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("male"));
            summaryData.modernApprenticeshipUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.parttimeEmploymentFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.parttimeEmploymentMale = studentData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.parttimeEmploymentUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.personalDevelopmentFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("female"));
            summaryData.personalDevelopmentMale = studentData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("male"));
            summaryData.personalDevelopmentUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.selfEmployedFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("female"));
            summaryData.selfEmployedMale = studentData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("male"));
            summaryData.selfEmployedUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.trainingFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("female"));
            summaryData.trainingMale = studentData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("male"));
            summaryData.trainingUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.voluntaryWorkFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("female"));
            summaryData.voluntaryWorkMale = studentData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("male"));
            summaryData.voluntaryWorkUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.custodyFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("female"));
            summaryData.custodyMale = studentData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("male"));
            summaryData.custodyUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.economicallyInactiveFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("female"));
            summaryData.economicallyInactiveMale = studentData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("male"));
            summaryData.economicallyInactiveUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.illHealthFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("female"));
            summaryData.illHealthMale = studentData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("male"));
            summaryData.illHealthUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.unemployedFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("female"));
            summaryData.unemployedMale = studentData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("male"));
            summaryData.unemployedUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("information not yet obtained"));


            summaryData.unknownFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("female"));
            summaryData.unknownMale = studentData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("male"));
            summaryData.unknownUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("information not yet obtained"));

            var temp = studentData.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().ToList();
            summaryData.AvgWeek = temp.First() == null ? 0.0 : temp.Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status ?? "0"));

            return summaryData;
        }

        static void Main(string[] args)
        {
            //FileStream ostrm;
            //StreamWriter writer;
            //TextWriter oldOut = Console.Out;
            //try
            //{
            //    ostrm = new FileStream("./Output.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    writer = new StreamWriter(ostrm);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Cannot open Output.txt for writing");
            //    Console.WriteLine(e.Message);
            //    return;
            //}
            //Console.SetOut(writer);

            var sessionFactory = CreateSessionFactory();
            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.WriteLine("Starting initial population...");
                    Stopwatch stopwatch_g = new Stopwatch();
                    stopwatch_g.Start();

                    // Populate session
                    //initialPopulation(session);

                    //IList<DatahubDataObj> studentDataAllPeriods = session.QueryOver<DatahubDataAberdeen>().List<DatahubDataObj>();
                    //var listOfDataZoneSummaries = session.QueryOver<AberdeenSummary>().Where(x => x.type == "Data Zone").List<AberdeenSummary>();
                    //foreach (var item in listOfDataZoneSummaries)

                    //{
                    //    Console.WriteLine("starting for datazone {0} period: {1}/{2}", item.name, item.dataMonth, item.dataYear);
                    //    var calculation = getSubsetStudentsByZone(session, studentDataAllPeriods, "data zone", item.dataCode, item.dataMonth, item.dataYear);
                    //    AberdeenSummary currSummary = CalculateSummaryData(calculation, item.dataCode, item.dataCode, "Data Zone", item.dataMonth, item.dataYear);
                    //    foreach (var prop in item.GetType().GetProperties())
                    //    {
                    //        if (prop.Name != "id") prop.SetValue(item, prop.GetValue(currSummary));

                    //    }
                    //    session.Update(item);
                    //}

                    //var test = session.QueryOver<DatahubDataAberdeen>().List<DatahubDataAberdeen>();
                    //var testa = session.QueryOver<DatahubDataGlasgow>().List<DatahubDataGlasgow>();


                    // This line of code does magic!!!
                    SummaryGeneratorHelper.initialPopulationForCouncil<DatahubDataAberdeenshire>(session);

                    
                    Stopwatch stopwatch_t = new Stopwatch();
                    Console.WriteLine("Starting transaction...");
                    stopwatch_t.Start();
                    
                     //Begin transaction
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        transaction.Rollback();
                        throw;
                    }
                    stopwatch_t.Stop();
                    Console.WriteLine("Transaction: Completed");
                    Console.WriteLine("Time elapsed: {0}", stopwatch_t.Elapsed);

                    stopwatch_g.Stop();
                    Console.WriteLine("Initial population: Finalized");
                    Console.WriteLine("Total time elapsed: {0}", stopwatch_g.Elapsed);

                    session.Dispose();
                }
            }
            //Console.SetOut(oldOut);
            //writer.Close();
            //ostrm.Close();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static IList<DatahubDataObj> getSubsetStudentsByCouncil (IList<DatahubDataObj> allStudents, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
            return subsetStudents;
        }

        private static IList<DatahubDataObj> getSubsetStudentsBySchool (IList<DatahubDataObj> allStudents, string seedCode, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year && x.SEED_Code != null && x.SEED_Code.Equals(seedCode)).ToList();
            return subsetStudents;
        }

        private static IList<DatahubDataObj> getSubsetStudentsByZone (ISession session, IList<DatahubDataObj> allStudents, string zonetype, string zonecode, int month, int year)
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
                IList<DatahubDataObj> subsetStudentsThisPeriod = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
                subsetStudents = (from s in subsetStudentsThisPeriod
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

        protected static void CreateOneMonthEntry(ISession session, IList<DatahubDataObj> allStudents, string type, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = new Collection<DatahubDataObj>();

            switch (type.ToLower())
            {
                case "council":
                    break;
                case "intermediate zone":
                    break;
                case "data zone":
                    break;
                case "school":
                    break;
            }

            //List<SummaryData> SummaryEntries = new List<SummaryData>();       
            IList<AllSchools> AllSchools = session.QueryOver<AllSchools>().List<AllSchools>();
            foreach (AllSchools school in AllSchools)
            {
                IList<DatahubDataObj> allStudentsThisSchool = allStudents.Where(x => x.SEED_Code!=null && x.SEED_Code.Equals(school.seedCode)).ToList();
                AberdeenSummary currentSummary = CalculateSummaryData(allStudentsThisSchool, school.seedCode, school.name, "School", month, year);

                //SaveRowForEntity(session, currentSummary);
            }
            
            //foreach (var property in SummaryEntries[0].GetType().GetProperties())
            //{
            //    Console.WriteLine(property.Name + " : " + property.GetValue(SummaryEntries[0]).ToString());
            //}
        }

        private static void initialPopulation(ISession session)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Preparing data for all periods: Started");

            IList<DatahubDataObj> studentDataAllPeriods = session.QueryOver<DatahubDataObj>().List<DatahubDataObj>();
            var monthYearList = studentDataAllPeriods.Select(x => new { x.Data_Month, x.Data_Year }).ToList().Distinct().ToList();
            IList dataZonesList = session.QueryOver<DataZoneObj>().Where(x => x.Reference_Council == "S12000033").List().Select(x => x.Reference).ToList();
            IList<IntermediateZoneObj> intermediateZonesList = session.QueryOver<IntermediateZoneObj>().Where(x => x.Reference_Council == "S12000033").List();
            IList<AllSchools> allSchoolsList = session.QueryOver<AllSchools>().List<AllSchools>();
          
            stopwatch.Stop();
            Console.WriteLine("Preparing data for all periods: Completed");
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            stopwatch.Reset();


            foreach (var period in monthYearList)
            {
                stopwatch.Start();
                Console.WriteLine("");
                Console.WriteLine("Starting routine for {0}/{1}", period.Data_Month, period.Data_Year);
                int currMonth = period.Data_Month;
                int currYear = period.Data_Year;

                // Calculate council summary

                Console.WriteLine("Generating summary data and saving to session for Aberdeen City: Started");
                var allCouncilStudentsThisPeriod = getSubsetStudentsByCouncil(studentDataAllPeriods,currMonth,currYear);
                var councilSummary = CalculateSummaryData(allCouncilStudentsThisPeriod, "S12000033", "Aberdeen City", "Council", currMonth, currYear);
                session.Save(councilSummary);

                stopwatch.Stop();
                Console.WriteLine("Generating summary data and saving to session for Aberdeen City: Completed");
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                stopwatch.Reset();

                // Calculate intermediate zones summary
                stopwatch.Start();
                Console.WriteLine("Generating summary data and saving to session for {0} intermediate zones: Started", intermediateZonesList.Count());
                foreach (IntermediateZoneObj intZone in intermediateZonesList)
                {
                    var allIntermediateZoneStudentsThisPeriod = getSubsetStudentsByZone(session, studentDataAllPeriods, "intermediate zone", intZone.Reference, currMonth, currYear);
                    AberdeenSummary currSummary = CalculateSummaryData(allIntermediateZoneStudentsThisPeriod, intZone.Reference, intZone.Name,"Intermediate Zone", currMonth, currYear);
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
                    var allIntermediateZoneStudentsThisPeriod = getSubsetStudentsByZone(session, studentDataAllPeriods, "Data Zone", zonecode, currMonth, currYear);
                    AberdeenSummary currSummary = CalculateSummaryData(allIntermediateZoneStudentsThisPeriod, zonecode, zonecode, "Data Zone", currMonth, currYear);
                    session.Save(currSummary);
                }

                stopwatch.Stop();
                Console.WriteLine("Generating summary data and saving to session for data zones: Completed");
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                stopwatch.Reset();

                // Calculate all schools summary
                stopwatch.Start();
                Console.WriteLine("Generating summary data and saving to session for {0} schools: Started", allSchoolsList.Count());
                foreach (AllSchools school in allSchoolsList)
                {
                    var allSchoolStudentsThisPeriod = getSubsetStudentsBySchool(studentDataAllPeriods, school.seedCode, currMonth, currYear);
                    AberdeenSummary currSummary = CalculateSummaryData(allSchoolStudentsThisPeriod, school.seedCode, school.name, "School", currMonth, currYear);
                    session.Save(currSummary);
                }
                stopwatch.Stop();
                Console.WriteLine("Generating summary data and saving to session for schools: Completed");
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                stopwatch.Reset();

            }
            
            //Test:
            //IList<DatahubDataObj> result = getSubsetStudentsByZone(session, studentDataAllPeriods, "intermediate zone", "S02000024", 08, 2016);
            //IList<DatahubDataObj> result2 = getSubsetStudentsByZone(session, studentDataAllPeriods, "data zone", "S01000011", 08, 2016);
        }

        private static bool checkIfRefferencesAreConsistent(ISession session)
        {
            //Work in progress (essensially useless class)
            //Coming from neighbourhood table
            IList<AllSchools> allSchoolsList = session.QueryOver<AllSchools>().List<AllSchools>();
            IList<NeighbourhoodObj> allPostCodes = session.QueryOver<NeighbourhoodObj>().List<NeighbourhoodObj>();

            IEnumerable<string> intermediateZonesList = allPostCodes.Select(x => x.IntDataZone ).Distinct().ToList();
            IEnumerable<string> dataZonesList = allPostCodes.Select(x => x.DataZone ).Distinct().ToList();

            //Coming from map_data_table
            IList<DataZoneObj> dzList = session.QueryOver<DataZoneObj>().Where(x => x.Reference_Council == "S12000033").List<DataZoneObj>();
            IList<IntermediateZoneObj> izList = session.QueryOver<IntermediateZoneObj>().Where(x => x.Reference_Council == "S12000033").List<IntermediateZoneObj>();

            IEnumerable<string> izL = izList.Select(x => x.Reference).ToList();
            IEnumerable<string> dzL = dzList.Select(x => x.Reference).ToList();

            var isEqual = ScrambledEquals<string>(intermediateZonesList, izL);
            var isEqua2l = ScrambledEquals<string>(dataZonesList, dzL);

            return (isEqua2l && isEqual);
        }

        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }
    }
    
}
