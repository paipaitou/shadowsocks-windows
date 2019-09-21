using Shadowsocks.Controller;
using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowsocks.Model
{
    public partial class Server
    {
        public int Latency = LATENCY_UNKNOWN;

        public const int LATENCY_UNKNOWN = -1;
        public const int LATENCY_TESTING = -2;
        public const int LATENCY_PENDING = -3;
        public const int LATENCY_ERROR = -4;

        public const int PREFIX_MENU = 0;
        public const int PREFIX_LIST = 1;

        public string NamePrefix(Configuration config, int PREFIX_FLAG)
        {
            string prefix = "[";
            if (PREFIX_FLAG == PREFIX_MENU)
            {                
                switch (Latency)
                {
                    case LATENCY_UNKNOWN:
                        prefix += I18N.GetString("Unknown");
                        break;
                    case LATENCY_TESTING:
                        prefix += I18N.GetString("Testing");
                        break;
                    case LATENCY_ERROR:
                        prefix += I18N.GetString("Error");
                        break;
                    case LATENCY_PENDING:
                        prefix += I18N.GetString("Pending");
                        break;
                    default:
                        prefix += Latency.ToString() + "ms";
                        break;
                }                
            }            

            prefix += "]";

            return prefix;
        }

        public void TcpingLatency()
        {
            Latency = LATENCY_TESTING;
            var latencies = new List<double>();
            var stopwatch = new Stopwatch();
            for (var testTime = 0; testTime <= 1; testTime++)
            {
                try
                {
                    var socket = new TcpClient();
                    var ip = Dns.GetHostAddresses(server);
                    stopwatch.Start();
                    var result = socket.BeginConnect(ip[0], server_port, null, null);
                    
                    if (result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)))
                    {
                        stopwatch.Stop();
                        latencies.Add(stopwatch.Elapsed.TotalMilliseconds);
                    }
                    else
                    {
                        stopwatch.Stop();
                    }
                    socket.Close();
                }
                catch (Exception)
                {

                }
                stopwatch.Reset();
            }
            
            if (latencies.Count != 0)
            {
                Latency = (int)latencies.Average();
            }
            else
            {
                Latency = LATENCY_ERROR;
            }
            
        }
    }
}
