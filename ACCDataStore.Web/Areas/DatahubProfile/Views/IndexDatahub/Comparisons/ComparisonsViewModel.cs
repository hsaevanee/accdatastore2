using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class ComparisonsViewModel
    {

        public IList<School> ListSchoolNameData { get; set; }
        public IList<School> ListNeighbourhoodsName { get; set; }
        public IList<DatahubData> ListSelectionData { get; set; }
    }
}