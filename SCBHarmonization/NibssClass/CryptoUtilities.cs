using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SCBHarmonization.NibssClass
{
    public class CryptoUtilities
    {
        public CryptoUtilities()
        {
        }

        public static class SHA
        {

            public static string GenerateSHA256String(string inputString)
            {
                using (SHA256 sha256 = SHA256Managed.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                    byte[] hash = sha256.ComputeHash(bytes);

                    return GetStringFromHash(hash);
                }


            }

            private static string GetStringFromHash(byte[] hash)
            {
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }
                return result.ToString();
            }

        }
    }


    public class AES
    {
        public string Key { get; }
        public string IV { get; }

        public AES(string Key, string IV)
        {
            this.Key = Key;
            this.IV = IV;


        }
        public string Encrypt(string word)
        {

            string prestr;


            byte[] wordBytes = Encoding.UTF8.GetBytes(word);
            using (MemoryStream ms = new MemoryStream())

            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                  
                    AES.Key = Encoding.UTF8.GetBytes(Key);
                    AES.IV = Encoding.UTF8.GetBytes(IV);
                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(wordBytes, 0, wordBytes.Length);
                        cs.Close();
                    }
                    byte[] encryptedBytes = ms.ToArray();

                    prestr = ByteArrayToString(encryptedBytes);

                }
            }
            return prestr;
        }

        public string Decrypt(string var)
        {
            byte[] cipher = StringToByteArray(var);
            string str = string.Empty;
            byte[] wordBytes = cipher;
            byte[] byteBuffer = new byte[wordBytes.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    //AES.KeySize = 256;
                    //AES.BlockSize = 128;
                    AES.Key = Encoding.UTF8.GetBytes(Key);
                    AES.IV = Encoding.UTF8.GetBytes(IV);
                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CBC;

                    try
                    {
                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {


                            cs.Write(wordBytes, 0, wordBytes.Length);
                            cs.Close();
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                        throw;
                    }
                    byte[] decryptedBytes = ms.ToArray();
                    str = System.Text.Encoding.UTF8.GetString(decryptedBytes);


                }
            }
            return str;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();
        }

    }
}
