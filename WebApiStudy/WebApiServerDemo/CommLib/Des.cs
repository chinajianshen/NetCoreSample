using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommLib
{
    public static class Des
    {
        private static readonly byte[] s_vector = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = GetLegalKey(encryptKey);
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
            byte[] rgbIV = s_vector;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return HEX.BytesToHexString(mStream.ToArray());
        }

        public static string Decode(string decryptString, string decryptKey)
        {
            decryptKey = GetLegalKey(decryptKey);
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = s_vector;
            byte[] inputByteArray = HEX.HexStringToBytes(decryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public static void EncodeFile(string strFileNameSrc, string strFileNameTgt, string encryptKey)
        {
            byte[] bytearrayinput;

            //读入源文件(未加密的)
            try
            {
                using (FileStream fsSrc = File.OpenRead(strFileNameSrc))
                {
                    bytearrayinput = new byte[fsSrc.Length];
                    fsSrc.Read(bytearrayinput, 0, bytearrayinput.Length);
                }

            }
            catch (Exception ex)
            {
                throw new ClientException("打开源文件失败,请确保源文件未被其它程序打开.", ex);
            }

            //计算摘要并写入
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byHash = md5.ComputeHash(bytearrayinput);

            md5.Clear();

            //DES加密并写文件
            try
            {
                using (FileStream fsTgt = File.OpenWrite(strFileNameTgt))
                {
                    fsTgt.Write(byHash, 0, byHash.Length);

                    DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                    encryptKey = GetLegalKey(encryptKey);
                    byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
                    byte[] rgbIV = s_vector;
                    using (CryptoStream cStream = new CryptoStream(fsTgt, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytearrayinput, 0, bytearrayinput.Length);
                        cStream.FlushFinalBlock();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ClientException("写目标文件失败,请确保磁盘空间足够和未写保护.", ex);
            }
        }

        public static void DecodeFile(string strFileNameSrc, string strFileNameTgt, string decryptKey)
        {
            byte[] bytearrayinput;

            //读入源文件(加密的)
            try
            {
                using (FileStream fsSrc = File.OpenRead(strFileNameSrc))
                {
                    bytearrayinput = new byte[fsSrc.Length];
                    fsSrc.Read(bytearrayinput, 0, bytearrayinput.Length);
                }
            }
            catch (Exception ex)
            {
                throw new ClientException("打开源文件失败,请确保源文件未被其它程序打开.", ex);
            }

            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            decryptKey = GetLegalKey(decryptKey);
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = s_vector;
            MemoryStream mStream = new MemoryStream();

            try
            {
                using (CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                {
                    cStream.Write(bytearrayinput, 16, bytearrayinput.Length - 16);
                    cStream.FlushFinalBlock();
                }
            }
            catch (Exception ex)
            {
                throw new ClientException("解密失败,可能因为提供了错误的密码.", ex);
            }

            byte[] bytesDecoded = mStream.GetBuffer();

            //MD5检验
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byHash = md5.ComputeHash(bytesDecoded);
            for (int i = 0; i < 16; i++)
            {
                if (byHash[i] != bytearrayinput[i])
                {
                    throw new ClientException("解密失败,密码错误.");
                }
            }
            md5.Clear();

            //写源文件
            try
            {
                using (FileStream fsTgt = File.OpenWrite(strFileNameTgt))
                {
                    fsTgt.Write(bytesDecoded, 0, bytesDecoded.Length);
                }
            }
            catch (Exception ex)
            {
                throw new ClientException("写目标文件失败,请确保磁盘空间足够和未写保护.", ex);
            }
        }

        private static string GetLegalKey(string key)
        {
            if (key.Length < 8)
                key = key.PadRight(8, ' ');
            if (key.Length > 8)
                key = key.Substring(0, 8);
            return key;
        }
    }
}
