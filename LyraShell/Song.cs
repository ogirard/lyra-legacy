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
            get { return this.id; }
        }

        // pay attention! changes id to next free ID over s700.
        public string nextID()
        {
            return this.id = PhysicalXml.HighestID;
        }


        // number
        private int nr;

        public int Number
        {
            get { return this.nr; }
        }

        // desc
        private string zus;

        public string Desc
        {
            get { return this.zus; }
            set
            {
                this.zus = value;
                this.toupdate = true;
            }
        }

        // title
        private string title;

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.toupdate = true;
            }
        }

        public static GUI owner = null;

        // text		
        private string text;

        public string Text
        {
            get { return this.text?.TrimEnd('\r', '\n'); }
            set
            {
                this.text = value;
                this.toupdate = true;
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
            get { return this.translations; }
        }

        public IDictionaryEnumerator GetTransEnum()
        {
            return this.translations.GetEnumerator();
        }

        public ITranslation GetTranslation(string lang)
        {
            return (ITranslation)this.translations[lang];
        }

        public bool HasTrans
        {
            get { return (this.translations.Count > 0); }
        }

        public void AddTranslation(ITranslation t)
        {
            this.translations.Add(t.ID, t);
            this.transMenu = this.getTransMenuItem();
        }

        public void RemoveTranslation(ITranslation t)
        {
            t.Delete();
            this.translations.Remove(t.ID);
            this.transMenu = this.getTransMenuItem();
        }

        public void RefreshTransMenu()
        {
            this.transMenu = this.getTransMenuItem();
        }

        public string UpdateTranslations(IPhysicalStorage pStorage)
        {
            return pStorage.CommitTranslations(this.translations);
        }

        public void ShowTranslations(ListBox box)
        {
            box.Items.Clear();
            var en = this.translations.GetEnumerator();
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
            get { return this.useDefaultStyle; }
            set { this.useDefaultStyle = value; }
        }

        private Style style;

        public Style Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        // cons
        public Song(int nr, string title, string text, string id, string zus)
        {
            this.nr = nr;
            this.title = title;
            this.text = text;
            this.id = id;
            this.zus = zus;
            this.internalId = Guid.NewGuid();
        }

        public Song(int nr, string title, string text, string id, string zus, bool isNew)
          : this(nr, title, text, id, zus)
        {
            this.toupdate = isNew;
        }

        // indicates if an update ist necessary
        private bool toupdate = false;

        public bool ToUpdate
        {
            get { return this.toupdate; }
        }

        public void Update()
        {
            this.toupdate = true;
        }

        // indicates if song is deleted
        private bool deleted = false;

        public bool Deleted
        {
            get { return this.deleted; }
        }

        public void Delete()
        {
            // remove translations
            var en = this.translations.GetEnumerator();
            en.Reset();
            while (en.MoveNext())
            {
                ((Translation)en.Value).Delete();
            }
            // this
            this.deleted = true;
        }

        // util
        public override string ToString()
        {
            return Util.toFour(this.nr) + ":\t" + this.title;
        }

        private View view = null;
        private MenuItem transMenu = null;
        private Guid internalId;

        public MenuItem GetTransMenu(View view)
        {
            this.view = view;
            return this.transMenu;
        }

        public MenuItem TransMenu
        {
            get
            {
                this.view = null;
                return this.transMenu;
            }
        }

        private MenuItem getTransMenuItem()
        {
            var en = this.translations.GetEnumerator();
            en.Reset();
            var menu = new MenuItem();
            menu.Text = "Überse&tzungen";
            while (en.MoveNext())
            {
                if (!((Translation)en.Value).Deleted)
                {
                    var newItem = new MenuItem(((Translation)en.Value).ToString());
                    newItem.Click += new EventHandler(this.handleTransClick);
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
                if (this.transMenu == null) return;
                foreach (MenuItem mi in this.transMenu.MenuItems)
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
                this.uncheck();
                while ((MenuItem)sender != this.transMenu.MenuItems[i]) i++;
                this.transMenu.MenuItems[i].Checked = true;

                if (this.view == null)
                {
                    View.ShowSong(this, (Translation)this.translations.GetByIndex(i), owner, owner.StandardNavigate);
                }
                else
                {
                    this.view.RefreshSong(this, ((Translation)this.translations.GetByIndex(i)));
                }
            }
        }

        // copy translations
        public void acceptTranslation(SortedList trans)
        {
            this.translations = trans;
        }

        public void copyTranslation(ISong to)
        {
            to.acceptTranslation(this.translations);
        }

        public ITranslation GetTranslation(int index)
        {
            if (index < 0 || index >= this.translations.Count)
            {
                index %= this.translations.Count;
            }
            return (ITranslation)this.translations.GetByIndex(index);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Song)
            {
                var s = (Song)obj;
                return this.ToString().CompareTo(s.ToString());
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
                                                                {TitleField, this.Title},
                                                                {TextField, RemoveTags(this.Text)},
                                                                {NumberField, this.Number.ToString(CultureInfo.InvariantCulture)}
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
            get { return this.internalId; }
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
            return Equals(other.id, this.id) && other.nr == this.nr;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Song)) return false;
            return this.Equals((Song)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.id != null ? this.id.GetHashCode() : 0) * 397) ^ this.nr;
            }
        }
    }
}