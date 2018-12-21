using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Transfer8Pro.Utils
{
    public static class CustomExtensions
    {
        public static string Prefix(this string src, string prefix)
        {
            return string.Concat(prefix, src);
        }

        /// <summary>
        /// 取前N个字符
        /// </summary>
        /// <param name="src"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string PreNChar(this string src,int num)
        {
            if (!string.IsNullOrEmpty(src))
            {
               return src.Trim().Substring(0, num);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 后N字符
        /// </summary>
        /// <param name="src"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string AfterNChar(this string src,int num)
        {
            if (!string.IsNullOrEmpty(src))
            {
                string str = src.Trim();
                return str.Substring(str.Length-num, num);
            }
            else
            {
                return "";
            }
        }


        public static bool IsAllNotNullOrEmpty(this string src, params string[] others)
        {
            if (string.IsNullOrEmpty(src))
            {
                return false;
            }
            foreach (string p in others)
            {
                if (string.IsNullOrEmpty(p))
                    return false;
            }
            return true;
        }
        public static string ToUnicodeString(this string src)
        {
            return CustomExtensions.ToUnicode(src);
        }

        public static string ReplaceMulti(this string src, string replaceto, params string[] replacesource)
        {
            foreach (string r in replacesource)
            {
                src = src.Replace(r, replaceto);
            }
            return src;
        }

        public static string ToJSON_Error_String(this string msg)
        {
            return string.Format("{{\"state\":0,\"msg\":\"{0}\"}}", msg).ToUnicodeString();
        }

        public static string ToJSON_Custom_String(this string msg, int code)
        {
            return string.Format("{{\"state\":{1},\"msg\":\"{0}\"}}", msg, code).ToUnicodeString();
        }

        public static string ToJSON_OK_String(this string msg)
        {
            return string.Format("{{\"state\":1,\"msg\":\"{0}\"}}", msg).ToUnicodeString();
        }

        public static string FormatString(this string msg, params object[] values)
        {
            return string.Format(msg, values);
        }

        public static int ToInt(this string src)
        {
            try
            {
                if (src == "--")
                    return int.MaxValue;
                if (string.IsNullOrEmpty(src))
                    return 0;
                return int.Parse(src);
            }
            catch
            {
                return 0;
            }
        }

        public static double ToDouble(this string src)
        {
            try
            {
                if (src == "--")
                    return double.MaxValue;
                if (string.IsNullOrEmpty(src))
                    return 0;
                return double.Parse(src);
            }
            catch
            {
                return 0d;
            }
        }

        public static List<int> ToIntList(this string[] src)
        {
            List<int> evt = new List<int>();
            foreach (string s in src)
            {
                evt.Add(s.ToInt());
            }
            return evt;
        }

        public static DateTime ToDateTime(this string src)
        {
            try
            {
                return DateTime.Parse(src);
            }
            catch { return DateTime.MinValue; }
        }

        public static string JoinBy<T>(this List<T> src, string spliter)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < src.Count; i++)
            {
                sb.Append(src[i].ToString());
                if (i < src.Count - 1)
                {
                    sb.Append(spliter);
                }
            }
            return sb.ToString();
        }


        public static string LongToIP(long ipvalue)
        {
            long ip1 = ipvalue / (256 * 256 * 256);
            long ip2 = (ipvalue - ip1 * 256 * 256 * 256) / (256 * 256);
            long ip3 = (ipvalue - ip1 * 256 * 256 * 256 - ip2 * 256 * 256) / 256;
            long ip4 = (ipvalue - ip1 * 256 * 256 * 256 - ip2 * 256 * 256 - ip3 * 256);
            return string.Format("{0}.{1}.{2}.{3}", ip1, ip2, ip3, ip4);
        }

        public static long IPToLong(string ip)
        {
#if DEBUG
            if (ip == "::1")
            {
                ip = "192.168.0.88";
            }
#endif
            string[] parts = ip.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                return long.Parse(parts[0]) * 256 * 256 * 256 + long.Parse(parts[1]) * 256 * 256 + long.Parse(parts[2]) * 256 + long.Parse(parts[3]);
            }
            else
                return 0;
        }


        /// <summary>
        /// 判断用户IP地址转换为Long型后是否在内网IP地址所在范围
        /// </summary>
        /// <param name="userIp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static bool IsInner(long userIp, long begin, long end)
        {
            return (userIp >= begin) && (userIp <= end);
        }


        /// <summary>
        /// 判断IP地址是否为内网IP地址
        /// </summary>
        /// <param name="ipAddress">IP地址字符串</param>
        /// <returns></returns>
        public static bool IsInnerIP(String ipAddress)
        {
            bool isInnerIp = false;
            long ipNum = IPToLong(ipAddress);
            /**
            私有IP：A类 10.0.0.0-10.255.255.255
            B类 172.16.0.0-172.31.255.255
            C类 192.168.0.0-192.168.255.255
            当然，还有127这个网段是环回地址 
            **/
            long aBegin = IPToLong("10.0.0.0");
            long aEnd = IPToLong("10.255.255.255");
            long bBegin = IPToLong("172.16.0.0");
            long bEnd = IPToLong("172.31.255.255");
            long cBegin = IPToLong("192.168.0.0");
            long cEnd = IPToLong("192.168.255.255");
            isInnerIp = IsInner(ipNum, aBegin, aEnd) || IsInner(ipNum, bBegin, bEnd) || IsInner(ipNum, cBegin, cEnd) || ipAddress.Equals("127.0.0.1") || ipAddress.Equals("localhost");
            return isInnerIp;
        }

        public static string Divide(string s, string total, bool ispercent)
        {
            if (string.IsNullOrEmpty(total))
            {
                return "--";
            }
            else
            {
                int t = total.ToInt();
                return t > 0 ? (s.ToInt() * 1d / t).ToString((ispercent ? "p" : "f2")) : "--";
            }
        }


        private static string script_key = "openbook.com.cn";

        public static string Encrypt(this string arg)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   

            byte[] key = Encoding.Unicode.GetBytes(script_key); //定义字节数组，用来存储密钥    

            byte[] data = Encoding.Unicode.GetBytes(arg);//定义字节数组，用来存储要加密的字符串  

            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      

            //使用内存流实例化加密流对象   
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);

            CStream.Write(data, 0, data.Length);  //向加密流中写入数据      

            CStream.FlushFinalBlock();              //释放加密流      

            return Convert.ToBase64String(MStream.ToArray());//返回加密后的字符串  
        }

        public static string Decrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象    

            byte[] key = Encoding.Unicode.GetBytes(script_key); //定义字节数组，用来存储密钥    

            byte[] data = Convert.FromBase64String(str);//定义字节数组，用来存储要解密的字符串  

            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      

            //使用内存流实例化解密流对象       
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);

            CStream.Write(data, 0, data.Length);      //向解密流中写入数据     

            CStream.FlushFinalBlock();               //释放解密流      

            return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串  
        }



        ///<summary>
        ///截取对应长度的字符串
        ///</summary>
        ///<param name="str">要截取的字符串</param>
        ///<param name="len">截取的长度</param>
        public static string GetSubString(string str, int len)
        {
            #region
            return len < str.Length ? str.Substring(0, len) : str;
            #endregion
        }

        ///<summary>
        ///简单清理输入字符串
        ///</summary>
        ///<param name="str">需要清理的字符串</param>
        ///<param name="maxlength">允许的字符串的最大长度</param>
        public static string CleanString(string str, int maxlength)
        {
            #region
            StringBuilder tempstr = new StringBuilder();
            if (str != null || str != string.Empty)
            {
                str = str.Trim();
                //str = (str.Length > maxlength ? str.Substring(0, maxlength) : str);
                if (str.Length > maxlength)
                    str = str.Substring(0, maxlength);
                for (int i = 0; i < str.Length; i++)
                {
                    switch (str[i])
                    {
                        case '"':
                            tempstr.Append("&quot;");
                            break;
                        case '<':
                            tempstr.Append("&lt;");
                            break;
                        case '>':
                            tempstr.Append("&gt;");
                            break;
                        default:
                            tempstr.Append(str[i]);
                            break;
                    }
                }
                tempstr.Replace("'", "''").Replace("&nbsp;", " ").Replace("%", "%").Replace(",", "，").Replace("script", " ").Replace(".js", " ").Replace("\n", "<br>").Replace("-", " ");
            }
            return tempstr.ToString();
            #endregion
        }
        public static string CleanString(string str)
        {
            return str.Replace("'", "''").Replace("%", "%").Replace(",", "，").Replace("script", " ").Replace(".js", " ");
        }
        public static string SuperCleanStr(string str)
        {
            #region
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            str = Regex.Replace(str, "[\\s]{2,}", " "); //替换多余空格
            str = Regex.Replace(str, "(<[b|B][r|R]/*>)+", "\n"); //代替 <br/>
            str = Regex.Replace(str, "(\\s*&[N|n][B|b][S|s][P|p];\\s*)+", " ");//代替&nbsp;
            str = Regex.Replace(str, "<(.|\\n)*?>", string.Empty);
            str = Regex.Replace(str, "</[.\\n]*?>", string.Empty);
            str = str.Replace("'", "''");

            return str;
            #endregion
        }
        ///<summary>
        ///安全清理输入的字符串
        ///</summary>
        ///<param name="str">需要清理的字符串</param>
        ///<param name="len">限定字符串长度</param>
        public static string SuperCleanStr(string str, int len)
        {
            #region
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (str.Length > len)
                str = str.Substring(0, len);
            str = Regex.Replace(str, "[\\s]{2,}", " "); //替换多余空格
            str = Regex.Replace(str, "(<[b|B][r|R]/*>)+", "\n"); //代替 <br/>
            str = Regex.Replace(str, "(\\s*&[N|n][B|b][S|s][P|p];\\s*)+", " ");//代替&nbsp;
            str = Regex.Replace(str, "<(.|\\n)*?>", string.Empty);
            str = Regex.Replace(str, "</[.\\n]*?>", string.Empty);
            str = str.Replace("'", "''");

            return str;
            #endregion
        }
        public static string TextToHtml(string txtStr)
        {
            return txtStr.Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace("\n", "<br />");
        }

        public static string GetSubStrWithoutHtml(string src, int len)
        {
            string str = RemoveHtml(src.Replace("&nbsp;", " "));
            return len < str.Length ? str.Substring(0, len) + "..." : str;
        }

        public static string RemoveHtml(string src)
        {
            string regimg = @"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>";
            string temp = Regex.Replace(src, regimg, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string regexstr = @"<[^>]*>";
            return Regex.Replace(temp, regexstr, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static string RemoveHtmlSpecial(string src)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(src.Replace("</li><li>", ">"), regexstr, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private static string REGEX_ISNUM = @"^\d+$";
        public static bool IsNum(string s)
        {
            return new Regex(REGEX_ISNUM).IsMatch(s);
        }
        public static int GetInt(string str)
        {
            int i = 0;
            if (!int.TryParse(str, out i))
                return -1;
            else
                return i;
        }
        //ConvertCHToEN
        //http://www.cnblogs.com/sxally/archive/2008/12/11/1352827.html


        public static Dictionary<string, string> SplitToDictionary(string str)
        {
            try
            {
                string[] strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length > 0)
                {
                    Dictionary<string, string> _t = new Dictionary<string, string>();
                    foreach (string s in strs)
                    {
                        string[] _ss = s.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (_ss.Length == 2)
                        {
                            _t.Add(_ss[0], _ss[1]);
                        }
                    }
                    return _t;
                }
                else
                    return new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new Dictionary<string, string>();
            }
        }



        public static string CombineFromDictionary(Dictionary<string, string> tar)
        {
            if (tar == null)
                return "";
            StringBuilder sbstr = new StringBuilder();
            foreach (KeyValuePair<string, string> v in tar)
            {
                sbstr.Append(v.Key + "_" + v.Value + ",");
            }
            return sbstr.ToString();
        }

        public static string DoubleToPercent(string doublestr)
        {
            double d = 0.0;
            if (double.TryParse(doublestr, out d))
            {
                return d.ToString("p");
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 截取double类型小数点后cnt位
        /// </summary>
        /// <param name="doublestr"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string TrimDouble(string doublestr, int cnt)
        {
            double d = 0.0;
            if (double.TryParse(doublestr, out d))
            {
                return string.Format("{0:N" + cnt.ToString() + "}", d);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encode(string src)
        {
            try
            {
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("gb2312").GetBytes(src));
                encoded = encoded.Replace("/", "_").Replace("+", "-");
                return encoded;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);

                return src;
            }
        }
        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Decode(string src)
        {
            try
            {
                string value = src.Replace("_", "/").Replace("-", "+");
                byte[] buffer = Convert.FromBase64String(value);
                return Encoding.GetEncoding("gb2312").GetString(buffer);
            }
            catch
            {
                //LogUtil.WriteLog(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 将字符床进行unicode编码
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string ToUnicode(string sourceString)
        {
            StringBuilder sb = new StringBuilder();
            char[] src = sourceString.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                if (bytes.Length > 1 && bytes[1] > 0)
                {
                    sb.Append(@"\u");
                    sb.Append(bytes[1].ToString("x2"));
                    sb.Append(bytes[0].ToString("x2"));
                }
                else
                {
                    sb.Append(src[i].ToString());
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="Length">字符串长度</param>
        /// <param name="Seed">随机函数种子值</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string RndString(int Length, params int[] Seed)
        {
            string strSep = ",";
            char[] chrSep = strSep.ToCharArray();

            //这里定义字符集
            string strChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            // + ",A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";

            string[] aryChar = strChar.Split(chrSep, strChar.Length);

            string strRandom = string.Empty;
            Random Rnd;
            if (Seed != null && Seed.Length > 0)
            {
                Rnd = new Random(Seed[0]);
            }
            else
            {
                Rnd = new Random();
            }
            //生成随机字符串
            for (int i = 0; i < Length; i++)
            {
                strRandom += aryChar[Rnd.Next(aryChar.Length)];
            }
            return strRandom;
        }

        public static List<int> SplitToListINT(string src)
        {

            string[] parts = src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> data = new List<int>();
            foreach (string s in parts)
            {
                data.Add(s.ToInt());
            }
            return data;
        }

        public static string CombineListINT(List<int> src)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int s in src)
            {
                sb.AppendFormat("{0}|", s);
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
                return sb.ToString().TrimEnd(new char[] { '|' });
            else
                return
                    sb.ToString();
        }
    }
}
