using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Doujin_Interface.ConnectionStuff;

namespace Doujin_Interface.ConnectionStuff.Shared
{
     class SendType
    {
        static string newparam = "<newParam>";
        public class User
        {
            string username;
            string email;
            string password;
            string seasionID;

            public static void CreateAccount(string username, string email, string password)
            {
                string message;
                byte[] msg = Encoding.ASCII.GetBytes("createAccount" + newparam + username + newparam + email + newparam + password + "<EOF>");
                StartConnection.SendToServer(msg);
            }
            
        }
    }
}
