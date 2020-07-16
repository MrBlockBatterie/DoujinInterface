using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.Connection.JSON
{
    public class LoginForm
    {
        public string username { get; set; }
        public string password { get; set; }

        public LoginForm(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
