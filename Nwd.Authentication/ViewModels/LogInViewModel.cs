using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwd.Authentication.ViewModels
{
    public class LogInViewModel
    {
        [Required]
        [Display( Name = "Nom d'utilisateur" )]
        public string Username { get; set; }

        [Required]
        [Display( Name = "Mot de passe" )]
        [DataType( DataType.Password )]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
