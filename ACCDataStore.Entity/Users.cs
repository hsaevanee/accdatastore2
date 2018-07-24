using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Users : BaseEntity
    {
        public virtual int UserID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string job_title { get; set; }
        public virtual string email { get; set; }
        public virtual bool IsAdministrator { get; set; }
        public virtual bool IsSchoolAdministrator { get; set; }
        public virtual bool IsDataHubAdministrator { get; set; }
        public virtual bool IsPublicUser { get; set; }
        public virtual string enablekey { get; set; }
        public virtual int enable { get; set; }

    }
}
