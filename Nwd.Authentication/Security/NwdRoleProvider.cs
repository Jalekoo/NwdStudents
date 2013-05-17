using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Nwd.Authentication.Model;

namespace Nwd.Authentication.Security
{
    public class NwdRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles( string[] usernames, string[] roleNames )
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return "NwdRoleProvider";
            }
            set
            {
                return;
            }
        }

        public override void CreateRole( string roleName )
        {
            using( var c = new NwdAuthContext() )
            {
                Role r = c.Roles.Where( m => m.RoleName == roleName ).FirstOrDefault();
                if( r != null ) return;

                c.Roles.Add( new Role() { RoleName = roleName } );
            }
        }

        public override bool DeleteRole( string roleName, bool throwOnPopulatedRole )
        {
            using( var c = new NwdAuthContext() )
            {
                Role r = c.Roles.Where( m => m.RoleName == roleName ).FirstOrDefault();
                if( r == null ) return false;

                c.Roles.Remove( r );
                c.SaveChanges();
                return true;
            }
        }

        public override string[] FindUsersInRole( string roleName, string usernameToMatch )
        {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles()
        {
            using( var c = new NwdAuthContext() )
            {
                return c.Roles.Select( m => m.RoleName ).ToArray();
            }
        }

        public override string[] GetRolesForUser( string username )
        {
            using( var c = new NwdAuthContext() )
            {
                return c.Users.Where( m => m.Username == username ).SelectMany( m => m.Roles ).Select( m => m.RoleName ).ToArray();
            }
        }

        public override string[] GetUsersInRole( string roleName )
        {
            using( var c = new NwdAuthContext() )
            {
                return c.Roles.Where( r => r.RoleName == roleName ).SelectMany( u => u.Users.Select( m => m.Username ) ).ToArray();
            }
        }

        public override bool IsUserInRole( string username, string roleName )
        {
            using( var c = new NwdAuthContext() )
            {
                return c.Roles.Where( r => r.RoleName == roleName ).SelectMany( u => u.Users.Where( m => m.Username == username ) ).Any();
            }
        }

        public override void RemoveUsersFromRoles( string[] usernames, string[] roleNames )
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists( string roleName )
        {
            using( var c = new NwdAuthContext() )
            {
                return c.Roles.Where( r => r.RoleName == roleName ).Any();
            }
        }
    }
}
