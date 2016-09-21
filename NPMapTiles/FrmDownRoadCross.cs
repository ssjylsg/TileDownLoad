using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MapDataTools;
using System.IO;
namespace NPMapTiles
{
    public partial class FrmDownRoadCross : Office2007Form
    {
        List<Province> provinces = MapDataTools.CityConfig.GetInstance().Countryconfig.countries;
        private Dictionary<string, string> dicCross = new Dictionary<string, string>();
        System.Threading.Thread crossThread = null;
        private DataTable crossDataTable = null;
        private string currentCity = "";
        private string path = "";
        public FrmDownRoadCross()
        {
            InitializeComponent();
            this.InitCity();
        }
        private void InitCity()
        {
            cmbProvince.Items.Clear();
            foreach (Province p in provinces)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = p.name;
                item.Tag = p.areacode;
                cmbProvince.Items.Add(item);
            }
            if (cmbProvince.Items.Count > 0)
                cmbProvince.SelectedIndex = 0;
        }
        private void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string provinceName = (cmbProvince.SelectedItem as ComboBoxItem).Text;
            cmbCity.Items.Clear();
            string name = (cmbProvince.SelectedItem as ComboBoxItem).Text;
            List<City> cities = CityConfig.GetCitiesByProvinceName(name);
            foreach (City c in cities)
            {
                ComboBoxItem item1 = new ComboBoxItem();
                item1.Text = c.name;
                item1.Tag = c.areacode;
                cmbCity.Items.Add(item1);
            }
            if (cmbCity.Items.Count > 0)
            {
                cmbCity.SelectedIndex = 0;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder =
                    new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbPath.Text = folder.SelectedPath;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            this.InitDataTable();
            if (cmbCity.SelectedItem == null || (cmbCity.SelectedItem as ComboBoxItem).Text == "")
            {
                MessageBox.Show("没有可选城市或者城市名称不能为空");
                return;
            }

            this.path = txbPath.Text.Trim();
            if (!Directory.Exists(this.path))
            {
                MessageBox.Show("当前路径不存在，请从新选择路径");
                return;
            }
            if (this.path.Substring(this.path.Length - 1, 1) == "\\")
                this.path = this.path.Substring(0, this.path.Length - 1);
            this.currentCity = (cmbCity.SelectedItem as ComboBoxItem).Text;
            crossThread = new System.Threading.Thread(downRoadCross);
            crossThread.Start();
            btnDown.Enabled = false;
        }
        /// <summary>
        /// 初始化表格式，
        /// </summary>
        private void InitDataTable()
        {
            this.crossDataTable = new DataTable();
            DataColumn dc = null;
            //dc = this.crossDataTable.Columns.Add("id", Type.GetType("System.Int32"));
            //dc.AutoIncrement = true;//自动增加
            //dc.AutoIncrementSeed = 1;//起始为1
            //dc.AutoIncrementStep = 1;//步长为1
            //dc.AllowDBNull = false;//
            dc = this.crossDataTable.Columns.Add("first_name", Type.GetType("System.String"));
            dc = this.crossDataTable.Columns.Add("second_name", Type.GetType("System.String"));
            //dc = this.crossDataTable.Columns.Add("first_id", Type.GetType("System.String"));
            //dc = this.crossDataTable.Columns.Add("second_id", Type.GetType("System.String"));
            dc = this.crossDataTable.Columns.Add("x", Type.GetType("System.String"));
            dc = this.crossDataTable.Columns.Add("y", Type.GetType("System.String"));
            dc = this.crossDataTable.Columns.Add("name", Type.GetType("System.String"));
        }
        private void downRoadCross()
        {
            GaoDeRoads r = new GaoDeRoads();
            r.roadCrossDowningHandler += new GaoDeRoads.RoadCrossDowningHandler(roadCrossDownHandler);
            r.downOverHandler += new GaoDeRoads.DownOverHandler(downEnd);
            r.downLoadRoadCrossByCityName(this.currentCity);
        }
        int crossCount = 0;
        private void roadCrossDownHandler(RoadCrossModel roadCross, int index, int count)
        {
            MethodInvoker invoker = delegate
            {
                if (roadCross.id != "" && !this.dicCross.ContainsKey(roadCross.id))
                {
                    crossCount++;
                    DataRow row = this.crossDataTable.NewRow();
                    row["first_name"] = roadCross.first_name;
                    row["second_name"] = roadCross.second_name;
                    //row["first_id"] = roadCross.first_id;
                    //row["second_id"] = roadCross.second_id;
                    row["x"] = roadCross.wgs_x;
                    row["y"] = roadCross.wgs_y;
                    row["name"] = string.Format("{0}_{1}", roadCross.first_name, roadCross.second_name);
                    this.crossDataTable.Rows.Add(row);
                    if (this.crossDataTable.Rows.Count % 10 == 0 || index == count)
                        SVCHelper.ExportToSvc(this.crossDataTable, this.path + "\\" + this.currentCity + "_WGS.csv");
                    this.progressBar.Value = index * 100 / count;
                    this.labMessage.Text = "提示:已下载路口数据：" + crossCount.ToString() + "条";
                    this.progressBar.Update();
                    this.dicCross.Add(roadCross.id, roadCross.id);
                }
            };
            if ((!base.IsDisposed) && base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }
        private void downEnd()
        {
            MethodInvoker invoker = delegate
            {
                ShpFileHelper.SaveShpFile(this.crossDataTable, this.path + "\\" + this.currentCity + "_路口.shp", OSGeo.OGR.wkbGeometryType.wkbPoint,ProjectConvert.NONE);
                this.btnDown.Enabled = true;
                this.progressBar.Value = 100;
                this.labMessage.Text = "提示:已下载路口数据完成";
                this.progressBar.Update();
            };
            if ((!base.IsDisposed) && base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要终止下载吗？",
                            "提示",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    if (crossThread != null && crossThread.ThreadState == System.Threading.ThreadState.Running)
                        crossThread.Abort();
                    crossThread = null;
                }
                catch { }
            }
            btnDown.Enabled = true;
        }
    }
}
