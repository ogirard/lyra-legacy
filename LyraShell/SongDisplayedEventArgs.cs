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

        public IList<JumpMark> Jumpmarks
        {
            get { return jumpmarks; }
        }

        public ISong DisplayedSong
        {
            get { return song; }
        }

        public ISong NextSong
        {
            get { return next; }
        }

        public ISong PreviousSong
        {
            get { return previous; }
        }

        public SongDisplayedEventArgs(ISong song, ISong next, ISong previous)
        {
            this.song = song;
            this.next = next;
            this.previous = previous;
            this.jumpmarks = new List<JumpMark>();
        }
    }
}