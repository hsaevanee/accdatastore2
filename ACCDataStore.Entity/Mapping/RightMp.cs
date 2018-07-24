using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping
{
    public class RightMp : ClassMap<Rights>
    {
        public RightMp() 
        {
            Id(x => x.ID);
            Map(x => x.SheetID);
            Map(x => x.CategoryID);
            Map(x => x.RightCode);

        }

    }
}
