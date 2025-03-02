using System;

using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class EncryptManager
{
    private static string b = "BFF3CBAF81A079729C11A25180C8E8069F8304B05262FC80D22F6D7DED6E7166";
    private static readonly string a = b.Substring(0, 128 / 8);
    
    public static string EncryptData(string data)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(data);

        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 128;

        MemoryStream memoryStream = new MemoryStream();
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(a));
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();
        return encryptString;
    }

    public static string DecryptData(string data)
    {
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        string plainString = String.Empty;
        try
        {
            byte[] encryptBytes = Convert.FromBase64String(data);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 256;
            rijndaelManaged.BlockSize = 128;

            memoryStream = new MemoryStream(encryptBytes);
            ICryptoTransform decryptor =
                rijndaelManaged.CreateDecryptor(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(a));
            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainBytes = new byte[encryptBytes.Length];

            int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

            plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);
            cryptoStream.Close();
            memoryStream.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        finally
        {
            if (cryptoStream != null)
            {
                cryptoStream.Clear();
                cryptoStream.Close();
            }
            if (memoryStream != null)
            {
                memoryStream.Close();
            }

        }
        return plainString;
    }
}
