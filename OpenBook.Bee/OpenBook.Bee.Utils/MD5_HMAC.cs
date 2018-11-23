using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utils
{
    public class MD5_HMAC
    {

        public static string Encode(string clientrandom, string Password)
        {
            //these for MD5_HMAC
            string ipad = "";
            string opad = "";

            {
                for (int i = 0; i < 64; i++)
                {
                    ipad += "6";
                    opad += "\\";
                }
            }

            int KLen = Password.Length;
            string iResult = "";
            {
                for (int i = 0; i < 64; i++)
                {
                    if (i < KLen)
                        iResult += Convert.ToChar(ipad[i] ^ Password[i]);
                    else
                        iResult += Convert.ToChar(ipad[i]);
                }
            }
            iResult += clientrandom;
            iResult = fun_MD5(iResult);

            byte[] Test = hexstr2array(iResult);

            iResult = "";

            char[] b = System.Text.Encoding.GetEncoding(1252).GetChars(Test);

            for (int i = 0; i < b.Length; i++)
            {
                iResult += b[i];
            }

            string oResult = "";
            {
                for (int i = 0; i < 64; i++)
                {
                    if (i < KLen)
                        oResult += Convert.ToChar(opad[i] ^ Password[i]);
                    else
                        oResult += Convert.ToChar(opad[i]);
                }
            }

            oResult += iResult;

            string Result = fun_MD5(oResult).ToUpper();
            return Result;
        }

        public static string fun_MD5(string str)
        {
            byte[] b = System.Text.Encoding.GetEncoding(1252).GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        static Byte[] hexstr2array(string HexStr)
        {
            string HEX = "0123456789ABCDEF";
            string str = HexStr.ToUpper();
            int len = str.Length;
            byte[] RetByte = new byte[len / 2];
            for (int i = 0; i < len / 2; i++)
            {
                int NumHigh = HEX.IndexOf(str[i * 2]);
                int NumLow = HEX.IndexOf(str[i * 2 + 1]);

                RetByte[i] = Convert.ToByte(NumHigh * 16 + NumLow);
            }
            return RetByte;
        }

        public static string MD5(string src)
        {
            ////获取加密服务
            //System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();

            ////获取要加密的字段，并转化为Byte[]数组
            //byte[] testEncrypt = System.Text.Encoding.Unicode.GetBytes(src);

            ////加密Byte[]数组
            //byte[] resultEncrypt = md5CSP.ComputeHash(testEncrypt);

            ////将加密后的数组转化为字段(普通加密)
            //string testResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);

            //作为密码方式加密 
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(src, "MD5");

        }
    }
}
