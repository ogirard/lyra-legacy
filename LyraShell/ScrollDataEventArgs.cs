using System;

namespace Lyra2.LyraShell
{
    public class ScrollDataEventArgs : EventArgs
    {
        public int DesiredHeight { get; set; }

        public int DisplayHeight { get; set; }

        public int ScrollPosition { get; set; }
    }
}