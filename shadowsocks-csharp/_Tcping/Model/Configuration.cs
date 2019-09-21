﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadowsocks.Model
{
    public partial class Configuration
    {
        
        public void TcpingLatencyAll()
        {
            new Task(() => {
                try
                {
                    foreach (var server in configs)
                    {
                        server.Latency = Server.LATENCY_PENDING;
                    }
                    foreach (var server in configs)
                    {
                        server.TcpingLatency();
                    }
                }
                catch (Exception)
                {

                }
            }).Start();
        }
       
    }
}
