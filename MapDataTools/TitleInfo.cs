namespace MapDataTools
{
    using System;

    /// <summary>
    /// 切片信息，包括行列号起止数值信息
    /// </summary>
    public class TitlesInfo
    {
        public int minRow = 0;
        public int maxRow = 0;
        public int minCol = 0;
        public int maxCol = 0;
    }
    public class Extent
    {
        public double minX = 0;
        public double maxX = 0;
        public double minY = 0;
        public double maxY = 0;

        public static Extent Resolve(string extentStr)
        {
            if (string.IsNullOrEmpty(extentStr))
            {
                return new Extent();
            }
            var e = extentStr.Replace(' ', ',').Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            return new Extent()
                       {
                           minX = double.Parse(e[0]),
                           minY = double.Parse(e[1]),
                           maxX = double.Parse(e[2]),
                           maxY = double.Parse(e[3])
                       };
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", this.minX, this.minY, this.maxX, this.maxY);
        }
    }
}
