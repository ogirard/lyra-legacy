using Lyra2.UtilShared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Manages physical storage. Contains methods to store current
    /// workspace and commit current data. And on the other side methods
    /// to load data from a XML-file.
    /// </summary>
    public class PhysicalXml : IPhysicalStorage
    {
        private readonly string xmlurl;
        private readonly XmlDocument doc;
        private static int highestID = 7000;
        private static int highestTrID;
        private string currentStylePath;

        public static string HighestID
        {
            get
            {
                highestID++;
                return "s" + Util.toFour(highestID);
            }
        }

        public static string HighestTrID
        {
            get
            {
                highestTrID++;
                return "t" + Util.toFour(highestTrID);
            }
        }

        public PhysicalXml(string url)
        {
            gitStore = new GitStore(Path.GetDirectoryName(url));
            xmlurl = url;
            doc = new XmlDocument();
        }

        public SortedList GetSongs()
        {
            return GetSongs(new SortedList(), xmlurl, false);
        }

        private IList<Style> styles;
        private GitStore gitStore;

        public IList<Style> Styles
        {
            get { return styles; }
        }

        public void SaveStyle(Style style)
        {
            if (style.IsNew)
            {
                styles.Add(style);
            }
            SaveStyleDoc();
        }

        public void DeleteStyle(Style style)
        {
            styles.Remove(style);
            SaveStyleDoc();
        }

        public void SetStyleAsDefault(Style style)
        {
            if (style.IsDefault)
            {
                return;
            }

            foreach (var s in Styles)
            {
                if (s.IsDefault)
                {
                    s.IsDefault = false;
                    SaveStyle(s);
                    break;
                }
            }

            style.IsDefault = true;
            SaveStyle(style);
        }

        private void SaveStyleDoc()
        {
            #region    Precondition

            if (string.IsNullOrEmpty(currentStylePath) || !File.Exists(currentStylePath)) return;

            #endregion Precondition

            var stylesDoc = new XmlDocument();
            stylesDoc.Load(currentStylePath);
            var stylesNode = stylesDoc.SelectSingleNode("//Styles");
            if (stylesNode != null)
            {
                stylesNode.RemoveAll();
            }
            else
            {
                stylesNode = stylesDoc.DocumentElement.AppendChild(stylesDoc.CreateElement("Styles"));
            }

            foreach (var style in Styles)
            {
                stylesNode.AppendChild(style.Serialize(stylesDoc));
            }

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                stylesDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                var xml = stringWriter.GetStringBuilder().ToString();
                gitStore.CommitFile(currentStylePath, xml);
            }
        }

        /// <summary>
        /// add songs to SortedList (Song as value,song-number as key)
        /// </summary>
        /// <returns>SortedList containing all songs, null on error</returns>
        public SortedList GetSongs(SortedList songs, string url, bool imp)
        {
            var start = Util.getCurrentTicks();
            try
            {
                doc.Load(url);
                InitStyles(url);
                var nodes = doc.GetElementsByTagName("Song");
                for (var i = 0; i < nodes.Count; i++)
                {
                    var songNode = nodes[i];

                    int nr;
                    string title;
                    string text;

                    var id = songNode.Attributes["id"].Value;
                    var desc = songNode.Attributes["zus"].Value;
                    var translations = songNode.Attributes["trans"].Value;

                    if (songNode["Number"] != null &&
                        songNode["Title"] != null &&
                        songNode["Text"] != null)
                    {
                        nr = Int32.Parse(songNode["Number"].InnerText);
                        highestID = nr > highestID ? nr : highestID;
                        title = songNode["Title"].InnerText;
                        text = songNode["Text"].InnerText;
                    }
                    else
                    {
                        throw new NotValidException();
                    }

                    var newSong = new Song(nr, title, text, id, desc, imp);

                    if (songNode.Attributes["style"] != null && !string.IsNullOrEmpty(songNode.Attributes["style"].InnerText))
                    {
                        var styleId = Guid.Parse(songNode.Attributes["style"].InnerText);
                        newSong.Style = Styles.FirstOrDefault(s => s.ID == styleId);
                        newSong.UseDefaultStyle = false;
                    }
                    else
                    {
                        // use default style for this song
                        newSong.Style = Styles.FirstOrDefault(s => s.IsDefault);
                        newSong.UseDefaultStyle = true;
                    }


                    GetTranslations(newSong, translations, imp);

                    try
                    {
                        songs.Add(newSong.ID, newSong);
                    }
                    catch (ArgumentException)
                    {
                        var msg = "Wollen Sie Lied Nr." + nr + " ersetzen?\n";
                        msg += "(drücken Sie \"abbrechen\", wenn Sie das Lied überspringen wollen.)";
                        var dr = MessageBox.Show(msg, "lyra import",
                                                          MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            songs.Remove(newSong.ID);
                            songs.Add(newSong.ID, newSong);
                        }
                        else if (dr == DialogResult.No)
                        {
                            newSong.nextID();
                            songs.Add(newSong.ID, newSong);
                        }
                        else
                        {
                            // cancel <-> ignore song
                        }
                        continue;
                    }
                }
                Util.addLoadTime(Util.getCurrentTicks() - start);
                // return SortedList with songs
                return songs;
            }
            catch (XmlException xmlEx)
            {
                Util.MBoxError(xmlEx.Message, xmlEx);
            }
            catch (NotValidException nValEx)
            {
                Util.MBoxError(nValEx.Message, nValEx);
            }
            catch (Exception e)
            {
                Util.MBoxError(e.Message, e);
            }
            return null;
        }

        private void InitStyles(string url)
        {
            styles = new List<Style>();
            var stylesDoc = new XmlDocument();

            var stylesRef = doc["lyra"].Attributes["stylesref"] != null ? doc["lyra"].Attributes["stylesref"].InnerText : "lyrastyles.xml";
            if (string.IsNullOrEmpty(stylesRef))
            {
                stylesRef = "lyrastyles.xml";
            }

            currentStylePath = Path.GetDirectoryName(url) + "\\" + stylesRef;
            if (!File.Exists(currentStylePath))
            {
                File.WriteAllText(currentStylePath,
                                  "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<lyra>" + Environment.NewLine + "  <Styles />" +
                                  Environment.NewLine + "</lyra>" + Environment.NewLine);
            }
            stylesDoc.Load(currentStylePath);
            foreach (XmlNode styleNode in stylesDoc.SelectNodes("//Styles/Style"))
            {
                styles.Add(new Style(styleNode, this));
            }

            if (styles.Count == 0)
            {
                var defaultStyle = Style.CreateNewStyle(this, "Standard");
                defaultStyle.BackgroundColor = Util.BGCOLOR;
                defaultStyle.ForegroundColor = Util.COLOR;
                defaultStyle.FontSize = (int)Util.FONT.SizeInPoints;
                defaultStyle.Font = Util.FONT.Name;
                defaultStyle.IsDefault = true;
                defaultStyle.Save();
            }
            else if (!styles.Any(s => s.IsDefault))
            {
                styles.First().IsDefault = true;
                styles.First().Save();
            }

        }

        private void GetTranslations(ISong song, string translations, bool imp)
        // throws XmlException, NotValidException
        {
            if (translations == "")
                return;
            var trs = translations.Split(',');
            XmlNode curt;
            for (var i = 0; i < trs.Length; i++)
            {
                if ((curt = doc.GetElementById(trs[i])) != null)
                {
                    var transchildren = curt.ChildNodes;

                    var lang = Util.getLanguageInt(curt.Attributes["lang"].Value);
                    var id = curt.Attributes["id"].Value;
                    var uf = curt.Attributes["unform"].Value.Equals("yes");
                    string text;
                    string title;
                    if ((transchildren[0].Name == "Title") &&
                        (transchildren[1].Name == "Text"))
                    {
                        title = transchildren[0].InnerText;
                        text = transchildren[1].InnerText;
                    }
                    else
                    {
                        throw new NotValidException();
                    }
                    var newid = HighestTrID;
                    song.AddTranslation(
                        new Translation(title, text, lang, uf, "t" + Util.toFour(highestTrID), imp || (newid != id)));
                }
                else
                {
                    Util.MBoxError("Die Übersetzung \"" + trs[i] + "\" konnte nicht gefunden werden!");
                }
            }
        }


        /// <summary>
        /// Commit current workspace
        /// </summary>
        /// <param name="internList">SortedList of current songs. From Storage.</param>
        /// <returns>successful?</returns>
        public bool Commit(SortedList internList)
        {
            try
            {
                XmlNode curNode, newNode, nr, title, text;
                XmlAttribute transattr, idattr, descattr;

                var songs = doc.GetElementsByTagName("Songs")[0];

                // remove all, if songs have been replaced!
                if (Util.DELALL)
                {
                    var translations = doc.GetElementsByTagName("Translations")[0];
                    songs.RemoveAll();
                    translations.RemoveAll();
                }

                Song curSong;

                // Delete
                var j = 0;
                var top = internList.Count;
                for (var i = 0; i < top; i++)
                {
                    curSong = (Song)internList.GetByIndex(j);
                    if (curSong.Deleted)
                    {
                        j--;
                        internList.Remove(curSong.ID);
                        if ((curNode = doc.GetElementById(curSong.ID)) != null)
                        {
                            songs.RemoveChild(curNode);
                            curSong.UpdateTranslations(this);
                        }
                    }
                    j++;
                }

                var en = internList.GetEnumerator();
                en.Reset();


                // iterate through current SongList and update/store each song!
                while (en.MoveNext())
                {
                    curSong = (Song)en.Value;
                    if (curSong.ToUpdate)
                    {
                        // id exists
                        if ((curNode = doc.GetElementById(curSong.ID)) != null)
                        {
                            // update node
                            var children = curNode.ChildNodes;
                            if ((children[0].Name == "Number") &&
                                (children[1].Name == "Title") &&
                                (children[2].Name == "Text"))
                            {
                                children[0].InnerText = curSong.Number.ToString();
                                children[1].InnerText = curSong.Title;
                                children[2].InnerText = curSong.Text;
                            }
                            else
                            {
                                throw new NotValidException();
                            }

                            if (curNode.Attributes["style"] == null)
                            {
                                var styleAttr = doc.CreateAttribute("style");
                                curNode.Attributes.Append(styleAttr);
                            }
                            curNode.Attributes["style"].InnerText = curSong.UseDefaultStyle ? "" : curSong.Style.ID.ToString().Trim('{', '}');
                        }
                        // id doesn't exist
                        else
                        {
                            // create node
                            newNode = doc.CreateNode(XmlNodeType.Element, "Song", doc.NamespaceURI);

                            nr = doc.CreateNode(XmlNodeType.Element, "Number", doc.NamespaceURI);
                            title = doc.CreateNode(XmlNodeType.Element, "Title", doc.NamespaceURI);
                            text = doc.CreateNode(XmlNodeType.Element, "Text", doc.NamespaceURI);

                            nr.InnerText = curSong.Number.ToString();
                            title.InnerText = curSong.Title;
                            text.InnerText = curSong.Text;

                            transattr = doc.CreateAttribute("trans", doc.NamespaceURI);
                            transattr.Value = "";

                            idattr = doc.CreateAttribute("id", doc.NamespaceURI);
                            idattr.Value = "";
                            descattr = doc.CreateAttribute("zus", doc.NamespaceURI);
                            descattr.Value = "";

                            newNode.Attributes.Append(doc.CreateAttribute("id"));
                            newNode.Attributes["id"].InnerText = curSong.Style.ID.ToString().Trim('{', '}');

                            newNode.AppendChild(nr);
                            newNode.AppendChild(title);
                            newNode.AppendChild(text);

                            newNode.Attributes.Append(idattr);
                            newNode.Attributes.Append(transattr);
                            newNode.Attributes.Append(descattr);
                            curNode = songs.AppendChild(newNode);
                        }

                        // translations
                        var transList = curSong.UpdateTranslations(this);
                        curNode.Attributes["trans"].Value = transList;
                        curNode.Attributes["id"].Value = curSong.ID;
                        curNode.Attributes["zus"].Value = curSong.Desc;
                    }
                }

                var dataFile = Util.BASEURL + "\\" + Util.URL;
                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    var xml = stringWriter.GetStringBuilder().ToString();
                    gitStore.CommitFile(dataFile, xml);
                }


                return true;
            }
            // on any error: return false => not commited!
            catch (Exception e)
            {
                Util.MBoxError(e.Message, e);
                return false;
            }
        }

        public string CommitTranslations(SortedList trans)
        // throws NotValidException, XmlException
        {
            var list = "";
            var translations = doc.GetElementsByTagName("Translations")[0];

            Translation curTrans;
            XmlNode transNode;
            XmlNode newTrans, title, text;
            XmlAttribute lang, id, uf;

            // Delete
            var j = 0;
            var top = trans.Count;
            for (var i = 0; i < top; i++)
            {
                curTrans = (Translation)trans.GetByIndex(j);
                if (curTrans.Deleted)
                {
                    trans.Remove(curTrans.ID);
                    j--;
                    if ((transNode = doc.GetElementById(curTrans.ID)) != null)
                    {
                        translations.RemoveChild(transNode);
                    }
                }
                j++;
            }

            var en = trans.GetEnumerator();
            en.Reset();
            while (en.MoveNext())
            {
                curTrans = (Translation)en.Value;
                list += curTrans.ID + ",";
                if (curTrans.ToUpdate)
                {
                    if ((transNode = doc.GetElementById(curTrans.ID)) != null)
                    {
                        var transChildren = transNode.ChildNodes;
                        if ((transChildren[0].Name == "Title") &&
                            (transChildren[1].Name == "Text"))
                        {
                            transChildren[0].InnerText = curTrans.Title;
                            transChildren[1].InnerText = curTrans.Text;
                            transNode.Attributes["lang"].Value = Util.getLanguageString(curTrans.Language, true);
                            transNode.Attributes["id"].Value = curTrans.ID;
                            transNode.Attributes["unform"].Value = curTrans.Unformatted ? "yes" : "no";
                        }
                        else
                        {
                            throw new NotValidException();
                        }
                    }
                    // doesn't exist, create new Node
                    else
                    {
                        newTrans = doc.CreateNode(XmlNodeType.Element, "Translation", doc.NamespaceURI);

                        title = doc.CreateNode(XmlNodeType.Element, "Title", doc.NamespaceURI);
                        text = doc.CreateNode(XmlNodeType.Element, "Text", doc.NamespaceURI);
                        lang = doc.CreateAttribute("lang", doc.NamespaceURI);
                        id = doc.CreateAttribute("id", doc.NamespaceURI);
                        uf = doc.CreateAttribute("unform", doc.NamespaceURI);

                        lang.Value = Util.getLanguageString(curTrans.Language, true);
                        id.Value = curTrans.ID;
                        uf.Value = curTrans.Unformatted ? "yes" : "no";
                        title.InnerText = curTrans.Title;
                        text.InnerText = curTrans.Text;

                        newTrans.Attributes.Append(lang);
                        newTrans.Attributes.Append(id);
                        newTrans.Attributes.Append(uf);
                        newTrans.AppendChild(title);
                        newTrans.AppendChild(text);

                        translations.AppendChild(newTrans);
                    }
                }
            }
            var len = list.Length - 1 < 0 ? 0 : list.Length - 1;
            return list.Substring(0, len);
        }
    }

    public class NotValidException : Exception
    {
        public NotValidException()
            : base("XML-Datei ungültig")
        {
        }

        public NotValidException(string msg)
            : base("XML-Datei ungültig!\n" + msg)
        {
        }
    }
}