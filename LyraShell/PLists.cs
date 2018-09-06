using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Lyra2.UtilShared;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Persönliche Listen.
    /// Vorsicht: Änderungen werden sofort übernommen!
    /// </summary>
    public class PLists
    {
        private ComboBox myComboBox;
        private XmlDocument doc;
        private IStorage storage;
        private MyList currentList = null;

        public PLists(ComboBox myComboBox, IStorage storage)
        {
            this.myComboBox = myComboBox;
            this.storage = storage;
            this.doc = new XmlDocument();
            try
            {
                this.doc.Load(Util.BASEURL + "\\" + Util.LISTURL);
                var nodes = this.doc.GetElementsByTagName("List");
                foreach (XmlNode node in nodes)
                {
                    myComboBox.Items.Add(new MyList(node, storage));
                }
                this.update();
            }
            catch (Exception e)
            {
                Util.MBoxError(e.Message, e);
            }
        }

        public void setCurrent(MyList list, ListBox show)
        {
            show.Items.Clear();
            var en = list.GetEnumerator();
            en.Reset();
            var check = "";
            while (en.MoveNext())
            {
                var toshow = (ISong)en.Value;
                if (!toshow.Deleted)
                {
                    show.Items.Add(toshow);
                }
                else
                {
                    check += en.Key + ",";
                }
            }

            if (check.Length > 0)
            {
                var todel = (check.Substring(0, check.Length - 1)).Split(',');
                foreach (var str in todel)
                {
                    list.Remove(Int32.Parse(str));
                }
                Util.MBoxError("Gelöschte Songs wurden von der aktuellen Liste entfernt!");
            }
            this.currentList = list;
        }

        public void Refresh(ListBox show)
        {
            this.setCurrent(this.currentList, show);
        }

        public void MoveLast(int pos)
        {
            if (pos >= 0)
            {
                for (var i = this.currentList.Count - 1; i > pos; i--)
                {
                    this.MoveSongUp(i);
                }
            }
        }

        public void DeleteCurrent()
        {
            this.doc.GetElementsByTagName("lists")[0].RemoveChild(this.currentList.RemoveMe());
            this.update();
        }

        public int MoveSongUp(int index)
        {
            var i = this.currentList.Up(index);
            this.update();
            return i;
        }

        public int MoveSongDown(int index)
        {
            var i = this.currentList.Down(index);
            this.update();
            return i;
        }

        public void AddSongToCurrent(ISong song)
        {
            this.currentList.AddSong(song);
            this.update();
        }

        public void AddSongToCurrentById(string id)
        {
            this.currentList.AddSongById(id);
            this.update();
        }

        public void RemoveSongFromCurrent(int index)
        {
            this.currentList.RemoveSong(index);
            this.update();
        }

        private void update()
        {
            try
            {
                this.doc.Save(Util.BASEURL + "\\" + Util.LISTURL);
            }
            catch (IOException ioe)
            {
                Util.MBoxError(ioe.Message, ioe);
            }
        }

        public MyList AddNewList(string name, string author, string date, string[] songs)
        {
            try
            {
                var lists = this.doc.GetElementsByTagName("lists")[0];
                var list = this.doc.CreateNode(XmlNodeType.Element, "List", this.doc.NamespaceURI);
                var namenode = this.doc.CreateNode(XmlNodeType.Element, "Title", this.doc.NamespaceURI);
                namenode.InnerText = name;
                var authornode = this.doc.CreateNode(XmlNodeType.Element, "Author", this.doc.NamespaceURI);
                authornode.InnerText = author;
                var datenode = this.doc.CreateNode(XmlNodeType.Element, "Date", this.doc.NamespaceURI);
                datenode.InnerText = date;
                var songsnode = this.doc.CreateNode(XmlNodeType.Element, "Songs", this.doc.NamespaceURI);
                var songsString = "";
                if (songs != null)
                {
                    foreach (var song in songs)
                    {
                        songsString += song + ",";
                    }
                }
                songsnode.InnerText = songsString.TrimEnd(',');
                list.AppendChild(namenode);
                list.AppendChild(authornode);
                list.AppendChild(datenode);
                list.AppendChild(songsnode);
                lists.AppendChild(list);
                this.update();
                this.myComboBox.Items.Add(new MyList(list, this.storage));
            }
            catch (XmlException xmlEx)
            {
                Util.MBoxError("Liste konnte nicht erstellt werden!", xmlEx);
            }
            return null;
        }
    }

    public class MyList : SortedList
    {
        private IStorage storage;
        private XmlNode myNode;

        private string name;
        private string author;
        private string date;


        public MyList(XmlNode myNode, IStorage storage)
        {
            this.storage = storage;
            this.myNode = myNode;

            var childnodes = myNode.ChildNodes;
            if ((childnodes[0].Name == "Title") &&
                (childnodes[1].Name == "Author") &&
                (childnodes[2].Name == "Date") &&
                (childnodes[3].Name == "Songs"))
            {
                this.name = childnodes[0].InnerText;
                this.author = childnodes[1].InnerText;
                this.date = childnodes[2].InnerText;

                var ids = childnodes[3].InnerText.Split(',');
                var notfound = false;
                var songs = "";
                foreach (var id in ids)
                {
                    var song = this.storage.getSongById(id);
                    if (song != null && !song.Deleted)
                    {
                        this.Add(this.Count, this.storage.getSongById(id));
                        songs += id + ",";
                    }
                    else if (id.Trim() != "")
                    {
                        notfound = true;
                    }
                }
                if (notfound)
                {
                    if (songs.Length > 0) songs = songs.Substring(0, songs.Length - 1);
                    childnodes[3].InnerText = songs;
                    Util.MBoxError("Nicht gefundene Lieder wurden aus Liste gelöscht!");
                }
            }
            else
            {
                throw new Exception("Lied-Listen wurden nicht geladen!");
            }
        }

        public void AddSong(ISong song)
        {
            this.Add(this.Count, song);
            this.myNode.ChildNodes[3].InnerText = this.updateList();
        }

        public void AddSongById(string id)
        {
            this.Add(this.Count, this.storage.getSongById(id));
            this.myNode.ChildNodes[3].InnerText += this.updateList();
        }

        public void RemoveSong(int index)
        {
            try
            {
                for (; index < this.Count - 1; index++)
                {
                    this.Down(index);
                }
                this.Remove(index);
                this.myNode.ChildNodes[3].InnerText = this.updateList();
            }
            catch (Exception ex)
            {
                Util.MBoxError(index + Util.NL + ex.StackTrace);
            }
        }

        public XmlNode RemoveMe()
        {
            return this.myNode;
        }

        private string updateList()
        {
            var en = this.GetEnumerator();
            en.Reset();
            var list = "";
            while (en.MoveNext())
            {
                list += ((ISong)en.Value).ID + ",";
            }
            if (list.Length > 0) list = list.Substring(0, list.Length - 1);
            return list;
        }

        public int Up(int index)
        {
            if (index > 0 && index < this.Count)
            {
                var temp = this.GetByIndex(index);
                this.SetByIndex(index, this.GetByIndex(index - 1));
                this.SetByIndex(index - 1, temp);
                this.myNode.ChildNodes[3].InnerText = this.updateList();
                return index - 1;
            }
            return index;
        }

        public int Down(int index)
        {
            if (index >= 0 && index < this.Count - 1)
            {
                var temp = this.GetByIndex(index + 1);
                this.SetByIndex(index + 1, this.GetByIndex(index));
                this.SetByIndex(index, temp);
                this.myNode.ChildNodes[3].InnerText = this.updateList();
                return index + 1;
            }
            return index;
        }

        public void exportMe(StreamWriter writer)
        {
            writer.WriteLine(this.name);
            writer.WriteLine(this.author);
            writer.WriteLine(this.date);
            writer.WriteLine(this.updateList());
            writer.Flush();
            writer.Close();
        }

        public override string ToString()
        {
            return this.name + "    {" + this.author + "@" + this.date + "}";
        }
    }
}