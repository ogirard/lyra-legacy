using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using log4net;
using log4net.Config;
using Lyra2.UtilShared;

namespace Lyra2.LyraShell
{
    internal static class Program
    {
        private static ILog logger;

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XmlConfigurator.Configure();
            logger = LogManager.GetLogger(typeof(Program));
            logger.Info($"Windows: {Environment.OSVersion.Platform}/{Environment.OSVersion.VersionString}");
            logger.Info($".NET: {AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName}");
            foreach (var screen in Screen.AllScreens)
            {
                logger.Info($"{(screen.Primary ? "Primary " : string.Empty)}Screen {screen.DeviceName}: {screen.WorkingArea.Width}x{screen.WorkingArea.Height}");
            }
            MigrateData(args.IsSwitchSet("clean"));
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.Run(new GUI());
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            logger.Fatal($"Unhandled exception {ex?.GetType().FullName}!!", ex);
            var lyraLog = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Lyra\\lyra.log";
            var desktopLyraLog = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\lyra-errorlog-{DateTime.UtcNow:yyyyMMddTHHmmss}.txt";

            try
            {
                using (var fileStream = new FileStream(lyraLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        var currentLogs = streamReader.ReadToEnd();
                        File.WriteAllText(desktopLyraLog, currentLogs);
                    }
                }
            }
            catch (Exception innerException)
            {
                logger.Error("Could not copy log file", innerException);
            }
        }

        private static void MigrateData(bool clean)
        {
            var lyrasongs = Util.BASEURL + "\\" + Util.URL;
            var lyrastyles = Util.BASEURL + "\\store\\lyrastyles.xml";
            var lists = Util.BASEURL + "\\" + Util.LISTURL;
            var pictures = Util.PICTDIR;

            if (clean)
            {
                var xml = File.ReadAllText(lyrasongs);
                xml = xml.Replace("&#xC;", string.Empty);
                File.WriteAllText(lyrasongs, xml);
            }

            if (!File.Exists(lyrasongs))
            {
                // migrate older songs (schema 1.7-3 from curtext.xml)
                var curtextPath = Util.BASEURL + "\\data\\curtext.xml";
                if (File.Exists(curtextPath))
                {
                    var curtextDoc = new XmlDocument();
                    curtextDoc.Load(curtextPath);
                    var lyraNode = curtextDoc.SelectSingleNode("//lyra");
                    var stylesRefAttr = curtextDoc.CreateAttribute("stylesref");
                    stylesRefAttr.InnerText = "lyrastyles.xml";
                    lyraNode.Attributes.Append(stylesRefAttr);

                    using (var xtw = new XmlTextWriter(Util.BASEURL + "\\" + Util.URL, Encoding.UTF8))
                    {
                        xtw.Formatting = Formatting.Indented;
                        curtextDoc.WriteContentTo(xtw);
                    }
                    logger.DebugFormat("Migrated from {0} to {1}!", curtextPath, Util.BASEURL + "\\" + Util.URL);
                }
                else
                {
                    var storeRoot = Path.GetDirectoryName(Util.BASEURL + "\\" + Util.URL);
                    if (!Directory.Exists(storeRoot))
                    {
                        Directory.CreateDirectory(storeRoot);
                    }

                    if (File.Exists(lyrasongs.Replace("\\store", "\\data")))
                    {
                        // move to store
                        File.Move(lyrasongs.Replace("\\store", "\\data"), lyrasongs);
                        File.Move(lyrastyles.Replace("\\store", "\\data"), lyrastyles);
                        File.Move(lists.Replace("\\store", "\\data"), lists);
                        Directory.CreateDirectory(pictures);
                        foreach (var pic in Directory.GetFiles(pictures.Replace("\\store", "\\data")))
                        {
                            File.Copy(pic, pic.Replace("\\data", "\\store"));
                        }

                        Directory.Delete(pictures.Replace("\\store", "\\data"), true);
                    }
                    else
                    {
                        // copy test data
                        var testDataRoot = Util.BASEURL + "\\" + Util.TESTDATA;
                        foreach (var file in Directory.GetFiles(testDataRoot, "*.*", SearchOption.AllDirectories))
                        {
                            var target = $"{storeRoot}" + file.Replace(testDataRoot, string.Empty);
                            if (!Directory.Exists(Path.GetDirectoryName(target)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(target));
                            }

                            File.Copy(file, $"{storeRoot}" + file.Replace(testDataRoot, string.Empty));
                        }

                        logger.DebugFormat($"Copied test data from {testDataRoot} to {storeRoot}!");
                    }

                    var gitstore = new GitStore(Path.GetDirectoryName(lyrasongs));
                    gitstore.CommitFile(lyrasongs, File.ReadAllText(lyrasongs));
                    gitstore.CommitFile(lyrastyles, File.ReadAllText(lyrastyles));
                    gitstore.CommitFile(lists, File.ReadAllText(lists));
                }
            }
        }
    }
}