using Shadowsocks.Controller;
using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class MenuViewController
    {
        private Configuration ConfigurationCurrent;

        private Configuration _GetConfigurationCurrent()
        {
            ConfigurationCurrent = controller.GetCurrentConfiguration();
            return ConfigurationCurrent;
        }


        private void PreloadMenu(object sender, EventArgs e)
        {
            UpdateServersMenu();
        }

        private void _InitOther()
        {            
            contextMenu1.Popup += PreloadMenu;
        }

        private MenuItem _AdjustServerName(Server server) {
            
            
            return new MenuItem(server.NamePrefix(ConfigurationCurrent, Server.PREFIX_MENU) + " " + server.FriendlyName());
        }

        private MenuItem _CreateTcpingLatency()
        {
            return new MenuItem(
                I18N.GetString("Tcping Latency"),
                (sender, e) => {
                    ConfigurationCurrent.TcpingLatencyAll();
                });
        }
    }
}
