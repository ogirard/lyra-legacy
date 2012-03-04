namespace Lyra2.LyraShell
{
    public interface ITranslation
    {
        string ID { get; }

        string Title { get; set; }

        string Text { get; set; }

        int Language { get; set; }

        bool Unformatted { get; set; }

        bool ToUpdate { get; }

        bool Deleted { get; }

        void Delete();
    }
}