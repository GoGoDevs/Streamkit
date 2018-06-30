using System.IO;
using System.Security.Cryptography;


public static class AES {
    public static byte[] GenerateKey() {
        RijndaelManaged algo = new RijndaelManaged();
        algo.KeySize = 256;
        algo.GenerateKey();
        return algo.Key;
    }

    public static byte[] Encrypt(byte[] data, byte[] key)  {
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