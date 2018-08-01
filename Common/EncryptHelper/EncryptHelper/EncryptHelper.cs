using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EncryptHelper
{
    public class EncryptHelperObj
    {
        public string SaltKey { get; set; }

        public string SaltKeyIV { get; set; }

        public string Value { get; set; }

        public byte[] EncryptString { get; set; }
    }

    public class EncryptHelper
    {
        public static EncryptHelperObj Encrypt_NewHelper()
        {
            EncryptHelperObj _EncryptHelperObj = new EncryptHelperObj();
            Aes myAes = Aes.Create();
            _EncryptHelperObj.SaltKey = Encoding.Default.GetString(myAes.Key);
            _EncryptHelperObj.SaltKeyIV = Encoding.Default.GetString(myAes.IV);
            return _EncryptHelperObj;
        }

        private static byte[] EncryptString_New(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        private static string DecryptString_New(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting                             stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        //Common Functions can be used in other projects Start
        private static bool CheckStringISNormal(string inputstring)
        {
            //inputstring = "[Q°{1¾¬µâjSAôxáRA/£ûÚJN^´€8,&";//"Mar 20 1965 12:00AM";// 
            bool valueIsUnknown = false;
            Regex regex = new Regex(@"[A-Za-z0-9 .,--_^!%$~#<>?"":;'`@&$*/=+(){}\[\]\\]");
            MatchCollection matches = regex.Matches(inputstring);

            if (matches.Count.Equals(inputstring.Length))
            { valueIsUnknown = true; }
            else
            { valueIsUnknown = false; }
            return valueIsUnknown;
        }

        public static string Get_DecryptedPassword(EncryptHelperObj _EncryptHelperObj, string EncryptString)
        {
            string Password = string.Empty;
            if (CheckStringISNormal(EncryptString))
            { Password = EncryptString; }
            else
            {
                try
                {
                    if (EncryptString.Length > 0)
                    {
                        Password = DecryptString_New(Encoding.Default.GetBytes(EncryptString), Encoding.Default.GetBytes(_EncryptHelperObj.SaltKey), Encoding.Default.GetBytes(_EncryptHelperObj.SaltKeyIV));
                    }
                }
                catch (Exception ex)
                { Password = string.Empty; }
            }
            return Password;
        }
        public static EncryptHelperObj Get_EncryptedPassword(EncryptHelperObj _EncryptHelperObj, string Password)
        {
            if (CheckStringISNormal(Password))
            {
                try
                {
                    if (Password.Length > 0)
                    {
                        if (_EncryptHelperObj == null || _EncryptHelperObj.SaltKey == null || _EncryptHelperObj.SaltKey.Length <= 0)
                        {
                            _EncryptHelperObj = EncryptHelper.Encrypt_NewHelper();
                        }
                        _EncryptHelperObj.EncryptString = EncryptString_New(Password, Encoding.Default.GetBytes(_EncryptHelperObj.SaltKey), Encoding.Default.GetBytes(_EncryptHelperObj.SaltKeyIV));
                    }
                    else
                    { _EncryptHelperObj.EncryptString = null; }

                }
                catch
                {
                    _EncryptHelperObj.EncryptString = null;
                }
                if (_EncryptHelperObj.EncryptString != null)
                {
                    _EncryptHelperObj.Value = Encoding.Default.GetString(_EncryptHelperObj.EncryptString);
                }
            }
            else
            { _EncryptHelperObj.Value = Password; }

            return _EncryptHelperObj;
        }
        //Common Functions can be used in other projects End


        public static void EncryptFile(string fileToBeEncrypted, string Key)
        {
            try
            {
                byte[] bytesToBeEncrypted = File.ReadAllBytes(fileToBeEncrypted);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(Key);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                File.WriteAllBytes(fileToBeEncrypted, bytesEncrypted);
            }
            catch { }
        }

        public static string DecryptFile(string fileToBeDecrypted, string PathToBeDecrypt, string Key)
        {
            string NewFile = fileToBeDecrypted;
            try
            {
                FileAttributes FileAttributes = File.GetAttributes(fileToBeDecrypted);

                FileInfo _FileInfo = new FileInfo(fileToBeDecrypted);

                byte[] bytesToBeDecrypted = File.ReadAllBytes(fileToBeDecrypted);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(Key);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                File.WriteAllBytes(PathToBeDecrypt + _FileInfo.Name, bytesDecrypted);
                if (File.Exists(PathToBeDecrypt + _FileInfo.Name))
                {
                    NewFile = PathToBeDecrypt + _FileInfo.Name;
                }
            }
            catch { }
            return NewFile;
        }
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
