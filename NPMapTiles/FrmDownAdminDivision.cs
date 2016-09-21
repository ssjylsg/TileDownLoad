using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MapDataTools;
using System.IO;
using MapDataTools.AdminDivision;
namespace NPMapTiles
{
    public partial class FrmDownAdminDivision : Office2007Form
    {
        DataTable districtDataTable = new DataTable();
        DataTable cityDataTable = new DataTable();
        DataTable provinceDataTable = new DataTable();
        DataTable countryDataTable = new DataTable();
        string path = "";
        List<Province> provinces = MapDataTools.CityConfig.GetInstance().Countryconfig.countries;
        System.Threading.Thread thread = null;
        public FrmDownAdminDivision()
        {
            InitializeComponent();
            this.InitCity();
        }
        private void InitCity()
        {
            cmbProvince.Items.Clear();
            ComboBoxItem item1 = new ComboBoxItem();
            item1.Text = "全国";
            item1.Tag = "";
            cmbProvince.Items.Add(item1);
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
            string provinceName = cmbProvince.SelectedItem.ToString();
            if (provinceName == "全国")
            {
                cmbCity.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.Text = "全部";
                item.Tag = "";
                cmbCity.Items.Add(item);
            }
            else
            {
                cmbCity.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.Text = "全部";
                item.Tag = "";
                cmbCity.Items.Add(item);
                string name = cmbProvince.SelectedItem.ToString();
                List<City> cities = CityConfig.GetCitiesByProvinceName(name);
                foreach (City c in cities)
                {
                    ComboBoxItem item1 = new ComboBoxItem();
                    item1.Text = c.name;
                    item1.Tag = c.areacode;
                    cmbCity.Items.Add(item1);
                }
                if (cmbCity.Items.Count == 2)//直辖市
                {
                    cmbCity.Items.Remove(cmbCity.Items[0]);
                }
            }
            if (cmbCity.Items.Count > 0)
            {
                cmbCity.SelectedIndex = 0;
            }
        }

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDistrict.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.Text = "全部";
            item.Tag = "";
            cmbDistrict.Items.Add(item);
            string name = cmbCity.SelectedItem.ToString();
            List<District> districts = CityConfig.GetDistrictsByCityName(name);
            foreach (District d in districts)
            {
                ComboBoxItem item1 = new ComboBoxItem();
                item1.Text = d.name;
                item1.Tag = d.areacode;
                cmbDistrict.Items.Add(item1);
            }
            if (cmbDistrict.Items.Count > 0)
            {
                cmbDistrict.SelectedIndex = 0;
            }
        }

        private void InitDataTable()
        {
            DataColumn dc = null;
            this.districtDataTable = new DataTable();
            dc = this.districtDataTable.Columns.Add("name", Type.GetType("System.String"));
            dc = this.districtDataTable.Columns.Add("PATH", Type.GetType("System.String"));
            dc = this.districtDataTable.Columns.Add("code", Type.GetType("System.String"));

            this.cityDataTable = new DataTable();
            dc = this.cityDataTable.Columns.Add("name", Type.GetType("System.String"));
            dc = this.cityDataTable.Columns.Add("PATH", Type.GetType("System.String"));
            dc = this.cityDataTable.Columns.Add("code", Type.GetType("System.String"));

            this.provinceDataTable = new DataTable();
            dc = this.provinceDataTable.Columns.Add("name", Type.GetType("System.String"));
            dc = this.provinceDataTable.Columns.Add("PATH", Type.GetType("System.String"));
            dc = this.provinceDataTable.Columns.Add("code", Type.GetType("System.String"));

            this.countryDataTable = new DataTable();
            dc = this.countryDataTable.Columns.Add("name", Type.GetType("System.String"));
            dc = this.countryDataTable.Columns.Add("PATH", Type.GetType("System.String"));
            dc = this.countryDataTable.Columns.Add("code", Type.GetType("System.String"));
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txbPath.Text = dialog.SelectedPath;
            }
        }
        private string _CurrentName = "";
        private string _AreaCode = "";
        private CityType _CurrentType = CityType.District;
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(this.txbPath.Text.Trim()))
            {
                MessageBox.Show("当前文件夹不存在");
                return;
            }
            btnDown.Enabled = false;
            btnCancel.Enabled = true;
            this.InitDataTable();
            this.path = this.txbPath.Text.Trim();
            string districtName = (this.cmbDistrict.SelectedItem as ComboBoxItem).Text;
            string cityName = (this.cmbCity.SelectedItem as ComboBoxItem).Text;
            string provinceName = (this.cmbProvince.SelectedItem as ComboBoxItem).Text;
            this._CurrentName=districtName;
            this._AreaCode = (this.cmbDistrict.SelectedItem as ComboBoxItem).Tag.ToString();
            if (districtName == "全部")
            {
                this._CurrentName = cityName;
                this._AreaCode = (this.cmbCity.SelectedItem as ComboBoxItem).Tag.ToString();
                this._CurrentType = CityType.City;
                if (cityName == "全部")
                {
                    this._CurrentName = provinceName;
                    this._AreaCode = (this.cmbProvince.SelectedItem as ComboBoxItem).Tag.ToString();
                    this._CurrentType = CityType.Province;
                }
                if (provinceName == "全国")
                {
                    this._CurrentType = CityType.Country;
                }
            }
            this.thread = new System.Threading.Thread(this.beginDownAdminDivision);
            this.thread.Start();
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
                    if (thread != null && thread.ThreadState == System.Threading.ThreadState.Running)
                        thread.Abort();
                    thread = null;
                }
                catch { }
            }
            btnDown.Enabled = true;
            btnCancel.Enabled = false;
        }
        private void beginDownAdminDivision()
        {
            AdminDivisionDown adminDivisionDown = new AdminDivisionDown();
            adminDivisionDown.AdminDivisionDowningEvent += new AdminDivisionDown.AdminDivisionDowningHandler(this.adminDivisionDowningHandler);
            adminDivisionDown.AdminDivisionDownedEvent += new AdminDivisionDown.AdminDivisionDownedHandler(this.adminDivisionDownedHandler);
            adminDivisionDown.DownAdminDivision(this._CurrentName,this._AreaCode, this._CurrentType);
        }
        private void adminDivisionDowningHandler(Divinsion divinsion, CityType type, int index, int count)
        {
            MethodInvoker invoker = delegate
            {
                switch(type)
                {
                    case CityType.Country:
                        foreach (string polyline in divinsion.polylines)
                        {
                            DataRow row = this.countryDataTable.NewRow();
                            row[0] = divinsion.name;
                            row[1] = polyline;
                            this.countryDataTable.Rows.Add(row);
                        }
                        break;
                    case CityType.City:
                        foreach (string polyline in divinsion.polylines)
                        {
                            DataRow row = this.cityDataTable.NewRow();
                            row[0] = divinsion.name;
                            row[1] = polyline;
                            this.cityDataTable.Rows.Add(row);
                        }
                        break;
                    case CityType.District:
                        foreach (string polyline in divinsion.polylines)
                        {
                            DataRow row = this.districtDataTable.NewRow();
                            row[0] = divinsion.name;
                            row[1] = polyline;
                            this.districtDataTable.Rows.Add(row);
                        }
                        break;
                    case CityType.Province:
                        foreach (string polyline in divinsion.polylines)
                        {
                            DataRow row = this.provinceDataTable.NewRow();
                            row[0] = divinsion.name;
                            row[1] = polyline;
                            this.provinceDataTable.Rows.Add(row);
                        }
                        break;
                }
                labMessage.Text = "提示:正在下载" + divinsion.name;
                this.progressBar.Value = index * 100 / count;
            };
            if (base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }
        private void adminDivisionDownedHandler()
        {
            MethodInvoker invoker = delegate
            {
                labMessage.Text = "提示:下载完成";
                this.progressBar.Value = 100;
                if (this.countryDataTable.Rows.Count > 0)
                {
                    ShpFileHelper.SaveShpFile(this.countryDataTable, this.path + "\\" + this._CurrentName + "_country.shp", OSGeo.OGR.wkbGeometryType.wkbPolygon, ProjectConvert.BAIDU_WGS);
                    labMessage.Text = "提示:保存" + this.path + "\\" + this._CurrentName + "_country.shp成功";
                }
                if (this.provinceDataTable.Rows.Count > 0)
                {
                    ShpFileHelper.SaveShpFile(this.provinceDataTable, this.path + "\\" + this._CurrentName + "_province.shp", OSGeo.OGR.wkbGeometryType.wkbPolygon, ProjectConvert.BAIDU_WGS);
                    labMessage.Text = "提示:保存" + this.path + "\\" + this._CurrentName + "_province.shp成功";
                }
                if (this.cityDataTable.Rows.Count > 0)
                {
                    ShpFileHelper.SaveShpFile(this.cityDataTable, this.path + "\\" + this._CurrentName + "_city.shp", OSGeo.OGR.wkbGeometryType.wkbPolygon, ProjectConvert.BAIDU_WGS);
                    labMessage.Text = "提示:保存" + this.path + "\\" + this._CurrentName + "_city.shp成功";
                }
                if (this.districtDataTable.Rows.Count > 0)
                {
                    ShpFileHelper.SaveShpFile(this.districtDataTable, this.path + "\\" + this._CurrentName + "_district.shp", OSGeo.OGR.wkbGeometryType.wkbPolygon,ProjectConvert.BAIDU_WGS);
                    labMessage.Text = "提示:保存" + this.path + "\\" + this._CurrentName + "_district.shp成功";
                }
                labMessage.Text = "提示:保存成功";
            };
            if (base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }
    }
}
