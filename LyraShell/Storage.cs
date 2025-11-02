using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Stores and manages the lyrics of the current workspace.
    /// Wrapper-Class for PhysicalXML. Access for commit/load only
    /// through this class! GUI shouldn't have direct access to these methods.
    /// </summary>
    public class Storage : IStorage
    {
        #region    Logging

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(Storage));

        #endregion Logging

        private SortedList _songList;
        private readonly IPhysicalStorage _physicalStorage;
        private readonly GUI _owner;
        private ISearch _search;

        private bool _toBeCommited;

        public bool ToBeCommited
        {
            get { return _toBeCommited; }
            set { _toBeCommited = value; }
        }

        public Storage(string url, GUI owner)
        {
            var lyrasongsFile = Util.BASEURL + "\\" + url;
            _physicalStorage = new PhysicalXml(lyrasongsFile);
            _songList = _physicalStorage.GetSongs();
            _owner = owner;
            Util.NRSONGS = _songList.Count;
            InitializeSearch();
        }

        private void InitializeSearch()
        {
            if (_search != null)
            {
                _search.Dispose();
            }

            _search = new FastSearch();
            IList<Song> songs = _songList.Values.OfType<Song>().ToList();
            _search.AddValues(songs);
        }

        public bool Commit()
        {
            if (!Util.NOCOMMIT)
            {
                if (_physicalStorage.Commit(_songList))
                {
                    _toBeCommited = false;
                    Util.NRSONGS = _songList.Count;
                    InitializeSearch();
                    return true;
                }
            }
            else
            {
                Logger.Info("Commit access not granted");
                Util.MBoxError("Sie haben keine Berechtigung, Änderungen vorzunehmen!\n" +
                               "Entfernen Sie unter Extras->Optionen..., Allgemein... den Schreibschutz\n" +
                               "oder wenden Sie sich an den Administrator.");
            }
            return false;
        }

        public bool CleanSearchIndex()
        {
            Cursor.Current = Cursors.WaitCursor;
            _owner.Enabled = false;
            try
            {
                InitializeSearch();
            }
            catch (Exception ex)
            {
                Logger.Error("Index could not be re-created!", ex);
                _owner.Enabled = true;
                Cursor.Current = Cursors.Default;
                return false;
            }

            _owner.Enabled = true;
            Cursor.Current = Cursors.Default;
            return true;
        }

        public IPhysicalStorage PhysicalStorage
        {
            get { return _physicalStorage; }
        }

        public bool IsStyleInUse(Style style)
        {
            return _songList.Values.Cast<Song>().Any(song => song.Style == style);
        }

        public void SetStyleAsDefault(Style style)
        {
            if (style.IsDefault) return;

            // set the given style as new default style
            PhysicalStorage.SetStyleAsDefault(style);

            // update all songs with default style
            foreach (var song in _songList.Values.Cast<Song>().Where(song => song.UseDefaultStyle))
            {
                song.Style = style;
            }
        }

        // get Song by ID
        public ISong getSongById(object id)
        {
            return (Song)_songList[id];
        }

        // get Song by Number
        public ISong GetSong(int nr)
        {
            var id = "s" + Util.toFour(nr);
            Song ret;
            if (_songList.ContainsKey(id))
            {
                ret = (Song)_songList[id];
                if (ret.Deleted)
                {
                    ret = null;
                }

                return ret;
            }

            if (_songList.ContainsKey("s7001"))
            {
                var i = _songList.IndexOfKey("7001");
                for (; i < _songList.Count; i++)
                {
                    if (((Song)_songList.GetByIndex(i)).Number == nr)
                    {
                        ret = (Song)_songList.GetByIndex(i);
                        if (ret.Deleted)
                        {
                            ret = null;
                        }

                        return ret;
                    }
                }
            }
            return null;
        }

        public IList<Style> Styles
        {
            get { return _physicalStorage.Styles; }
        }

        // reset to XML-File status! pay attention!
        public void ResetToLast()
        {
            _songList = _physicalStorage.GetSongs();
            Util.NRSONGS = _songList.Count;
            _owner.UpdateListBox();
            _owner.ToUpdate(false);
        }

        // import
        public bool Import(string url, bool append)
        {
            try
            {
                if (!append)
                {
                    _songList = _physicalStorage.GetSongs(new SortedList(), url, true);
                }
                else
                {
                    _songList = _physicalStorage.GetSongs(_songList, url, true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // export
        public bool Export(string url)
        {
            try
            {
                File.Copy(Util.BASEURL + "\\" + Util.URL, url, true);
                return true;
            }
            catch (Exception ex)
            {
                Util.MBoxError("Fehler beim Exportieren\n" + ex.Message, ex);
            }
            return false;
        }

        public void DisplaySongs(ListBox box)
        {
            box.BeginUpdate();
            box.Items.Clear();
            foreach (var song in _songList.Values.Cast<Song>().Where(song => !song.Deleted))
            {
                box.Items.Add(song);
            }

            Util.NRSONGS = box.Items.Count;
            box.EndUpdate();
        }

        public void Clear()
        {
            _songList.Clear();
        }

        public void RemoveSong(string id)
        {
            _songList.Remove(id);
        }

        public void AddSong(ISong song)
        {
            _songList.Add(song.ID, song);
        }


        // Search
        public void Search(string query, SongListBox resultBox, bool text, bool matchCase, bool whole, bool trans, SortMethod sortMethod)
        {
            if (!_search.SearchCollection(query, _songList, resultBox, text, matchCase, whole, trans, sortMethod))
            {
                resultBox.Items.Add("Leider keinen passenden Eintrag gefunden.");
                _owner.Status = "query done - no results :-(";
            }
            else
            {
                _owner.Status = "query done - successful :-)";
            }
        }

        // Export for Pocket PC
        private static string FormatText(string text)
        {
            var ret = "";
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    while (i < text.Length - 1 && text[i++] != '>') ;
                    i--;
                }
                else
                {
                    if (text[i] == '\n' || text[i] == '\t' || text[i] == '\r' || text[i] == '>' || text[i] == '%')
                    {
                        ret += " ";
                    }
                    else
                    {
                        ret += text[i];
                    }
                }
            }
            if (ret.Length > 100) ret = ret.Substring(0, 100);
            return ret + "...";
        }

        public bool ExportPPC(string url)
        {
            try
            {
                var sw = new StreamWriter(url, false);
                sw.AutoFlush = true;
                sw.WriteLine("[ lyra for Pocket PC ]");
                sw.WriteLine();
                sw.WriteLine("####$SNAPSHOT");
                sw.WriteLine(DateTime.Now.ToShortDateString() + "@" + DateTime.Now.ToShortTimeString());
                sw.WriteLine("####$COUNT");
                sw.WriteLine(_songList.Count.ToString(CultureInfo.InvariantCulture));
                sw.WriteLine("####$DATA");

                var en = _songList.GetEnumerator();
                en.Reset();
                while (en.MoveNext())
                {
                    var song = (Song)en.Value;
                    var title = song.Title.Length > 0 ? song.Title : "  ";
                    var txt = FormatText(song.Text);
                    sw.WriteLine(song.Number.ToString(CultureInfo.InvariantCulture) + " " + title + "%" + txt);
                }
                sw.Close();
                var text = "Liste für Pocket PC wurde erfolgreich erstellt!" + Util.NL + "Sie finden Sie in " + url;

                MessageBox.Show(_owner, text, "lyra", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("PPC export not successful", ex);
            }
            return false;
        }
    }
}