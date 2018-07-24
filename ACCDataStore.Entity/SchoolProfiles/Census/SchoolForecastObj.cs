using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class SchoolForecastObj: BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual int SeedCode { get; set; }
        //public virtual double F2016 { get; set; }
        public virtual double F2008 { get; set; }
        public virtual double F2009 { get; set; }
        public virtual double F2010 { get; set; }
        public virtual double F2011 { get; set; }
        public virtual double F2012 { get; set; }
        public virtual double F2013 { get; set; }
        public virtual double F2014 { get; set; }
        public virtual double F2015 { get; set; }
        public virtual double F2016 { get; set; }
        public virtual double F2017 { get; set; }
        public virtual double F2018 { get; set; }
        public virtual double F2019 { get; set; }
        public virtual double F2020 { get; set; }
        public virtual double F2021 { get; set; }
        public virtual double F2022 { get; set; }
    }

    public class SchoolForecast : SchoolForecastObj
    {
    }

}
