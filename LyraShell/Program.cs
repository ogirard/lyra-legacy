using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using log4net.Config;

namespace Lyra2.LyraShell
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XmlConfigurator.Configure();
            Application.Run(new GUI());
        }
    }
}