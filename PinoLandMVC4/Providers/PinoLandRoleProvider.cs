using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

using MG = Fuqua.CompetativeAnalysis.MarketGame;

namespace PinoLandMVC4.Providers
{
    public class PinoLandRoleProvider : RoleProvider
    {
        private static string APPLICATION_NAME = "PinoLand";
        private string connectionString = null;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            string connectionStringName = config["connectionStringName"];
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            base.Initialize(name, config);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return APPLICATION_NAME;
            }
            set
            {
                throw new ArgumentException("ApplicationName is read only.");
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (MG.GameDataObjectContext context = new MG.GameDataObjectContext(connectionString))
            {
                return context.IM_User.SingleOrDefault(u => string.Compare(u.UserName, username, true) == 0)
                    .Roles.Select(r => r.Name).ToArray<string>();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (MG.GameDataObjectContext context = new MG.GameDataObjectContext(connectionString))
            {
                return
                    //Get the user
                    context.IM_User.SingleOrDefault(u => string.Compare(u.UserName, username, true) == 0)
                    //and any matching role(s)
                    .Roles.Where(r => string.Compare(r.Name, roleName, true) == 0).Count() >= 1;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}