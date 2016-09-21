using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetposaTest
{
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.IO;

    using MapDataTools;
    using MapDataTools.Util;

    using Npgsql;

    using OSGeo.OGR;

    public enum TableType
    {
        [Description("roadcross")]
        RoadCorss,

        [Description("roadnet")]
        RoadNet,

        [Description("poi")]
        Poi
    }

    public class ProviceCity
    {
        public string Provice { get; set; }

        public string City { get; set; }

        public string CityPinyin { get; set; }

        public Extent Extent { get; set; }

        public string BasePath = @"C:\Program Files\PostgreSQL\9.4\bin\sql";

        /// <summary>
        /// readme 文件生成
        /// </summary>
        public void GenerateReadMe()
        {
            string file = Path.Combine(BasePath, City);
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(file);
            }
            file = Path.Combine(file, "readme.txt");
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            string content = @"sql			表名			备注
{0}.sql		{0}		{3}POI数据
{1}.sql		{1}		{3}路网数据
{2}.sql		{2}		{3}路口数据";
            using (var stream = new FileStream(file, FileMode.OpenOrCreate))
            {
                var buffer =
                    UTF8Encoding.UTF8.GetBytes(
                        string.Format(
                            content,
                            CityPinyin + "_poi",
                            CityPinyin + "_roadnet",
                            CityPinyin + "_roadcross",
                            City));
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// db to sql
        /// </summary>
        /// <param name="dbHost"></param>
        /// <param name="dbUser"></param>
        /// <param name="dbase"></param>
        public void GetDbToSql(string dbHost = "192.168.60.242", string dbUser = "postgres", string dbase = "routing")
        {
            var list = new string[] { "roadcross", "roadnet", "poi" };
            string cmd;
            foreach (string s in list)
            {
                string table = string.Format("{0}_{1}", CityPinyin, s).ToLower();
                string file = Path.Combine(BasePath, City);
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                if (File.Exists(Path.Combine(BasePath, string.Format("{0}.sql", table))))
                {
                    Console.WriteLine(table);
                    continue;
                }
                cmd = string.Format(
                    "pg_dump -h {1} -U {2} -t {0} -f  sql/{4}/{0}.sql  {3}",
                    table,
                    dbHost,
                    dbUser,
                    dbase,
                    City);
                this.RunCmd(cmd);
                Console.WriteLine(cmd);
                System.Threading.Thread.Sleep(500);
            }
        }

        private string con = "Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = routing;";

        public void GetNullData()
        {
            DbHelper dbHelper = new DbHelper(con);
            var count =
                dbHelper.ExecuteScalar(
                    string.Format("SELECT count(1)   FROM {0}_poi where districtname is null ", this.CityPinyin));
            Console.WriteLine(string.Format("{0}:{1}", this.City, count));
        }

        /// <summary>
        /// 数据检测
        /// </summary>
        public void CheckData()
        {
            var list = new string[] { "roadcross", "roadnet", "poi" };

            foreach (string s in list)
            {
                string table = string.Format("{0}_{1}.sql", CityPinyin, s).ToLower();
                string file = Path.Combine(BasePath, City);
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                if (!File.Exists(Path.Combine(file, table)))
                {
                    Console.WriteLine(table);
                }
            }
        }

        private void RunCmd(string msg)
        {
            string cmdtext = @"C:"; //CD C:\Program Files\PostgreSQL\9.4\bin
            Process MyProcess = new Process();
            //设定程序名 
            MyProcess.StartInfo.FileName = "cmd.exe";
            //关闭Shell的使用 
            MyProcess.StartInfo.UseShellExecute = false;
            //重定向标准输入 
            MyProcess.StartInfo.RedirectStandardInput = true;
            //重定向标准输出 
            MyProcess.StartInfo.RedirectStandardOutput = true;
            //重定向错误输出 
            MyProcess.StartInfo.RedirectStandardError = true;
            //设置不显示窗口 
            MyProcess.StartInfo.CreateNoWindow = true;
            //执行VER命令 
            MyProcess.Start();
            MyProcess.StandardInput.WriteLine(cmdtext);
            MyProcess.StandardInput.WriteLine(@"CD C:\Program Files\PostgreSQL\9.4\bin");
            MyProcess.StandardInput.WriteLine(msg);
        }

        public string GetCityDirectory()
        {
            var directory = Path.Combine(
                  AppDomain.CurrentDomain.BaseDirectory,
                    "shap\\" + this.Provice);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            directory = Path.Combine(directory, this.City);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
        #region 数据下载
        /// <summary>
       /// 路网数据下载
       /// </summary>
        public void DownLoadRoadLine()
        {
            GaoDeRoads roads = new GaoDeRoads();
            var roaddataTable = new DataTable(this.City);
            roaddataTable.Columns.AddRange(
                new[]
                    {
                        new DataColumn("Name"), 
                        new DataColumn("WIDTH"), 
                        new DataColumn("TYPE"), 
                        new DataColumn("PATH")
                    });

            Console.WriteLine("下载{0}中", City);
            roads.downOverHandler += () =>
                {
                    var directory = this.GetCityDirectory();
                    var path = Path.Combine(directory, this.CityPinyin + "_roadNet.shp");
                    ShpFileHelper.saveShpFile(
                        roaddataTable,
                        path,
                        wkbGeometryType.wkbLineString,
                        ProjectConvert.GAODE84_WGS);
                    roaddataTable.Rows.Clear();
                };
            roads.roadDateDowningHandler += (RoadModel road, int index, int count) =>
                {
                    for (int i = 0; i < road.paths.Count; i++)
                    {
                        DataRow row = roaddataTable.NewRow();
                        row[0] = road.name;
                        row[1] = road.width;
                        row[2] = road.type;
                        row[3] = road.paths[i];
                        roaddataTable.Rows.Add(row);
                    }
                };
            roads.downLoadRoadsByCityName(City);
        }
        /// <summary>
        /// POI 下载
        /// </summary>
        public void DownLoadPoi()
        {
            if (this.Extent == null)
            {
                Console.WriteLine(string.Format("{0}范围为空", this.City));
                return;
            }
            BaiduMap baiduMap = new BaiduMap();
            var dataTable = new DataTable();

            dataTable.Columns.Add("name", Type.GetType("System.String"));
            dataTable.Columns.Add("X", Type.GetType("System.String"));
            dataTable.Columns.Add("Y", Type.GetType("System.String"));
            dataTable.Columns.Add("r_addr", Type.GetType("System.String"));
            dataTable.Columns.Add("type", Type.GetType("System.String"));
            dataTable.Columns.Add("phone", Type.GetType("System.String"));
            baiduMap.downingPOIHandler += (poi, index, count) =>
            {
                DataRow row = dataTable.NewRow();
                row["name"] = poi.name.Replace("'"," ");
                row["X"] = poi.cx;
                row["Y"] = poi.cy;
                row["r_addr"] = poi.address.Replace(",", "");
                row["type"] = poi.type;
                row["phone"] = poi.phone.Replace(",", ";");
                dataTable.Rows.Add(row);
            };
            baiduMap.downEndPOIHandler += message =>
                {
                    var directory = this.GetCityDirectory();
                    AsposeCellsHelper.ExportToExcel(dataTable,Path.Combine(directory, City + "_poi.xlsx"));
                    dataTable.Rows.Clear();
                };
            baiduMap.getPOIByExtentKeyWords(this.Extent, string.Empty);
        }
        /// <summary>
        /// 路口数据下载
        /// </summary>
        public void DownLoadRoadCross()
        {
            var crossDataTable = new DataTable();
            crossDataTable.Columns.Add("first_name", Type.GetType("System.String"));
            crossDataTable.Columns.Add("second_name", Type.GetType("System.String"));

            crossDataTable.Columns.Add("x", Type.GetType("System.String"));
            crossDataTable.Columns.Add("y", Type.GetType("System.String"));
            crossDataTable.Columns.Add("name", Type.GetType("System.String"));

            var directory = this.GetCityDirectory();

            var path = Path.Combine(directory, this.CityPinyin + "_RoadCross.xlsx");

            var road = new GaoDeRoads();
            road.roadCrossDowningHandler += (roadCross, index, count) =>
                {
                    DataRow row = crossDataTable.NewRow();
                    row["first_name"] = roadCross.first_name;
                    row["second_name"] = roadCross.second_name;
                    row["x"] = roadCross.wgs_x;
                    row["y"] = roadCross.wgs_y;
                    row["name"] = string.Format("{0}_{1}", roadCross.first_name, roadCross.second_name);
                    crossDataTable.Rows.Add(row);
                };
            road.downOverHandler += () =>
                {
                    try
                    {
                        AsposeCellsHelper.ExportToExcel(crossDataTable, path);
                        crossDataTable.Rows.Clear();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error" + City);
                    }
                };
            road.downLoadRoadCrossByCityName(City);
        }

        #endregion

        #region 数据入库
        /// <summary>
        /// excel 导成shap
        /// </summary>
        public void ExcelToShap ()
        {
            var files = Directory.GetFiles(this.GetCityDirectory(), "*.xls");
            foreach (var file in files)
            {
                var dataTable = AsposeCellsHelper.ExportToDataTable(file,true);

                var shpPath = Path.Combine(
                    this.GetCityDirectory(),
                    string.Format("{0}_{1}.shp", this.CityPinyin, file.Contains("roadcross") ? "roadcross" : "poi"));
                ShpFileHelper.saveShpFile(
                    dataTable,
                    shpPath,
                    file.Contains("roadnet") ? wkbGeometryType.wkbLineString : wkbGeometryType.wkbPoint,
                    ProjectConvert.NONE);
            }
        }
        /// <summary>
        /// shap 文件导成 sql 文件准备录入数据库
        /// </summary>
        public void ShapToSql()
        {
            var shap2sql = "shp2pgsql -W \"gbk\" {0} >{1}";

            Directory.GetFiles(this.GetCityDirectory(), "*.shp").ToList().ForEach(
                m =>
                    {
                        var fileInfo = new FileInfo(m);
                        var targetFile = Path.Combine(
                            fileInfo.DirectoryName,
                            fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".")) + ".sql");
                        if (!File.Exists(targetFile))
                        {
                            string msg = string.Format(shap2sql, m, targetFile);
                            RunCmd(msg);
                            System.Threading.Thread.Sleep(1000);
                        }
                       // this.RunCmd(string.Format(shap2sql, m));
                    });
        }
        /// <summary>
        /// sql 导入到数据库
        /// </summary>
        public void SqlToDb()
        {
             var sqlTodb = "psql -h 192.168.60.242  -d routing  -U postgres -f  {0}";
            Directory.GetFiles(this.GetCityDirectory(), "*.sql").ToList().ForEach(
                m =>
                    {
                        this.RunCmd(string.Format(sqlTodb, m));
                    });
        }
        #endregion

        #region 数据处理
        /// <summary>
        /// 执行路网扩展
        /// </summary>
        public void ExecuteRoadNet()
        {
            string msg = @"
--select distinct highspeed  from shanghai_roadnet_supermap;

--- 新增表字段
alter table xian_roadNet add length double precision;
alter table xian_roadNet add car integer;
alter table xian_roadNet add walk integer;
alter table xian_roadNet add speed double precision;
alter table xian_roadNet add highspeed integer;
alter table xian_roadNet add np_level integer;

-- 道路长度计算
update xian_roadNet set length=st_length(st_transform(st_setsrid(geom,4326),900913)) ;

-- 道路分级
--select distinct type from  xian_roadNet ;

-- 设置道路等级

-- 设置道路速度
update xian_roadNet set speed = 100,np_level=1 where type = '高速公路';
update xian_roadNet set speed = 70,np_level =2 where type = '主要道路（城市主干道）';
update xian_roadNet set speed = 50,np_level =3 where type = '次要道路（城市次干道）';
update xian_roadNet set speed = 50,np_level =3 where type = '一般道路';
update xian_roadNet set speed = 50,np_level =4 where type = '城市环路/城市快速路';
update xian_roadNet set speed = 18,np_level =4 where type = '非导航道路';

update xian_roadNet set speed = 60,np_level =3 where type = '国道';
update xian_roadNet set speed = 80,np_level =2 where type = '省道';
update xian_roadNet set speed = 40,np_level =3 where type = '县道';
update xian_roadNet set speed = 20,np_level =4 where type = '区县内部道路';
update xian_roadNet set speed = 18,np_level =4 where type = '乡村道路';

-- 设置高速道路
update xian_roadNet set highspeed = 0;
update xian_roadNet set highspeed = 1 where np_level in (1);

-- 设置道路是否可步行和通车
update xian_roadNet set walk = 0 , car = 0 ;
update xian_roadNet set walk = 0 , car = 1 where np_level in (1);
update xian_roadNet set walk = 1 , car = 1 where np_level != 1
";
            var dbHelper = new DbHelper(this.con);
             
                var sql = msg.Replace("xian_roadNet", this.CityPinyin + "_roadNet");
                dbHelper.ExecuteNonQuery(sql);
          
        }
        #endregion

    }

    public class ProviceCityPinYin
    {
        #region data
        private string extents = @"
乌鲁木齐;87.516452787599,43.763635565918,87.674381254396,43.871782233398&
济南;116.87938674512,36.611262322265,117.19181045117,36.715632439453&
南京;118.67849240675,31.97905925879,118.91401181593,32.215265313477&
南宁;108.22799350195,22.730636210938,108.51020480566,22.896117778321&
北京;116.20753109375,39.799245591797,116.56321346679,40.000432725586&
天津;116.74989619727,38.705155707031,117.87874141211,39.446732855469&
重庆;106.36026301855,29.398362494141,106.67749324316,29.686066961915&
石家庄;114.34659246972,37.942477173828,114.65558294824,38.14572424414&
太原;112.42908503613,37.729984581054,112.67421748242,38.015629112304&
沈阳;123.24665849903,41.690011142579,123.61401384571,41.915230869141&
长春;125.15970507812,43.785217464843,125.44397631836,44.020050228515&
哈尔滨;126.46623604004,45.630960394531,126.8404578418,45.863733221679&
苏州;120.42526432128,31.214921602539,120.91072269531,31.468980440429&
杭州;120.04027949121,30.100691421875,120.36574946192,30.39114247168&
合肥;117.17317871875,31.780509152833,117.42105774707,31.941870847168&
福州;119.21972166602,25.98123156543,119.42296873633,26.138473386719&
南昌;115.74967205714,28.585169147461,116.0583192129,28.774339984863&
青岛;120.2837618672,36.046589788085,120.6881960713,36.426304753906&
呼和浩特;111.46206076562,40.689908290039,111.89327414453,40.893155360351&
武汉;114.12691062306,30.438099506836,114.55881064747,30.674992207031&
长沙;112.85456293555,28.09040652832,113.17453974219,28.30052005371&
广州;113.11309549609,23.029047628907,113.47289774218,23.211695333985&
兰州;103.68073473437,36.020273497071,103.94783983691,36.117433836426&
成都;103.94969190918,30.584617639648,104.19413770996,30.76932528125&
贵阳;106.58026877149,26.462299909179,106.80068197949,26.691639508789&
海口;110.1464823584,19.966053031249,110.45272625488,20.079349540038&
昆明;102.62779360254,24.956151602539,102.81524782617,25.128156302246&
西宁;101.61103117335,36.569791311768,101.87985288966,36.676908010987&
西安;108.78460236913,34.18934660791,109.08603974706,34.35723143457&
拉萨;91.064062433351,29.62654370874,91.205854730714,29.696753211914&
银川;106.07870847999,38.414184941406,106.32658750831,38.583099736328&
郑州;113.48276429493,34.643991845704,113.82265382129,34.835565942383&
淄博;117.76627003125,36.589102521483994,118.50510059766,37.049155011718
";

        private string data = @"河北省,石家庄,shijiazhuang;
江苏省,常州,changzhou;
山东省,淄博,zibo;
安徽省,芜湖,wuhu;
黑龙江,哈尔滨,haerbin;
甘肃省,兰州,lanzhou;
上海市,上海,shanghai;
天津市,天津,tianjin;
云南省,昆明,kunming;
北京市,北京,beijing;
重庆市,重庆,chongqing;
宁夏回族自治区,银川,yinchuan;
新疆维吾尔自治区,乌鲁木齐,wulumuqi;
四川省,成都,chengdu;
吉林省,长春,changchun;
广西壮族自治区,南宁,nanning;
辽宁省,沈阳,shenyang;
青海省,西宁,xining;
陕西省,西安,xian;
河南省,郑州,zhengzhou;
山西省,太原,taiyuan;
安徽省,合肥,hefei;
湖北省,武汉,wuhan;
湖南省,长沙,changsha;
贵州省,贵阳,guiyang;
浙江省,杭州,hangzhou;
江西省,南昌,nanchang;
广东省,广州,guangzhou;
海南省,海口,haikou;
内蒙古,呼和浩特,huhehaote;
西藏,拉萨,lasa;
山东省,济南,jinan;
江苏省,南京,nanjing;
福建省,福州,fuzhou";
        #endregion
        private Dictionary<string, ProviceCity> dictionary = new Dictionary<string, ProviceCity>();

        public ProviceCityPinYin()
        {
            DbHost = "192.168.60.242";
            this.Dbusername = "postgres";
            this.Database = "routing";
            this.Password = "123456";
            this.Resolve();
        }

        public string DbHost { get;private set; }
        public string Dbusername { get; private set; }
        public string Database { get; private set; }
        public string Password { get; private set; }
        /// <summary>
        /// 设置数据库属性
        /// </summary>
        /// <param name="dbhost">192.168.60.242</param>
        /// <param name="dbusername">postgres</param>
        /// <param name="database">routing</param>
        /// <param name="password">123456</param>
        public void SetDbProperty(string dbhost = "192.168.60.242", string dbusername = "postgres", string database = "routing", string password = "123456")
        {
            DbHost = dbhost;
            this.Dbusername = dbusername;
            this.Database = database;
            this.Password = password;
            con = string.Format(
                "Server ={0};Port=5432;user id ={1};password ={3};Database = {2};",
                dbhost,
                dbusername,
                database,
                password);
        }
        public Dictionary<string, ProviceCity> GetProviceCities()
        {
            return this.dictionary;
        }

        private void Resolve()
        {
            Dictionary<string, Extent> cityDictionary = new Dictionary<string, Extent>();
            var cityExtents = extents.Replace('\r', ' ').Split('&');
            cityExtents.ToList().ForEach(
                m =>
                {
                    cityDictionary[m.Split(';')[0].Trim()] = Extent.Resolve(m.Split(';')[1]);
                });

            data.Replace('\r', ' ').Split(';').ToList().ForEach(
                m =>
                    {
                        var temp = m.Split(',');
                        this.dictionary[temp[1].Trim()] = new ProviceCity()
                                                              {
                                                                  City = temp[1].Trim(),
                                                                  CityPinyin = temp[2].Trim(),
                                                                  Provice = temp[0].Trim(),
                                                                  Extent =
                                                                      cityDictionary.ContainsKey(
                                                                          temp[1].Trim())
                                                                          ? cityDictionary[temp[1].Trim()]
                                                                          : null
                                                              };
                    });
        }

        string con = "Server =192.168.60.242;Port=5432;user id =postgres;password =123456;Database = routing;";
        private List<string> GetTables(string nameLike)
        {
            string sql = !string.IsNullOrEmpty(nameLike) ? string.Format(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public' 
and TABLE_NAME like '%_{0}%'
order by TABLE_NAME", nameLike) : string.Format(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public'  
order by TABLE_NAME");
            List<string> list = new List<string>();

            var dbHelper = new DbHelper(con);
            var reader = dbHelper.ExecuteReader(sql);
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 执行路口扩展
        /// </summary>
        public void ExcuteRoadNet()
        {
            var tables = GetTables("roadnet");
            string msg = @"
--select distinct highspeed  from shanghai_roadnet_supermap;

--- 新增表字段
alter table xian_roadNet add length double precision;
alter table xian_roadNet add car integer;
alter table xian_roadNet add walk integer;
alter table xian_roadNet add speed double precision;
alter table xian_roadNet add highspeed integer;
alter table xian_roadNet add np_level integer;

-- 道路长度计算
update xian_roadNet set length=st_length(st_transform(st_setsrid(geom,4326),900913)) ;

-- 道路分级
--select distinct type from  xian_roadNet ;

-- 设置道路等级

-- 设置道路速度
update xian_roadNet set speed = 100,np_level=1 where type = '高速公路';
update xian_roadNet set speed = 70,np_level =2 where type = '主要道路（城市主干道）';
update xian_roadNet set speed = 50,np_level =3 where type = '次要道路（城市次干道）';
update xian_roadNet set speed = 50,np_level =3 where type = '一般道路';
update xian_roadNet set speed = 50,np_level =4 where type = '城市环路/城市快速路';
update xian_roadNet set speed = 18,np_level =4 where type = '非导航道路';

update xian_roadNet set speed = 60,np_level =3 where type = '国道';
update xian_roadNet set speed = 80,np_level =2 where type = '省道';
update xian_roadNet set speed = 40,np_level =3 where type = '县道';
update xian_roadNet set speed = 20,np_level =4 where type = '区县内部道路';
update xian_roadNet set speed = 18,np_level =4 where type = '乡村道路';

-- 设置高速道路
update xian_roadNet set highspeed = 0;
update xian_roadNet set highspeed = 1 where np_level in (1);

-- 设置道路是否可步行和通车
update xian_roadNet set walk = 0 , car = 0 ;
update xian_roadNet set walk = 0 , car = 1 where np_level in (1);
update xian_roadNet set walk = 1 , car = 1 where np_level != 1
";
            var dbHelper = new DbHelper(this.con);
            foreach (var table in tables)
            {
                var sql = msg.Replace("xian_roadNet", table);
                dbHelper.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 路口/兴趣点 增加拼音和拼音首字母
        /// </summary>
       
        public void ExcuteRoadCrossAndPoi()
        {
            var tables = GetTables("poi");
            tables.AddRange(this.GetTables("roadcross"));
            var dbHelper = new DbHelper(this.con);
            var connection = new NpgsqlConnection(con);
            connection.Open();
            var command = connection.CreateCommand();
            var readcon = new NpgsqlConnection(con);
            readcon.Open();
            var readcommand = readcon.CreateCommand();
            readcommand.CommandType = CommandType.Text;
            foreach (var table in tables)
            {
                dbHelper.AddColumn("quanpin", table);
                dbHelper.AddColumn("szm", table);

                Console.WriteLine(table);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 1000 * 60;
                command.CommandText = string.Format("SELECT COUNT(1) FROM {0} where quanpin is null", table);
                int rowCount = int.Parse(command.ExecuteScalar().ToString());
                if (rowCount != 0)
                {
                    var max = rowCount / 2000 + 1;

                    Console.WriteLine(max);
                    for (int i = 0; i < max; i++)
                    {
                        Console.WriteLine(i);
                        readcommand.CommandText = string.Format("SELECT gid,name from {0} where quanpin is null  limit 2000", table);
                        var reader = readcommand.ExecuteReader();
                        while (reader.Read())
                        {
                            var gid = reader.GetInt32(0);
                            var name = reader.GetString(1);

                            command.CommandText = string.Format(
                                "UPDATE {0} SET quanpin='{1}',szm='{2}' where gid = {3}",
                                table,
                                Hz2Py.GetPinyin(name).Replace('\'', ' '),
                                Hz2Py.GetFirstPinyin(name).Replace('\'', ' '),
                                gid);
                            command.ExecuteNonQuery();
                        }
                        reader.Close();
                    }
                    //readcommand.Dispose();
                }

                // readcon.Close();
                //readcon.Dispose();

                // command.Dispose();
                // connection.Close();
                //connection.Dispose();
            }

        }

        /// <summary>
        /// POI 数据填充区号
        /// </summary>
       
        public void WritePoiDirect()
        {
            var tables = GetTables("poi");

            var dbHelper = new DbHelper(this.con);
            var connection = new NpgsqlConnection(con);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            foreach (KeyValuePair<string, ProviceCity> keyValuePair in dictionary)
            {
                var provice = keyValuePair.Value.Provice;
                var cityName = keyValuePair.Value.City;
                var table = keyValuePair.Value.CityPinyin + "_poi";
                if (!tables.Contains(table))
                {
                    continue;
                }
                dbHelper.AddColumn("districtname", table);
                Console.WriteLine(cityName);
                var cityList = CityConfig.GetInstance().getCityByName(cityName);
                if (cityList.Count != 1)
                {
                    Console.WriteLine(string.Format("{0}重复",cityName));
                    continue;
                }
                var code = cityList[0].baiduCode;
                if (cityName == "长沙")
                {
                    code = "158";
                }
                var districts = new Dictionary<string, string>(); // code name
                command.CommandText = string.Format(
                    "SELECT area_code,area_name,gid FROM CITY WHERE parent_code = '{0}'",
                    code);
                var read = command.ExecuteReader();
                while (read.Read())
                {
                    districts[read.GetInt32(2).ToString()] = read.GetString(1);
                }
                read.Close();

                var sql = @" UPDATE {0} set districtName = '{1}' WHERE GID IN  (
                                            SELECT m.gid FROM {0} as m  , city as c
                                            where st_intersects(st_setsrid(m.geom,4326),st_setsrid(c.geom,4326))   and c.gid = {2}
                                            )  ";

                districts.Keys.ToList().ForEach(
                    m =>
                    {
                        command.CommandText = (string.Format(sql, table, districts[m], m));
                        command.ExecuteNonQuery();
                    });
            }
        }
    }
}
