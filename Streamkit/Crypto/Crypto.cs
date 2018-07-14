using System;
using System.IO;
using System.Security.Cryptography;

using Streamkit.Utils;

namespace Streamkit.Crypto {
    public static class TokenGenerator {
        public static string Generate() {
            return Guid.NewGuid().ToString();
        }
    }


    public static class AES {
        private static readonly byte[] salt = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        public static string GenerateKeyString() {
            return Base64.Encode(GenerateKey());
        }

        public static byte[] GenerateKey() {
            RijndaelManaged algo = new RijndaelManaged();
            algo.KeySize = 256;
            algo.GenerateKey();
            return algo.Key;
        }

        public static string Encrypt(string str) {
            return Encrypt(str, Config.AESKey);
        }

        public static string Decrypt(string str) {
            return Decrypt(str, Config.AESKey);
        }

        public static string Encrypt(string str, string key) {
            string strBase64 = Base64.Encode(str);
            byte[] bytes = Encrypt(Base64.DecodeToBytes(strBase64), key);
            return Base64.Encode(bytes);
        }

        public static string Decrypt(string strAes, string key) {
            byte[] bytes = Decrypt(Base64.DecodeToBytes(strAes), key);
            return Base64.DecodeToString(Base64.Encode(bytes));
        }

        public static byte[] Encrypt(byte[] plain, string password) {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(plain, 0, plain.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public static byte[] Decrypt(byte[] cipher, string password) {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(cipher, 0, cipher.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }
    }
}
