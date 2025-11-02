namespace Lyra2.LyraShell
{
    /// <summary>
    /// Based on Song-Type.
    /// To store a Translation.
    /// </summary>
    public class Translation : ITranslation
    {
        private string id;

        public string ID
        {
            get { return id; }
        }

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

        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                toupdate = true;
            }
        }

        private int lang;

        public int Language
        {
            get { return lang; }
            set
            {
                lang = value;
                toupdate = true;
            }
        }

        private bool unformatted = false;

        public bool Unformatted
        {
            get { return unformatted; }
            set
            {
                unformatted = value;
                toupdate = true;
            }
        }


        // to update?
        private bool toupdate = false;

        public bool ToUpdate
        {
            get { return toupdate; }
        }

        private bool deleted = false;

        public bool Deleted
        {
            get { return deleted; }
        }

        public void Delete()
        {
            deleted = true;
        }

        public Translation(string title, string text, int lang, bool unformatted, string id, bool isNew)
            : this(title, text, lang, unformatted, id)
        {
            toupdate = isNew;
        }

        public Translation(string title, string text, int lang, bool unformatted, string id)
        {
            this.title = title;
            this.text = text;
            this.lang = lang;
            this.id = id;
            this.unformatted = unformatted;
        }

        public override string ToString()
        {
            /*string str = "";
			switch (this.lang)
			{
				case (int) Util.Lang.EN:
					str += "english translation: ";
					break;
				case (int) Util.Lang.FR:
					str += "traduction française: ";
					break;
				case (int) Util.Lang.IT:
					str += "traduzione italiana: ";
					break;
				case (int) Util.Lang.ES:
					str += "traducción española: ";
					break;
				case (int) Util.Lang.DT:
					str += "deutsche Übersetzung: ";
					break;
				case (int) Util.Lang.LT:
					str += "lateinische Übersetzung: ";
					break;
				case (int) Util.Lang.HB:
					str += "hebräische Übersetzung: ";
					break;
				default:
					str += "unknown language: ";
					break;
			}
			str += "\"" + this.title + "\"";*/
            return title;
        }
    }
}