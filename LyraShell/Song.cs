using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Lyra2.LyraShell.Search;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Datatype Song.
    /// Stored in a SortedList, accessible only through the Storage-Class.
    /// For further efficiency, I chose to directly load ALL data into the ram
    /// on initialising. So there's NO lazy binding.
    /// </summary>
    public class Song : ISong, IIndexObject
    {
        /// <summary>
        /// Data section
        /// </summary>
        // id
        private string id;

        public string ID
        {
            get { return id; }
        }

        // pay attention! changes id to next free ID over s700.
        public string nextID()
        {
            return id = PhysicalXml.HighestID;
        }


        // number
        private int nr;

        public int Number
        {
            get { return nr; }
        }

        // desc
        private string zus;

        public string Desc
        {
            get { return zus; }
            set
            {
                zus = value;
                toupdate = true;
            }
        }

        // title
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                toupdate = true;
            }
        }

        public static GUI owner = null;

        // text		
        private string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                toupdate = true;
            }
        }

        /// <summary>
        /// translations
        /// </summary>
        private SortedList translations = new SortedList();

        /// <summary>
        /// Gets the translations of the song
        /// </summary>
        public SortedList Translations
        {
            get { return translations; }
        }

        public IDictionaryEnumerator GetTransEnum()
        {
            return translations.GetEnumerator();
        }

        public ITranslation GetTranslation(string lang)
        {
            return (ITranslation)translations[lang];
        }

        public bool HasTrans
        {
            get { return (translations.Count > 0); }
        }

        public void AddTranslation(ITranslation t)
        {
            translations.Add(t.ID, t);
            transMenu = getTransMenuItem();
        }

        public void RemoveTranslation(ITranslation t)
        {
            t.Delete();
            translations.Remove(t.ID);
            transMenu = getTransMenuItem();
        }

        public void RefreshTransMenu()
        {
            transMenu = getTransMenuItem();
        }

        public string UpdateTranslations(IPhysicalStorage pStorage)
        {
            return pStorage.CommitTranslations(translations);
        }

        public void ShowTranslations(ListBox box)
        {
            box.Items.Clear();
            var en = translations.GetEnumerator();
            en.Reset();
            while (en.MoveNext())
            {
                if (!((ITranslation)en.Value).Deleted)
                {
                    box.Items.Add(en.Value);
                }
            }
        }

        private bool useDefaultStyle;

        public bool UseDefaultStyle
        {
            get { return useDefaultStyle; }
            set { useDefaultStyle = value; }
        }

        private Style style;

        public Style Style
        {
            get { return style; }
            set { style = value; }
        }

        // cons
        public Song(int nr, string title, string text, string id, string zus)
        {
            this.nr = nr;
            this.title = title;
            this.text = text;
            this.id = id;
            this.zus = zus;
            internalId = Guid.NewGuid();
        }

        public Song(int nr, string title, string text, string id, string zus, bool isNew)
          : this(nr, title, text, id, zus)
        {
            toupdate = isNew;
        }

        // indicates if an update ist necessary
        private bool toupdate = false;

        public bool ToUpdate
        {
            get { return toupdate; }
        }

        public void Update()
        {
            toupdate = true;
        }

        // indicates if song is deleted
        private bool deleted = false;

        public bool Deleted
        {
            get { return deleted; }
        }

        public void Delete()
        {
            // remove translations
            var en = translations.GetEnumerator();
            en.Reset();
            while (en.MoveNext())
            {
                ((Translation)en.Value).Delete();
            }
            // this
            deleted = true;
        }

        // util
        public override string ToString()
        {
            return Util.toFour(nr) + ":\t" + title;
        }

        private View view = null;
        private MenuItem transMenu = null;
        private Guid internalId;

        public MenuItem GetTransMenu(View view)
        {
            this.view = view;
            return transMenu;
        }

        public MenuItem TransMenu
        {
            get
            {
                view = null;
                return transMenu;
            }
        }

        private MenuItem getTransMenuItem()
        {
            var en = translations.GetEnumerator();
            en.Reset();
            var menu = new MenuItem();
            menu.Text = "Überse&tzungen";
            while (en.MoveNext())
            {
                if (!((Translation)en.Value).Deleted)
                {
                    var newItem = new MenuItem(((Translation)en.Value).ToString());
                    newItem.Click += new EventHandler(handleTransClick);
                    menu.MenuItems.Add(newItem);
                }
            }
            if (menu.MenuItems.Count != 0)
            {
                return menu;
            }
            return null;
        }


        public void uncheck()
        {
            try
            {
                if (transMenu == null) return;
                foreach (MenuItem mi in transMenu.MenuItems)
                {
                    mi.Checked = false;
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        private void handleTransClick(object sender, EventArgs e)
        {
            if (!((MenuItem)sender).Checked)
            {
                var i = 0;
                uncheck();
                while ((MenuItem)sender != transMenu.MenuItems[i]) i++;
                transMenu.MenuItems[i].Checked = true;

                if (view == null)
                {
                    View.ShowSong(this, (Translation)translations.GetByIndex(i), owner, owner.StandardNavigate, "Übersetzung");
                }
                else
                {
                    view.RefreshSong(this, ((Translation)translations.GetByIndex(i)));
                }
            }
        }

        // copy translations
        public void acceptTranslation(SortedList trans)
        {
            translations = trans;
        }

        public void copyTranslation(ISong to)
        {
            to.acceptTranslation(translations);
        }

        public ITranslation GetTranslation(int index)
        {
            if (index < 0 || index >= translations.Count)
            {
                index %= translations.Count;
            }
            return (ITranslation)translations.GetByIndex(index);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Song)
            {
                var s = (Song)obj;
                return ToString().CompareTo(s.ToString());
            }
            return 0;
        }

        #endregion

        #region Implementation of IIndexObject

        /// <summary>
        /// Searchable text relating to the object's content
        /// </summary>
        public IDictionary<string, string> SearchableText
        {
            get
            {
                // renew each time ("fresh" data required for correct indexing)
                var searchableText = new Dictionary<string, string>
                                                              {
                                                                {TitleField, Title},
                                                                {TextField, RemoveTags(Text)},
                                                                {NumberField, Number.ToString(CultureInfo.InvariantCulture)}
                                                              };
                return searchableText;
            }
        }

        private static string RemoveTags(string text)
        {
            var cleanText = "";
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
                else if (!skip)
                {
                    cleanText += c;
                }
            }
            return cleanText;
        }

        /// <summary>
        /// Unique key identifying the object
        /// </summary>
        public Guid Key
        {
            get { return internalId; }
        }

        #region    Static

        public const string TitleField = "Title";
        public const string TextField = "Text";
        public const string NumberField = "Number";

        static Song()
        {
            SongIndexFields = new Dictionary<string, int> { { TitleField, 5 }, { TextField, 3 }, { NumberField, 2 } };
        }

        public static readonly IDictionary<string, int> SongIndexFields;

        #endregion Static

        #endregion

        public bool Equals(Song other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.id, id) && other.nr == nr;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Song)) return false;
            return Equals((Song)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((id != null ? id.GetHashCode() : 0) * 397) ^ nr;
            }
        }
    }
}