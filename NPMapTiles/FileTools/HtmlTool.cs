using System;
using System.IO;

namespace NPMapTiles.FileTools
{
    using System.Windows.Forms;

    using MapDataTools.Util;

    public class HtmlTool
    {
        public static void CreateHtmlDemo(double centerX,double centerY,double minX,double minY,double maxX,double maxY,int minZoom,int maxZoom,string path,string tilesName)
        {
               ///定义和html标记数目一致的数组
            string[] newContent = new string[5];
            string str = "" ;
            try 
            {
                ///创建StreamReader对象
                string htmlPath = System.Windows.Forms.Application.StartupPath + "//createHTML//template.html";
                using (StreamReader sr = new StreamReader(htmlPath)) 
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch(Exception err)
            {
            }

           ///根据上面新的内容生成html文件
            try
            {
                ///指定要生成的HTML文件
                string fname = path + "//temp.html";
       
                ///替换html模版文件里的标记为新的内容
                str = str.Replace("$centerX$", centerX.ToString());
                str = str.Replace("$centerY$", centerY.ToString());
                str = str.Replace("$minX$", minX.ToString());
                str = str.Replace("$minY$", minY.ToString());
                str = str.Replace("$maxX$", maxX.ToString());
                str = str.Replace("$maxY$", maxY.ToString());
                str = str.Replace("$minZoom$", minZoom.ToString());
                str = str.Replace("$maxZoom$", maxZoom.ToString());
                str = str.Replace("$zoom$", (maxZoom - minZoom).ToString());
                str = str.Replace("$path$", tilesName + "/");
               ///创建文件信息对象
                FileInfo finfo = new FileInfo(fname);
        
                ///以打开或者写入的形式创建文件流
                using(FileStream fs = finfo.OpenWrite())
                {
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter sw = new StreamWriter(fs,System.Text.Encoding.GetEncoding("utf-8"));
            
                    ///把新的内容写到创建的HTML页面中
                    sw.WriteLine(str);
                    sw.Flush();
                    sw.Close();
                }
                string jsPath = System.Windows.Forms.Application.StartupPath + "//createHTML//Init.js";
                if(File.Exists(jsPath))
                    File.Copy(jsPath, path + "//Init.js");
                CopyFolder(System.Windows.Forms.Application.StartupPath + "//createHTML//OpenLayers", path + "//OpenLayers//");
            }
            catch(Exception err)
            { 
            }
        }
        public static void CreateHtmlDemo(WorkInfo workInfo, int minZoom, int maxZoom)
        {
            ///定义和html标记数目一致的数组
            string[] newContent = new string[5];
            string str = "";
            string imgType = "png";
            try
            {
                ///创建StreamReader对象
                string htmlPath = System.Windows.Forms.Application.StartupPath + "//createHTML//template.html";
                if (workInfo.mapType == MapType.Tiandi||workInfo.mapType==MapType.TiandiImage)
                {
                    htmlPath = System.Windows.Forms.Application.StartupPath + "//createHTML//tempTD.html";
                }
                if (workInfo.mapType == MapType.Gaode||workInfo.mapType==MapType.GaodeImage)
                {
                    htmlPath = System.Windows.Forms.Application.StartupPath + "//createHTML//tempGaoDe.html";
                }
                if (workInfo.mapType == MapType.GaodeImage || workInfo.mapType == MapType.TiandiImage || workInfo.mapType == MapType.GoogleImage)
                {
                    imgType = "jpg";
                }
                if (workInfo.mapType == MapType.Baidu || workInfo.mapType == MapType.BaiduImageTile)
                {
                    htmlPath = System.Windows.Forms.Application.StartupPath + "//createHTML//tempBaidu.html";
                }
                if (string.IsNullOrEmpty(htmlPath))
                {
                    MessageBox.Show("");
                    return;
                }
                using (StreamReader sr = new StreamReader(htmlPath))
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception err)
            {
            }

            ///根据上面新的内容生成html文件
            try
            {
                ///指定要生成的HTML文件
                string fname = workInfo.filePath + "//temp.html";

                ///替换html模版文件里的标记为新的内容
                str = str.Replace("$centerX$", ((workInfo.minX+workInfo.maxX)/2).ToString());
                str = str.Replace("$centerY$", ((workInfo.minY + workInfo.maxY) / 2).ToString());
                str = str.Replace("$minX$", workInfo.minX.ToString());
                str = str.Replace("$minY$", workInfo.minY.ToString());
                str = str.Replace("$maxX$", workInfo.maxX.ToString());
                str = str.Replace("$maxY$", workInfo.maxY.ToString());
                str = str.Replace("$minZoom$", minZoom.ToString());
                str = str.Replace("$maxZoom$", maxZoom.ToString());
                str = str.Replace("$zoom$", (maxZoom - minZoom).ToString());
                str = str.Replace("$type$", imgType);
                string type = "s";
                if (workInfo.mapType == MapType.Tiandi || workInfo.mapType == MapType.TiandiImage)
                {
                    type = "Vector";
                }
                if (workInfo.mapType == MapType.Baidu)
                {
                    str = str.Replace("$path$", type + "/${z}/${x}/${y}.png");
                }
                else if (workInfo.mapType == MapType.BaiduImageTile)
                {
                    str = str.Replace("$path$", type + "/${z}/${x}/${y}.jpg");
                }
                else if (workInfo.mapType == MapType.OpenStreetMap)
                {
                    str = str.Replace("$path$", type + "/${z}/${x}/${y}.png");
                }
                else
                {
                    str = str.Replace("$path$", type + "/");
                }
                ///创建文件信息对象
                FileInfo finfo = new FileInfo(fname);

                ///以打开或者写入的形式创建文件流
                using (FileStream fs = finfo.OpenWrite())
                {
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("utf-8"));

                    ///把新的内容写到创建的HTML页面中
                    sw.WriteLine(str);
                    sw.Flush();
                    sw.Close();
                }
                string jsPath = System.Windows.Forms.Application.StartupPath + "//createHTML//init.js";
                if (File.Exists(jsPath))
                    File.Copy(jsPath, workInfo.filePath + "//init.js");
                CopyFolder(System.Windows.Forms.Application.StartupPath + "//createHTML//Netposa", workInfo.filePath + "//Netposa//");
            }
            catch (Exception err)
            {
            }
        }
        private static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
                CopyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");

            // 文件
            foreach (string file in Directory.GetFiles(from))
                File.Copy(file, to + Path.GetFileName(file), true);
        }
    }
}
