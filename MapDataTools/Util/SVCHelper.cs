using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapDataTools
{
    public static class SVCHelper
    {
        /// <summary>
        /// 将datatable中的数据保存到csv中
        /// </summary>
        /// <param name="dt">数据来源</param>
        /// <param name="savaPath">保存的路径</param>
        /// <param name="strName">保存文件的名称</param>
        public static bool ExportToSvc(System.Data.DataTable dt, string savaPath)
        {
            //string strPath = Path.GetTempPath() + strName + ".csv";//保存到本项目文件夹下

            if (File.Exists(savaPath))
            {
                File.Delete(savaPath);
            }
            //先打印标头
            StringBuilder strColu = new StringBuilder();
            StringBuilder strValue = new StringBuilder();
            int i = 0;
            try
            {
                StreamWriter sw = new StreamWriter(new FileStream(savaPath, FileMode.CreateNew), Encoding.GetEncoding("GB2312"));
                for (i = 0; i <= dt.Columns.Count - 1; i++)
                {
                    strColu.Append(dt.Columns[i].ColumnName);
                    strColu.Append(",");
                }
                strColu.Remove(strColu.Length - 1, 1);//移出掉最后一个,字符
                sw.WriteLine(strColu);
                foreach (DataRow dr in dt.Rows)
                {
                    strValue.Remove(0, strValue.Length);//移出
                    for (i = 0; i <= dt.Columns.Count - 1; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                        {
                            value = "\"" + value + "\"";
                        }
                        strValue.Append(value);
                        strValue.Append(",");

                        //strValue.Append(dr[i].ToString());
                        //strValue.Append(",");
                    }
                    strValue.Remove(strValue.Length - 1, 1);//移出掉最后一个,字符
                    sw.WriteLine(strValue);
                }
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ReadCsvFile(string filePath)
        {
            return AsposeCellsHelper.ExportToDataTable(filePath, true);

            //StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open), Encoding.GetEncoding("GB2312"));
            //String line = sr.ReadLine();
            //string[] cloumns = line.Split(',');
            //DataTable dataTable = new DataTable();
            //DataColumn dc = null;
            //for (int i = 0; i < cloumns.Length; i++)
            //{
            //    dc = dataTable.Columns.Add(cloumns[i], Type.GetType("System.String"));
            //}
            //while ((line = sr.ReadLine()) != null)
            //{
            //    DataRow row = dataTable.NewRow();
            //    List<string> list = getRowValue(line, cloumns.Length);
            //    row.ItemArray = list.ToArray();
            //    dataTable.Rows.Add(row);
            //}
            //sr.Dispose();
            //return dataTable;
        }

        private static List<string> getRowValue(string line, int count)
        {
            List<string> list = new List<string>();
            line = line.Replace("\"\"", "\"");
            string[] values = line.Split(',');
            if (values.Length == count)
            {
                return values.ToList();
            }
            else
            {
                int n = 0, beign = 0;
                bool flag = false;
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].IndexOf("\"") == -1 || (flag == false && values[i][0] != '\"'))
                    {
                        list.Add(values[i]);
                        continue;
                    }
                    n = 0;
                    foreach (char ch in values[i])
                    {
                        if (ch == '\"')
                        {
                            n++;
                        }
                    }
                    if (n % 2 == 0)
                    {
                        list.Add(values[i]);
                        continue;
                    }
                    flag = true;
                    beign = i;
                    i++;
                    for (i = beign + 1; i < values.Length; i++)
                    {
                        foreach (char ch in values[i])
                        {
                            if (ch == '\"')
                            {
                                n++;
                            }
                        }
                        if (values[i][values[i].Length - 1] == '\"' && n % 2 == 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (; beign <= i; beign++)
                            {
                                sb.Append(values[beign]);
                                if (beign != i)
                                {
                                    sb.Append(",");
                                }
                            }
                            list.Add(sb.ToString().Replace("\"", ""));
                            break;
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 将datatable中的数据保存到csv中
        /// </summary>
        public static bool AddToSvc(System.Data.DataRow row, string savaPath)
        {
            //string strPath = Path.GetTempPath() + strName + ".csv";//保存到本项目文件夹下

            if (!File.Exists(savaPath))
            {
                return false;
            }
            //先打印标头
            StringBuilder strColu = new StringBuilder();
            StringBuilder strValue = new StringBuilder();
            int i = 0;
            try
            {
                StreamWriter sw = new StreamWriter(new FileStream(savaPath, FileMode.Append), Encoding.GetEncoding("GB2312"));
                for (i = 0; i <= row.ItemArray.Length - 1; i++)
                {
                    string value = row[i].ToString();
                    if (value.Contains(","))
                    {
                        value = "\"" + value + "\"";
                    }
                    strValue.Append(value);
                    strValue.Append(",");
                }
                strValue.Remove(strValue.Length - 1, 1);//移出掉最后一个,字符
                sw.WriteLine(strValue);
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 查看文件 中有无指定字段
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="name">列名</param>
        /// <returns>true或false</returns>
        public static bool IsContainsCloumnName(string filePath, string name)
        {
            StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open));
            String line = sr.ReadLine();
            string[] cloumns = line.Split(',');
            for (int i = 0; i < cloumns.Length; i++)
            {
                if (cloumns[i].ToUpper() == name.ToUpper())
                {
                    sr.Dispose();
                    return true;
                }
            }
            sr.Dispose();
            return false;
        }
    }
}
