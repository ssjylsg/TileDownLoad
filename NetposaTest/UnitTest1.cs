using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetposaTest
{
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.AccessControl;
    using System.Text;
    using System.Threading;

    using MapDataTools;
    using MapDataTools.Util;

    using Microsoft.International.Converters.PinYinConverter;

    using Npgsql;

    using NUnit.Framework;

    using OSGeo.OGR;

    using Encoder = System.Text.Encoder;
    using XmlStorageHelper = MapDataTools.XmlStorageHelper;

    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var person = XmlHelper.ToXml(new Person() { Name = "两三个", Id = 1, Sex = "NAN" });
        //    Console.WriteLine(person);
        //    Assert.IsNotNull(person);
        //    Assert.IsTrue(person.Length !=0);
        //    var p = XmlHelper.ToEntity<Person>(person);
        //    Assert.IsNotNull(p);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        // [TestMethod]
        //[TestCategory("路口测试")]
        //public void Test_GaoRoadCross()
        //{
        //    new GaoDeRoads().downLoadRoadCrossByCityName("宝鸡市");
        //}
        // [TestMethod]
        // [TestCategory("路口测试")]
        //public void Test_RoadName()
        //{
        //  var road =  new RoadNameLoad().GetRoadsByCityName("http://baoji.city8.com/", "宝鸡", 100);
        //     Assert.IsTrue(road.Roads.Count !=0);
        //}
        // [TestMethod]
        // [TestCategory("路口测试")]
        // public void Test_AllRoads() {
        //     new RoadNameLoad().UpdateRoads();
        // }
        //[TestMethod]
        //[TestCategory("检验数据有效性")]
        // public void Test_CityRoads() {
        //     foreach (var city in CityRoadConfig.GetInstance().cityRoadConfig.cityRoadList) {
        //         Assert.IsTrue(city.Roads.Count != 0);
        //         Assert.IsFalse(city.cityName.Contains("地图"));
        //     }
        // }
        //private string removeChar(string str, char c)
        //{
        //   // return str.TrimStart(c);

        //    int index = 0;

        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (str[i] == c)
        //        {
        //            index++;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    string result = "";
        //    for (int i = index; i < str.Length; i++)
        //    {
        //        result += str[i];
        //    }
        //    return result;
        //}
        // [TestMethod]
        //public void TestChar()
        //{
        //    string msg =
        //    "R00010e6a";
        //    msg = removeChar(msg, 'R');
        //    msg = removeChar(msg, '0');
        //    Console.WriteLine(msg);
        //}
        //[TestMethod]
        //public void TestHotCity()
        //{
        //    new HotCity().DownLoad();
        //}
        //[TestMethod]
        //public void TestJsCode()
        //{
        //    var obj = new HotCity().ResolvGeom(".=YfuaHB/TZoXA;");
        //    Console.WriteLine(obj);
        //    //.=YfuaHB/TZoXA;
        //}
        //[TestMethod]
        //public void TestCity()
        //{
        //    //\city.csv
        //    new HotCity().ReadCityBound(@"F:\map\NPGIS\trunk\arcgis\DbConfig\行政区\citydata.js", @"F:\map\NPGIS\trunk\arcgis\DbConfig\行政区");
        //}

        //[TestMethod]
        //public void FillBaiduCode()
        //{
        //    NpgsqlConnection conn = new NpgsqlConnection("Server=192.168.60.242;Database=Road;Port=5432;Userid=postgres;Password=123456");


        //    NpgsqlCommand comm = new NpgsqlCommand();
        //    comm.Connection = conn;
        //    conn.Open();
        //    var citys = CityConfig.GetInstance().Countryconfig.countries;
        //    var municipalities = new string[] { "北京市市辖区", "香港特别行政区", "澳门特别行政区", "天津市市辖区", "重庆市市辖区", "上海市市辖区" };
        //    foreach (Province province in citys)
        //    {
        //        comm.CommandText = string.Format(
        //            "select area_code from city where area_name like '{0}%'",
        //            province.name.TrimEnd(new[] { '市', '辖', '区' }));
        //        var read = comm.ExecuteReader();
        //        if (read.Read())
        //        {
        //            province.baiduCode = read.GetString(0);
        //        }
        //        read.Close();

        //        foreach (var city in province.cities)
        //        {
        //            var parentCode = province.baiduCode;
        //            if (municipalities.Contains(city.name))
        //            {
        //                parentCode = "1";
        //            }
        //            comm.CommandText = string.Format(
        //           "select area_code from city where area_name like '{0}%' and parent_code = '{1}'",
        //           city.name.TrimEnd(new[] { '市', '辖', '区' }), parentCode);
        //              read = comm.ExecuteReader();
        //            if (read.Read())
        //            {
        //                city.baiduCode = read.GetString(0);
        //            }
        //            read.Close();
        //            foreach (var district in city.districts)
        //            {
        //                comm.CommandText = string.Format(
        //               "select area_code from city where area_name like '{0}%' and parent_code = '{1}'",
        //               district.name.TrimEnd(new[] { '市', '辖', '区' }),city.baiduCode);
        //                read = comm.ExecuteReader();
        //                if (read.Read())
        //                {
        //                    district.baiduCode = read.GetString(0);
        //                }
        //                read.Close();
        //            }
        //        }
        //    }
        //    CityConfig.GetInstance().saveConfig();
        //}

        //[TestMethod]
        //public void FillBussinessCode()
        //{
        //    NpgsqlConnection conn =
        //        new NpgsqlConnection("Server=192.168.60.242;Database=Road;Port=5432;Userid=postgres;Password=123456");
        //    NpgsqlCommand comm = new NpgsqlCommand();
        //    comm.Connection = conn;
        //    conn.Open();
        //    var citys = CityConfig.GetInstance().Countryconfig.countries;
        //    foreach (Province province in citys)
        //    {
        //        foreach (City city in province.cities)
        //        {
        //            foreach (var district in city.districts)
        //            {
        //                comm.CommandText =
        //                    string.Format(
        //                        "select area_code,area_name from bussiness_area where city_code = '{0}'",
        //                        district.baiduCode);
        //                var read = comm.ExecuteReader();
        //                while (read.Read())
        //                {
        //                    district.business.Add(
        //                        new Business() { baiduCode = read.GetString(0), name = read.GetString(1) });
        //                }
        //                read.Close();
        //            }
        //        }
        //    }
        //    CityConfig.GetInstance().saveConfig();
        //}
//        [Test]
//        public void ExportChina()
//        {
//            new HotCity().ExportChina();
//        }

//        [Test]
//        public void TestMapBarRoadLine()
//        {
//            new MapbarRoadLine().UpdateRoads();
//        }

//        [Test]
//        public void MerageRoadLineData()
//        {

//            var cityRoadConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/CityRoadConfig.xml");
//            var mapbarConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/mapbarCityRoadConfig.xml");
//            var DefaultConfigXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/CityRoadConfig_all.xml");
//            var mapBarData = new CityRoadConfig(mapbarConfig).cityRoadConfig.cityRoadList;
//            var city8Data = new CityRoadConfig(cityRoadConfig).cityRoadConfig.cityRoadList;

//            foreach (var c in mapBarData)
//            {
//                var city8 =
//                    city8Data.FirstOrDefault(m => m.cityName.Contains(c.cityName) || c.cityName.Contains(m.cityName));
//                if (city8 != null)
//                {
//                    c.Roads.AddRange(city8.Roads);
//                    c.Roads = c.Roads.Distinct().ToList();
//                }
//            }
//            var cp = new CityRoads();
//            cp.cityRoadList = mapBarData;
//            XmlStorageHelper xmler = new XmlStorageHelper();
//            xmler.SaveToFile(cp, DefaultConfigXml);
//        }

//        [Test]
//        public void DownLoadCity()
//        {
//            //            甘肃省,兰州;
//            //云南省,昆明;
//            //四川省,成都;
//            //吉林省,长春;
//            //辽宁省,沈阳;
//            //青海省,西宁;
//            //陕西省,西安;
//            //河南省,郑州;
//            //山东省,济南;
//            //山西省,太原;
//            //安徽省,合肥;
//            //湖北省,武汉;
//            //湖南省,长沙;
//            //江苏省,南京;
//            //贵州省,贵阳;
//            //浙江省,杭州;江西省,南昌;


//            string citys = @"
//  福建省,福州; 
//  海南省,海口;
//  内蒙古,呼和浩特;
//  西藏,拉萨;
//  北京市,北京;
//  上海市,上海;
//  天津市,天津;
//  重庆市,重庆;
//  宁夏回族自治区,银川;
//  新疆维吾尔自治区,乌鲁木齐;
//广西壮族自治区,南宁
//  ";
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            ThreadPool.SetMaxThreads(2, 2);
//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var citName = city.Split(',')[1].Trim();

//                //ThreadPool.QueueUserWorkItem(
//                //   (o) =>
//                //       {
//                try
//                {
//                    DownLoad(citName, provice);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex);
//                }
//                //}); 
//            }
//        }

//        private void DownLoad(string citName, string provice)
//        {
//            GaoDeRoads roads = new GaoDeRoads();
//            var roaddataTable = new DataTable(citName);
//            roaddataTable.Columns.Add("Name", Type.GetType("System.String"));
//            roaddataTable.Columns.Add("WIDTH", Type.GetType("System.String"));
//            roaddataTable.Columns.Add("TYPE", Type.GetType("System.String"));
//            roaddataTable.Columns.Add("PATH", Type.GetType("System.String"));
//            Console.WriteLine("下载{0}中", citName);
//            roads.downOverHandler += () =>
//                {
//                    var directory = System.IO.Path.Combine(
//                        System.AppDomain.CurrentDomain.BaseDirectory,
//                        "shap\\" + provice);
//                    if (!System.IO.Directory.Exists(directory))
//                    {
//                        Directory.CreateDirectory(directory);
//                    }
//                    var path = System.IO.Path.Combine(directory, citName + ".shp");
//                    System.Threading.Thread.Sleep(1000);
//                    ShpFileHelper.SaveShpFile(
//                        roaddataTable,
//                        path,
//                        wkbGeometryType.wkbLineString,
//                        ProjectConvert.GAODE84_WGS);
//                    roaddataTable.Rows.Clear();
//                    GC.Collect();
//                    Console.WriteLine("下载{0}完成", citName);
//                };
//            roads.roadDateDowningHandler += (RoadModel road, int index, int count) =>
//                {
//                    for (int i = 0; i < road.paths.Count; i++)
//                    {
//                        DataRow row = roaddataTable.NewRow();
//                        row[0] = road.name;
//                        row[1] = road.width;
//                        row[2] = road.type;
//                        row[3] = road.paths[i];
//                        roaddataTable.Rows.Add(row);
//                    }
//                };
//            roads.downLoadRoadsByCityName(citName);
//        }

//        #region 数据 

//        private string citys = @"
//            湖南省,长沙;
//        河北省,石家庄;
//         黑龙江,哈尔滨;
//         甘肃省,兰州;
//        上海市,上海;
//         天津市,天津;
//         云南省,昆明;
//        北京市,北京;
//重庆市,重庆;
//宁夏回族自治区,银川;
//新疆维吾尔自治区,乌鲁木齐;
//         四川省,成都;
//            吉林省,长春;
//广西壮族自治区,南宁;
//          辽宁省,沈阳;
//            青海省,西宁;
//            陕西省,西安;
//            河南省,郑州;
//          山西省,太原;
//            安徽省,合肥;
//            湖北省,武汉;
//        贵州省,贵阳;
//            浙江省,杭州;
//江西省,南昌;
//        广东省,广州;
//        海南省,海口;
//内蒙古,呼和浩特;
//        西藏,拉萨;
//            山东省,济南;
//            江苏省,南京;
//            福建省,福州";

//        private string extents = @"
//乌鲁木齐;87.516452787599,43.763635565918,87.674381254396,43.871782233398&
//济南;116.87938674512,36.611262322265,117.19181045117,36.715632439453&
//南京;118.67849240675,31.97905925879,118.91401181593,32.215265313477&
//南宁;108.22799350195,22.730636210938,108.51020480566,22.896117778321&
//北京;116.20753109375,39.799245591797,116.56321346679,40.000432725586&
//天津;116.74989619727,38.705155707031,117.87874141211,39.446732855469&
//重庆;106.36026301855,29.398362494141,106.67749324316,29.686066961915&
//石家庄;114.34659246972,37.942477173828,114.65558294824,38.14572424414&
//太原;112.42908503613,37.729984581054,112.67421748242,38.015629112304&
//沈阳;123.24665849903,41.690011142579,123.61401384571,41.915230869141&
//长春;125.15970507812,43.785217464843,125.44397631836,44.020050228515&
//哈尔滨;126.46623604004,45.630960394531,126.8404578418,45.863733221679&
//苏州;120.42526432128,31.214921602539,120.91072269531,31.468980440429&
//杭州;120.04027949121,30.100691421875,120.36574946192,30.39114247168&
//合肥;117.17317871875,31.780509152833,117.42105774707,31.941870847168&
//福州;119.21972166602,25.98123156543,119.42296873633,26.138473386719&
//南昌;115.74967205714,28.585169147461,116.0583192129,28.774339984863&
//青岛;120.2837618672,36.046589788085,120.6881960713,36.426304753906&
//呼和浩特;111.46206076562,40.689908290039,111.89327414453,40.893155360351&
//武汉;114.12691062306,30.438099506836,114.55881064747,30.674992207031&
//长沙;112.85456293555,28.09040652832,113.17453974219,28.30052005371&
//广州;113.11309549609,23.029047628907,113.47289774218,23.211695333985&
//兰州;103.68073473437,36.020273497071,103.94783983691,36.117433836426&
//成都;103.94969190918,30.584617639648,104.19413770996,30.76932528125&
//贵阳;106.58026877149,26.462299909179,106.80068197949,26.691639508789&
//海口;110.1464823584,19.966053031249,110.45272625488,20.079349540038&
//昆明;102.62779360254,24.956151602539,102.81524782617,25.128156302246&
//西宁;101.61103117335,36.569791311768,101.87985288966,36.676908010987&
//西安;108.78460236913,34.18934660791,109.08603974706,34.35723143457&
//拉萨;91.064062433351,29.62654370874,91.205854730714,29.696753211914&
//银川;106.07870847999,38.414184941406,106.32658750831,38.583099736328&
//郑州;113.48276429493,34.643991845704,113.82265382129,34.835565942383
//";

//        #endregion

//        [Test]
//        public void GetMainCity()
//        {
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            List<string> list = new List<string>();
//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var citName = city.Split(',')[1].Trim();
//                list.Add(citName);
//            }
//            Console.WriteLine(string.Join(",", list.Select(m => string.Format("'{0}市'", m)).ToArray()));
//        }

//        [Test]
//        public void DownLoadPois()
//        {

//            Dictionary<string, Extent> cityDictionary = new Dictionary<string, Extent>();
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            var cityExtents = extents.Replace('\r', ' ').Split('&');
//            cityExtents.ToList().ForEach(
//                m =>
//                    {
//                        cityDictionary[m.Split(';')[0].Trim()] = Extent.Resolve(m.Split(';')[1]);
//                    });
//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var citName = city.Split(',')[1].Trim();
//                if (!cityDictionary.ContainsKey(citName))
//                {
//                    continue;
//                }
//                try
//                {
//                    Console.WriteLine(citName);
//                    DownLoadPoi(citName, provice, cityDictionary[citName]);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(city);
//                    Console.WriteLine(ex);
//                }
//            }
//        }

//        private void DownLoadPoi(string citName, string provice, Extent extent)
//        {
//            BaiduMap baiduMap = new BaiduMap();
//            var dataTable = new DataTable();

//            dataTable.Columns.Add("name", Type.GetType("System.String"));
//            dataTable.Columns.Add("X", Type.GetType("System.String"));
//            dataTable.Columns.Add("Y", Type.GetType("System.String"));
//            dataTable.Columns.Add("r_addr", Type.GetType("System.String"));
//            dataTable.Columns.Add("type", Type.GetType("System.String"));
//            dataTable.Columns.Add("phone", Type.GetType("System.String"));
//            baiduMap.DowningEvent += (poi, index, count) =>
//                {
//                    DataRow row = dataTable.NewRow();
//                    row["name"] = poi.name;
//                    row["X"] = poi.cx;
//                    row["Y"] = poi.cy;
//                    row["r_addr"] = poi.address.Replace(",", "");
//                    row["type"] = poi.type;
//                    row["phone"] = poi.phone.Replace(",", ";");
//                    dataTable.Rows.Add(row);
//                };
//            baiduMap.DownEndEvent += message =>
//                {
//                    var directory = System.IO.Path.Combine(
//                        System.AppDomain.CurrentDomain.BaseDirectory,
//                        "shap\\" + provice);
//                    if (!System.IO.Directory.Exists(directory))
//                    {
//                        Directory.CreateDirectory(directory);
//                    }
//                    var path = System.IO.Path.Combine(directory, citName + "_poi.shp");
//                    AsposeCellsHelper.ExportToExcel(dataTable, System.IO.Path.Combine(directory, citName + "_poi.xlsx"));
//                    try
//                    {
//                        //System.Threading.Thread.Sleep(1000);
//                        //ShpFileHelper.saveShpFile(dataTable, path, OSGeo.OGR.wkbGeometryType.wkbPoint, ProjectConvert.NONE);
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(e);
//                    }
//                    dataTable.Rows.Clear();
//                    GC.Collect();
//                };
//            baiduMap.GetPoiByExtentKeyWords(extent, string.Empty);
//        }

//        [Test]
//        public void PoiToShap()
//        {
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            List<string> priList = new List<string>();
//            tempCitys.ToList().ForEach(
//                o =>
//                    {
//                        priList.Add(o.Split(',')[0].Trim());
//                    });
//            var directories =
//                Directory.GetDirectories(
//                    @"F:\map\NPGIS\trunk\tools\NPGISMapTileDown\GetTileImage\NetposaTest\bin\Release\shap");
//            foreach (string directory in directories)
//            {
//                //if (priList.FirstOrDefault(m => directory.Contains(m)) == null)
//                //{
//                //    continue;
//                //}
//                var files = Directory.GetFiles(directory, "*.xlsx");
//                if (files.Length == 0)
//                {
//                    Console.WriteLine(directory);
//                    continue;
//                }
//                foreach (string file in files)
//                {
//                    Console.WriteLine(file);
//                    try
//                    {
//                        var dataTable = AsposeCellsHelper.ExportToDataTable(file, true);
//                        ShpFileHelper.SaveShpFile(
//                            dataTable,
//                            file.Replace("xlsx", "shp"),
//                            wkbGeometryType.wkbPoint,
//                            ProjectConvert.NONE);
//                        dataTable.Rows.Clear();
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine(ex);
//                    }
//                }
//            }
//        }

//        public void FileRename()
//        {

//            var directories =
//                Directory.GetDirectories(
//                    @"F:\map\NPGIS\trunk\tools\NPGISMapTileDown\GetTileImage\NetposaTest\bin\Release\shap");
//            foreach (string directory in directories)
//            {
//                Directory.GetFiles(directory).ToList().ForEach(
//                    m =>
//                        {
//                            if (!m.Contains(".xlsx"))
//                            {
//                                if (m.Contains("_poi_poi."))
//                                {
//                                    File.Move(m, m.Replace("_poi.", "."));
//                                }
//                                else
//                                {
//                                    //File.Move(m, m.Replace("_poi.", "."));
//                                }
//                            }
//                        });
//                //Directory.GetDirectories(directory).ToList().ForEach(
//                //    m =>
//                //        {
//                //            Directory.GetFiles(m).ToList().ForEach(
//                //                o =>
//                //                    {
//                //                        //var name = m.Substring(m.LastIndexOf('\\')+1).TrimEnd(new []{'s','h','a','p','.'});
//                //                        //var extent = o.Substring(o.LastIndexOf("."));
//                //                        //new FileInfo(o).CopyTo(
//                //                        //    Path.Combine(directory, string.Format("{0}_poi{1}", name, extent)),false);

//                //                        File.Delete(o);
//                //                    });
//                //            Directory.Delete(m);
//                //        });
//            }
//        }

//        public void DownLoadRoadCross()
//        {

//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            var crossDataTable = new DataTable();
//            crossDataTable.Columns.Add("first_name", Type.GetType("System.String"));
//            crossDataTable.Columns.Add("second_name", Type.GetType("System.String"));
//            //crossDataTable.Columns.Add("first_id", Type.GetType("System.String"));
//            //crossDataTable.Columns.Add("second_id", Type.GetType("System.String"));
//            crossDataTable.Columns.Add("x", Type.GetType("System.String"));
//            crossDataTable.Columns.Add("y", Type.GetType("System.String"));
//            crossDataTable.Columns.Add("name", Type.GetType("System.String"));

//            var dataTable = new DataTable();
//            foreach (string city in tempCitys)
//            {
//                crossDataTable.Rows.Clear();
//                var provice = city.Split(',')[0].Trim();
//                var cityName = city.Split(',')[1].Trim();
//                Console.WriteLine(cityName);
//                var directory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "shap\\" + provice);
//                if (!System.IO.Directory.Exists(directory))
//                {
//                    Directory.CreateDirectory(directory);
//                }
//                var path = System.IO.Path.Combine(directory, cityName + "_RoadCross.xlsx");
//                if (File.Exists(path))
//                {
//                    dataTable = AsposeCellsHelper.ExportToDataTable(path, true);
//                    try
//                    {
//                        //dataTable.Columns.Remove("first_id");
//                        //dataTable.Columns.Remove("second_id");
//                        var shpPath = System.IO.Path.Combine(directory, cityName + "_RoadCross.shp");
//                        ShpFileHelper.SaveShpFile(
//                            dataTable,
//                            shpPath,
//                            OSGeo.OGR.wkbGeometryType.wkbPoint,
//                            ProjectConvert.NONE);
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(e);
//                    }
//                    dataTable.Rows.Clear();
//                    continue;
//                }
//                var road = new GaoDeRoads();
//                road.roadCrossDowningHandler += (roadCross, index, count) =>
//                    {
//                        DataRow row = crossDataTable.NewRow();
//                        row["first_name"] = roadCross.first_name;
//                        row["second_name"] = roadCross.second_name;
//                        row["x"] = roadCross.wgs_x;
//                        row["y"] = roadCross.wgs_y;
//                        row["name"] = string.Format("{0}_{1}", roadCross.first_name, roadCross.second_name);
//                        crossDataTable.Rows.Add(row);
//                    };
//                road.downOverHandler += () =>
//                    {
//                        try
//                        {
//                            AsposeCellsHelper.ExportToExcel(crossDataTable, path);
//                            crossDataTable.Rows.Clear();
//                        }
//                        catch (Exception)
//                        {
//                            Console.WriteLine("Error" + cityName);
//                        }
//                    };
//                road.downLoadRoadCrossByCityName(cityName);
//            }
//        }

//        public static string GetPinyin(string str)
//        {
//            if (string.IsNullOrEmpty(str))
//            {
//                return string.Empty;
//            }
//            string r = string.Empty;
//            foreach (char obj in str)
//            {
//                try
//                {
//                    ChineseChar chineseChar = new ChineseChar(obj);
//                    var pinyings =
//                        chineseChar.Pinyins.ToList()
//                            .Where(m => m != null)
//                            .Select(m => m.Substring(0, m.Length - 1))
//                            .Distinct()
//                            .ToArray();
//                    if (pinyings.Length > 1)
//                    {
//                        return string.Empty;
//                    }
//                    r += pinyings[0];
//                }
//                catch
//                {
//                    r += obj.ToString();
//                }
//            }
//            return r;
//        }

//        [Test]
//        public void CopyData()
//        {
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            Dictionary<string, string> cityPinyin = new Dictionary<string, string>();

//            duoPinyin.Replace('\r', ' ').Split(',').ToList().ForEach(
//                m =>
//                    {
//                        cityPinyin[m.Split(';')[0].Trim()] = m.Split(';')[1].Trim();
//                    });

//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var cityName = city.Split(',')[1].Trim();
//                //  Console.WriteLine(cityName);

//                var directory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "shap\\" + provice);
//                var cityDirectory = System.IO.Path.Combine(directory, cityName);
//                if (!System.IO.Directory.Exists(cityDirectory))
//                {
//                    Directory.CreateDirectory(cityDirectory);
//                }
//                var pinyin = GetPinyin(cityName).ToLower();
//                if (string.IsNullOrEmpty(pinyin))
//                {
//                    if (cityPinyin.ContainsKey(cityName))
//                    {
//                        pinyin = cityPinyin[cityName];
//                    }
//                    else
//                    {
//                        Console.WriteLine("多音字" + cityName);
//                        continue;
//                    }
//                }
//                Directory.GetFiles(cityDirectory).ToList().ForEach(
//                    m =>
//                        {

//                            var fileInfo = new FileInfo(m);
//                            var targetFile = Path.Combine(cityDirectory, fileInfo.Name.Replace(cityName, pinyin));
//                            if (!File.Exists(targetFile))
//                            {
//                                fileInfo.MoveTo(targetFile);
//                            }
//                        });
//            }
//        }

//        private string duoPinyin = @"石家庄;shijiazhuang
//,哈尔滨;haerbin
//,上海;shanghai
//,天津;tianjin
//,昆明;kunming
//,重庆;chongqing
//,银川;yinchuan
//,乌鲁木齐;wulumuqi
//,成都;chengdu
//,长春;changchun
//,南宁;nanning
//,西宁;xining
//,西安;xian
//,太原;taiyuan
//,合肥;hefei
//,长沙;changsha
//,杭州;hangzhou
//,南昌;nanchang
//,广州;guangzhou
//,海口;haikou
//,呼和浩特;huhehaote
//,拉萨;lasa
//,济南;jinan
//,南京;nanjing";

//        [Test]
//        public void WriteDb()
//        {

//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            var shap2sql = "shp2pgsql -W \"gbk\" {0} >{1}";
//            var sqlTodb = "psql -h 192.168.60.242  -d routing  -U postgres -f  {0}";
//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var cityName = city.Split(',')[1].Trim();


//                var directory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "shap\\" + provice);
//                var cityDirectory = System.IO.Path.Combine(directory, cityName);
//                if (!System.IO.Directory.Exists(cityDirectory))
//                {
//                    Directory.CreateDirectory(cityDirectory);
//                }
//                Console.WriteLine(cityName);

//                Directory.GetFiles(cityDirectory, "*.sql").ToList().ForEach(
//                    m =>
//                        {
//                            //var fileInfo = new FileInfo(m);
//                            //var targetFile = Path.Combine(
//                            //    fileInfo.DirectoryName,
//                            //    fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".")) + ".sql");
//                            //if (!File.Exists(targetFile))
//                            //{
//                            //    string msg = string.Format(shap2sql, m, targetFile);
//                            //    RunCmd(msg);
//                            //    System.Threading.Thread.Sleep(1000);
//                            //}
//                            this.RunCmd(string.Format(sqlTodb, m));
//                            System.Threading.Thread.Sleep(10000);
//                        });
//            }
//        }

//        private void RunCmd(string msg)
//        {
//            string cmdtext = @"C:"; //CD C:\Program Files\PostgreSQL\9.4\bin
//            Process MyProcess = new Process();
//            //设定程序名 
//            MyProcess.StartInfo.FileName = "cmd.exe";
//            //关闭Shell的使用 
//            MyProcess.StartInfo.UseShellExecute = false;
//            //重定向标准输入 
//            MyProcess.StartInfo.RedirectStandardInput = true;
//            //重定向标准输出 
//            MyProcess.StartInfo.RedirectStandardOutput = true;
//            //重定向错误输出 
//            MyProcess.StartInfo.RedirectStandardError = true;
//            //设置不显示窗口 
//            MyProcess.StartInfo.CreateNoWindow = true;
//            //执行VER命令 
//            MyProcess.Start();
//            MyProcess.StandardInput.WriteLine(cmdtext);
//            //
//            MyProcess.StandardInput.WriteLine(@"CD C:\Program Files\PostgreSQL\9.4\bin");
//            MyProcess.StandardInput.WriteLine(msg);
//        }

//        private string con = "Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = routing;";

//        private List<string> GetTables(string nameLike)
//        {
//            string sql = !string.IsNullOrEmpty(nameLike)
//                             ? string.Format(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public' 
//and TABLE_NAME like '%_{0}%'
//order by TABLE_NAME",
//                                 nameLike)
//                             : string.Format(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public'  
//order by TABLE_NAME");
//            List<string> list = new List<string>();

//            var dbHelper = new DbHelper(con);
//            var reader = dbHelper.ExecuteReader(sql);
//            while (reader.Read())
//            {
//                list.Add(reader.GetString(0));
//            }
//            reader.Close();
//            return list;
//        }

//        /// <summary>
//        /// 路网表扩展
//        /// </summary>
//        [Test]
//        public void ExcuteRoadNet()
//        {
//            var tables = GetTables("roadnet");
//            string msg = @"
//--select distinct highspeed  from shanghai_roadnet_supermap;
//
//--- 新增表字段
//alter table xian_roadNet add length double precision;
//alter table xian_roadNet add car integer;
//alter table xian_roadNet add walk integer;
//alter table xian_roadNet add speed double precision;
//alter table xian_roadNet add highspeed integer;
//alter table xian_roadNet add np_level integer;
//
//-- 道路长度计算
//update xian_roadNet set length=st_length(st_transform(st_setsrid(geom,4326),900913)) ;
//
//-- 道路分级
//--select distinct type from  xian_roadNet ;
//
//-- 设置道路等级
//
//-- 设置道路速度
//update xian_roadNet set speed = 100,np_level=1 where type = '高速公路';
//update xian_roadNet set speed = 70,np_level =2 where type = '主要道路（城市主干道）';
//update xian_roadNet set speed = 50,np_level =3 where type = '次要道路（城市次干道）';
//update xian_roadNet set speed = 50,np_level =3 where type = '一般道路';
//update xian_roadNet set speed = 50,np_level =4 where type = '城市环路/城市快速路';
//update xian_roadNet set speed = 18,np_level =4 where type = '非导航道路';
//
//update xian_roadNet set speed = 60,np_level =3 where type = '国道';
//update xian_roadNet set speed = 80,np_level =2 where type = '省道';
//update xian_roadNet set speed = 40,np_level =3 where type = '县道';
//update xian_roadNet set speed = 20,np_level =4 where type = '区县内部道路';
//update xian_roadNet set speed = 18,np_level =4 where type = '乡村道路';
//
//-- 设置高速道路
//update xian_roadNet set highspeed = 0;
//update xian_roadNet set highspeed = 1 where np_level in (1);
//
//-- 设置道路是否可步行和通车
//update xian_roadNet set walk = 0 , car = 0 ;
//update xian_roadNet set walk = 0 , car = 1 where np_level in (1);
//update xian_roadNet set walk = 1 , car = 1 where np_level != 1
//";
//            var dbHelper = new DbHelper(this.con);
//            foreach (var table in tables)
//            {
//                var sql = msg.Replace("xian_roadNet", table);
//                dbHelper.ExecuteNonQuery(sql);
//            }
//        }

//        /// <summary>
//        /// 路口/兴趣点 增加拼音和拼音首字母
//        /// </summary>
//        [Test]
//        public void ExcuteRoadCross()
//        {
//            var tables = GetTables("poi");
//            var dbHelper = new DbHelper(this.con);
//            var connection = new NpgsqlConnection(con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            var readcon = new NpgsqlConnection(con);
//            readcon.Open();
//            var readcommand = readcon.CreateCommand();
//            readcommand.CommandType = CommandType.Text;
//            foreach (var table in tables)
//            {
//                dbHelper.AddColumn("quanpin", table);
//                dbHelper.AddColumn("szm", table);

//                Console.WriteLine(table);
//                command.CommandType = CommandType.Text;
//                command.CommandTimeout = 1000 * 60;
//                command.CommandText = string.Format("SELECT COUNT(1) FROM {0} where quanpin is null", table);
//                int rowCount = int.Parse(command.ExecuteScalar().ToString());
//                if (rowCount != 0)
//                {
//                    var max = rowCount / 2000 + 1;

//                    Console.WriteLine(max);
//                    for (int i = 0; i < max; i++)
//                    {
//                        Console.WriteLine(i);
//                        readcommand.CommandText =
//                            string.Format("SELECT gid,name from {0} where quanpin is null  limit 2000", table);
//                        var reader = readcommand.ExecuteReader();
//                        while (reader.Read())
//                        {
//                            var gid = reader.GetInt32(0);
//                            var name = reader.GetString(1);

//                            command.CommandText = string.Format(
//                                "UPDATE {0} SET quanpin='{1}',szm='{2}' where gid = {3}",
//                                table,
//                                Hz2Py.GetPinyin(name).Replace('\'', ' '),
//                                Hz2Py.GetFirstPinyin(name).Replace('\'', ' '),
//                                gid);
//                            command.ExecuteNonQuery();
//                        }
//                        reader.Close();
//                    }
//                    //readcommand.Dispose();
//                }

//                // readcon.Close();
//                //readcon.Dispose();

//                // command.Dispose();
//                // connection.Close();
//                //connection.Dispose();
//            }

//        }

//        /// <summary>
//        /// 获取城市名称拼音多音字
//        /// </summary>
//        [Test]
//        public void GetCityPinyin()
//        {
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            Dictionary<string, string> cityPinyin = new Dictionary<string, string>();

//            duoPinyin.Replace('\r', ' ').Split(',').ToList().ForEach(
//                m =>
//                    {
//                        cityPinyin[m.Split(';')[0].Trim()] = m.Split(';')[1].Trim();
//                    });
//            StringBuilder stringBuilder = new StringBuilder();
//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var cityName = city.Split(',')[1].Trim();
//                var pinyin = GetPinyin(cityName).ToLower();
//                if (string.IsNullOrEmpty(pinyin))
//                {
//                    if (cityPinyin.ContainsKey(cityName))
//                    {
//                        pinyin = cityPinyin[cityName];
//                    }
//                    else
//                    {
//                        Console.WriteLine("ERROR" + cityName);
//                    }
//                }
//                stringBuilder.AppendFormat("{2},{0},{1};", cityName, pinyin, provice).AppendLine();
//            }
//            Console.WriteLine(stringBuilder.ToString());
//        }

//        /// <summary>
//        /// POI 数据填充区号
//        /// </summary>
//        [Test]
//        public void WritePoiDirect()
//        {
//            var tables = GetTables("poi");

//            var data = new ProviceCityPinYin().GetProviceCities();
//            var dbHelper = new DbHelper(this.con);
//            var connection = new NpgsqlConnection(con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            command.CommandType = CommandType.Text;

//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            foreach (string city in tempCitys)
//            {
//                var provice = city.Split(',')[0].Trim();
//                var cityName = city.Split(',')[1].Trim();
//                var table = data[cityName].CityPinyin + "_poi";
//                if (!tables.Contains(table))
//                {
//                    continue;
//                }
//                dbHelper.AddColumn("districtname", table);
//                Console.WriteLine(cityName);
//                var cityList = CityConfig.GetInstance().GetCityByName(cityName);
//                if (cityList.Count != 1)
//                {
//                    continue;
//                }
//                var code = cityList[0].baiduCode;
//                if (cityName == "长沙")
//                {
//                    code = "158";
//                }
//                var districts = new Dictionary<string, string>(); // code name
//                command.CommandText = string.Format(
//                    "SELECT area_code,area_name,gid FROM CITY WHERE parent_code = '{0}'",
//                    code);
//                var read = command.ExecuteReader();
//                while (read.Read())
//                {
//                    districts[read.GetInt32(2).ToString()] = read.GetString(1);
//                }
//                read.Close();

//                var sql = @" UPDATE {0} set districtName = '{1}' WHERE GID IN  (
//                                            SELECT m.gid FROM {0} as m  , city as c
//                                            where st_intersects(st_setsrid(m.geom,4326),st_setsrid(c.geom,4326))   and c.gid = {2}
//                                            )  ";

//                districts.Keys.ToList().ForEach(
//                    m =>
//                        {
//                            command.CommandText = (string.Format(sql, table, districts[m], m));
//                            command.ExecuteNonQuery();
//                        });
//            }
//        }

//        /// <summary>
//        /// 查询POI 数据中区号为空的数据
//        /// </summary>
//        [Test]
//        public void CheckData()
//        {
//            var tables = GetTables("poi");
//            var dbHelper = new DbHelper(this.con);
//            foreach (string table in tables)
//            {
//                var rowCount =
//                    int.Parse(
//                        dbHelper.ExecuteScalar(
//                            string.Format("SELECT COUNT(1) FROM {0} WHERE districtname IS NULL", table)).ToString());
//                if (rowCount != 0)
//                {
//                    Console.WriteLine(table + "," + rowCount);
//                }
//            }
//        }

//        /// <summary>
//        /// 将数据库路口数据和CityRoadConfig 数据融合
//        /// </summary>
//        [Test]
//        public void UnionCityRoads()
//        {
//            var tempCitys = citys.Replace('\r', ' ').Split(';');
//            var data = new ProviceCityPinYin().GetProviceCities();
//            var roadcross = this.GetTables("roadcross");
//            var connection = new NpgsqlConnection(con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            command.CommandType = CommandType.Text;
//            string sql = @"SELECT  distinct first_name
//                      FROM {0}
//                     union 
//                     SELECT  distinct second_nam
//                      FROM {0}";
//            CityRoad roadCity;
//            List<string> list = new List<string>();
//            foreach (string city in tempCitys)
//            {
//                list.Clear();
//                var provice = city.Split(',')[0].Trim();
//                var cityName = city.Split(',')[1].Trim();
//                var table = data[cityName].CityPinyin + "_roadcross";
//                if (!roadcross.Contains(table))
//                {
//                    Console.WriteLine("ERROR:" + cityName);
//                    continue;
//                }
//                command.CommandText = string.Format(sql, table);
//                var reader = command.ExecuteReader();
//                roadCity = CityRoadConfig.GetInstance().GetRoadName(cityName);
//                while (reader.Read())
//                {
//                    list.Add(reader.GetString(0));
//                }
//                Console.WriteLine(cityName + ":" + list.Count);
//                // WriteFile(list, cityName, provice);
//                list.AddRange(roadCity.Roads);
//                list = list.Distinct().ToList();
//                list.Sort((m, n) => { return string.Compare(m, n); });
//                roadCity.Roads = list;
//            }
//            CityRoadConfig.GetInstance().SaveConfig();
//        }

//        private void WriteFile(List<string> list, string cityName, string provice)
//        {
//            var directory =
//                Path.Combine(
//                    @"F:\map\NPGIS\trunk\tools\NPGISMapTileDown\GetTileImage\NetposaTest\bin\Release\shap",
//                    provice);
//            using (var fileStream = new FileStream(Path.Combine(directory, cityName + ".txt"), FileMode.OpenOrCreate))
//            {
//                fileStream.Seek(0, SeekOrigin.Begin);
//                var buffer = System.Text.Encoding.UTF8.GetBytes(string.Join(",", list.ToArray()));
//                fileStream.Write(buffer, 0, buffer.Length);
//            }
//        }

//        /// <summary>
//        /// 判断是否每个城市都有POI ROADNET ROADCROSS 三种数据
//        /// </summary>
//        [Test]
//        public void CheckDbData()
//        {
//            var provices = new ProviceCityPinYin().GetProviceCities();
//            var tables = this.GetTables(null);
//            foreach (var proviceCity in provices)
//            {
//                var pinyin = proviceCity.Value.CityPinyin;
//                if (tables.Where(m => m.Contains(pinyin)).ToList().Count != 3)
//                {
//                    Console.WriteLine(proviceCity.Key);
//                }
//            }
//        }

//        /// <summary>
//        /// SQL 数据入库
//        /// </summary>
//        [Test]
//        public void InsertBeiJingData()
//        {
//            var file =
//                @"F:\map\NPGIS\trunk\tools\NPGISMapTileDown\GetTileImage\NetposaTest\bin\Release\shap\广东省\广州\guangzhou_poi.sql";

//            var connection = new NpgsqlConnection(con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            command.CommandType = CommandType.Text;
//            using (var fileRead = new System.IO.StreamReader(file, Encoding.UTF8))
//            {
//                while (!fileRead.EndOfStream)
//                {
//                    var line = fileRead.ReadLine();
//                    if (!string.IsNullOrEmpty(line))
//                    {
//                        command.CommandText = line;
//                        try
//                        {
//                            command.ExecuteNonQuery();
//                        }
//                        catch (Exception)
//                        {
//                            Console.WriteLine(line);
//                        }
//                    }
//                }

//            }
//        }

//        /// <summary>
//        /// 广州POI 数据入库 EXCEL TO DB
//        /// </summary>
//        [Test]
//        public void AddGuangzhouPoi()
//        {
//            string file =
//                @"F:\map\NPGIS\trunk\tools\NPGISMapTileDown\GetTileImage\NetposaTest\bin\Release\shap\广东省\广州\guangzhou_poi.xlsx";
//            var dataTable = AsposeCellsHelper.ExportToDataTable(file, true);
//            string sql =
//                @"INSERT INTO guangzhou_poi (name,x,y,r_addr,type,phone) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')";
//            var connection = new NpgsqlConnection(con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            command.CommandType = CommandType.Text;
//            var insertSql = "";
//            foreach (DataRow row in dataTable.Rows)
//            {
//                insertSql = string.Format(
//                    sql,
//                    row["name"] ?? string.Empty,
//                    row["X"] ?? string.Empty,
//                    row["Y"] ?? string.Empty,
//                    row["r_addr"] ?? string.Empty,
//                    row["type"] ?? string.Empty,
//                    row["phone"] ?? string.Empty);

//                try
//                {
//                    command.CommandText = insertSql;
//                    command.ExecuteNonQuery();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(insertSql);
//                }
//            }
//        }

//        /// <summary>
//        /// 检测POI 和RoadCross 数据
//        /// </summary>
//        [Test]
//        public void CheckPoiAndRoadCorss()
//        {
//            var roadNets = this.GetTables("roadcross");
//            var pois = this.GetTables("poi");
//            var dbHelper = new DbHelper(this.con);
//            foreach (string roadNet in roadNets)
//            {
//                if (dbHelper.GetTableColumns(roadNet).Count(m => m == "quanpin" || m == "szm") != 2)
//                {
//                    Console.WriteLine(roadNet);
//                }
//            }

//            foreach (string roadNet in pois)
//            {
//                if (dbHelper.GetTableColumns(roadNet).Count(m => m == "districtname" || m == "quanpin" || m == "szm")
//                    != 3)
//                {
//                    Console.WriteLine(roadNet);
//                }
//            }
//        }

//        /// <summary>
//        /// 路网数据检测
//        /// </summary>
//        [Test]
//        public void CheckRoadNet()
//        {
//            var roadNets = this.GetTables("roadnet");

//            var dbHelper = new DbHelper(this.con);
//            foreach (string roadNet in roadNets)
//            {
//                if (dbHelper.GetTableColumns(roadNet).Count(m => m == "length") != 1)
//                {
//                    Console.WriteLine(roadNet);
//                }
//            }
//        }


//        [Test]
//        public void DbToSql()
//        {
//            var provices = new ProviceCityPinYin().GetProviceCities();
//            foreach (KeyValuePair<string, ProviceCity> keyValuePair in provices)
//            {
//                keyValuePair.Value.GetDbToSql();
//            }
//        }

//        [Test]
//        public void CheckSqlData()
//        {
//            var provices = new ProviceCityPinYin().GetProviceCities();
//            foreach (KeyValuePair<string, ProviceCity> keyValuePair in provices)
//            {
//                keyValuePair.Value.CheckData();
//            }
//        }

//        [Test]
//        public void GenerateReadMe()
//        {
//            var provices = new ProviceCityPinYin().GetProviceCities();
//            foreach (KeyValuePair<string, ProviceCity> keyValuePair in provices)
//            {
//                keyValuePair.Value.GenerateReadMe();
//            }
//        }

//        [Test]
//        public void GetNullData()
//        {
//            var provices = new ProviceCityPinYin().GetProviceCities();
//            foreach (KeyValuePair<string, ProviceCity> keyValuePair in provices)
//            {
//                keyValuePair.Value.GetNullData();
//            }
//        }

//        [Test]
//        public void DownWuhuRoadCross()
//        {
//            var provices = new ProviceCityPinYin().GetProviceCities();
//            if (provices.ContainsKey("常州"))
//            {
//                provices["常州"].DownLoadRoadCross();
//            }
//        }

//        /// <summary>
//        /// 整理全国城市拼音名称 方便今后做数据录入
//        /// </summary>
//        [Test]
//        public void AddCityPinYing()
//        {
//            var list = CityConfig.GetInstance().Countryconfig.countries;
//            foreach (var province in list)
//            {
//                province.cities.ForEach(
//                    m =>
//                        {
//                            if (string.IsNullOrEmpty(m.pinyin))
//                            {
//                                var pinyin = GetPinyin(m.name.TrimEnd('市')).ToLower();
//                                if (!string.IsNullOrEmpty(pinyin))
//                                {
//                                    m.pinyin = pinyin;
//                                }
//                                else
//                                {
//                                    Console.WriteLine(m.name);
//                                }
//                            }
//                        });
//            }
//            CityConfig.GetInstance().SaveConfig();
//        }

//        /// <summary>
//        /// 执行pgrouting 扩展
//        /// </summary>
//        [Test]
//        public void ExecutePostGis()
//        {
//            var tables = this.GetTables("roadnet");
//            var connection = new NpgsqlConnection(this.con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            command.CommandType = CommandType.Text;
//            command.CommandTimeout = 1000 * 6;
//            foreach (var table in tables)
//            {
//                Console.WriteLine(table);
//                try
//                {
//                    command.CommandText = string.Format(@"
//                ALTER TABLE {0} ADD COLUMN source integer; 
//                ALTER TABLE {0} ADD COLUMN target integer;
//                ", table);
//                    command.ExecuteNonQuery();
//                }
//                catch (Exception)
//                {
//                }

//                command.CommandText = string.Format("SELECT pgr_createTopology('{0}',0.00001, 'geom', 'gid');", table);
//                command.ExecuteNonQuery();
//            }
//        }

//        /// <summary>
//        /// 北京POI 下载
//        /// </summary>
//        [Test]
//        public void DownLoadBeiJingPoi()
//        {
//            var city = new ProviceCityPinYin();
//            city.GetProviceCities()["北京"].ExcelToShap();
//            city.GetProviceCities()["北京"].ShapToSql();
//        }

//        [Test]
//        public void GetPoiCount()
//        {
//            var tables = this.GetTables("poi");
//            var connection = new NpgsqlConnection(this.con);
//            connection.Open();
//            var command = connection.CreateCommand();
//            command.CommandType = CommandType.Text;
//            foreach (var table in tables)
//            {
//                command.CommandText = string.Format("SELECT COUNT(1) FROM {0}", table);
//                Console.WriteLine(string.Format("{0}_{1}", table, command.ExecuteScalar()));
//            }
//        }

//        /// <summary>
//        /// 淄博数据下载
//        /// </summary>
//        [Test]
//        public void DownLoadZiBo()
//        {
//            var city = new ProviceCityPinYin();
//            var data = city.GetProviceCities()["淄博"];
//            //city.GetProviceCities()["淄博"].DownLoadPoi();
//            //city.GetProviceCities()["淄博"].DownLoadRoadLine();

//            //city.GetProviceCities()["淄博"].ExcelToShap();

//            //data.ShapToSql();

//            // data.SqlToDb();

//            //data.ExecuteRoadNet();

//            data.GetDbToSql();
//        }
//        [Test]
//        public void CreatePoiIndex()
//        {
//            var city = new ProviceCityPinYin();
//            city.CreatePoiIndex();
//        }
//         [Test]
//        public void CreateIndex()
//        {
//            new ProviceCityPinYin().CreateIndex();
//        }
//        [Test]
//        public void RoadNetPinying()
//        {
//            DbHelper dbHelper = new DbHelper("Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = routing;");
//            var reader = dbHelper.ExecuteReader(
//                 "SELECT  TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE  TABLE_NAME like '%_roadnet'");
//            var tables = new List<string>();
//            while (reader.Read())
//            {
//                tables.Add(reader.GetString(0));
//            }
//            reader.Close();
//            var connection = new NpgsqlConnection(this.con);
//            connection.Open();
//            var readcon = new NpgsqlConnection(this.con);
//            readcon.Open();
//            var command = connection.CreateCommand();
//            var readcommand = readcon.CreateCommand();
//            readcommand.CommandType = CommandType.Text;
//            PinyinHelper helper;
//            foreach (var table in tables)
//            {
//                dbHelper.AddColumn("quanpin", table);
//                dbHelper.AddColumn("szm", table);

//                Console.WriteLine(table);
//                command.CommandType = CommandType.Text;
//                command.CommandTimeout = 1000 * 60;
//                command.CommandText = string.Format("SELECT COUNT(1) FROM {0} where quanpin is null", table);
//                int rowCount = int.Parse(command.ExecuteScalar().ToString());
//                if (rowCount != 0)
//                {
//                    var max = rowCount / 2000 + 1;

//                    Console.WriteLine(max);
//                    for (int i = 0; i < max; i++)
//                    {
//                        Console.WriteLine(i);
//                        readcommand.CommandText = string.Format("SELECT gid,name from {0} where (name is not null) and  (quanpin is null)  limit 2000", table);
//                        reader = readcommand.ExecuteReader(CommandBehavior.CloseConnection);
//                        while (reader.Read())
//                        {
//                            var gid = reader.GetInt32(0);
//                            var name = reader.GetString(1);
//                          helper =  TextToPinyin.Convert(name);
//                            command.CommandText = string.Format(
//                                "UPDATE {0} SET quanpin='{1}',szm='{2}' where gid = {3}",
//                                table,
//                                helper.Pinyin,
//                                helper.Szm,
//                                gid);
//                            command.ExecuteNonQuery();
//                        }
//                       // reader.Close();
//                    }
                    
//                }
//            }
//        }

//        [Test]
//        public void TestTextToPinyin()
//        {
//            System.Console.WriteLine(TextToPinyin.Convert("中国移动(安徽有限公司六安市城南镇营业部)"));
//            System.Console.WriteLine(TextToPinyin.Convert("西安高新6路"));
//        }
//        [Test]
//        public void ReadPng()
//        {

//            string name = "16";
//            string file = @"E:\芜湖市矢量图\芜湖市矢量图\" + name + "_0";
//            System.IO.FileStream stream = new FileStream(file,FileMode.Open);
//            var buffer = new byte[stream.Length];
//            stream.Read(buffer, 0, buffer.Length);
//            stream.Close(); 

//            var temp =
//                string.Join(",", buffer.Select(m => m.ToString()))
//                    .Replace("137,80,78,71,13", "&")
//                    .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
//            int i = 0;
//            System.IO.Directory.CreateDirectory(@"E:\芜湖市矢量图\" + name  );
//            temp.ToList().ForEach(
//                m =>
//                    {
//                        var bytes = ("137,80,78,71,13," + m.Trim(',')).Split(',').Select(s => byte.Parse(s)).ToArray();
//                        Image.FromStream(new MemoryStream(bytes))
//                            .Save(@"E:\芜湖市矢量图\" + name + "\\" + (i++) + ".png");
//                    });


//            stream = new FileStream(@"C:\Users\Administrator\Pictures\下载.png", FileMode.Open);
//            var  buffer1 = new byte[stream.Length];
//            stream.Read(buffer1, 0, buffer1.Length);
//            stream.Close();
//        }

//        [Test]
//        public void ReadPngIndex()
//        {
//            string file = @"E:\芜湖市矢量图\芜湖市矢量图\4_png";
//            System.IO.FileStream stream = new FileStream(file, FileMode.Open);
//            var buffer = new byte[stream.Length / 2];
//            stream.Read(buffer, 0, buffer.Length);
//            stream.Close();

//            var result = System.Text.UTF8Encoding.Default.GetString(buffer);
//            Console.WriteLine(result);

//        }

//        [Test]
//        public void CreateTiff()
//        {

//            var encoderParam1 = new EncoderParameter(
//                System.Drawing.Imaging.Encoder.SaveFlag,
//                (long)EncoderValue.MultiFrame);

//            var encoderParam2 = new EncoderParameter(
//                System.Drawing.Imaging.Encoder.Compression,
//                (long)EncoderValue.CompressionNone);

//            var encoderParams = new EncoderParameters(2);
//            encoderParams.Param[0] = encoderParam1;
//            encoderParams.Param[1] = encoderParam2;

//            string dir = @"E:\芜湖市矢量图\4";
//            var fs = Directory.GetFiles(dir).ToList();
//            Image image = null;
//            for (int i = 0; i < fs.Count; i++)
//            {
//                if (i == 0)
//                {
//                    image = new Bitmap(256 * 3, 256 * 3, PixelFormat.Format24bppRgb);
//                }
//                else
//                {
//                    image.SaveAdd(Image.FromStream(new FileStream(fs[0], FileMode.Open)), encoderParams);
//                }
//            }
//            image.Save(@"E:\芜湖市矢量图\4\big.png");
//        }
//        [Test]
//        public void TifHelper_Test()
//        {
//            new TifHelper().GetMultifyElevation("");
//        }
        [Test]
        public void TestSql()
        {
            string filePath = @"C:\Program Files\PostgreSQL\9.4\bin\sql\安徽省\安庆\anhui_anqing_poi.sql";
            using (var fileStream = new FileStream(filePath,FileMode.Open))
            {
                StreamReader reader = new StreamReader(fileStream,Encoding.GetEncoding("gbk"));
                StringBuilder createTableBuilder = new StringBuilder();
                bool isEnd = false;
                MemoryStream memoryStream = new MemoryStream();
                byte[] buffer;
                while (!reader.EndOfStream)
                {
                    var value = reader.ReadLine();
                    if (!isEnd && value.StartsWith("COPY"))
                    {
                        isEnd = true;
                        continue;
                    }
                    if (!isEnd)
                    {
                        createTableBuilder.Append(value + System.Environment.NewLine);
                    }
                    else
                    {
                        buffer = Encoding.GetEncoding("gbk").GetBytes(value + System.Environment.NewLine);
                        memoryStream.Write(buffer,0,buffer.Length);
                    }
                }
               buffer = memoryStream.ToArray();
                filePath = @"C:\Program Files\PostgreSQL\9.4\bin\sql\安徽省\安庆\anhui_anqing_poi.txt";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                new FileStream(filePath, FileMode.Create).Write(buffer, 0, buffer.Length);
               Console.Write(createTableBuilder.ToString());
            }
        }
        [Test]
        public void Post_Copy_In()
        {
            var con = new NpgsqlConnection("Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = Road;");
            con.Open();
            var cmd = new NpgsqlCommand(@"COPY anhui_anqing_poi (gid, name, x, y, r_addr, type, phone, geom, quanpin, szm, districtname) FROM stdin ", con);

            var filePath = @"C:\Users\Administrator\Desktop\anhui_anqing_poi.txt";
                // @"C:\Program Files\PostgreSQL\9.4\bin\sql\安徽省\安庆\anhui_anqing_poi_1.txt";
            var stream = new FileStream(filePath, FileMode.Open);
            var cin = new Npgsql.NpgsqlCopyIn(cmd, con, stream);
            cin.Start(); 
            cin.End();
        }
        [Test]
        public void Post_Copy_Out()
        {
            var con = new NpgsqlConnection("Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = routing;");
            con.Open();

            var cmd = new NpgsqlCommand("COPY anhui_anqing_poi  (gid, name, x, y, r_addr, type, phone, geom, quanpin, szm, districtname) TO STDOUT ", con);
            var filePath = @"C:\Program Files\PostgreSQL\9.4\bin\sql\安徽省\安庆\anhui_anqing_poi_out.txt";
            var stream = new FileStream(filePath, FileMode.Create);
            var cin = new Npgsql.NpgsqlCopyOut(cmd, con, stream);
            if (File.Exists(filePath))
            {
                //File.Delete(filePath);
            }
           
            cin.Start();
           // cin.CopyStream
            cin.End();
          Console.Write("d");
        }
        [Test]
        public void Test_Db_Out()
        {
            string msg = "copy {0}({1}) to 'c:/postgresData/{0}.csv' delimiter as ',' NULL AS 'NULL' csv quote as '\"'";
            var citys = new string[] { "北京", "上海", "重庆", "天津" };
            var directories = System.IO.Directory.GetDirectories(@"C:\Program Files\PostgreSQL\9.4\bin\sql");
            var farMachine = @"\\192.168.60.242\postgresData\";
            var table = new System.Collections.Generic.Dictionary<string, string>(); //string[] { "roadcross", "poi", "roadnet" };
            table["roadcross"] =  "gid, first_name, second_nam, x, y, name, geom, quanpin, szm";
            table["poi"] =  "gid,name,x,y,r_addr,type,phone,districtname,geom,quanpin,szm";
            table["roadnet"] = "gid, name, width, type, geom, length, car, walk, speed, highspeed,np_level, quanpin, szm";
            var con = new NpgsqlConnection("Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = routing;");
          
            con.Open(); 

            directories.ToList().ForEach(
                m =>
                    {
                        var privoce = m.Substring(m.LastIndexOf('\\') + 1);
                        Console.Write(privoce);
                        if (Directory.GetDirectories(m).Length > 0)
                        {
                            
                            Directory.GetDirectories(m).ToList().ForEach(
                                f =>
                                    {
                                       var city = f.Substring(f.LastIndexOf('\\') + 1);
                                        var pinyin = new FileInfo(Directory.GetFiles(f, "*.sql")[0]).Name;
                                        var cityPath = string.Format("{0}/{1}/{2}", farMachine, privoce, city);
                                        if (!Directory.Exists(cityPath))
                                        {
                                            Directory.CreateDirectory(cityPath);
                                        }
                                       // Directory.GetFiles(string.Format("{0}/{1}/{2}", farMachine,privoce,city), "*.csv").ToList().ForEach(csv => File.Delete(csv));
                                        pinyin = pinyin.Substring(0, pinyin.LastIndexOf('_'));
                                        table.ToList().ForEach(
                                            t =>
                                                {
                                                    var file = string.Format(
                                                        "{0}/{3}_{4}.csv",
                                                        farMachine,
                                                        privoce,
                                                        city,
                                                        pinyin,
                                                        t.Key);
                                                  var stream =  File.Create(file,1,FileOptions.Asynchronous);

                                                    var cmd = new NpgsqlCommand(string.Format(msg, string.Format("{0}_{1}", pinyin, t.Key),t.Value), con);
                                                    cmd.CommandTimeout = 214748;
                                                    Console.WriteLine(cmd.CommandText);
                                                    try
                                                    {
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex);
                                                    }

                                                   
                                                    //var cin = new Npgsql.NpgsqlCopyOut(
                                                    //    cmd,
                                                    //    con,
                                                    //    stream);
                                                    //cin.Start();
                                                    //cin.End();
                                                    stream.Close();
                                                    System.Threading.Thread.Sleep(50);
                                                });
                                    });
                        }
                        else
                        {
                           
                           // Directory.GetFiles(string.Format("{0}/{1}", farMachine, privoce), "*.csv").ToList().ForEach(csv => File.Delete(csv));
                            var pinyin =  new FileInfo(Directory.GetFiles(m, "*.sql")[0]).Name;
                            pinyin = pinyin.Substring(0, pinyin.LastIndexOf('_'));
                            table.ToList().ForEach(
                                t =>
                                    {
                                        var file = string.Format("{0}/{3}_{4}.csv", farMachine, privoce, "", pinyin, t.Key);

                                        var stream = File.Create(file, 1, FileOptions.Asynchronous);
                                        
                                        var cmd =
                                            new NpgsqlCommand(
                                                string.Format(msg, string.Format("{0}_{1}", pinyin, t.Key), t.Value),
                                                con);
                                        Console.WriteLine(cmd.CommandText);
                                        cmd.CommandTimeout = 214748;
                                        cmd.ExecuteNonQuery();
                                        //var cin = new Npgsql.NpgsqlCopyOut(cmd, con, stream);
                                        //cin.Start();
                                        //cin.End();
                                        stream.Close();
                                    });
                        }
                    });
            con.Close();

        }
        [Test]
        public void MoveCsv()
        {
            var directories = System.IO.Directory.GetDirectories(@"C:\Program Files\PostgreSQL\9.4\bin\sql");
            var table = new string[] { "roadcross", "poi", "roadnet" };
            var farMachine = @"\\192.168.60.242\postgresData2\";
            directories.ToList().ForEach(
               m =>
               {
                   var privoce = m.Substring(m.LastIndexOf('\\') + 1);
                   Console.Write(privoce);
                   if (Directory.GetDirectories(m).Length > 0)
                   {

                       Directory.GetDirectories(m).ToList().ForEach(
                           f =>
                           {
                               var city = f.Substring(f.LastIndexOf('\\') + 1);
                                var pinyin = new FileInfo(Directory.GetFiles(f, "*.sql")[0]).Name;
                                pinyin = pinyin.Substring(0, pinyin.LastIndexOf('_'));
                               table.ToList().ForEach(
                                   t =>
                                       {
                                           var source = string.Format("{0}\\{1}_{2}.csv", farMachine, pinyin, t);
                                           var target = Path.Combine(f, string.Format("{0}_{1}.csv", pinyin, t));
                                           if (File.Exists(source))
                                           {
                                               if (File.Exists(target))
                                               {
                                                   File.Delete(target);
                                               }
                                               new FileInfo(source).MoveTo(Path.Combine(f,string.Format("{0}_{1}.csv",pinyin,t)));
                                           }
                                       });
                           });
                   }
                   else
                   {
                       var pinyin = new FileInfo(Directory.GetFiles(m, "*.sql")[0]).Name;
                       pinyin = pinyin.Substring(0, pinyin.LastIndexOf('_'));
                       table.ToList().ForEach(
                           t =>
                           {
                               var source = string.Format("{0}\\{1}_{2}.csv", farMachine, pinyin, t);
                               var target = Path.Combine(m, string.Format("{0}_{1}.csv", pinyin, t));
                               if (File.Exists(source))
                               {
                                   if (File.Exists(target))
                                   {
                                       File.Delete(target);
                                   }
                                   new FileInfo(source).MoveTo(target);
                               }
                           });
                   }
               });
        }
    }
}
