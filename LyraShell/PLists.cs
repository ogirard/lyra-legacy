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
        private GitStore gitStore;

        public PLists(ComboBox myComboBox, IStorage storage)
        {
            this.myComboBox = myComboBox;
            this.storage = storage;
            gitStore = new GitStore(Path.GetDirectoryName(Util.BASEURL + "\\" + Util.LISTURL));
            doc = new XmlDocument();
            try
            {
                doc.Load(Util.BASEURL + "\\" + Util.LISTURL);
                var nodes = doc.GetElementsByTagName("List");
                foreach (XmlNode node in nodes)
                {
                    myComboBox.Items.Add(new MyList(node, storage));
                }
                update();
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
            currentList = list;
        }

        public void Refresh(ListBox show)
        {
            setCurrent(currentList, show);
        }

        public void MoveLast(int pos)
        {
            if (pos >= 0)
            {
                for (var i = currentList.Count - 1; i > pos; i--)
                {
                    MoveSongUp(i);
                }
            }
        }

        public void DeleteCurrent()
        {
            doc.GetElementsByTagName("lists")[0].RemoveChild(currentList.RemoveMe());
            update();
        }

        public int MoveSongUp(int index)
        {
            var i = currentList.Up(index);
            update();
            return i;
        }

        public int MoveSongDown(int index)
        {
            var i = currentList.Down(index);
            update();
            return i;
        }

        public void AddSongToCurrent(ISong song)
        {
            currentList.AddSong(song);
            update();
        }

        public void AddSongToCurrentById(string id)
        {
            currentList.AddSongById(id);
            update();
        }

        public void RemoveSongFromCurrent(int index)
        {
            currentList.RemoveSong(index);
            update();
        }

        private void update()
        {
            try
            {
                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    var xml = stringWriter.GetStringBuilder().ToString();
                    gitStore.CommitFile(Util.LISTURL, xml);
                }
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
                var lists = doc.GetElementsByTagName("lists")[0];
                var list = doc.CreateNode(XmlNodeType.Element, "List", doc.NamespaceURI);
                var namenode = doc.CreateNode(XmlNodeType.Element, "Title", doc.NamespaceURI);
                namenode.InnerText = name;
                var authornode = doc.CreateNode(XmlNodeType.Element, "Author", doc.NamespaceURI);
                authornode.InnerText = author;
                var datenode = doc.CreateNode(XmlNodeType.Element, "Date", doc.NamespaceURI);
                datenode.InnerText = date;
                var songsnode = doc.CreateNode(XmlNodeType.Element, "Songs", doc.NamespaceURI);
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
                update();
                myComboBox.Items.Add(new MyList(list, storage));
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
                name = childnodes[0].InnerText;
                author = childnodes[1].InnerText;
                date = childnodes[2].InnerText;

                var ids = childnodes[3].InnerText.Split(',');
                var notfound = false;
                var songs = "";
                foreach (var id in ids)
                {
                    var song = this.storage.getSongById(id);
                    if (song != null && !song.Deleted)
                    {
                        Add(Count, this.storage.getSongById(id));
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
            Add(Count, song);
            myNode.ChildNodes[3].InnerText = updateList();
        }

        public void AddSongById(string id)
        {
            Add(Count, storage.getSongById(id));
            myNode.ChildNodes[3].InnerText += updateList();
        }

        public void RemoveSong(int index)
        {
            try
            {
                for (; index < Count - 1; index++)
                {
                    Down(index);
                }
                Remove(index);
                myNode.ChildNodes[3].InnerText = updateList();
            }
            catch (Exception ex)
            {
                Util.MBoxError(index + Util.NL + ex.StackTrace);
            }
        }

        public XmlNode RemoveMe()
        {
            return myNode;
        }

        private string updateList()
        {
            var en = GetEnumerator();
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
            if (index > 0 && index < Count)
            {
                var temp = GetByIndex(index);
                SetByIndex(index, GetByIndex(index - 1));
                SetByIndex(index - 1, temp);
                myNode.ChildNodes[3].InnerText = updateList();
                return index - 1;
            }
            return index;
        }

        public int Down(int index)
        {
            if (index >= 0 && index < Count - 1)
            {
                var temp = GetByIndex(index + 1);
                SetByIndex(index + 1, GetByIndex(index));
                SetByIndex(index, temp);
                myNode.ChildNodes[3].InnerText = updateList();
                return index + 1;
            }
            return index;
        }

        public void exportMe(StreamWriter writer)
        {
            writer.WriteLine(name);
            writer.WriteLine(author);
            writer.WriteLine(date);
            writer.WriteLine(updateList());
            writer.Flush();
            writer.Close();
        }

        public override string ToString()
        {
            return name + "    {" + author + "@" + date + "}";
        }
    }
}