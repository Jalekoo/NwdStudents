using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwd.Authentication.Model
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
