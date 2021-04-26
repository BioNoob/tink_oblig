using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tinkoff.Trading.OpenApi.Network;
using Tinkoff.Trading.OpenApi.Models;
using tink_oblig.classes;

namespace tink_oblig
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
        private static Context _CurrentContext;
        public static Context CurrentContext
        {
            get
            {
                return _CurrentContext;
            }
            set
            {
                InnerAccount = new Accounts();
                _CurrentContext = value;
            }
        }
        public static Accounts InnerAccount { get; set; }
    }
}
