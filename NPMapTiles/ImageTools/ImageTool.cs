using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NPMapTiles.ImageTools
{
    using MapDataTools.Tile;
    using MapDataTools.Util;

    public class ImageTool
    {
        /// <summary>
        /// 自动拼接图片
        /// </summary>
        /// <param name="dicImage"></param>
        /// <param name="path">保存路径</param>
        public static void CreateImage(Dictionary<string, List<Image>> dicImage, string path,Handlers.ProcessNotifyHandler processNotifyHandler,RowColumns rc)
        {
            try
            {
                //图片列表
                if (dicImage.Count <= 0)
                    return;
                int width = 0;
                int height = 0;
                //计算总长度
                List<Image> temp = null;
                foreach (var i in dicImage)
                {
                    if (i.Value.Count == 0)
                        width += 256;
                    else
                        width += i.Value[0].Width;
                    temp = i.Value;
                }
                foreach (Image image in temp)
                {
                    height += image.Height;
                }
                //构造最终的图片白板
                Bitmap tableChartImage = new Bitmap(width, height);
                Graphics graph = Graphics.FromImage(tableChartImage);
                //初始化这个大图
                graph.DrawImage(tableChartImage, width, height);

                int currentWidth = 0;
                int k = 0;
                int count = (rc.maxRow - rc.minRow + 1) * (rc.maxCol - rc.minCol + 1);
                foreach (var i in dicImage)
                {
                    //拼图
                    Image currentImage = null;
                    int currentHeight = 0;
                    foreach (Image image in i.Value)
                    {
                        graph.DrawImage(image, currentWidth, currentHeight);
                        //拼接改图后，当前宽度
                        currentHeight += image.Height;
                        currentImage = image;
                        k++;
                        string msg = "提示：已处理第" + rc.zoom.ToString() + "级," + k.ToString() + "条,共" + count.ToString() + "条";
                        if (processNotifyHandler != null)
                        {
                            processNotifyHandler(msg, (k * 100) / count);
                        }
                    }
                    currentWidth += currentImage.Width;
                }
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                tableChartImage.Save(path + "\\temp.tif", System.Drawing.Imaging.ImageFormat.Tiff);
                graph.Dispose();
                tableChartImage.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 自动拼接图片
        /// </summary>
        /// <param name="dicImage">图片字典，以图片的行号为key值，每个键值对应当前行号中的一行的图片列表</param>
        /// <param name="path">保存路径</param>
        public static void CreateImage(Dictionary<string, List<Image>> dicImage, string path)
        {
            //图片列表
            if (dicImage.Count <= 0)
                return;
            int width = 0;
            int height = 0;
            //计算总长度
            List<Image> temp = null;
            foreach (var i in dicImage)
            {
                if (i.Value.Count == 0)
                    width += 256;
                else
                    width += i.Value[0].Width;
                temp = i.Value;
            }
            foreach (Image image in temp)
            {
                height += image.Height;
            }
            //构造最终的图片白板
            Bitmap tableChartImage = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(tableChartImage);
            //初始化这个大图
            graph.DrawImage(tableChartImage, width, height);

            int currentWidth = 0;
            foreach (var i in dicImage)
            {
                //拼图
                Image currentImage = null;
                int currentHeight = 0;
                foreach (Image image in i.Value)
                {
                    graph.DrawImage(image, currentWidth, currentHeight);
                    //拼接改图后，当前宽度
                    currentHeight += image.Height;
                    currentImage = image;
                }
                currentWidth += currentImage.Width;
            }
            try
            {
                tableChartImage.Save(path + "\\temp.tif", System.Drawing.Imaging.ImageFormat.Tiff);
            }
            catch (Exception ex)
            {
            }
        }
        public static Dictionary<string, List<Image>> GetImagesFromPath(string path)
        {
            Dictionary<string, List<Image>> dic = new Dictionary<string, List<Image>>();
            if (!Directory.Exists(path))
            {
                return dic;
            }
            DirectoryInfo theFolder = new DirectoryInfo(path);
            foreach (DirectoryInfo nextFloder in theFolder.GetDirectories())
            {
                List<Image> images = new List<Image>();
                foreach (FileInfo file in nextFloder.GetFiles())
                {
                    Image image = Image.FromFile(file.FullName);
                    images.Add(image);
                }
                dic.Add(nextFloder.Name, images);
            }
            return dic;
        }


    }
}
