using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lyra2.UtilShared
{
    public class ImageSerializer
    {
        public static string SerializeBase64(Image image)
        {
            if (image == null) return string.Empty;

            try
            {
                // Serialize to a base 64 string
                byte[] bytes;
                long length = 0;
                MemoryStream ws = new MemoryStream();
                BinaryFormatter sf = new BinaryFormatter();
                sf.Serialize(ws, image);
                length = ws.Length;
                bytes = ws.GetBuffer();
                string encodedData = bytes.Length + ":" + Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None);
                return encodedData;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static Image DeserializeBase64(string imageString)
        {
            if (string.IsNullOrEmpty(imageString)) return null;

            try
            {
                // We need to know the exact length of the string - Base64 can sometimes pad us by a byte or two
                int p = imageString.IndexOf(':');
                int length = Convert.ToInt32(imageString.Substring(0, p));

                // Extract data from the base 64 string!
                byte[] memorydata = Convert.FromBase64String(imageString.Substring(p + 1));
                MemoryStream rs = new MemoryStream(memorydata, 0, length);
                BinaryFormatter sf = new BinaryFormatter();
                object o = sf.Deserialize(rs);
                return o as Image;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}