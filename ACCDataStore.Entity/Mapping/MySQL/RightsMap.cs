using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ACCDataStore.Entity;

namespace ACCDataStore.Entity
{
    public class RightsMap : ClassMap<Rights>
    {
        public RightsMap()
        {
            Id(x => x.ID);
            References(x => x.Users, "UserID");
            Map(x => x.SheetID);
            Map(x => x.GroupID);
            //References(x => x.Category, "CategoryID");
            //References(x => x.LocationInfo, "LocationID");
            //References(x => x.SiteInfo, "SiteID");
            //References(x => x.DeviceList, "DeviceID");
            //References(x => x.DeviceParams, "ParamID").NotFound.Ignore();
            References(x => x.RightsCode, "RightCode");
            Map(x => x.RightValue);
        }
    }
}
