using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Identity
{
    public class AppConstants
    {
        public const string JWTKEY = "publicconststringpublicconststringpublic";

        public const string SUPERADMIN_USERNAME = "a@a.com";
        public const string SUPERADMIN_PASSWORD = "admin123";

        public const string OWNER_USERNAME = "owner@owner.com";
        public const string OWNER_PASSWORD = "salam123";

        public const string CUS_USERNAME = "cus@cus.com";
        public const string CUS_PASSWORD = "salam123";

        public static class Roles
        {
            public const string ADMINROLE = "Admin";
            public const string CUSTOMERROLE = "Customer";
            public const string OWNERROLE = "OWNER";
        }
    }
}
