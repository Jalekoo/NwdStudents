using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwd.Authentication.Model
{
    public class NwdAuthContext : DbContext
    {
        public NwdAuthContext()
            : base( "NwdMusikAuth" )
        {
        }
        
        public DbSet<User> Users { get; set; }
    }
}
