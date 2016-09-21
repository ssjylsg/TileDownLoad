using System;
using System.Collections.Generic;
using System.Linq;

namespace MapDataTools
{
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Net;

    using OSGeo.OGR;

    public class HotCity
    {
        private string chinaCode = "1";

        private string queryUrl =
            "http://api.map.baidu.com/shangquan/forward/?qt=sub_area_list&ext=1&level=1&areacode={0}&business_flag=0";

        private string bussinessUrl =
            "http://api.map.baidu.com/shangquan/forward/?qt=sub_area_list&ext=1&level=1&areacode={0}&business_flag=1";

        public void DownLoad()
        {
            var dataTable = new DataTable();
            var bussinessTable = new DataTable();

            bussinessTable.Columns.AddRange(
                new DataColumn[]
                    {
                        new DataColumn("city_code"), new DataColumn("geo"), new DataColumn("area_code"),
                        new DataColumn("area_name"), new DataColumn("PATH"), new DataColumn("business_type"),
                    });

            dataTable.Columns.AddRange(
                new DataColumn[]
                    {
                        new DataColumn("area_code"), new DataColumn("area_type"), new DataColumn("X"),
                        new DataColumn("area_name"), new DataColumn("parent_code"), new DataColumn("Y"),
                    });

            var chinaRow = dataTable.NewRow();
            chinaRow["parent_code"] = 0;
            chinaRow["area_code"] = 1;
            chinaRow["area_name"] = "中国";
            chinaRow["area_type"] = 0;
            // chinaRow["geo"] = "116.413554,39.911013";
            chinaRow["X"] = 116.413554;
            chinaRow["Y"] = 39.911013;
            dataTable.Rows.Add(chinaRow);

            getSubItem(this.chinaCode, dataTable, bussinessTable);

            AsposeCellsHelper.ExportToExcel(
                dataTable,
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "行政区.xls"));
            AsposeCellsHelper.ExportToExcel(
                bussinessTable,
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "商业圈.xls"));
        }

        private void FillBaiduCode(DataTable dataTable)
        {
            //var list = CityConfig.GetInstance().Countryconfig.countries;
            //foreach (DataRow row in dataTable.Rows)
            //{
            //    var name = row["area_name"].ToString();
            //    var code = row["area_code"].ToString();
            //    var p = list.Find(m => name.Contains(m.name) || m.name.Contains(name));
            //    if (p != null)
            //    {
            //        p.baiduCode = code;
            //    }
            //    else
            //    {
            //        var c = list.Find(m => m.cities.Find(n => n.name.Contains(name) || name.Contains(n.name)) != null);
            //        c.cities.Find(n => n.name.Contains(name) || name.Contains(n.name));
            //    }
            //}
        }

        private void FillBaiduCode(List<City> cities)
        {
            var list = CityConfig.GetInstance().Countryconfig.countries;
            foreach (Province province in list)
            {
                var city = cities.Find(m => m.CityName.Contains(province.name) || province.name.Contains(m.CityName));
                if (city != null)
                {
                    province.baiduCode = city.Code;
                }
            }
        }
        private void getSubItem(string code, DataTable dataTable, DataTable bussinessTable)
        {
            var privicenCity =
                this.DownLoadJson(string.Format(this.queryUrl, code))["content"] as Dictionary<string, object>;
            if (!privicenCity.ContainsKey("sub"))
            {
                return;
            }
            var privicenList = privicenCity["sub"] as System.Collections.ArrayList;
            var parentCode = privicenCity["area_code"].ToString();
            foreach (var o in privicenList)
            {
                var p = o as Dictionary<string, object>;
                var row = dataTable.NewRow();
                var area_code = p["area_code"].ToString();
                row["area_code"] = area_code;
                row["area_code"] = p["area_code"];
                row["area_name"] = p["area_name"];
                row["area_type"] = p["area_type"];
                Console.WriteLine(p["area_name"]);
                row["parent_code"] = parentCode;
                var coord = getBaiduLonlat(p["geo"].ToString());
                row["X"] = coord.lon;
                row["Y"] = coord.lat;
                dataTable.Rows.Add(row);
                if (p["area_type"].ToString() != "3")
                {
                    this.getSubItem(area_code, dataTable, bussinessTable);
                }
                else
                {
                    var bussiness = this.DownLoadJson(string.Format(this.bussinessUrl, area_code));
                    if (!bussiness.ContainsKey("content"))
                    {
                        continue;
                    }
                    var parentBussiness = bussiness["content"] as Dictionary<string, object>;
                    if (!parentBussiness.ContainsKey("sub"))
                    {
                        continue;
                    }
                    var sublist = parentBussiness["sub"] as ArrayList;
                    if (sublist == null)
                    {
                        continue;
                    }
                    foreach (var bu in sublist)
                    {
                        var subBussiness = bu as Dictionary<string, object>;
                        var bussinessRow = bussinessTable.NewRow();
                        bussinessRow["city_code"] = area_code;
                        bussinessRow["area_code"] = subBussiness["area_code"];
                        bussinessRow["area_name"] = subBussiness["area_name"];
                        Console.WriteLine(bussinessRow["area_name"]);
                        if (subBussiness.ContainsKey("business_geo"))
                        {
                            bussinessRow["PATH"] = this.PathConvert(subBussiness["business_geo"].ToString());
                        }
                        bussinessRow["geo"] = subBussiness["geo"];
                        if (subBussiness.ContainsKey("business_type")) bussinessRow["business_type"] = subBussiness["business_type"];
                        bussinessTable.Rows.Add(bussinessRow);
                    }
                }
            }
        }

        private string PathConvert(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            var paths = path.Split(';').ToList().Select(
                m =>
                    {
                       // var temp = m.Split(',');
                        var lonlat = this.getBaiduPixLonlat(m);
                           // CoordHelper.WebMercator2lonLat(new Coord(double.Parse(temp[0]), double.Parse(temp[1])));
                        return CoordHelper.BdDecrypt(lonlat.lat, lonlat.lon).ToString();

                    }).ToArray();
            return string.Join(";", paths);
        }

        private Coord getBaiduLonlat(string i)
        {
            //if (string.IsNullOrEmpty(i))
            //{
            //    return new Coord(0, 0);
            //}
            //i = i.Split('|')[2];
            //i = i.Substring(0, i.Length - 1);
            //var lon = double.Parse(i.Split(',')[0]);
            //var lat = double.Parse(i.Split(',')[1]);
            //var lonlat = CoordHelper.WebMercator2lonLat(new Coord(lon, lat));
            i = i.Split('|')[2];
            i = i.Substring(0, i.Length - 1);
            var lonlat = this.getBaiduPixLonlat(i);
            lonlat = CoordHelper.BdDecrypt(lonlat.lat, lonlat.lon);
            return lonlat;
            //  return CoordHelper.gcj2wgs(lonlat.lon, lonlat.lat);
        }
        //private  Coord Fb(Coord a) {
        //    a.lng = a.lon;
        //    a.lat = a.lat;
        //    if (!a || 180 < a.lng || -180 > a.lng || 90 < a.lat || -90 > a.lat)
        //        return new H(0, 0);
        //    var b, c;
        //    a.lng = this.JD(a.lng, -180, 180);
        //    a.lat = this.ND(a.lat, -74, 74);
        //    b = new H(a.lng, a.lat);
        //    for (var d = 0; d < this.Au.length; d++)
        //        if (b.lat >= this.Au[d]) {
        //            c = this.iG[d];
        //            break
        //        }
        //    if (!c)
        //        for (d = this.Au.length - 1; 0 <= d; d--)
        //            if (b.lat <= -this.Au[d]) {
        //                c = this.iG[d];
        //                break
        //            }
        //    a = this.gK(a, c);
        //    a = new H(a.lng, a.lat)
        //    return new NPMapLib.Geometry.Point(a.lng, a.lat);
        //}
        private Coord getBaiduPixLonlat(string i)
        {
            var zp = new double[] { 1.289059486E7, 8362377.87, 5591021, 3481989.83, 1678043.12, 0 };
            var St = new List<double[]>()
                         {
                             new double[]
                                 {
                                     1.410526172116255E-8, 8.98305509648872E-6, -1.9939833816331,
                                     200.9824383106796, -187.2403703815547, 91.6087516669843,
                                     -23.38765649603339, 2.57121317296198, -0.03801003308653,
                                     1.73379812E7
                                 },
                             new double[]
                                 {
                                     -7.435856389565537E-9, 8.983055097726239E-6, -0.78625201886289,
                                     96.32687599759846, -1.85204757529826, -59.36935905485877,
                                     47.40033549296737, -16.50741931063887, 2.28786674699375,
                                     1.026014486E7
                                 },
                             new double[]
                                 {
                                     -3.030883460898826E-8, 8.98305509983578E-6, 0.30071316287616,
                                     59.74293618442277, 7.357984074871, -25.38371002664745,
                                     13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37
                                 },
                             new double[]
                                 {
                                     -1.981981304930552E-8, 8.983055099779535E-6, 0.03278182852591,
                                     40.31678527705744, 0.65659298677277, -4.44255534477492,
                                     0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06
                                 },
                             new double[]
                                 {
                                     3.09191371068437E-9, 8.983055096812155E-6, 6.995724062E-5,
                                     23.10934304144901, -2.3663490511E-4, -0.6321817810242,
                                     -0.00663494467273, 0.03430082397953, -0.00466043876332, 2555164.4
                                 },
                             new double[]
                                 {
                                     2.890871144776878E-9, 8.983055095805407E-6, -3.068298E-8,
                                     7.47137025468032, -3.53937994E-6, -0.02145144861037, -1.234426596E-5,
                                     1.0322952773E-4, -3.23890364E-6, 826088.5
                                 }
                         };

            if (string.IsNullOrEmpty(i))
            {
                return null;
            }

            var lon = double.Parse(i.Split(',')[0]);
            var lat = double.Parse(i.Split(',')[1]);

            var a = new LonLat() { Lon = lon, Lat = lat };
            var b = new LonLat { Lon = Math.Abs(lon), Lat = Math.Abs(lat) };
            double[] c = new double[0];
            for (var d = 0; d < zp.Length; d++)
                if (b.Lat >= zp[d])
                {
                    c = St[d];
                    break;
                }
            a = this.zr(a, c);
            return new Coord(a.Lon, a.Lat);
        }

        private LonLat zr(LonLat a, double[] b)
        {
            var c = b[0] + b[1] * Math.Abs(a.Lon);
            var d = Math.Abs(a.Lat) / b[9];
            d = b[2] + b[3] * d + b[4] * d * d + b[5] * d * d * d + b[6] * d * d * d * d + b[7] * d * d * d * d * d
                + b[8] * d * d * d * d * d * d;
            c = c * (0 > a.Lon ? -1 : 1);
            d = d * (0 > a.Lat ? -1 : 1);
            return new LonLat(c, d);
        }

        class LonLat
        {
            public LonLat(){}

            public LonLat(double lon, double lat)
            {
                this.Lon = lon;
                this.Lat = lat;
            }
            public double Lon { get; set; }
            public double Lat { get; set; }
        }
        private Dictionary<string, object> DownLoadJson(string url)
        {
            using (var client = new System.Net.WebClient())
            {
                string result = client.DownloadString(url);
                return
                    new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(
                        result);
            }
        }

        public object ResolvGeom(string geom)
        {
            var jsCode =
                new StreamReader(
                    new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "baidu.js"), FileMode.Open))
                    .ReadToEnd();
            return ExecuteScript(string.Format("test('{0}')", geom), jsCode);
        }

        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private object ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            return scriptControl.Eval(sExpression);

        }

        private class City
        {
            public string CityName { get; set; }

            public string Center { get; set; }

            public string Bound { get; set; }

            public string Code { get; set; }

            public string GetGJC02Bound()
            {
                if (string.IsNullOrEmpty(this.Bound))
                {
                    return string.Empty;
                }
               var points = this.Bound.Split(';').Select(
                   m =>
                       {
                           var x = double.Parse(m.Split(',')[0]);
                           var y = double.Parse(m.Split(',')[1]);

                           // 百度坐标转为火星坐标
                           return CoordHelper.BdDecrypt(y, x).ToString();
                       }).ToArray();
                return String.Join(";",points);
            }
            /// <summary>
            /// 获取中心点【火星坐标】
            /// </summary>
            /// <returns></returns>
            public Coord getGJC02Center()
            {
                if (string.IsNullOrEmpty(this.Center))
                {
                    return null;
                }
                var m = this.Center.Split('|')[0];
                var x = double.Parse(m.Split(',')[0]);
                var y = double.Parse(m.Split(',')[1]);

                // 百度坐标转为火星坐标
                return CoordHelper.BdDecrypt(y, x);
            }
 
        }

        public void ReadCityBound(string path,string excelPath)
        {
            var result = new StreamReader(new FileStream(path, FileMode.Open)).ReadToEnd();
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = int.MaxValue;
            var cityList = javaScriptSerializer.Deserialize<Dictionary<string, object>>(result);

            List<City> cities =  new List<City>();
            var municipalities = cityList["municipalities"] as ArrayList;
            var other = cityList["other"] as ArrayList;
            var provinces = cityList["provinces"] as ArrayList;
            AddCity(cities, municipalities);
            AddCity(cities, other);
            AddCity(cities, provinces);
            CityToExcel(cities, excelPath);
        }

        private void CityToExcel(List<City> cities, string fileName)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
                                           {
                                               new DataColumn("name"),
                                               new DataColumn("X"),
                                               new DataColumn("Y"),
                                               new DataColumn("PATH"), 
                                           });
            foreach (City city in cities)
            {
                var row = dataTable.NewRow();
                row["name"] = city.CityName;
                row["PATH"] = city.GetGJC02Bound();
                var center = city.getGJC02Center();
                row["X"] = center.lon;
                row["Y"] = center.lat;

                dataTable.Rows.Add(row);
            }
            ShpFileHelper.SaveShpFile(dataTable, fileName, wkbGeometryType.wkbPolygon,ProjectConvert.GCJ_WGS);
           //SVCHelper.ExportToSvc(dataTable, fileName);
            //AsposeCellsHelper.ExportToExcel(dataTable,fileName);
        }

        private void AddCity(List<City> cities, ArrayList list)
        {
            foreach (var p in list)
            {
                var o = p as Dictionary<string, object>;
                cities.Add(
                    new City()
                        {
                            Bound = o["b"].ToString(),
                            Center = o["g"].ToString().Split('|')[0],
                            CityName = o["n"].ToString()
                        });
                if (o.ContainsKey("cities"))
                {
                    this.AddCity(cities, o["cities"] as ArrayList);
                }
            }
        }

        public void ExportChina()
        {
            var otherNames = new string[] { "2912", "2911" };
            var provinces =
                CityConfig.GetInstance()
                    .Countryconfig.countries.Where(m => !otherNames.Contains(m.baiduCode))
                .Where(m=>m.name == "上海市")
                    .ToList();
            var others = new string[] { "北京市", "天津市", "重庆市", "上海市" };
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(
                new DataColumn[]
                    {
                        new DataColumn("area_code"),  new DataColumn("X"),
                        new DataColumn("area_name"), new DataColumn("parent_code"), new DataColumn("Y"),
                        new DataColumn("PATH")
                    });
            foreach (Province province in provinces)
            {
                //dataTable.Rows.Clear();
                if (others.Contains(province.name))
                {
                    province.cities.ForEach(
                        m =>
                            {
                                m.name = province.name;
                                ResoleGoadeData(m, dataTable, "1");
                            });
                }
                else
                {
                    ResoleProvinceData(province, dataTable);
                    province.cities.ForEach(m => ResoleGoadeData(m, dataTable, province.baiduCode));
                }
            }
            //SVCHelper.ExportToSvc(dataTable, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "上海市.csv"));
            ShpFileHelper.SaveShpFile(dataTable, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "上海市.shp"), wkbGeometryType.wkbPolygon, ProjectConvert.GCJ_WGS);
            
        }

        private void ResoleProvinceData(Province province,DataTable dataTable)
        {
            var pathData = this.DownLoad(province.name);
            var districts = pathData["districts"] as ArrayList;
            var mainDistrict = districts[0] as Dictionary<string, object>;
            var row = dataTable.NewRow();
            row["area_code"] = province.baiduCode;
            row["area_name"] = province.name;
            row["parent_code"] = "1";
            row["X"] = mainDistrict["center"].ToString().Split(',')[0];
            row["Y"] = mainDistrict["center"].ToString().Split(',')[1];
            row["PATH"] = mainDistrict["polyline"];

            dataTable.Rows.Add(row);
        }
        private void ResoleGoadeData(MapDataTools.City city, DataTable dataTable, string provinceCode)
        {
            var pathData = this.DownLoad(city.gaodeCode);
            var districts = pathData["districts"] as ArrayList;
            var mainDistrict = districts[0] as Dictionary<string,object>;
            var row = dataTable.NewRow();
            row["area_code"] = city.baiduCode;
            row["area_name"] = city.name;
            row["parent_code"] = provinceCode;
            row["X"] = mainDistrict["center"].ToString().Split(',')[0];
            row["Y"] = mainDistrict["center"].ToString().Split(',')[1];
            row["PATH"] = mainDistrict["polyline"];

            dataTable.Rows.Add(row);
            if (city.name == "三亚市")
            {
                return;
            }
            for (int i = 1; i < districts.Count; i++)
            {
                var district = districts[i] as Dictionary<string, object>;
                var districtName = district["name"].ToString();
                var o = city.districts.FirstOrDefault(m => m.name == districtName);
                if (o != null)
                {
                    row = dataTable.NewRow();
                    row["area_code"] = o.baiduCode;
                    row["area_name"] = o.name;
                    row["parent_code"] = city.baiduCode;
                    row["X"] = district["center"].ToString().Split(',')[0];
                    row["Y"] = district["center"].ToString().Split(',')[1];
                    row["PATH"] = district["polyline"];

                    dataTable.Rows.Add(row);
                }
            }
        }
        private string[] gaodeKeys = new string[] { "6064051679efa89524860e1de482e294", "ddcf114352f37eb1295049911533e97e", "05d0b6e52d9ed5da644bf6922513a73a" };
        private Dictionary<string, object> DownLoad(string gaodeCode)
        {
            Random rd = new Random();
            int kindex = rd.Next(0, gaodeKeys.Length);
            string tempUrl =
                string.Format(
                    "http://restapi.amap.com/v3/config/district?subdistrict=1&extensions=all&level=district&key={0}&s=rsv3&output=json&keywords={1}",
                    gaodeKeys[kindex],
                    gaodeCode);
             using (var client = new WebClient())
             {
                 client.Encoding = System.Text.ASCIIEncoding.UTF8;
                 string result = client.DownloadString(tempUrl);
                 var javaScirpt = new System.Web.Script.Serialization.JavaScriptSerializer();
                 javaScirpt.MaxJsonLength = int.MaxValue;
                 return javaScirpt.Deserialize<Dictionary<string, object>>(result);
             }
        }

       
    }
}
