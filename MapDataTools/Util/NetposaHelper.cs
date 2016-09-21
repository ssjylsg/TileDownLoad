using System;

namespace MapDataTools
{
    using System.IO;

    public static class NetposaHelper
    {
        private static string RemoveChar(string str, char c)
        {
            return str.TrimStart(c);
        }
        public static void InputImage(int zoom, int row, int col, string imagePath, string newDirPath)
        {
            int size = 128;
            String l = "0" + zoom;
            int lLength = l.Length;
            if (lLength > 2)
            {
                l = l.Substring(lLength - 2);
            }
            l = "L" + l;

            int rGroup = size * (row / size);
            String r = rGroup.ToString("X");
            int rLength = r.Length;
            if (rLength < 4)
            {
                for (int i = 0; i < 4 - rLength; i++)
                {
                    r = "0" + r;
                }
            }
            r = "R" + r;
            int cGroup = size * (col / size);
            String c = cGroup.ToString("X");
            int cLength = c.Length;
            if (cLength < 4)
            {
                for (int i = 0; i < 4 - cLength; i++)
                {
                    c = "0" + c;
                }
            }
            c = "C" + c;
            if (!Directory.Exists( newDirPath + "/" + l))
            {
                Directory.CreateDirectory(newDirPath + "/" + l);
            }
            String bundleBase = newDirPath + "/" + l + "/" + r + c;
            String bundlxFileName = bundleBase + ".npslx";
            String bundleFileName = bundleBase + ".npsle";
            FileStream file = null;
            FileStream fileIndex = null;
            if (!File.Exists(bundleFileName))
            {
                file = File.Create(bundleFileName);
                file.Write(new Byte[16], 0, 16);
            }
            else
            {
                file = File.OpenWrite(bundleFileName);
            }
            if (!File.Exists(bundlxFileName))
            {
                fileIndex = File.Create(bundlxFileName);
                fileIndex.Write(new Byte[16], 0, 16);
            }
            else
            {
                fileIndex = File.OpenWrite(bundlxFileName);
            }
            int index = size * (col - cGroup) + (row - rGroup);
            FileStream imageStream = File.OpenRead(imagePath);
            byte[] data = new byte[imageStream.Length];
            imageStream.Read(data, 0, data.Length);
            int l1 = data.Length / 16777216;
            int l2 = (data.Length % 16777216) / 65536;
            int l3 = ((data.Length % 16777216) % 65536) / 256;
            int l4 = ((data.Length % 16777216) % 65536) % 256;
            byte[] le = new byte[4];
            le[0] = (byte)l4;
            le[1] = (byte)l3;
            le[2] = (byte)l2;
            le[3] = (byte)l1;
            long currentLength = file.Length;
            file.Seek(currentLength, SeekOrigin.Begin);
            file.Write(le, 0, 4);
            file.Write(data, 0, data.Length);
            file.Flush();
            file.Dispose();

            long lindex0 = (currentLength / 4294967296);
            long lindex1 = (currentLength % 4294967296) / 16777216;
            long lindex2 = (currentLength % 4294967296) % 16777216 / 65536;
            long lindex3 = (currentLength % 4294967296) % 16777216 % 65536 / 256;
            long lindex4 = (currentLength % 4294967296) % 16777216 % 65536 % 256;
            byte[] indexBytes = new byte[5];
            indexBytes[0] = (byte)lindex4;
            indexBytes[1] = (byte)lindex3;
            indexBytes[2] = (byte)lindex2;
            indexBytes[3] = (byte)lindex1;
            indexBytes[4] = (byte)lindex0;
            fileIndex.Seek(16 + 5 * index, SeekOrigin.Begin);
            fileIndex.Write(indexBytes, 0, 5);
            fileIndex.Flush();
            fileIndex.Dispose();
        }
        public static void CovertTiles(string oldDirPath, string newDirPath)
       {
           string[] ds = Directory.GetDirectories(oldDirPath);
           for (int i = 0; i < ds.Length; i++)
           {
               string[] rs = Directory.GetDirectories(ds[i]);
               DirectoryInfo d = new DirectoryInfo(ds[i]);
               string zstring = d.Name;
               int zoom = 0;
               if (int.TryParse(zstring.Replace("L", ""), out zoom))
               {
                   for (int j = 0; j < rs.Length; j++)
                   {
                       string[] cs = Directory.GetFiles(rs[j]);
                       DirectoryInfo r = new DirectoryInfo(rs[j]);
                       string rstring = r.Name;
                       int row = 0;
                       if (rstring.Contains("R"))
                       {
                           rstring = RemoveChar(rstring, 'R');
                           rstring = RemoveChar(rstring, '0');
                           row = System.Convert.ToInt32(rstring, 16);
                       }
                       else
                       {
                           int.TryParse(rstring, out row);
                       }
                       for (int k = 0; k < cs.Length; k++)
                       {
                           FileInfo file = new FileInfo(cs[k]);
                           string cstring = file.Name.Split('.')[0];
                           int col = 0;
                           if (cstring.Contains("C"))
                           {
                               cstring = RemoveChar(cstring, 'C');
                               cstring = RemoveChar(cstring, '0');
                               col = System.Convert.ToInt32(cstring, 16);
                           }
                           else
                           {
                               int.TryParse(cstring, out col);
                           }
                           InputImage(zoom, row, col, cs[k], newDirPath);
                       }
                   }
               }
           }
       }
    }
}
