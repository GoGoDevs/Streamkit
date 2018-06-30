using System.IO;
using System.Security.Cryptography;

using Streamkit.Utils;

namespace Streamkit.Crypto {
    public static class AES {
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
            return ByteConverter.ToString(
                    Encrypt(ByteConverter.ToBytes(str), ByteConverter.ToBytes(key)));
        }

        public static string Decrypt(string str, string key) {
            return ByteConverter.ToString(
                    Decrypt(ByteConverter.ToBytes(str), ByteConverter.ToBytes(key)));
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
