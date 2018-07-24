using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class SchoolRollForecastMap : ClassMap<SchoolForecast>
    {
        public SchoolRollForecastMap() {
            Table("schoolrollforecast");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.F2008);
            Map(x => x.F2009);
            Map(x => x.F2010);
            Map(x => x.F2011);
            Map(x => x.F2012);
            Map(x => x.F2013);
            Map(x => x.F2014);
            Map(x => x.F2015);
            Map(x => x.F2016);
            Map(x => x.F2017);
            Map(x => x.F2018);
            Map(x => x.F2019);
            Map(x => x.F2020);
            Map(x => x.F2021);
            Map(x => x.F2022);
        }

    }

}
