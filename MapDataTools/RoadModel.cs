using System.Collections.Generic;
//using System.Linq;

namespace MapDataTools
{
    using System.Linq;

    public class RoadModel
    {
        public string name = "";

        public string type = "";

        public double width = 0;

        public List<string> paths = new List<string>();

        public string getWgsPath(int index)
        {
            if (index >= this.paths.Count)
            {
                return string.Empty;
            }
            string[] wgspaths = paths[index].Replace(",", "|").Split(';').ToList().Select(
                m =>
                    {
                        var lonlat = CoordHelper.Gcj2Wgs(double.Parse(m.Split('|')[0]), double.Parse(m.Split('|')[1]));
                        return string.Format("{0}|{1}", lonlat.lon, lonlat.lat);
                    }).ToArray();
            return string.Join(";", wgspaths);
        }


        public bool IsHeightWay
        {
            get
            {
                return this.type.Trim().Equals("高速公路");
            }
        }


        public double getSpeed()
        {
            if (string.IsNullOrEmpty(this.type))
            {
                return 0;
            }

            switch (this.type.Trim())
            {
                case "高速公路":
                    return 100;
                case "主要道路（城市主干道）":
                    return 70;
                case "省道":
                    return 80;
                case "次要道路（城市次干道）":
                    return 50;
                case "国道":
                    return 60;
                case "县道":
                    return 40;
                case "城市环路/城市快速路":
                    return 50;
                case "非导航道路":
                    return 18;
                case "区县内部道路":
                    return 20;
                case "乡村道路":
                    return 18;
                case "一般道路":
                    return 50;
                default:
                    return 0;
            }
        }

        public int getNpLevel()
        {
            if (string.IsNullOrEmpty(this.type))
            {
                return 0;
            }
            switch (this.type.Trim())
            {
                case "高速公路":
                    return 1;
                case "主要道路（城市主干道）":
                case "省道":
                    return 2;
                case "次要道路（城市次干道）":
                case "国道":
                case "县道":
                case "一般道路":
                    return 3;
                case "城市环路/城市快速路":
                case "非导航道路":
                case "区县内部道路":
                case "乡村道路":
                    return 4;
                default:
                    return int.MaxValue;
            }
        }
    }

    public class RoadCrossModel
    {
        public string id = "";

        public string first_name = "";

        public string second_name = "";

        public string first_id = "";

        public string second_id = "";

        public double x = 0;

        public double y = 0;

        public double wgs_x;

        public double wgs_y;
    }
}
