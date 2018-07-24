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
    public class SummaryDataHelper
    {
        
        public SummaryDataHelper(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        private IGenericRepository2nd rpGeneric2nd;

        public SummaryDataViewModel GetSummaryDataForCouncil<T>(string code, int month, int year) where T : SummaryData
        {
            //SummaryData result = new SummaryData();
            //switch (councilName.ToLower())
            //{
            //    case "aberdeen city":
            //        result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            //        break;
            //    default:
            //        result = null;
            //        break;
            //}

            var result = rpGeneric2nd.QueryOver<T>().Where(x => x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        public IList<SummaryDataViewModel> GetSummaryDataForAllDataZones<T>(int month, int year) where T : SummaryData
        {
            var result = rpGeneric2nd.QueryOver<T>().Where(x => x.type.Equals("Data Zone") && x.dataMonth == month && x.dataYear == year).List<SummaryData>();
            return _CreateListOfViewModels(result);
        }

        public SummaryDataViewModel GetSummaryDataForSingleDataZone<T>(string code, int month, int year) where T : SummaryData
        {
            var result = rpGeneric2nd.QueryOver<T>().Where(x => x.type.Equals("Data Zone") && x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        public SummaryDataViewModel GetSummaryDataForSingleIntermediateZone<T>(string code, int month, int year) where T : SummaryData
        {
            var result = rpGeneric2nd.QueryOver<T>().Where(x => x.type.Equals("Intermediate Zone") && x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        public IList<SummaryDataViewModel> GetSummaryDataForAllIntermediateZones<T>(int month, int year) where T : SummaryData
        {
            var result = rpGeneric2nd.QueryOver<T>().Where(x => x.type.Equals("Intermediate Zone") && x.dataMonth == month && x.dataYear == year).List<SummaryData>();

            return _CreateListOfViewModels(result);
        }

        public IList<SummaryDataViewModel> GetSummaryDataForAllSchools<T>(string councilCode, int month, int year) where T : SummaryData
        {
            IList<SummaryData> result = new Collection<SummaryData>();
            IList<AllSchools> allSchoolsForCouncil = this.rpGeneric2nd.QueryOver<AllSchools>().Where(x => x.referenceCouncil == councilCode).List();
            foreach (var school in allSchoolsForCouncil)
            {
                // At this point we should know to witch council/city to point to [TODO]
                //var query = _SummaryDataQueryCouncilHelper("Aberdeen City");
                //Type type = typeof(AberdeenSummary);
                //Assembly n = type.Assembly;
                //System.Reflection.Assembly a = typeof(AberdeenSummary).Assembly;

                SummaryData currentSummary = (SummaryData)this.rpGeneric2nd.Query<T>()
                                            .Where(x => x.type.Equals("School") && x.dataCode.Equals(school.seedCode) && x.dataMonth == month && x.dataYear == year)
                                            .SingleOrDefault();
                result.Add(currentSummary);
            }
            return _CreateListOfViewModels(result);
        }

        public SummaryDataViewModel GetSummaryDataForSingleSchool<T>(string seedCode, int month, int year ) where T : SummaryData
        { 
            SummaryData currentSummary = (SummaryData) this.rpGeneric2nd.Query<T>()
                                            .Where(x => x.type.Equals("School") && x.dataCode.Equals(seedCode) && x.dataMonth == month && x.dataYear == year)
                                            .SingleOrDefault();
            return new SummaryDataViewModel(currentSummary); 
        }


        //// WIP
        //private IQueryable<SummaryData> _SummaryDataQueryCouncilHelper(string councilName)
        //{
        //    IQueryable<SummaryData> queryOver;
        //    switch (councilName.ToLower())
        //    {
        //        case "aberdeen city":
        //            queryOver = (IQueryable<AberdeenSummary>) this.rpGeneric2nd.Query<AberdeenSummary>();
        //            break;
        //        default:
        //            queryOver = null;
        //            break;
        //    }
        //    return queryOver;
        //}

        //public IList<SummaryData> _Tester<T>(string inz) where T : SummaryData
        //{
            
        //    var q = this.rpGeneric2nd.QueryOver<T>().Where(x => x.name == inz).List<SummaryData>();
            

        //    return q;
        //}


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
                allCouncils.Add(GetSummaryDataForCouncil<AberdeenSummary>(city, month, year));
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