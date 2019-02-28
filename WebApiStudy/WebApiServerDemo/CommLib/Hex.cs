using System;
using System.Globalization;
using System.Text;

namespace CommLib
{
    public static class HEX
    {
        public static string StringToHexString(string strOrg)
        {
            return BytesToHexString(Encoding.UTF8.GetBytes(strOrg));
        }

        public static string StringFromHexString(string strHex)
        {
            try
            {
                return Encoding.UTF8.GetString(HexStringToBytes(strHex));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder();
            foreach (byte t in input)
                hexString.Append(String.Format("{0:X2}", t));
            return hexString.ToString();
        }

        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
                return new byte[] { 0 };
            if (hex.Length % 2 == 1)
                hex = "0" + hex;
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                result[i] = byte.Parse(hex.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
            return result;
        }
    }
}
