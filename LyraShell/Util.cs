using Lyra2.UtilShared;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace Lyra2.LyraShell
{
    /// <summary>
    ///   static help methods, vars and constants.
    /// </summary>
    public class Util
    {
        #region    Log4Net Logger

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(Util));

        #endregion Log4Net Logger

        public static string CONFIGPATH = Application.StartupPath + "\\lyra.config";

        // info & build 
        public const string NAME = "Lyra";
        private const string Version = "LocalBuild";
        public static readonly string VER = $"{Version}";
        private static readonly string BuildDate = DateTime.Now.ToString("yyyyMMdd");
        public static readonly string BUILD = $"{VER}.{BuildDate}";
        public static string GUINAME = NAME;

        // lyra update
        public const string UPDATESERVER = "http://cgi.ethz.ch/~ogirard/lyraupdate";

        // status / paths
        public const int MAXOPEN = 5;
        public const int WAIT = 1000; //ms
        public static bool DELALL;
        public static string URL = "store\\lyrasongs.xml";
        public static string HLPURL = "doc\\lyrahelp.chm";
        public static string NEWSURL = "doc\\buildnews.txt";
        public static string INFORTF = "doc\\info.rtf";
        public static string LISTURL = "store\\lists.xml";
        public static string IMAGECACHE = "store\\cache";
        public static string BASEURL = Application.StartupPath;
        public static string TESTDATA = "data\\testdata";
        public static bool PREVIEW_BOTTOM_MODE = true;

        // format
        public const string REF = "refrain";
        public const string SPEC = "special";
        public const string BOLD = "b";
        public const string ITALIC = "i";
        public const string BLOCK = "p";
        public const string PGBR = "pagebreak";
        public const string JMP = "jumpmark";
        public static Color REFCOLOR;
        public static Color COLOR = Color.Black;
        public static Color BGCOLOR = Color.White;
        public static Font SPECFONT;
        public static Font TRANSFONT;
        public static Font FONT;
        public static bool refmode = true;

        public static Color TITLECOLOR = Color.SteelBlue;
        public static Color TITLEBGCOLOR = Color.White;
        public static Font TITLEFONT;


        // pictures
        public static string PICTDIR = Application.StartupPath + "\\store\\pictures\\";
        public static bool COPYPICS = true;
        public const string PICTSSYM = "{pics}\\";
        public static bool KEEPRATIO;

        // pres
        public static Color UNICOLOR;
        public static Color GRADCOL1;
        public static Color GRADCOL2;
        public static int PREMODE;
        public static bool CASCADEPIC;
        private static string picturi;
        public static Image BGIMG;

        public static string PICTURI
        {
            set
            {
                if (!File.Exists(value)) return;

                try
                {
                    BGIMG = Image.FromFile(value);
                    picturi = value;
                }
                catch (Exception)
                {
                    picturi = "";
                    BGIMG = null;
                }
            }
            get { return picturi; }
        }

        // FX
        public static string[] FX = new string[6];


        // OPTIONS
        // show german title with translation?
        public static bool SHOWGER = true;
        public static bool SHOWRIGHT = true;
        public static bool NOCOMMIT;
        public static bool SHOWBUILDNEWS;
        public static bool SHOWNR = true;
        public static bool CTRLSHOWNR;
        public static int TIMER = 3000;
        public static int SCREEN_ID;
        public static bool SHOW_PREVIEW;

        // help methods
        public const string NL = "\r\n";
        public const string RTNL = "{\\par}";
        public const string HTMLNL = "<br />\n\t";

        public static void MBoxError()
        {
            MBoxError("", null);
        }

        public static void MBoxError(string umsg)
        {
            MBoxError(umsg, null);
        }

        public static void MBoxError(string umsg, Exception ex)
        {
            var msg = "Es ist ein Fehler aufgetreten!" + NL + umsg + NL;
            Debug(umsg, ex);
            MessageBox.Show(msg, "lyra error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (ex != null)
            {
                Logger.Debug("Lyra Fehler! " + umsg, ex);
            }
            else
            {
                Logger.Debug("Lyra Fehler! " + umsg);
            }
        }

        public static string MD5FileHash(FileInfo file)
        {
            if (!file.Exists) return "";
            FileStream stream = null;
            try
            {
                stream = new FileStream(file.FullName, FileMode.Open);
                var md5 = MD5.Create();

                var hash = md5.ComputeHash(stream);
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (var i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
                stream.Close();
                return sBuilder.ToString().ToUpper();
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static string GetDate()
        {
            var date = "";
            date += DateTime.Now.Year + "/";
            var month = (DateTime.Now.Month < 10) ? ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString();
            var day = (DateTime.Now.Day < 10) ? ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString();
            date += month + "/" + day;
            return date;
        }

        public static string toFour(int nr)
        {
            if (nr == 0) return "0000";

            var zeros = "";
            var nrstr = nr.ToString();
            while ((nr *= 10) < 10000)
            {
                zeros += "0";
            }
            return zeros + nrstr;
        }

        public static string toVertical(string hor)
        {
            var vert = "";
            for (var i = 0; i < hor.Length - 1; i++)
            {
                vert += hor[i] + NL;
            }
            return vert + hor[hor.Length - 1];
        }

        public static string handlePicture(string fn)
        {
            fn = fn.ToLower();
            if (COPYPICS && !fn.StartsWith(PICTSSYM) && isPict(fn))
            {
                var converted = fn.Replace('\\', '_').Replace(":", "");
                if (!File.Exists(PICTDIR + converted))
                {
                    File.Copy(fn, PICTDIR + converted);
                }
                return PICTSSYM + converted;
            }
            if (isPict(fn))
            {
                return fn;
            }
            // auto correct invalid file names
            return "";
        }

        public static string CleanText(string text)
        {
            var cleanedString = "";
            text = text.Replace("\r", string.Empty);
            var skip = false;
            foreach (var c in text)
            {
                if (c == '<')
                {
                    skip = true;
                }
                else if (c == '>')
                {
                    skip = false;
                }
                else
                {
                    if (!skip)
                    {
                        cleanedString += c;
                        // if (lines >= 10)
                        // {
                        //     cleanedString += Util.NL + "[...]";
                        //     cleanedString = cleanedString.Replace("&lt;", "<").Replace("&gt;", ">");
                        //     return cleanedString;
                        // }
                    }
                }
            }
            cleanedString = cleanedString.Replace("&lt;", "<").Replace("&gt;", ">");
            return cleanedString;
        }

        private static bool isPict(string pictPath)
        {
            return (pictPath.EndsWith(".gif") || pictPath.EndsWith(".jpg") || pictPath.EndsWith(".bmp") ||
                    pictPath.EndsWith(".bmp")) &&
                   (File.Exists(pictPath) || pictPath.StartsWith(PICTSSYM));
        }

        // FX
        public static void OpenFile(int fnr)
        {
            if (!FX[fnr].Equals("-"))
            {
                if (FX[fnr].StartsWith("pict://"))
                {
                    PictView.ShowPictView(FX[fnr].Substring(7));
                }
                else
                {
                    FileLauncher.openFile(FX[fnr]);
                }
            }
        }

        // stretch an image
        public static Bitmap stretchProportional(Image img, Size bounds)
        {
            Bitmap ret;
            if (img.Width > bounds.Width || img.Height > bounds.Height)
            {
                if (img.Width / bounds.Width > img.Height / bounds.Height)
                {
                    var h = img.Height * bounds.Width / img.Width;
                    ret = new Bitmap(img, new Size(bounds.Width, h));
                }
                else
                {
                    var w = img.Width * bounds.Height / img.Height;
                    ret = new Bitmap(img, new Size(w, bounds.Height));
                }
            }
            else
            {
                ret = new Bitmap(img);
            }
            return ret;
        }

        // stretch an image
        public static Bitmap handlePic(bool scale, Image img, Size bounds, bool keepRatio, int transparency)
        {
            Bitmap ret;
            if (scale)
            {
                if (!keepRatio)
                {
                    ret = new Bitmap(img, bounds);
                }
                else
                {
                    if (img.Width / bounds.Width > img.Height / bounds.Height)
                    {
                        var h = img.Height * bounds.Width / img.Width;
                        ret = new Bitmap(img, new Size(bounds.Width, h));
                    }
                    else
                    {
                        var w = img.Width * bounds.Height / img.Height;
                        ret = new Bitmap(img, new Size(w, bounds.Height));
                    }
                }
            }
            else
            {
                var centered = new Bitmap(bounds.Width, bounds.Height);
                var g = Graphics.FromImage(centered);
                g.FillRectangle(Brushes.White, 0, 0, bounds.Width, bounds.Height);
                g.DrawImage(img, (bounds.Width - img.Width) / 2, (bounds.Height - img.Height) / 2, img.Width, img.Height);
                ret = centered;
            }

            if (transparency > 0)
            {
                ret = GenerateMagicImage(ret, transparency);
            }

            return ret;
        }

        /**
         * LANGUAGES
         */
        public static int LangNR = 8;

        public enum Lang
        {
            EN,
            FR,
            IT,
            ES,
            DT,
            LT,
            HB,
            OT
        }

        public static Bitmap GenerateMagicImage(Bitmap bFront, int transparency)
        {
            var opacity = (100 - transparency) / 100f;
            var bHidden = new Bitmap(bFront.Width, bFront.Height);
            // the following code draws the front image over the hidden image using the alpha blending effect
            var g = Graphics.FromImage(bHidden);
            g.FillRectangle(Brushes.White, 0, 0, bFront.Width, bFront.Height);
            float[][] ptsArray = {
                             new float[] {1, 0, 0, 0, 0},
                             new float[] {0, 1, 0, 0, 0},
                             new float[] {0, 0, 1, 0, 0},
                             new[] {0, 0, 0, opacity, 0},
                             new float[] {0, 0, 0, 0, 1}
                           };
            var clrMatrix = new ColorMatrix(ptsArray);
            var imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            g.DrawImage(bFront, new Rectangle(0, 0, bFront.Width, bFront.Height), 0, 0, bFront.Width, bFront.Height,
                        GraphicsUnit.Pixel, imgAttributes);

            // return the result image
            // Bitmap bResult = new Bitmap( bHidden.Width, bHidden.Height );
            // bResult = (Bitmap)bHidden.Clone();
            return bHidden;
        }

        public static string getLanguageString(int l, bool abrev)
        {
            if (abrev)
            {
                switch (l)
                {
                    case (int)Lang.EN:
                        return "en";
                    case (int)Lang.FR:
                        return "fr";
                    case (int)Lang.IT:
                        return "it";
                    case (int)Lang.ES:
                        return "es";
                    case (int)Lang.DT:
                        return "dt";
                    case (int)Lang.LT:
                        return "lt";
                    case (int)Lang.HB:
                        return "hb";
                    default:
                        return "ot";
                }
            }
            switch (l)
            {
                case (int)Lang.EN:
                    return "english";
                case (int)Lang.FR:
                    return "fran�ais";
                case (int)Lang.IT:
                    return "italiano";
                case (int)Lang.ES:
                    return "espa�ol";
                case (int)Lang.DT:
                    return "deutsch";
                case (int)Lang.LT:
                    return "lateinisch";
                case (int)Lang.HB:
                    return "hebr�isch";
                default:
                    return "unknown";
            }
        }

        public static int getLanguageInt(string lang)
        {
            if (lang == "en") return (int)Lang.EN;
            if (lang == "fr") return (int)Lang.FR;
            if (lang == "it") return (int)Lang.IT;
            if (lang == "es") return (int)Lang.ES;
            if (lang == "lt") return (int)Lang.LT;
            if (lang == "hb") return (int)Lang.HB;
            return (int)Lang.OT;
        }

        private static Font GetFont(string key)
        {
            var str = key.Split(';');
            var nr = Int32.Parse(str[1]);
            var style = FontStyle.Regular;
            if (str[2] == "b")
                style = FontStyle.Bold;
            else if (str[2] == "i")
                style = FontStyle.Italic;


            return new Font(str[0], nr, style);
        }

        private static Color GetColor(string key)
        {
            var nrs = key.Split(';');
            return Color.FromArgb(Int32.Parse(nrs[0]),
                                  Int32.Parse(nrs[1]),
                                  Int32.Parse(nrs[2]));
        }

        public static string hexValue(int i)
        {
            var res = Convert.ToString(i, 16);
            if (res.Length == 1) res = "0" + res;
            return res;
        }

        private static string ColorToString(Color color)
        {
            return ((int)color.R) + ";" +
                   ((int)color.G) + ";" +
                   ((int)color.B);
        }

        private static string FontToString(Font font)
        {
            var res = "";
            res += font.Name + ";" + ((int)font.Size) + ";";

            if (font.Style == FontStyle.Regular)
                res += "s";
            else if (font.Style == FontStyle.Bold)
                res += "b";
            else if (font.Style == FontStyle.Italic)
                res += "i";

            return res;
        }

        public static void updateRegSettings()
        {
            try
            {
                var configFile = new ConfigFile(CONFIGPATH);

                configFile["1"] = SHOWBUILDNEWS ? "yes" : "no";
                configFile["ger"] = SHOWGER ? "yes" : "no";
                configFile["right"] = SHOWGER ? "yes" : "no";
                configFile["ger"] = SHOWRIGHT ? "yes" : "no";
                configFile["ac"] = NOCOMMIT ? "yes" : "no";
                configFile["shnr"] = SHOWNR ? "yes" : "no";
                configFile["timer"] = TIMER.ToString();
                configFile["screen_id"] = SCREEN_ID.ToString();
                configFile["show_preview"] = SHOW_PREVIEW ? "yes" : "no";

                configFile["fonts.standard"] = FontToString(FONT);
                configFile["fonts.special"] = FontToString(SPECFONT);
                configFile["fonts.transfont"] = FontToString(TRANSFONT);
                configFile["fonts.color"] = ColorToString(COLOR);
                configFile["fonts.refcolor"] = ColorToString(REFCOLOR);
                configFile["fonts.refmode"] = refmode ? "normal" : "fett";

                configFile["presentation.unicol"] = ColorToString(UNICOLOR);
                configFile["presentation.gradcol1"] = ColorToString(GRADCOL1);
                configFile["presentation.gradcol2"] = ColorToString(GRADCOL2);
                configFile["presentation.mode"] = PREMODE.ToString();
                configFile["presentation.picuri"] = picturi;
                configFile["presentation.cascade"] = CASCADEPIC ? "yes" : "no";

                for (var i = 0; i < FX.Length; i++)
                {
                    configFile["FX.f" + (i + 1)] = FX[i];
                }

                configFile.Save(CONFIGPATH);
            }
            catch (Exception e)
            {
                MBoxError(e.Message, e);
            }
        }

        static Util()
        {
            try
            {
                var configFile = new ConfigFile(CONFIGPATH);

                HLPURL = configFile["help"];
                //Util.URL = configFile["url"];
                //LISTURL = configFile["lists"];
                SHOWBUILDNEWS = configFile["1"].Equals("yes");
                SHOWRIGHT = configFile["right"].Equals("yes");
                NOCOMMIT = configFile["ac"].Equals("yes");
                SHOWGER = configFile["ger"].Equals("yes");
                SHOWNR = configFile["shnr"].Equals("yes");
                TIMER = Int32.Parse(configFile["timer"]);

                try
                {
                    SCREEN_ID = Int32.Parse(configFile["screen_id"]);
                    View.Display = GetScreen(SCREEN_ID);
                }
                catch (Exception)
                {
                    SCREEN_ID = 0;
                    View.Display = Screen.PrimaryScreen;
                    configFile.addProperty("screen_id", "0");
                    configFile.Save(CONFIGPATH);
                }


                if (configFile["show_preview"] == "n/a")
                {
                    SHOW_PREVIEW = false;
                    configFile.addProperty("show_preview", "no");
                    configFile.Save(CONFIGPATH);
                }
                else
                {
                    SHOW_PREVIEW = configFile["show_preview"] == "yes";
                }


                FONT = GetFont(configFile["fonts.standard"]);
                TITLEFONT = new Font(FONT.FontFamily, FONT.Size + 2);
                SPECFONT = GetFont(configFile["fonts.special"]);
                TRANSFONT = GetFont(configFile["fonts.transfont"]);
                REFCOLOR = GetColor(configFile["fonts.refcolor"]);
                COLOR = GetColor(configFile["fonts.color"]);
                refmode = configFile["fonts.refmode"] == "normal";
                UNICOLOR = GetColor(configFile["presentation.unicol"]);
                GRADCOL1 = GetColor(configFile["presentation.gradcol1"]);
                GRADCOL2 = GetColor(configFile["presentation.gradcol2"]);
                PREMODE = Int32.Parse(configFile["presentation.mode"]);
                PICTURI = configFile["presentation.picuri"];
                CASCADEPIC = configFile["presentation.cascade"].Equals("yes");

                for (var i = 0; i < FX.Length; i++)
                {
                    FX[i] = configFile["FX.f" + (i + 1)];
                }

                loadStats();
            }
            catch (Exception e)
            {
                MBoxError(e.Message, e);
            }
        }

        // ### STATS ###
        public static int NRSONGS;
        public static long TOTALLOAD;
        public static int NRUSE = 1;
        public static long TOTALSEARCH;
        public static int NRSEARCH = 1;
        private static long TOTALUSE;
        public static string DOTNET = new FrameworkName(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName).Version.ToString();

        public static long getCurrentTicks()
        {
            return DateTime.Now.Ticks;
        }

        public static void addSearchTime(long ticks)
        {
            TOTALSEARCH += ticks;
            NRSEARCH++;
        }

        public static void addLoadTime(long ticks)
        {
            TOTALLOAD += ticks;
            NRUSE++;
        }

        public static long startticks;
        public static string PREVIEW_SONG_ID = "sPreview";

        public static string getUseTime()
        {
            var ms = (TOTALUSE + DateTime.Now.Ticks - startticks) / TimeSpan.TicksPerMillisecond;
            var s = (long)Math.Floor((double)ms / 1000);
            // ms = ms - s * 1000;
            var min = (long)Math.Floor((double)s / 60);
            s = s - 60 * min;
            var h = (long)Math.Floor((double)min / 60);
            min = min - 60 * h;
            var d = (long)Math.Floor((double)h / 24);
            h = h - 24 * d;
            return Convert.ToString(d) + "d " + Convert.ToString(h) + "h " + Convert.ToString(min) + "min " +
                   Convert.ToString(s) + "s ";
        }

        public static void storeStats()
        {
            try
            {
                var configFile = new ConfigFile(CONFIGPATH);
                configFile["stats.TLD"] = TOTALLOAD.ToString();
                configFile["stats.NRUSE"] = NRUSE.ToString();
                configFile["stats.TSRC"] = TOTALSEARCH.ToString();
                configFile["stats.NRSRC"] = NRSEARCH.ToString();
                TOTALUSE += getCurrentTicks() - startticks;
                configFile["stats.TUSE"] = TOTALUSE.ToString();
                configFile.Save(CONFIGPATH);
            }
            catch (Exception)
            {
                MBoxError("error@Util.storeStats()");
            }
        }

        private static void loadStats()
        {
            try
            {
                var configFile = new ConfigFile(CONFIGPATH);
                TOTALLOAD = Convert.ToInt64(configFile["stats.TLD"]);
                NRUSE = Convert.ToInt32(configFile["stats.NRUSE"]);
                TOTALSEARCH = Convert.ToInt64(configFile["stats.TSRC"]);
                NRSEARCH = Convert.ToInt32(configFile["stats.NRSRC"]);
                TOTALUSE = Convert.ToInt64(configFile["stats.TUSE"]);
                startticks = DateTime.Now.Ticks;
            }
            catch (Exception)
            {
                MBoxError("error@Util.loadStats()");
            }
        }

        public static Screen GetScreen(int id)
        {
            if (id == 0)
            {
                return Screen.PrimaryScreen;
            }
            if (id == 1)
            {
                var secScr = Screen.AllScreens[0];
                if (secScr == Screen.PrimaryScreen && Screen.AllScreens.Length == 2)
                {
                    secScr = Screen.AllScreens[1];
                }
                return secScr;
            }
            return null;
        }

        private static readonly KeysConverter keysConverter = new KeysConverter();

        public static bool KeyMatches(Keys key, params Keys[] matches)
        {
            #region    Precondition

            if (matches.Length == 0) return false;

            #endregion Precondition

            var allmatches = matches[0];
            for (var i = 1; i < matches.Length; i++)
            {
                allmatches |= matches[i];
            }
            return keysConverter.Compare(key, allmatches) == 0;
        }

        // ### DEBUG ###
        public static void Debug(string msg)
        {
            Debug(msg, null);
        }

        public static void Debug(Exception dbex)
        {
            Debug("", dbex);
        }

        public static void Debug(string msg, Exception dbex)
        {
            if (GUI.DEBUG) // if -d flag set
            {
                DebugConsole.Append(msg, dbex);
            }
        }
    }
}
