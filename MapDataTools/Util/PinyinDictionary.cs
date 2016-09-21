using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDataTools.Util
{
    using System.Xml;

    class PinyinDictionary
    {
        //定义词典
        private Dictionary<string, string> dictionary;
        public Dictionary<string, string> Dictionary
        {
            get { return dictionary; }
        }

        private static PinyinDictionary _Instance;
        public static PinyinDictionary Instance
        {
            get
            {
               _Instance =  _Instance ?? new PinyinDictionary();
                return _Instance;
            }
        }

        /// <summary>
        /// 默认词典的构造函数
        /// </summary>
        public PinyinDictionary()
            : this(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Dictionary\\Dictionary.xml"))
        {
            
        }

        /// <summary>
        /// 构造函数将生成词典
        /// </summary>
        /// <param name="filename">词典文件的路径</param>
        public PinyinDictionary(string filename)
        {
            dictionary = new Dictionary<string, string>();
            string cn = "";
            string pinyin = "";
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "P":
                                pinyin = reader.ReadElementContentAsString();
                                break;
                            case "N":
                                cn = reader.ReadElementContentAsString();
                                break;
                            case "D":
                                {
                                    if (!string.IsNullOrEmpty(cn) && !string.IsNullOrEmpty(pinyin))
                                    {
                                        dictionary.Add(cn, pinyin);
                                        cn = "";
                                        pinyin = "";
                                    }
                                }
                                break;
                        }
                    }
                }
            } 
        }

        /// <summary>
        /// 根据给出的中文词汇，在字典中查找对应拼音。
        /// 找不到就返回null。
        /// 注意：dictionary不能为空
        /// </summary>
        /// <param name="cn"></param>
        /// <returns>中文词汇，对应的拼音</returns>
        public string GetPinyin(string cn)
        {
            if (string.IsNullOrEmpty(cn))
            {
                return null;
            }

            if (dictionary == null)
            {
                return null;
            }

            if (dictionary.Count == 0)
            {
                return null;
            }

            if (dictionary.ContainsKey(cn))
            {
                return dictionary[cn];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据中文词汇，通过查字典，得到拆分后，每个中文单字对应的拼音
        /// </summary>
        /// <param name="cn"></param>
        /// <returns>每个单字对应拼音，组成的字典</returns>
        public Dictionary<char, string> GetCnCharPinyin(string cn)
        {
            if (string.IsNullOrEmpty(cn))
            {
                return null;
            }

            if (dictionary == null)
            {
                return null;
            }

            if (dictionary.Count == 0)
            {
                return null;
            }

            if (!dictionary.ContainsKey(cn))
            {
                return null;
            }

            Dictionary<char, string> cnCharPinyin = new Dictionary<char, string>();
            string[] pinyins = dictionary[cn].Split(' ');
            char[] cnChars = cn.ToCharArray();
            for (int i = 0; i < cn.Length; i++)
            {
                cnCharPinyin.Add(cnChars[i], pinyins[i]);
            }
            return cnCharPinyin;
        }
    }
}
