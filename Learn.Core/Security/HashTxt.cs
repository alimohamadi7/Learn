using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Configuration;


public class HashTxt
{
	
		
        public static string MD5_Hash(string TxtInput)
        {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.Unicode.GetBytes(TxtInput);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return  s.ToString();

    }
    public static string ShA1_Hash(string TxtInput)
    {
        System.Security.Cryptography.SHA1CryptoServiceProvider x = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.Unicode.GetBytes(TxtInput);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();

    }
    public static string ShA2_Hash(string TxtInput)
    {
        System.Security.Cryptography.SHA256CryptoServiceProvider x = new System.Security.Cryptography.SHA256CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.Unicode.GetBytes(TxtInput);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();

    }
    public static string EncodingTxT( string  inputTxt)
    {
        string pwd = "XFVqwe!@@#155U";//System.Configuration.ConfigurationManager.AppSettings["mypss"]; ;//iS Password---------------------------------------
        if (string.IsNullOrEmpty(pwd))
        {            
            return "";
        }
        byte[] msgToPersist = System.Text.Encoding.UTF8.GetBytes(inputTxt);//IS Text ---------------------------Text
        RandomNumberGenerator rndGen = RandomNumberGenerator.Create();
        byte[] salt = new byte[32];
        rndGen.GetBytes(salt);
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pwd, salt);
        byte[] key = pdb.GetBytes(32);

        Rijndael algo = Rijndael.Create();
        algo.Key = key;
        MemoryStream ms = new MemoryStream();
        ms.Write(salt, 0, salt.Length);
        ms.Write(algo.IV, 0, algo.IV.Length);
        CryptoStream cs = new CryptoStream(ms, algo.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(msgToPersist, 0, msgToPersist.Length);
        cs.Close();
        byte[] encryptedBytes = ms.ToArray();
        ms.Close();
        string encryptedMsgInBase64 = Convert.ToBase64String(encryptedBytes);
        return encryptedMsgInBase64;
    }
    public static string DecodingTxT(string inputTxT)
    {
        try {
            string encryptedMsgInBase64 = inputTxT.Replace(" ", "+"); ;//Messge Encription;_____________________Text Encription
            if (string.IsNullOrEmpty(encryptedMsgInBase64))
                return "";
            byte[] encryptedBytes = Convert.FromBase64String(encryptedMsgInBase64);
            Rijndael algo = Rijndael.Create();
            MemoryStream ms = new MemoryStream(encryptedBytes);
            byte[] salt = new byte[32];
            ms.Read(salt, 0, salt.Length);
            byte[] IV = new byte[algo.IV.Length];
            ms.Read(IV, 0, IV.Length);
            string pwd = "XFVqwe!@@#155U";//System.Configuration.ConfigurationManager.AppSettings["mypss"]; ;//iS Password---------------------------------------
            if (string.IsNullOrEmpty(pwd))
            {

                return "";
            }
            Rfc2898DeriveBytes db = new Rfc2898DeriveBytes(pwd, salt);
            byte[] key = db.GetBytes(32);
            algo.IV = IV;
            algo.Key = key;
            CryptoStream cs = new CryptoStream(ms, algo.CreateDecryptor(), CryptoStreamMode.Read);
            MemoryStream ms1 = new MemoryStream();
            byte[] buffer = new byte[256];    //will be used as a buffer to get the decrypted data
            int bytesRead = 0;
            byte[] decryptedData = null;
            try
            {
                do
                {
                    bytesRead = cs.Read(buffer, 0, buffer.Length);
                    ms1.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);

                decryptedData = ms1.ToArray();
                cs.Close();
                ms.Close();
                ms1.Close();
            }
            catch
            {

                return "";
            }
            return Encoding.UTF8.GetString(decryptedData);
        }catch
        {
            return "";
        }
    }
}