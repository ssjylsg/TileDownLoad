using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace UntilTools
{
    interface XMLFile
    {
        //获取XML文档对象
        XmlDocument GetXML(string path);
        //保存XML文档对象
        void SaveXML(string path, XmlDocument xmldoc);
        //将该对象的内容写入XML中
        void ToXML(XmlDocument xmldoc);
        //用该对象的属性来更新XML中的对应的属性值
        void UpdateXML(XmlDocument xmldoc);
        /// <summary>
        /// 设置XML文件中的属性值
        /// </summary>
        /// <param name="path">XML文件对象</param>
        /// <param name="updateName">更新的属性名</param>
        /// <param name="updateValue">更新的值</param>
        void SetAttribute(XmlDocument xmldoc, string updateName, string updateValue);
        //设置有多个同名节点的XML文件的属性值
        void SetAttribute(XmlDocument xmldoc, int index, string updateName, string updateValue);
        /// <summary>
        /// 从XML文件中读取指定的属性值
        /// </summary>
        /// <param name="path">XML文件</param>
        /// <param name="attributeName">属性名称</param>
        /// <returns>返回该属性值的字符串</returns>
        string GetAttribute(XmlDocument xmldoc, string attributeName);
        //返回多个同名节点的属性值
        string GetAttribute(XmlDocument xmldoc, string attributeName, int index);
    }
}
