using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace subdClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DataSource dataSource = DataSource.WebAPI;
        public static string WebApiHost = "http://localhost:58339";
        public static DataReader dataReader;
        public static string Name { get; set; }
        public static string SecurityLevel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
           // WebApiHost = Properties.Settings
            base.OnStartup(e);
        }
    }
}
