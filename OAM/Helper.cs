using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace OAM
{
    public class Helper
    {
        public static IEnumerable GetUsersOfGroup(string groupName)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, System.Configuration.ConfigurationManager.AppSettings["DomainNAme"]);
            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, System.Configuration.ConfigurationManager.AppSettings["UserGroup"]);

            IEnumerable res;

            if (grp != null)
            {
                res = from c in grp.Members
                      select new { name = c.SamAccountName };
            }
            else
                 res = null;

            return res;
        }
    }
}