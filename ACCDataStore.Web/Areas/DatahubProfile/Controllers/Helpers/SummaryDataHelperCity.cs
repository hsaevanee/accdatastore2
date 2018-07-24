using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Helpers
{

    public class CouncilHelper
    {
        /// <summary>
        /// Class instantiator
        /// </summary>
        /// <param name="repository">NHibernate repository object used to query the database</param>
        public CouncilHelper(IGenericRepository2nd repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// NHibernate repository object used to query the database
        /// </summary>
        protected IGenericRepository2nd repository;

        public SummaryDataHelperNew ByName(string name)
        {
            switch(name.ToLower())
            {
                case "aberdeen city":
                    return new SummaryDataHelperNew<AberdeenSummary>(repository,"S12000033");
                case "glasgow city":
                    return new SummaryDataHelperNew<GlasgowSummary>(repository, "S12000046");
                case "aberdeenshire":
                    return new SummaryDataHelperNew<AberdeenshireSummary>(repository, "S12000034");
                default:
                    return null;

            }
        }

        /// <summary>
        /// Retrieves a list of all council names.
        /// </summary>
        /// <returns>IList of School objects containing council data.</returns>
        public IList<School> GetAllCouncilList()
        {
            IList<School> results = this.repository.QueryOver<CouncilObj>().List().Select(x => new School { name = x.Name, seedcode = x.Reference }).ToList();
            return results;
        }

        public SummaryDataViewModel GetScotlandSummary(int month, int year)
        {
            SummaryData dummy = new SummaryData();
            List<string> exclussions = new List<string>();
            string[] cities = new string[2] { "aberdeen city", "glasgow city" };
            exclussions.Add("name");
            exclussions.Add("dataCode");
            exclussions.Add("type");
            dummy.name = "Scotland";
            dummy.dataCode = "SC0000001";
            dummy.type = "National";
            foreach (var prop in dummy.GetType().GetProperties())
            {
                if (!exclussions.Contains(prop.Name))
                {
                    prop.SetValue(dummy, 0);
                }
            }
            SummaryDataViewModel allScotland = new SummaryDataViewModel(dummy);
            List<SummaryDataViewModel> allCouncils = new List<SummaryDataViewModel>();
            foreach (string city in cities)
            {
                var tempCouncilData = ByName(city);
                if (tempCouncilData != null)
                {
                    allCouncils.Add(tempCouncilData.GetSummaryDataForCouncil(month, year));
                }
            }
            foreach (SummaryDataViewModel council in allCouncils)
            {
                if (council != null)
                {
                    foreach (var prop in council.GetType().GetProperties())
                    {
                        if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(allScotland, ((int)prop.GetValue(allScotland) + (int)prop.GetValue(council)));
                        }
                        if (prop.PropertyType == typeof(double))
                        {
                            prop.SetValue(allScotland, ((double)prop.GetValue(allScotland) + (double)prop.GetValue(council)));
                        }
                    }
                }
            }
            return allScotland;
        }

        /// <summary>
        /// Check whether the specified council name is valid
        /// </summary>
        /// <param name="name">Council name</param>
        /// <returns>Returns True if council name is valid</returns>
        public bool ValidateCouncilName(string name)
        {
            if (name != null)
            {
                switch (name.ToLower())
                {
                    case "aberdeen city":
                        return true;
                    case "glasgow city":
                        return true;
                    case "aberdeenshire":
                        return true;
                    default:
                        return false;

                }
            }
            return false;
        }
    }

    public interface SummaryDataHelperNew
    {
        SummaryDataViewModel GetSummaryDataForCouncil(int month, int year);
        SummaryDataViewModel GetSummaryDataForSingleIntermediateZone(string code, int month, int year);
        IList<SummaryDataViewModel> GetSummaryDataForAllIntermediateZones(int month, int year);
        SummaryDataViewModel GetSummaryDataForSingleDataZone(string code, int month, int year);
        IList<SummaryDataViewModel> GetSummaryDataForAllDataZones(int month, int year);
        SummaryDataViewModel GetSummaryDataForSingleSchool(string seedCode, int month, int year);
        IList<SummaryDataViewModel> GetSummaryDataForAllSchools(int month, int year);
        IList<School> GetSchoolsList();
        IList<School> GetIntermediateZonesList();
        IList<School> GetDataZonesList();
    }

    /// <summary>
    /// This class contains helpers to access summary data for a specific council
    /// </summary>
    /// <typeparam name="T">Fluent NHibernate object class used to map the specific city table</typeparam>
    public class SummaryDataHelperNew<T> : CouncilHelper, SummaryDataHelperNew where T : SummaryData
    {
        private string councilId;
        /// <summary>
        /// Class instantiator
        /// </summary>
        /// <param name="repository">NHibernate repository object used to query the database</param>
        public SummaryDataHelperNew(IGenericRepository2nd repository, string councilId) : base (repository)
        {
            this.repository = repository;
            this.councilId = councilId;
        }

        /// <summary>
        /// Retrieves summary data for the specific council, for a specific period in time
        /// </summary>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>A SummaryDataViewModel object</returns>
        public SummaryDataViewModel GetSummaryDataForCouncil(int month, int year)
        {
            try
            {
                var result = repository.QueryOver<T>().Where(x => x.type.Equals("Council") && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
                return new SummaryDataViewModel(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves summary data for single intermediate zone for a specific period in time
        /// </summary>
        /// <param name="code">Intermediate zone code as persisted in the database</param>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>A SummaryDataViewModel object</returns>
        public SummaryDataViewModel GetSummaryDataForSingleIntermediateZone(string code, int month, int year)
        {
            var result = repository.QueryOver<T>().Where(x => x.type.Equals("Intermediate Zone") && x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            SummaryDataViewModel output = null;
            if (result != null)
            {
                output = new SummaryDataViewModel(result);
            }
            return output;
        }

        /// <summary>
        /// Retrieves summary data for all intermediate zones for a specific period in time
        /// </summary>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>An IList containing SummaryDataViewModel objects</returns>
        public IList<SummaryDataViewModel> GetSummaryDataForAllIntermediateZones(int month, int year)
        {
            var result = repository.QueryOver<T>().Where(x => x.type.Equals("Intermediate Zone") && x.dataMonth == month && x.dataYear == year).List<SummaryData>();

            return _CreateListOfViewModels(result);
        }

        /// <summary>
        /// Retrieves summary data for single data zone for a specific period in time
        /// </summary>
        /// <param name="code"></param>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>A SummaryDataViewModel object</returns>
        public SummaryDataViewModel GetSummaryDataForSingleDataZone(string code, int month, int year)
        {
            var result = repository.QueryOver<T>().Where(x => x.type.Equals("Data Zone") && x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        /// <summary>
        /// Retrieves summary data for all data zones for a specific period in time
        /// </summary>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>An IList containing SummaryDataViewModel objects</returns>
        public IList<SummaryDataViewModel> GetSummaryDataForAllDataZones(int month, int year)
        {
            var result = repository.QueryOver<T>().Where(x => x.type.Equals("Data Zone") && x.dataMonth == month && x.dataYear == year).List<SummaryData>();
            return _CreateListOfViewModels(result);
        }

        /// <summary>
        /// Retrieves summary data for single school for a specific period in time
        /// </summary>
        /// <param name="seedCode"></param>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>A SummaryDataViewModel object</returns>
        public SummaryDataViewModel GetSummaryDataForSingleSchool(string seedCode, int month, int year)
        {
            SummaryData currentSummary = (SummaryData)this.repository.Query<T>()
                                            .Where(x => x.type.Equals("School") && x.dataCode == seedCode && x.dataMonth == month && x.dataYear == year)
                                            .SingleOrDefault();
            SummaryDataViewModel output = null;
            if (currentSummary != null)
            {
                output = new SummaryDataViewModel(currentSummary);
            }
            return output;
        }

        /// <summary>
        /// Retrieves summary data for all schools for a specific period in time
        /// </summary>
        /// <param name="month">Month for the period of interest. Accepted values: [1,2,3,4,5,6,7,8,9,10,11,12]</param>
        /// <param name="year">Year for the period of interest. Accepted values: four digit integers</param>
        /// <returns>An IList containing SummaryDataViewModel objects</returns>
        public IList<SummaryDataViewModel> GetSummaryDataForAllSchools(int month, int year)
        {
            //This has to change; we need to infer councilcode from the type paramether
            //Easy solution is to execute a query over this, get type
            //So this begins here
            //string councilCode = this.repository.QueryOver<T>().Where(x => x.type == "Council" && x.dataMonth == month && x.dataYear == year).Select(x => x.dataCode).SingleOrDefault<string>();
            //We should test this
            //End of bulcrap, everything below is fine, trust me! :D

            IList<SummaryData> result = new Collection<SummaryData>();
            IList<AllSchools> allSchoolsForCouncil = this.repository.QueryOver<AllSchools>().Where(x => x.referenceCouncil == this.councilId).List();
            foreach (var school in allSchoolsForCouncil)
            {
                SummaryData currentSummary = (SummaryData)this.repository.Query<T>()
                                            .Where(x => x.type.Equals("School") && x.dataCode.Equals(school.seedCode) && x.dataMonth == month && x.dataYear == year)
                                            .SingleOrDefault();
                result.Add(currentSummary);
            }
            return _CreateListOfViewModels(result);
        }

        //  Ideally we would want to use a different name for School object => i.e. object Selected { name, code, type }  
        /// <summary>
        /// Retrieves schools of interest for the particular council from the database
        /// </summary>
        /// <returns>IList of School objects with their corresponding name and seedcode</returns>
        public IList<School> GetSchoolsList()
        {
            IList<School> result = this.repository.QueryOver<AllSchools>().Where(x => x.referenceCouncil == this.councilId).List().Select(x => new School {seedcode = x.seedCode, name = x.name, schooltype = "School" }).ToList();
            return result;
        }

        /// <summary>
        /// Retrieves all intermediate zones for council
        /// </summary>
        /// <returns>IList of School objects with their corresponding name and datacode</returns>
        public IList<School> GetIntermediateZonesList()
        {
            IList<School> result = this.repository.QueryOver<IntermediateZoneObj>().Where(x => x.Reference_Council == this.councilId).List().Select(x => new School { seedcode = x.Reference, name = x.Name, schooltype = "Intermediate Zone" }).ToList();
            return result;
        }

        /// <summary>
        /// Retrieves all data zones for council
        /// </summary>
        /// <returns>IList of School objects with their corresponding name and datacode</returns>
        public IList<School> GetDataZonesList()
        {
            IList<School> result = this.repository.QueryOver<DataZoneObj>().Where(x => x.Reference_Council == this.councilId).List().Select(x => new School { seedcode = x.Reference, name = x.Reference, schooltype = "Data Zone" }).ToList();
            return result;
        }

        

        /// <summary>
        /// Helper method used to convert IList of SummaryData objects to IList of SummaryDataViewModel objects
        /// </summary>
        /// <param name="summaryDataList">IList of SummaryData objects</param>
        /// <returns>An IList containing SummaryDataViewModel objects</returns>
        private IList<SummaryDataViewModel> _CreateListOfViewModels(IList<SummaryData> summaryDataList)
        {
            IList<SummaryDataViewModel> summaryDataViewModelList = new Collection<SummaryDataViewModel>();
            IList<decimal> percentageData = new Collection<decimal>();

            foreach (SummaryData item in summaryDataList)
            {
                summaryDataViewModelList.Add(new SummaryDataViewModel(item));
            }
            return summaryDataViewModelList;
        }

        //ToDo: Helper classes for error handling
        //Maybe: Queries for a subset of schools, intermediate and data zones


        /// <summary>
        /// This class should should be defined in the superclass
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public SummaryDataViewModel GetScotlandSummary(int month, int year)
        {
            SummaryData dummy = new SummaryData();
            List<string> exclussions = new List<string>();
            string[] cities = new string[2] { "Aberdeen City", "Glasgow City" };
            exclussions.Add("name");
            exclussions.Add("dataCode");
            exclussions.Add("type");
            dummy.name = "Scotland";
            dummy.dataCode = "SC0000001";
            dummy.type = "National";
            foreach (var prop in dummy.GetType().GetProperties())
            {
                if (!exclussions.Contains(prop.Name))
                {
                    prop.SetValue(dummy, 0);
                }
            }
            SummaryDataViewModel allScotland = new SummaryDataViewModel(dummy);
            List<SummaryDataViewModel> allCouncils = new List<SummaryDataViewModel>();
            foreach (string city in cities) 
            {
                allCouncils.Add(GetSummaryDataForCouncil(month, year));
            }
            foreach (SummaryDataViewModel council in allCouncils)
            {
                foreach (var prop in council.GetType().GetProperties()) 
                {
                    prop.SetValue(allScotland, ((double)prop.GetValue(allScotland) + (double)prop.GetValue(council)));// <--- Questionable code!!! Start from here
                }
            } 
            return allScotland;
        }
    }
}