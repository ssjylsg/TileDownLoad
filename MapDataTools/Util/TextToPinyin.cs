using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDataTools.Util
{
    using System.Text.RegularExpressions;

    public class PinyinHelper
    {
        public string Pinyin { get; set; }

        public string Szm { get; set; }

        public override string ToString()
        {
            return string.Format("拼音:{0},拼音首字母：{1}", this.Pinyin, this.Szm);
        }

        public static PinyinHelper ConvertPinyin(string chinese)
        {
            return TextToPinyin.Convert(chinese);
        }
    }
    /// <summary>
    /// 拼音转换
    /// </summary>
   public class TextToPinyin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
       public static PinyinHelper Convert(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return new PinyinHelper();
            }
            //生成词典
            var dict = PinyinDictionary.Instance;

            //只取词典中的中文词汇，无需拼音
            var wordList = dict.Dictionary.Keys.ToList<string>();

            //进行正向分词
            var wordsLeft = Segmentation.SegMMLeftToRight(message, ref wordList);

            //判断分词是否正常返回
            if (wordsLeft == null)
            {
                return new PinyinHelper() { Pinyin = Hz2Py.GetPinyin(message), Szm = Hz2Py.GetFirstPinyin(message) };
            }

            List<string> stringBuilder = new List<string>();
            string pinyin;

            foreach (string word in wordsLeft)
            {
                //如果是单字，要检测字典中是否包含该单字
                if (word.Length == 1 && !dict.Dictionary.ContainsKey(word))
                {
                    //如果词典中不包含该中文单字，就要从微软的dll库读取拼音
                    if (UTF8Encoding.UTF8.GetBytes(word).Length == 1)
                    {
                        pinyin = word;
                    }
                    else
                    {
                        pinyin = PinyinConvert.GetFirstPinYinCount(word.ToCharArray()[0]).ToUpper();
                        pinyin = Regex.Replace(pinyin, @"\d", "");
                    }
                }
                else
                {
                    //一般情况不用检测，直接取词典中的拼音即可
                    pinyin = dict.Dictionary.ContainsKey(word) ? dict.Dictionary[word].ToUpper() : word;
                    pinyin = Regex.Replace(pinyin, @"\d", "");
                }
                stringBuilder.Add(pinyin);
            }
            return new PinyinHelper()
                       {
                           Pinyin = string.Join("", stringBuilder.ToArray()).Replace(" ","").Replace('\'', ' '),
                           Szm = Hz2Py.GetFirstPinyin(message).Replace('\'', ' ')
                       };
        }
    }
}
