using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Id(x => x.UserID);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            Map(x => x.job_title);
            Map(x => x.email);
            Map(x => x.IsAdministrator);
            Map(x => x.IsSchoolAdministrator);
            Map(x => x.IsDataHubAdministrator);
            Map(x => x.IsPublicUser);
            Map(x => x.enable);
            Map(x => x.enablekey);

        }
    }
}
