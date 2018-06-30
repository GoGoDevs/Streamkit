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
            byte[] bytes = Encrypt(Base64.DecodeToBytes(strBase64), Base64.DecodeToBytes(key));
            return Base64.DecodeToString(Base64.Encode(bytes));
        }

        public static string Decrypt(string str, string key) {
            string strBase64 = Base64.Encode(str);
            byte[] bytes = Decrypt(Base64.DecodeToBytes(strBase64), Base64.DecodeToBytes(key));
            return Base64.DecodeToString(Base64.Encode(bytes));
        }

        public static byte[] Encrypt(byte[] data, byte[] key) {
            RijndaelManaged algo = new RijndaelManaged();
            algo.Key = key;
            ICryptoTransform encryptor = algo.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    using (StreamWriter sw = new StreamWriter(cs)) {
                        sw.Write(data);
                    }
                    return ms.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] date, byte[] key) {
            RijndaelManaged algo = new RijndaelManaged();
            algo.Key = key;
            ICryptoTransform decryptor = algo.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    return ms.ToArray();
                }
            }
        }
    }
}
