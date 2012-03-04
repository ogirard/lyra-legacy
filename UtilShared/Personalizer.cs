using System;
using System.Collections.Generic;
using System.IO;

namespace Lyra2.UtilShared
{
    public class Personalizer
    {
        private readonly string fileName;
        private Dictionary<string, string> values;

        public Personalizer(string fileName)
        {
            this.fileName = fileName;
            this.values = new Dictionary<string, string>();
        }

        public bool Load()
        {
            if(File.Exists(fileName))
            {
                this.values.Clear();
                using(StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        string[] entry = line.Split(new char[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                        if(entry.Length == 2)
                        {
                            this.values.Add(entry[0].Trim(), entry[1].Trim());
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public string this[string key]
        {
            get
            {
                if(this.values.ContainsKey(key))
                {
                    return this.values[key];
                }
                return null;
            }
            set
            {
                if(this.values.ContainsKey(key))
                {
                    this.values[key] = value;    
                }
                else
                {
                    this.values.Add(key, value);
                }
            }
        }

        public int GetIntValue(string key)
        {
            if(this.values.ContainsKey(key))
            {
                return int.Parse(this[key]);
            }
            return -1;
        }

        public void Write()
        {
            using(StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (KeyValuePair<string, string> entry in values)
                {
                    writer.WriteLine(entry.Key + " = " + entry.Value);
                }
            }
            
        }
    }
}
