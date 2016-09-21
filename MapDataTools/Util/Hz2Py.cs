 

namespace MapDataTools.Util
{
    using Microsoft.International.Converters.PinYinConverter;

    internal class Hz2Py
    {
        public static string GetFirstPinyin(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 汉字转化为拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPinyin(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

    }
}
