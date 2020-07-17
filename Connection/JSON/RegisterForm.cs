using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.Connection.JSON
{
    class RegisterForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisterForm(string email, string password, string confirm)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirm;
        }
    }
}
