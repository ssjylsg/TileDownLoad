using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NPMapTiles
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    using log4net;

    using MapDataTools;
    using MapDataTools.Util;
    /// <summary>
    /// POI ROADCROSS ROADNET 三种数据下载
    /// </summary>
    public partial class FrmAllData : Form
    {
        private ILog log;
        public FrmAllData()
        {
            InitializeComponent();
            this.log = log4net.LogManager.GetLogger(this.GetType());
        }
        private DbHelper dbcon = null;
        private void FrmAllData_Load(object sender, EventArgs e)
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["postgres"].ConnectionString;
            if (string.IsNullOrEmpty(connection))
            {
                MessageBox.Show("请配置数据库连接");
                return;
            }
            this.dbcon = new DbHelper(connection);
            BindCity("1", this.cmbProvice);
        }
        private void BindCity(string code, ComboBox comboBox)
        {
            comboBox.Items.Clear();
            var read = GetCityReader(code);
            comboBox.DisplayMember = "Text";
            comboBox.ValueMember = "Value";
            while (read.Read())
            {
                comboBox.Items.Add(new ComboboxItem() { Text = read.GetString(1), Value = read.GetString(0) });
            }
            read.Close();
            comboBox.Items.Insert(0, new ComboboxItem() { Text = "请选择", Value = string.Empty });
            comboBox.SelectedIndex = 0; 
        }
        private IDataReader GetCityReader(string code)
        {
            return this.dbcon.ExecuteReader(
                string.Format("SELECT area_code,area_name,gid FROM CITY WHERE parent_code = '{0}' and area_code is not null", code));
        }

        private void cmbProvice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null)
            {
                return;
            }
            var hotCity = new string[] { "131", "332", "132", "289", };
            var selectItem = ((ComboboxItem)((ComboBox)sender).SelectedItem);
            var code = selectItem.Value.ToString();
            if (hotCity.Contains(code))
            {
                this.cmbCity.Items.Clear();
                this.cmbCity.Items.Add(selectItem);
                this.cmbCity.SelectedIndex = 0;
            }
            else
            {
                this.BindCity(code, this.cmbCity);
            }
        }

        private ProviceCity CurrentCity
        {
            get
            {
                return new ProviceCity()
                           {
                               Provice = ((ComboboxItem)this.cmbProvice.SelectedItem).Text.TrimEnd('省'),
                               City = ((ComboboxItem)cmbCity.SelectedItem).Text.TrimEnd('市'),
                               CityPinyin = this.txbPinyin.Text,
                               Extent = Extent.Resolve(this.txbExtent.Text)
                           };
            }
        }

        private void SetSatus(string status)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() => this.SetSatus(status)));
            }
            else
            {
                this.log.Info(status);
                this.lblStatus.Text = status;
                this.richTextBox1.AppendText(status);
            }
        }

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = string.Format(
                "{0}_{1}",
                ((ComboboxItem)this.cmbProvice.SelectedItem).Text.TrimEnd('省'),
                ((ComboboxItem)((ComboBox)sender).SelectedItem).Text.TrimEnd('市'));

            this.txbPinyin.Text = TextToPinyin.Convert(name).Pinyin.ToLower();
        }
        /// <summary>
        /// Poi 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SetSatus("POI 下载");
            ((Button)sender).Enabled = false;
            CurrentCity.DownLoadPoi();
            ((Button)sender).Enabled = true;
            SetSatus("POI 下载完成");
        }
        /// <summary>
        /// 路口下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SetSatus("路口下载");
            ((Button)sender).Enabled = false;
            CurrentCity.DownLoadRoadCross();
            SetSatus("路口下载完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// 道路下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SetSatus("道路下载");
            ((Button)sender).Enabled = false;
            CurrentCity.DownLoadRoadLine();
            SetSatus("道路下载完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// Excel 转shap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SetSatus("Excel 转shap");
            ((Button)sender).Enabled = false;
            CurrentCity.ExcelToShap();
            SetSatus("Excel 转shap 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// ShapToSql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SetSatus("ShapToSql");
            ((Button)sender).Enabled = false;
            CurrentCity.ShapToSql();
            SetSatus("ShapToSql 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// SqlToDb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            SetSatus("SqlToDb");
            ((Button)sender).Enabled = false;
            CurrentCity.SqlToDb();
            SetSatus("SqlToDb 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// 路网扩展
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            SetSatus("路网扩展");
            ((Button)sender).Enabled = false;
            CurrentCity.ExecuteRoadNet();
            SetSatus("路网扩展 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// POI 拼音和行政区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            SetSatus("POI 拼音和行政区");
            ((Button)sender).Enabled = false;
            CurrentCity.PoiExpand();
            SetSatus("POI 拼音和行政区 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// 路口拼音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            SetSatus("路口拼音");
            ((Button)sender).Enabled = false;
            CurrentCity.RoadCroassExpand();
            SetSatus("路口拼音 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button18_Click(object sender, EventArgs e)
        {
            SetSatus("创建索引");
            ((Button)sender).Enabled = false;
             
            CurrentCity.CreateSimilarityIndex();
            SetSatus("创建索引 完成");
            ((Button)sender).Enabled = true;
        }
        /// <summary>
        /// 自动执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            var city = this.CurrentCity;
            ((Button)sender).Enabled = false;
            var therad = new Thread(
                o =>
                    {
                        city.AutoRun(this.SetSatus);
                        this.BeginInvoke(
                            new MethodInvoker(
                                () =>
                                    {
                                        ((Button)sender).Enabled = true;
                                        var path = Path.Combine(
                                            city.BasePath,
                                            string.Format("{0}省\\{1}", city.Provice, city.City));
                                        System.Diagnostics.Process.Start(
                                            "explorer.exe", path
                                           );
                                    }));
                    });
            therad.IsBackground = true;
            therad.Start();
        }

        private int GetCurrentCityTablesCout(ProviceCity currentCity)
        {
            var sql = string.Format(
                "SELECT count(1) FROM INFORMATION_SCHEMA.TABLES WHERE  TABLE_NAME in ('{0}_poi','{0}_roadcross','{0}_roadnet')",
               currentCity.CityPinyin);
            return int.Parse(dbcon.ExecuteScalar(sql).ToString());
        }
        /// <summary>
        /// DbToSql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            this.CurrentCity.GetDbToSql();
        }
        /// <summary>
        /// 生成全省数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            var provice = ((ComboboxItem)this.cmbProvice.SelectedItem).Text.TrimEnd('省');
            var provicePinYin = Hz2Py.GetPinyin(provice).ToLower();
            this.richTextBox1.Text = "";
            
            this.richTextBox1.AppendText(GetPoiSql(provicePinYin));
            this.richTextBox1.AppendText(GetRoadNetSql(provicePinYin));
            this.richTextBox1.AppendText(GetRoadCrossSql(provicePinYin));

        }

        private string GetRoadCrossSql(string provicePinYin)
        {
            var poiSql = @"SELECT first_name, second_nam, x, y, name, geom, quanpin, szm
	        INto {0}_roadcross  
            FROM {0}_{1}_roadcross;";

            var insertPoiSql =
                @"INSERT INTO {0}_roadcross(first_name, second_nam, x, y, name, geom, quanpin, szm) 
SELECT first_name, second_nam, x, y, name, geom, quanpin, szm FROM  {0}_{1}_roadcross; ";


            var sql = @"ALTER TABLE {0}_roadcross
  ADD COLUMN gid serial;
ALTER TABLE {0}_roadcross
  ADD CONSTRAINT {0}_roadcross_pk_gid PRIMARY KEY (gid);";

            return string.Format(sql,provicePinYin) +  GetSql(insertPoiSql, poiSql, provicePinYin);
        }

        private string GetRoadNetSql(string provicePinYin)
        {
            var poiSql = @"SELECT  name, width, type, geom, length, car, walk, speed, highspeed,        np_level
	        INto {0}_roadnet  
            FROM {0}_{1}_roadnet;";

            var insertPoiSql =
                @"INSERT INTO {0}_roadnet(name, width, type, geom, length, car, walk, speed, highspeed,        np_level) 
SELECT name, width, type, geom, length, car, walk, speed, highspeed, np_level FROM  {0}_{1}_roadnet ;";

            var sql = @"ALTER TABLE {0}_roadnet
  ADD COLUMN gid serial;
ALTER TABLE {0}_roadnet
  ADD CONSTRAINT {0}_roadnet_pk_gid PRIMARY KEY (gid);";
            return string.Format(sql, provicePinYin) + GetSql(insertPoiSql, poiSql, provicePinYin);
        }

        private string GetPoiSql(string provicePinYin)
        {
            var poiSql = @"SELECT  name, x, y, r_addr, type, phone, geom, quanpin, szm, districtname
	        INto {0}_poi  
            FROM {0}_{1}_poi;";

            var insertPoiSql =
                @"INSERT INTO {0}_poi(name, x, y, r_addr, type, phone, geom, quanpin, szm, districtname) 
SELECT name, x, y, r_addr, type, phone, geom, quanpin, szm, districtname FROM  {0}_{1}_poi; ";

            var sql = @"ALTER TABLE {0}_poi
  ADD COLUMN gid serial;
ALTER TABLE {0}_poi
  ADD CONSTRAINT {0}_poi_pk_gid PRIMARY KEY (gid);";
            return string.Format(sql, provicePinYin) + GetSql(insertPoiSql, poiSql, provicePinYin);
        }

        private string GetSql(string insertPoiSql,string poiSql, string provicePinYin)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            var first = true;
            for (int i = 0; i < this.cmbCity.Items.Count; i++)
            {
                var name = ((ComboboxItem)this.cmbCity.Items[i]).Text.TrimEnd('市');
                if (name == "请选择")
                {
                    continue;
                }
                var pinYin = Hz2Py.GetPinyin(name).ToLower();

                if (first)
                {
                    stringBuilder.AppendFormat(poiSql, provicePinYin, pinYin).AppendLine();
                    first = false;
                }
                else
                {
                    stringBuilder.AppendFormat(insertPoiSql, provicePinYin, pinYin).AppendLine();
                }
            }
            stringBuilder.AppendLine().AppendLine();
            return stringBuilder.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var provice = ((ComboboxItem)this.cmbProvice.SelectedItem).Text.TrimEnd('省');
            var provicePinYin = Hz2Py.GetPinyin(provice).ToLower();
            StringBuilder message = new StringBuilder();
            new string[] { "{0}_roadnet", "{0}_poi", "{0}_roadcross", }.ToList().ForEach(
                m =>
                    {
                        var cmd = string.Format(
                            "pg_dump -h {1} -U {2} -t {0} -f  sql/{4}省/{0}.sql  {3}",
                            string.Format(m, provicePinYin),
                            "192.168.60.242",
                            "postgres",
                            "routing",
                            provice);
                        message.AppendLine(cmd);
                    });
            this.richTextBox1.Text = message.ToString();
        }
        /// <summary>
        /// 自动任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var list = new List<ProviceCity>();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (var reader = new StreamReader(dialog.FileName, Encoding.UTF8))
                {
                    var value = reader.ReadLine();
                    while (value != null)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            list.Add(ProviceCity.Resolve(value));
                        }
                        value = reader.ReadLine();
                    }
                }
            }
            var thread = new Thread(
                () =>
                    {
                        list.ForEach(
                            m =>
                                {
                                    if (Directory.GetFiles(m.GetCityDirectory(), "*.sql").Length == 0)
                                    {
                                        m.AutoRun(this.SetSatus);
                                    }
                                });
                        this.SetSatus("处理完毕");
                    });
            thread.IsBackground = true;
            thread.Start();
            MessageBox.Show("批量下载开始执行！");
        }
        /// <summary>
        /// 批量配置文件生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var provice = ((ComboboxItem)this.cmbProvice.SelectedItem).Text.TrimEnd('省');
           
            for (int i = 0; i < this.cmbCity.Items.Count; i++)
            {
                var item = this.cmbCity.Items[i] as ComboboxItem;
                if (item != null && !item.Text.Equals("请选择",StringComparison.CurrentCultureIgnoreCase))
                {
                    var city = item.Text.TrimEnd('市');
                    var provicePinYin = Hz2Py.GetPinyin(string.Format("{0}_{1}", provice, city)).ToLower();
                    stringBuilder.AppendFormat("{0}&{1}&{2}&", provice, city,provicePinYin).AppendLine();
                }
            }
            this.richTextBox1.Text = stringBuilder.ToString();
            Clipboard.SetDataObject(stringBuilder.ToString());
            MessageBox.Show("配置已生成");
        }
        /// <summary>
        /// Shap 文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(
                                          "explorer.exe", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shap")
                                         );
        }
        /// <summary>
        /// SQL 文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button17_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(
                                         "explorer.exe", @"C:\Program Files\PostgreSQL\9.4\bin\sql"
                                        );
        }
        /// <summary>
        /// 生成CVS命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button19_Click(object sender, EventArgs e)
        {
            String msg_roadCross =
                "copy {0}_roadcross(gid, first_name, second_nam, x, y, name, geom, quanpin, szm) to 'c:/postgresData/{0}_roadcross.csv' delimiter as ',' NULL AS 'NULL'  csv quote as '\"';";

            String msg_poi =
                "copy {0}_poi(gid,name,x,y,r_addr,type,phone,districtname,geom,quanpin,szm)       to 'c:/postgresData/{0}_poi.csv' delimiter as ',' NULL AS 'NULL'  csv quote as '\"'";

            String msg_roadnet =
                "copy {0}_roadnet(gid, name, width, type, geom, length, car, walk, speed, highspeed,np_level, quanpin, szm) to 'c:/postgresData/{0}_roadnet.csv' delimiter as ',' NULL AS 'NULL'  csv quote as '\"';";

            this.richTextBox1.Text = string.Format(msg_poi, this.txbPinyin.Text);
            this.richTextBox1.AppendText(Environment.NewLine);
            this.richTextBox1.AppendText(string.Format(msg_roadCross,this.txbPinyin.Text));
            this.richTextBox1.AppendText(Environment.NewLine);
            this.richTextBox1.AppendText(string.Format(msg_roadnet, this.txbPinyin.Text));
            this.richTextBox1.AppendText(Environment.NewLine);
        }


    }
}
