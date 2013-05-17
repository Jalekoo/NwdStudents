using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwd.Authentication.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display( Name = "Nom d'utilisateur" )]
        public string Username { get; set; }

        [Required]
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [Required]
        [Display( Name = "Email" )]
        public string Email { get; set; }
    }
}
