using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwd.Authentication.Model
{
    public class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }

        public ICollection<Role> Roles { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime LastLockedOutDate { get; set; }
        
        public DateTime LastLockoutDate { get; set; }
    }
}
