using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Helpers
{
    class ServerHelper
    {
        public const int MAX_PORT = 50000;
        public const int MIN_PORT = 40000;

        public int Port { get; private set; }
        public Process Server { get; private set; }

        public ServerHelper(int? port = null)
        {
            if (port != null)
            {
                if (!IsPortAvaliable(port.Value))
                {
                    throw new ArgumentException(string.Format("The port {0} is unavailable.", port));
                }
            }
            else
            {
                Random rand = new Random();

                do
                {
                    port = rand.Next(MIN_PORT, MAX_PORT);
                } while (!IsPortAvaliable(port.Value));
            }

            Port = port.Value;
        }

        ~ServerHelper()
        {
            Stop();
        } 

        private bool IsPortAvaliable(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    return false;
                }
            }

            return true;
        }

        public void Start()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration("Catfish");
        }

        public void Stop()
        {

        }
    }
}
