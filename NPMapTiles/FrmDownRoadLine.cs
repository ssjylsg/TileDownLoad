using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MapDataTools;
using System.IO;
using System.Threading;
using OSGeo.OGR;
namespace NPMapTiles
{
    public partial class FrmDownRoadLine : Office2007Form
    {
        List<Province> provinces = MapDataTools.CityConfig.GetInstance().Countryconfig.countries;
        private string roadSavePath = "";
        private string roadCurrentCity = "";
        Thread roadThread = null;
        public FrmDownRoadLine()
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
            if (!Directory.Exists(this.txbPath.Text.Trim()))
            {
                MessageBox.Show("请输入正确的路径");
                return;
            }
            this.roadSavePath = this.txbPath.Text.Trim();
            if (cmbCity.SelectedItem == null || (cmbCity.SelectedItem as ComboBoxItem).Text == "")
            {
                MessageBox.Show("没有可选城市或者城市名称不能为空");
                return;
            }
            if (this.roadSavePath.Substring(this.roadSavePath.Length - 1, 1) == "\\")
                this.roadSavePath = this.roadSavePath.Substring(0, this.roadSavePath.Length - 1);
            this.roadCurrentCity = (this.cmbCity.SelectedItem as ComboBoxItem).Text;
            this.roadThread = new Thread(this.downRoadData);
            this.roadThread.Start();
            return;
        }
        DataTable RoaddataTable = null;
        /// <summary>
        /// 初始化表格式，
        /// </summary>
        private void InitRoadDataTable()
        {
            this.RoaddataTable = new DataTable();
            DataColumn dc = null;
            dc = this.RoaddataTable.Columns.Add("Name", Type.GetType("System.String"));
            dc = this.RoaddataTable.Columns.Add("WIDTH", Type.GetType("System.String"));
            dc = this.RoaddataTable.Columns.Add("TYPE", Type.GetType("System.String"));
            dc = this.RoaddataTable.Columns.Add("PATH", Type.GetType("System.String"));

            //dc = this.RoaddataTable.Columns.Add("WGSPATH", typeof(string));
            //dc = this.RoaddataTable.Columns.Add("length", typeof(double));
            //dc = this.RoaddataTable.Columns.Add("car", typeof(int));
            //dc = this.RoaddataTable.Columns.Add("walk", typeof(int));
            //dc = this.RoaddataTable.Columns.Add("speed", typeof(double));
            //dc = this.RoaddataTable.Columns.Add("highspeed", typeof(double));
            //dc = this.RoaddataTable.Columns.Add("np_level", typeof(int));
        }
        private void downRoadData()
        {
            this.InitRoadDataTable();
            GaoDeRoads gaodeRoad = new GaoDeRoads();
            gaodeRoad.roadDateDowningHandler += new GaoDeRoads.RoadDateDowningHandler(roadDownHandler);
            gaodeRoad.downOverHandler += new GaoDeRoads.DownOverHandler(saveDataInShp);
            gaodeRoad.downLoadRoadsByCityName(this.roadCurrentCity);
        }
        private void saveDataInShp()
        {
            MethodInvoker invoker = delegate
            {
                this.progressBar.Value = 100;
                this.labMessage.Text = "正在保存文件到shp";
                string path = this.roadSavePath + "\\" + this.roadCurrentCity + "_路网.shp";
                int x = 0;
                while (File.Exists(path))
                {
                    x++;
                    path = path.Substring(0, path.LastIndexOf('.')) + x.ToString() + ".shp";
                }
                ShpFileHelper.SaveShpFile(this.RoaddataTable, path, wkbGeometryType.wkbLineString,ProjectConvert.GAODE84_WGS);
                this.progressBar.Value = 100;
                this.labMessage.Text = "保存完成";
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
        int m = 0;
        private void roadDownHandler(RoadModel road, int index, int count)
        {
            MethodInvoker invoker = delegate
                {
                    for (int i = 0; i < road.paths.Count; i++)
                    {
                        m++;
                        DataRow row = this.RoaddataTable.NewRow();
                        row[0] = road.name;
                        row[1] = road.width;
                        row[2] = road.type;
                        row[3] = road.paths[i];
                        //row["WGSPATH"] = road.getWgsPath(i);


                        //row["length"] = 0;
                        //row["car"] = 1;
                        //row["walk"] = road.IsHeightWay ? 0 : 1;
                        //row["speed"] = road.getSpeed();
                        //row["highspeed"] = road.IsHeightWay ? 1 : 0;
                        //row["np_level"] = road.getNpLevel();

                        this.RoaddataTable.Rows.Add(row);
                    }
                    SVCHelper.ExportToSvc(
                        this.RoaddataTable,
                        this.roadSavePath + "\\" + this.roadCurrentCity + "_道路.csv");
                    this.progressBar.Value = index * 100 / count;
                    this.labMessage.Text = "已下载完道路：" + road.name;
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
                    if (roadThread != null && roadThread.ThreadState == System.Threading.ThreadState.Running)
                        roadThread.Abort();
                    roadThread = null;
                }
                catch { }
            }
            btnDown.Enabled = true;
        }
    }
}
