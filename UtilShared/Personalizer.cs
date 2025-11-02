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
            values = new Dictionary<string, string>();
        }

        public bool Load()
        {
            if(File.Exists(fileName))
            {
                values.Clear();
                using(StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        string[] entry = line.Split(new char[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                        if(entry.Length == 2)
                        {
                            values.Add(entry[0].Trim(), entry[1].Trim());
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
                if(values.ContainsKey(key))
                {
                    return values[key];
                }
                return null;
            }
            set
            {
                if(values.ContainsKey(key))
                {
                    values[key] = value;    
                }
                else
                {
                    values.Add(key, value);
                }
            }
        }

        public int GetIntValue(string key)
        {
            if(values.ContainsKey(key))
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
