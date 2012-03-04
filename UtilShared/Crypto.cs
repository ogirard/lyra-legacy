using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lyra2.UtilShared
{
	/// <summary>
	/// Summary description for Crypto.
	/// </summary>
	public class Crypto
	{
		// Function to Generate a 64 bits Key.
		public static string GenerateKey()
		{
			// Create an instance of Symetric Algorithm. Key and IV is generated automatically.
			DESCryptoServiceProvider desCrypto =(DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

			// Use the Automatically generated key for Encryption.
			return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
		}

		public static string KEY = "^=F3rP2";

		public static void EncryptFile(string input, string sOutputFilename)
		{
			FileStream fsEncrypted = new FileStream(sOutputFilename,
				FileMode.Create,
				FileAccess.Write);
			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			DES.Key = ASCIIEncoding.ASCII.GetBytes(KEY);
			DES.IV = ASCIIEncoding.ASCII.GetBytes(KEY);
			ICryptoTransform desencrypt = DES.CreateEncryptor();
			CryptoStream cryptostream = new CryptoStream(fsEncrypted,
				desencrypt,
				CryptoStreamMode.Write);

			StreamWriter wrt = new StreamWriter(cryptostream);
			wrt.Write(input);
			wrt.Close();
			fsEncrypted.Close();
		}

		public static string DecryptFile(string sInputFilename)
		{
			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			//A 64 bit key and IV is required for this provider.
			//Set secret key For DES algorithm.
			DES.Key = ASCIIEncoding.ASCII.GetBytes(KEY);
			//Set initialization vector.
			DES.IV = ASCIIEncoding.ASCII.GetBytes(KEY);

			//Create a file stream to read the encrypted file back.
			FileStream fsread = new FileStream(sInputFilename,
				FileMode.Open,
				FileAccess.Read);
			//Create a DES decryptor from the DES instance.
			ICryptoTransform desdecrypt = DES.CreateDecryptor();
			//Create crypto stream set to read and do a
			//DES decryption transform on incoming bytes.
			CryptoStream cryptostreamDecr = new CryptoStream(fsread,
				desdecrypt,
				CryptoStreamMode.Read);
			//return decrypted filecontent
			StreamReader sr = new StreamReader(cryptostreamDecr);
			string s = sr.ReadToEnd();
			sr.Close();
			fsread.Close();
			return s;
		}
	}
}
