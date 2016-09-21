using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NPMapTiles
{
    using System.Threading;

    using MapDataTools;
    using MapDataTools.Util;

    using OSGeo.OGR;

    public partial class FrmRoadLineOpr : Form
    {
        public FrmRoadLineOpr()
        {
            InitializeComponent();
        }
        private DbHelper dbcon = null;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.txbDataBase.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入DataBase！");
                return;
            }
            if (this.txbServer.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入Server！");
                return;
            }
            if (this.txbUser.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入User！");
                return;
            }
            if (this.txbPassWord.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入PassWord！");
                return;
            }
            //连接数据库
            string connString =
                //"Server = " + this.txbServer.Text.Trim() + ";Port=5432;user id = "
                //                + this.txbUser.Text.Trim() + ";password = " + this.txbPassWord.Text.Trim()
                //                + ";Database = " + this.txbDataBase.Text.Trim() + ";";
            string.Format(
                "server={0};Port=5432;user id ={1};password ={2};Database ={3}",
                this.txbServer.Text.Trim(),
                this.txbUser.Text.Trim(),
                this.txbPassWord.Text.Trim(),
                this.txbDataBase.Text.Trim());
            this.dbcon = new DbHelper(connString);
            BindCity("1", this.cmbProvice);
            LoadDataTable(this.cmbPoi);
        }
        private void LoadDataTable(ComboBox cmbTableName)
        {
            cmbTableName.Items.Clear();
            cmbTableName.DisplayMember = "Text";
            cmbTableName.ValueMember = "Value";

            var dr =
                this.dbcon.ExecuteReader(
                    "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public' order by TABLE_NAME");
            while (dr.Read())
            {
                cmbTableName.Items.Add(new ComboboxItem() { Text = dr.GetString(0), Value = dr.GetString(0) });
            }
            dr.Close();
            if (cmbTableName.Items.Count > 0) cmbTableName.SelectedIndex = 0;
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
                string.Format("SELECT area_code,area_name,gid FROM CITY WHERE parent_code = '{0}'", code));
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.rtxbRoadNames.Text))
            {
                return;
            }
            if (this.cmbCity.SelectedItem == null)
            {
                return;
            }
            var cityName = ((ComboboxItem)this.cmbCity.SelectedItem).Text;
            var roadNames = this.rtxbRoadNames.Text.Replace("，", ",").Split(',');

            var roadMap = new GaoDeRoads();
            var roaddataTable = new DataTable();
            roaddataTable.Columns.Add("Name", Type.GetType("System.String"));
            roaddataTable.Columns.Add("WIDTH", Type.GetType("System.String"));
            roaddataTable.Columns.Add("TYPE", Type.GetType("System.String"));
            roaddataTable.Columns.Add("PATH", Type.GetType("System.String"));
            var m = 0;
            
            roadMap.roadDateDowningHandler += (RoadModel road, int index, int count) =>
                {
                    for (int i = 0; i < road.paths.Count; i++)
                    {
                        m++;
                        DataRow row = roaddataTable.NewRow();
                        row[0] = road.name;
                        row[1] = road.width;
                        row[2] = road.type;
                        row[3] = road.paths[i];
                        roaddataTable.Rows.Add(row);
                    }
                    this.BeginInvoke(
                           new MethodInvoker(
                               () =>
                               {
                                   if (this.IsDisposed)
                                   {
                                       return;
                                   }
                                   this.progressBar.Value = index * 100 / count;
                                   this.labMessage.Text = "已下载完道路：" + road.name;
                                   this.progressBar.Update();
                               }));
                };
           
            roadMap.downOverHandler += () =>
                {
                    var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, cityName + ".shp");
                    ShpFileHelper.SaveShpFile(roaddataTable, path, wkbGeometryType.wkbLineString, ProjectConvert.GAODE84_WGS);
                    this.BeginInvoke(
                        new MethodInvoker(
                            () =>
                                {
                                    if (this.IsDisposed)
                                    {
                                        return;
                                    }
                                    this.progressBar.Value = 100;
                                    this.labMessage.Text = "保存完成";
                                    this.progressBar.Update();
                                }));
                };
            new Thread(
               () =>
               {
                   roadMap.DownLoadRoads(cityName, roadNames.ToList());
               }).Start();
            
        }
        /// <summary>
        /// 根据道路交叉口检测道路名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.cmbCity.SelectedItem == null)
            {
                return;
            }
            var cityName = ((ComboboxItem)this.cmbCity.SelectedItem).Text;
             var roadMap = new GaoDeRoads();
            this.rtxbRoadNames.ReadOnly = true;
            var list = new List<string>();
            roadMap.roadCrossDowningHandler += (roadcross, index, count) =>
                { 
                    this.BeginInvoke(new MethodInvoker(
                        () =>
                        {
                            if (this.IsDisposed)
                            {
                                return;
                            }
                            this.labMessage.Text = string.Format(
                                    "{0}_{1}",
                                    roadcross.first_name,
                                    roadcross.second_name);
                            }));
                    list.Add(roadcross.first_name);
                    list.Add(roadcross.second_name);
                };
            roadMap.downOverHandler += () =>
                {
                    list = list.Distinct().ToList();
                    var temp = list;
                    this.BeginInvoke(
                        new MethodInvoker(
                            () =>
                                {
                                    if (this.IsDisposed)
                                    {
                                        return;
                                    }
                                    this.rtxbRoadNames.ReadOnly = false;
                                    this.rtxbRoadNames.Text = string.Join(",", temp.Distinct().ToArray());
                                    this.labMessage.Text = string.Format("更新成功,新增{0}条数据", temp.Count);
                                }));
                    var currentCity = CityRoadConfig.GetInstance().GetRoadName(cityName);
                    list.AddRange(currentCity.Roads);
                    list = list.Distinct().ToList();
                    list.Sort((m, n) => { return string.Compare(m, n); });
                    currentCity.Roads = list;
                    CityRoadConfig.GetInstance().SaveConfig();
                };
            new Thread(
                () =>
                    {
                        roadMap.downLoadRoadCrossByCityName(cityName);
                    }).Start();
        }
    }
}
