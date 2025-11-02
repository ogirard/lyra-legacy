using System;
using System.Collections;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    public interface ISong : IComparable
    {
        string ID { get; }

        string nextID();

        int Number { get; }

        string Desc { get; set; }

        string Title { get; set; }

        string Text { get; set; }

        IDictionaryEnumerator GetTransEnum();
        ITranslation GetTranslation(string lang);

        bool HasTrans { get; }

        void AddTranslation(ITranslation t);
        void RemoveTranslation(ITranslation t);
        void RefreshTransMenu();
        string UpdateTranslations(IPhysicalStorage pStorage);
        void ShowTranslations(ListBox box);

        bool UseDefaultStyle { get; set; }

        Style Style { get; set; }

        bool ToUpdate { get; }

        void Update();

        bool Deleted { get; }

        void Delete();
        string ToString();
        MenuItem GetTransMenu(View view);

        MenuItem TransMenu { get; }

        void uncheck();
        void acceptTranslation(SortedList trans);
        void copyTranslation(ISong to);
        ITranslation GetTranslation(int index);

        SortedList Translations { get; }
    }
}