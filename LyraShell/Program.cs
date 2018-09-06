using log4net.Config;
using System;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    internal static class Program
    {
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