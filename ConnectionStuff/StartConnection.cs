using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.ConnectionStuff
{
    class StartConnection
    {
        public static string SendToServer(byte[] msg)
        {
            byte[] bytes = new byte[4096];
            Socket sender;
            IPEndPoint remoteEP;
            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, 42088);

                // Create a TCP/IP  socket.    
                    sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            try
            {
                // Connect to Remote EndPoint  
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());
                    
                // Send the data through the socket.    
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.    
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.    
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return null;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return null;
            }
        }
    }
}
