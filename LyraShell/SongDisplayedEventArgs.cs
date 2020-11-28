using System;
using System.Collections.Generic;

namespace Lyra2.LyraShell
{
    public delegate void SongDisplayedEventHandler(object sender, SongDisplayedEventArgs args);

    public class SongDisplayedEventArgs : EventArgs
    {
        private readonly ISong song;
        private readonly ISong next;
        private readonly ISong previous;
        private readonly IList<JumpMark> jumpmarks;
        private readonly string source;

        public IList<JumpMark> Jumpmarks
        {
            get { return this.jumpmarks; }
        }

        public ISong DisplayedSong
        {
            get { return this.song; }
        }

        public ISong NextSong
        {
            get { return this.next; }
        }

        public ISong PreviousSong
        {
            get { return this.previous; }
        }

        public string Source
        {
            get { return this.source; }
        }



        public SongDisplayedEventArgs(ISong song, ISong next, ISong previous, string source)
        {
            this.song = song;
            this.next = next;
            this.previous = previous;
            this.source = source;
            this.jumpmarks = new List<JumpMark>();
        }
    }
}