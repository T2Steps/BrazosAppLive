using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility
{
    public static class SD
    {
        //Roles
        public const string SuperAdmin = "SuperAdmin";
        public const string AdminInspector = "Admin Inspector";
        public const string Admin = "Admin";
        public const string Inspector = "Inspector";
        public const string Clerk = "Clerk";
        public const string ViewOnly = "View Only";

        //Assign Type
        public const string SystemAssigned = "System Assigned";
        public const string DefaultAssigned = "Default Assigned";
        public const string NormalAssigned = "Normal Assigned";

        //Permit Status Name Fetch
        public static string PermitStatName(int id)
        {
            if (id == 1)
            {
                return "Incomplete";
            }
            else if (id == 2)
            {
                return "Pending Admin Review";
            }
            else if (id == 3)
            {
                return "Admin Review";
            }
            else if (id == 4)
            {
                return "Pending Plan Review";
            }
            else if (id == 5)
            {
                return "Plan Review";
            }
            else if (id == 6)
            {
                return "Pending Build-Out";
            }
            else if (id == 7)
            {
                return "Pending Payment";
            }
            else if (id == 8)
            {
                return "Opening Inspection";
            }
            else if (id == 9)
            {
                return "Active";
            }
            else if (id == 13)
            {
                return "Inactive";
            }
            return "";
        }
    }
}
