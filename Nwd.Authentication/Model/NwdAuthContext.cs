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

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Entity<User>()
                .Property( f => f.CreationDate )
                .HasColumnType( "datetime2" )
                .HasPrecision( 0 );

            modelBuilder.Entity<User>()
                .Property( f => f.LastActivityDate )
                .HasColumnType( "datetime2" )
                .HasPrecision( 0 );

            modelBuilder.Entity<User>()
                .Property( f => f.LastLockedOutDate )
                .HasColumnType( "datetime2" )
                .HasPrecision( 0 );

            modelBuilder.Entity<User>()
                .Property( f => f.LastLockoutDate )
                .HasColumnType( "datetime2" )
                .HasPrecision( 0 );

            modelBuilder.Entity<User>()
                .Property( f => f.LastLoginDate )
                .HasColumnType( "datetime2" )
                .HasPrecision( 0 );

            modelBuilder.Entity<User>()
                .Property( f => f.LastPasswordChangedDate )
                .HasColumnType( "datetime2" )
                .HasPrecision( 0 );

            base.OnModelCreating( modelBuilder );
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
    }
}
