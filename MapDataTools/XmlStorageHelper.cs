using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.IO;

namespace MapDataTools
{
    public class XmlStorageHelper
    {
        /// <summary>
        /// 内部类：运行时信息（assembly名称＋完整类名）
        /// </summary>
        class Runtime
        {
            /// <summary>
            /// 类名（完整名称）
            /// </summary>
            public string FullTypeName = string.Empty;
            /// <summary>
            /// 运行库名（不带版本信息）
            /// </summary>
            public string AssemblyName = string.Empty;
            /// <summary>
            /// 判断该运行库是否已经被加载的标示
            /// </summary>
            bool isAssemblyLoaded = false;
            /// <summary>
            /// 已经加载的运行库实例
            /// </summary>
            Assembly loadedAssembly = null;

            public Runtime()
            {
            }

            public Runtime(string typeName, string asmName)
            {
                FullTypeName = typeName;
                AssemblyName = asmName;
            }

            /// <summary>
            /// 创建运行库中的实例
            /// </summary>
            /// <param name="className">类名（全名）</param>
            /// <returns>创建成功后返回实例，创建失败返回Null</returns>
            public Type GetTypeByFullName(string className)
            {
                Assembly asm = LoadedAssembly;
                if (asm == null)
                    return null;
                return asm.GetType(className);
            }

            /// <summary>
            /// 获取运行库的实例
            /// </summary>
            Assembly LoadedAssembly
            {
                get
                {
                    if (!isAssemblyLoaded)
                    {
                        isAssemblyLoaded = true;
                        try
                        {
                            loadedAssembly = Assembly.Load(AssemblyName);
                            if (loadedAssembly == null)
                            {
                                string exePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                                string assemblyFile = Path.Combine(exePath, AssemblyName);
                                if (File.Exists(assemblyFile))
                                    loadedAssembly = Assembly.LoadFile(assemblyFile);
                            }
                        }
                        catch
                        {
                        }
                    }
                    return loadedAssembly;
                }
            }
        }

        /// <summary>
        /// XML文档对象
        /// </summary>
        XmlDocument xmldoc = new XmlDocument();

        const string Collection_Tag = "InnerCollection";
        const string Dictionary_Tag = "InnerDictionary";

        List<Runtime> runtimeList = new List<Runtime>();

        void AddRuntime(string typeName, string asmName)
        {
            foreach (Runtime rt in runtimeList)
            {
                if (rt.FullTypeName.Equals(typeName) && rt.AssemblyName.Equals(asmName))
                {
                    return;
                }
            }
            //add new runtime
            Runtime r = new Runtime(typeName, asmName);
            runtimeList.Add(r);
        }

        void SetCollectionToXml(ICollection colObject, XmlElement parentEle)
        {
            if (colObject == null)
                return;
            foreach (object o in colObject)
            {
                if (o == null)
                    continue;
                if (IsBasicType(o.GetType()))
                {
                    AddRuntime(o.GetType().FullName, o.GetType().Assembly.GetName().Name);
                    string attValue = o.ToString();
                    XmlElement subEle = xmldoc.CreateElement(o.GetType().Name);
                    subEle.SetAttribute("value", attValue);
                    parentEle.AppendChild(subEle);
                }
                //customer data type
                else
                {
                    SetValueToXml(o, parentEle);
                }
            }
        }

        void SetDictionaryToXml(IDictionary dictObject, XmlElement parentEle)
        {
            if (dictObject == null)
                return;
            foreach (DictionaryEntry entry in dictObject)
            {
                XmlElement entryEle = xmldoc.CreateElement(entry.GetType().Name);
                if (IsBasicType(entry.Key.GetType()))
                {
                    AddRuntime(entry.Key.GetType().FullName, entry.Key.GetType().Assembly.GetName().Name);
                    XmlElement keyEle = xmldoc.CreateElement(entry.Key.GetType().Name);
                    keyEle.SetAttribute("key", entry.Key.ToString());
                    entryEle.AppendChild(keyEle);
                }
                else
                {
                    SetValueToXml(entry.Key, entryEle);
                }
                if (IsBasicType(entry.Value.GetType()))
                {
                    AddRuntime(entry.Value.GetType().FullName, entry.Value.GetType().Assembly.GetName().Name);
                    XmlElement valueEle = xmldoc.CreateElement(entry.Value.GetType().Name);
                    valueEle.SetAttribute("value", entry.Value.ToString());
                    entryEle.AppendChild(valueEle);
                }
                else
                {
                    SetValueToXml(entry.Value, entryEle);
                }
                parentEle.AppendChild(entryEle);
            }
        }

        void SetValueToXml(object xmlObject, XmlElement parentEle)
        {
            SetValueToXml(xmlObject, parentEle, null);
        }

        void SetValueToXml(object xmlObject, XmlElement parentEle, string tagName)
        {
            if (xmlObject == null)
                return;
            XmlElement classEle = null;
            Type objType = xmlObject.GetType();
            //1.对象是否为集合
            if (IsCollection(objType))
            {
                classEle = null;
                string typeString = objType.FullName.Replace('`', '#');
                if (IsDictionary(objType))
                {
                    classEle = xmldoc.CreateElement(Dictionary_Tag);
                    classEle.SetAttribute("TypeName", typeString);
                    SetDictionaryToXml((IDictionary)xmlObject, classEle);
                }
                else
                {
                    classEle = xmldoc.CreateElement(Collection_Tag);
                    classEle.SetAttribute("TypeName", typeString);
                    SetCollectionToXml((ICollection)xmlObject, classEle);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tagName))
                    classEle = xmldoc.CreateElement(objType.Name);
                else
                    classEle = xmldoc.CreateElement(tagName);
                AddRuntime(objType.FullName, objType.Assembly.GetName().Name);
                foreach (FieldInfo field in objType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
                {
                    object objValue = field.GetValue(xmlObject);
                    //如果是集合类型，则需要获取下级
                    if (IsCollection(field.FieldType))
                    {
                        XmlElement colEle = xmldoc.CreateElement(field.Name);
                        if (IsDictionary(field.FieldType))  //是否为字典
                        {
                            SetDictionaryToXml((IDictionary)objValue, colEle);
                        }
                        else
                        {
                            SetCollectionToXml((ICollection)objValue, colEle);
                        }
                        classEle.AppendChild(colEle);
                    }
                    else
                    {
                        if (IsBasicType(field.FieldType))
                        {
                            string attValue = "";
                            if (objValue != null)
                                attValue = objValue.ToString();
                            classEle.SetAttribute(field.Name, attValue);
                        }
                        //customer data type
                        else
                        {
                            SetValueToXml(objValue, classEle, field.Name);
                        }
                    }
                }
            }
            if (parentEle != null)
                parentEle.AppendChild(classEle);
            else
                xmldoc.AppendChild(classEle);
        }

        void GetCollectionFromXml(IList colObject, XmlNode colNode)
        {
            if (colObject == null || colNode == null)
                return;
            //初始化集合中的每个元素，并将元素添加到集合中
            foreach (XmlNode eleNode in colNode.ChildNodes)
            {
                Type eleType = null;
                if (eleNode.Name.Equals(Collection_Tag) || eleNode.Name.Equals(Dictionary_Tag))
                {
                    XmlAttribute typeAtt = eleNode.Attributes["TypeName"];
                    if (typeAtt != null)
                    {
                        string typeString = typeAtt.Value.Replace('#', '`');
                        try
                        {
                            eleType = Type.GetType(typeString);
                        }
                        catch { }
                    }
                }
                else
                {
                    eleType = TryToGetType(eleNode.Name);
                }
                if (eleType == null)
                    continue;
                if (IsBasicType(eleType))
                {
                    XmlAttribute att = eleNode.Attributes["value"];
                    if (att != null)
                    {
                        try
                        {
                            if (eleType.IsSubclassOf(typeof(Enum)))
                            {
                                object enumValue = Enum.Parse(eleType, att.Value);
                                colObject.Add(enumValue);
                            }
                            else
                            {
                                //转换为目标类型
                                object ovalue = Convert.ChangeType(att.Value, eleType);
                                colObject.Add(ovalue);
                            }

                        }
                        catch { }
                    }
                }
                else
                {
                    object o = Activator.CreateInstance(eleType, true);
                    GetValueFromXml(o, eleNode);
                    colObject.Add(o);
                }
            }
        }

        void GetDictionaryFromXml(IDictionary dictObject, XmlNode dictNode)
        {
            if (dictObject == null || dictNode == null)
                return;
            //初始化字典中的每个元素，并将元素添加到字典中
            foreach (XmlNode kvNode in dictNode.ChildNodes)
            {
                if (kvNode.Name.Equals("DictionaryEntry"))
                {
                    if (kvNode.ChildNodes.Count != 2)
                        continue;
                    object entryKey = new object();
                    object entryValue = new object();
                    XmlNode keyNode = kvNode.ChildNodes[0];
                    Type keyType = null;
                    if (keyNode.Name.Equals(Collection_Tag) || keyNode.Name.Equals(Dictionary_Tag))
                    {
                        XmlAttribute typeAtt = keyNode.Attributes["TypeName"];
                        if (typeAtt != null)
                        {
                            string typeString = typeAtt.Value.Replace('#', '`');
                            try
                            {
                                //entryKey = Activator.CreateInstance("mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", typeString);
                                keyType = Type.GetType(typeString);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        keyType = TryToGetType(keyNode.Name);
                    }
                    if (keyType == null)
                        continue;
                    if (IsBasicType(keyType))
                    {
                        XmlAttribute att = keyNode.Attributes["key"];
                        if (att != null)
                        {
                            try
                            {
                                if (keyType.IsSubclassOf(typeof(Enum)))
                                    entryKey = Enum.Parse(keyType, att.Value);
                                else
                                    entryKey = Convert.ChangeType(att.Value, keyType);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        entryKey = Activator.CreateInstance(keyType, true);
                        GetValueFromXml(entryKey, keyNode);
                    }
                    XmlNode valueNode = kvNode.ChildNodes[1];
                    Type valueType = null;
                    if (valueNode.Name.Equals(Collection_Tag) || valueNode.Name.Equals(Dictionary_Tag))
                    {
                        XmlAttribute typeAtt = valueNode.Attributes["TypeName"];
                        if (typeAtt != null)
                        {
                            string typeString = typeAtt.Value.Replace('#', '`');
                            try
                            {
                                //entryValue = Activator.CreateInstance("mscorlib", typeString);
                                valueType = Type.GetType(typeString);
                            }
                            catch { }
                        }
                    }
                    else
                        valueType = TryToGetType(valueNode.Name);
                    if (valueType == null)
                        continue;
                    if (IsBasicType(valueType))
                    {
                        XmlAttribute att = valueNode.Attributes["value"];
                        if (att != null)
                        {
                            try
                            {
                                if (valueType.IsSubclassOf(typeof(Enum)))
                                    entryValue = Enum.Parse(valueType, att.Value);
                                else
                                    entryValue = Convert.ChangeType(att.Value, valueType);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        entryValue = Activator.CreateInstance(valueType, true);
                        GetValueFromXml(entryValue, valueNode);
                    }
                    dictObject.Add(entryKey, entryValue);
                }//if

            }//foreach   
        }

        void GetValueFromXml(object xmlObject, XmlNode objectNode)
        {
            GetValueFromXml(xmlObject, objectNode, null);
        }


        void GetValueFromXml(object xmlObject, XmlNode objectNode, string eleName)
        {
            if (objectNode == null || xmlObject == null)
                return;
            if (objectNode.Name.Equals(Collection_Tag)) //普通集合
            {
                GetCollectionFromXml(xmlObject as IList, objectNode);
            }
            else if (objectNode.Name.Equals(Dictionary_Tag)) //字典
            {
                GetDictionaryFromXml(xmlObject as IDictionary, objectNode);
            }
            else
            {
                Type objType = xmlObject.GetType();
                string tagName = eleName;
                if (string.IsNullOrEmpty(tagName))
                    tagName = objType.Name;
                if (objectNode.Name.ToLower().Equals(tagName.ToLower()))
                {
                    XmlAttributeCollection attCol = objectNode.Attributes;
                    foreach (FieldInfo field in objType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        object objValue = field.GetValue(xmlObject);
                        //判断字段是否为集合类型
                        if (IsCollection(field.FieldType))
                        {
                            foreach (XmlNode node in objectNode.ChildNodes)
                            {
                                if (node.Name.Equals(field.Name))
                                {
                                    //初始化集合
                                    object colObject = Activator.CreateInstance(field.FieldType, true);
                                    if (IsDictionary(field.FieldType))
                                    {
                                        GetDictionaryFromXml(colObject as IDictionary, node);
                                    }
                                    else
                                    {
                                        GetCollectionFromXml(colObject as IList, node);
                                    }
                                    field.SetValue(xmlObject, colObject);
                                }
                            }

                        }
                        else  //非集合类型
                        {
                            if (IsBasicType(field.FieldType)) //简单内置类型
                            {
                                XmlAttribute att = attCol[field.Name];
                                if (att != null)
                                {
                                    try
                                    {
                                        if (field.FieldType.IsSubclassOf(typeof(Enum)))
                                        {
                                            object enumValue = Enum.Parse(field.FieldType, att.Value);
                                            field.SetValue(xmlObject, enumValue);
                                        }
                                        else
                                        {
                                            //转换为目标类型
                                            object ovalue = Convert.ChangeType(att.Value, field.FieldType);
                                            field.SetValue(xmlObject, ovalue);
                                        }

                                    }
                                    catch { }
                                }
                            }
                            else  //自定义类型
                            {
                                foreach (XmlNode node in objectNode.ChildNodes)
                                {
                                    if (node.Name.Equals(field.Name))
                                    {
                                        object o = Activator.CreateInstance(field.FieldType, true);
                                        GetValueFromXml(o, node, field.Name);
                                        field.SetValue(xmlObject, o);
                                        break;
                                    }
                                }
                            }
                        }//else

                    }//foreach
                }
            }//else
        }

        /// <summary>
        /// 尝试获取指定的类型
        /// </summary>
        /// <param name="typeName">类型名称（简单名称，非全名）</param>
        /// <returns></returns>
        Type TryToGetType(string typeName)
        {
            Type t = null;
            t = Type.GetType(typeName, false);
            if (t != null)
                return t;
            foreach (Runtime r in runtimeList)
            {
                if (r.FullTypeName.EndsWith(typeName, StringComparison.CurrentCultureIgnoreCase))
                {
                    t = r.GetTypeByFullName(r.FullTypeName);
                    if (t != null)
                        return t;
                }
            }
            return t;
        }


        /// <summary>
        /// 从XML文件初始化对象
        /// </summary>
        /// <param name="xmlObject">要初始化的对象</param>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns></returns>
        public bool LoadFromFile(object xmlObject, string xmlPath)
        {
            if (xmlObject == null)
                return false;
            Type objType = xmlObject.GetType();
            try
            {
                this.xmldoc.Load(xmlPath);
                this.LoadRuntimeList();
                string xmltag = "//";
                if (IsDictionary(objType))
                    xmltag += Dictionary_Tag;
                else if (IsCollection(objType))
                    xmltag += Collection_Tag;
                else
                    xmltag += objType.Name;
                XmlNode rootNode = this.xmldoc.SelectSingleNode(xmltag);
                if (rootNode != null)
                {
                    this.GetValueFromXml(xmlObject, rootNode);
                }
            }
            catch (Exception ex)
            {
                string msg = "加载配置文件失败，发生异常：" + ex.Message;
                throw new Exception(msg);
            }
            return true;
        }

        /// <summary>
        /// 从XML字符串中初始化对象
        /// </summary>
        /// <param name="xmlObject">要初始化的对象</param>
        /// <param name="xmlString">XML字符串</param>
        /// <returns></returns>
        public bool LoadFromString(object xmlObject, string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
                return false;
            if (xmlObject == null)
                return false;
            Type objType = xmlObject.GetType();
            try
            {
                this.xmldoc.LoadXml(xmlString);
                this.LoadRuntimeList();
                string xmltag = "//";
                if (IsDictionary(objType))
                    xmltag += Dictionary_Tag;
                else if (IsCollection(objType))
                    xmltag += Collection_Tag;
                else
                    xmltag += objType.Name;
                XmlNode rootNode = this.xmldoc.SelectSingleNode(xmltag);
                if (rootNode != null)
                {
                    this.GetValueFromXml(xmlObject, rootNode);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 将对象序列化到XML文件中
        /// </summary>
        /// <param name="xmlObject">要序列化的对象</param>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns></returns>
        public bool SaveToFile(object xmlObject, string xmlPath)
        {
            if (xmlObject == null)
                return false;
            Type objType = xmlObject.GetType();
            try
            {
                this.xmldoc = new XmlDocument();    //重新构造一个XML文档
                this.runtimeList.Clear();
                XmlDeclaration xDeclare = this.xmldoc.CreateXmlDeclaration("1.0", "GB2312", null);
                this.xmldoc.AppendChild(xDeclare);
                this.SetValueToXml(xmlObject, null);
                SaveRuntimeList();
                this.xmldoc.Save(xmlPath);
                return true;
            }
            catch (XmlException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 将数据内容格式化为字符串，以便保存到数据库中
        /// </summary>
        /// <param name="xmlObject">要序列化的对象</param>
        /// <returns></returns>
        public string ConvertToString(object xmlObject)
        {
            try
            {
                this.xmldoc = new XmlDocument();    //重新构造一个XML文档
                this.runtimeList.Clear();
                XmlDeclaration xDeclare = this.xmldoc.CreateXmlDeclaration("1.0", "GB2312", null);
                this.xmldoc.AppendChild(xDeclare);
                this.SetValueToXml(xmlObject, null);
                this.SaveRuntimeList();
                return this.xmldoc.InnerXml;
            }
            catch (XmlException ex)
            {
                string msg = "转换XML数据格式失败，发生异常：" + ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 加载运行时动态库信息
        /// </summary>
        void LoadRuntimeList()
        {
            this.runtimeList.Clear();
            try
            {
                XmlNode runNode = this.xmldoc.SelectSingleNode("//RuntimeList");
                if (runNode != null)
                {
                    //获取下一级子节点的值
                    XmlNodeList nodeList = runNode.ChildNodes;
                    if (nodeList != null && nodeList.Count > 0)
                    {
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            XmlNode node = nodeList[i];
                            if (node.Name.ToLower().Equals("runtime"))
                            {
                                Runtime r = new Runtime();
                                XmlAttributeCollection attCol = node.Attributes;
                                for (int j = 0; j < attCol.Count; j++)
                                {
                                    XmlAttribute att = attCol[j];
                                    if (att.Name.ToLower().Equals("fulltypename"))
                                        r.FullTypeName = att.Value;
                                    if (att.Name.ToLower().Equals("assemblyname"))
                                        r.AssemblyName = att.Value;
                                }
                                this.runtimeList.Add(r);
                            }
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
            }
        }

        /// <summary>
        /// 保存运行时动态库信息
        /// </summary>
        /// <param name="refNode"></param>
        void SaveRuntimeList()
        {
            try
            {
                XmlElement listEle = this.xmldoc.CreateElement("RuntimeList");
                XmlNode refNode = this.xmldoc.ChildNodes[1].ChildNodes[0];
                this.xmldoc.ChildNodes[1].InsertBefore(listEle, refNode);
                foreach (Runtime r in runtimeList)
                {
                    XmlElement runtimeEle = this.xmldoc.CreateElement("Runtime");
                    runtimeEle.SetAttribute("FullTypeName", r.FullTypeName);
                    runtimeEle.SetAttribute("AssemblyName", r.AssemblyName);
                    listEle.AppendChild(runtimeEle);
                }
            }
            catch (XmlException)
            {
            }
        }


        /// <summary>
        /// 判断类型是否为基本数据类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        bool IsBasicType(Type type)
        {
            ////由于使用contains方法判断是否包含字符串，byte,int16,int32,int64已包含在sbyte,uint16,uint32,uint64中
            //string basictypes = "string|uint16|uint32|uint64|datetime|double|single|boolean|sbyte|char|decimal";
            //return basictypes.Contains(typeName.ToLower());

            if (type.IsValueType)
                return true;
            else if (type.Name.ToLower().Equals("string") || type.Name.ToLower().Equals("datetime"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断给定类型是否是集合类型
        /// </summary>
        /// <param name="type">给定类型</param>
        /// <returns></returns>
        bool IsCollection(Type type)
        {
            if (type == null)
                return false;
            Type[] inters = type.GetInterfaces();
            foreach (Type t in inters)
            {
                if (t.Name.Equals(typeof(ICollection).Name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断给定类型是否为字典类型
        /// </summary>
        /// <param name="type">给定类型</param>
        /// <returns></returns>
        bool IsDictionary(Type type)
        {
            if (type == null)
                return false;
            Type[] inters = type.GetInterfaces();
            foreach (Type t in inters)
            {
                if (t.Name.Equals(typeof(IDictionary).Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
