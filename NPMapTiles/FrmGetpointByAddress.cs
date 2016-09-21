using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NPMapTiles
{
    public partial class FrmGetpointByAddress : Form
    {
        string filePath = "";
        string dirctoryPath = "";
        string savePath = "";
        DataTable myDataTable = null;
        DataTable dataTable = null;
        System.Threading.Thread thread = null;
        bool isWgs = false;
        public FrmGetpointByAddress()
        {
            InitializeComponent();
            this.cmbPointType.SelectedIndex = 0;
        }

        private void btnScanFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "csv files (*.csv)|*.csv";
            if (of.ShowDialog() == DialogResult.OK)
                this.txbFilePath.Text = of.FileName;
        }

        private void btnScanDirctory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txbDirctoryPath.Text = dialog.SelectedPath;
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.filePath = this.txbFilePath.Text.Trim();
            if (!System.IO.File.Exists(filePath))
            {
                MessageBox.Show("坐标文件不存在!");
                return;
            }
            System.IO.FileInfo file = new System.IO.FileInfo(this.filePath);
            if (file.Extension != ".csv")
            {
                MessageBox.Show("坐标文件格式不正确");
                return;
            }
            if (!MapDataTools.SVCHelper.IsContainsCloumnName(this.filePath, "Address") || !MapDataTools.SVCHelper.IsContainsCloumnName(this.filePath, "City"))
            {
                MessageBox.Show("请确保文件中包含Address或City字段");
                return;
            }
            this.dirctoryPath = this.txbDirctoryPath.Text.Trim();
            if (!System.IO.Directory.Exists(this.dirctoryPath))
            {
                MessageBox.Show("存储路径不存在!");
                return;
            }
            dataTable = MapDataTools.SVCHelper.ReadCsvFile(this.filePath);
            myDataTable = new DataTable();
            foreach (DataColumn c in dataTable.Columns)
            {
                myDataTable.Columns.Add(c.ColumnName, c.DataType);
            }
            if (!MapDataTools.SVCHelper.IsContainsCloumnName(this.filePath, "X"))
            {
                myDataTable.Columns.Add("X", Type.GetType("System.String"));
            }
            if (!MapDataTools.SVCHelper.IsContainsCloumnName(this.filePath, "Y"))
            {
                myDataTable.Columns.Add("Y", Type.GetType("System.String"));
            }
            if (cmbPointType.SelectedIndex == 1)
            {
                this.isWgs = true;
            }
            if (this.dirctoryPath.Substring(this.dirctoryPath.Length - 1, 1) == "\\")
                this.dirctoryPath = this.dirctoryPath.Substring(0, this.dirctoryPath.Length - 1);
            this.savePath = this.dirctoryPath + "\\" + file.Name.Split('.')[0] + "_result.csv";
            MapDataTools.SVCHelper.ExportToSvc(myDataTable, savePath);
            btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.thread = new System.Threading.Thread(this.getLocation);
            this.thread.Start();
        }
        private void getLocation()
        {
            MapDataTools.GaodeMap gaodeMap = new MapDataTools.GaodeMap();
            int k = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                k++;
                string address = row["Address"].ToString();
                string cityName = row["City"].ToString();
                string cityCode = "";
                List<MapDataTools.City> cities = MapDataTools.CityConfig.GetInstance().GetCityByName(cityName);
                if (cities.Count > 0)
                    cityCode = cities[0].gaodeCode;
                if (address!="")
                {
                    MapDataTools.Coord c = gaodeMap.GetLocationByAddress(address, cityCode, isWgs);
                    if (c == null)
                        continue;
                    MethodInvoker invoker = delegate
                    {
                        DataRow r = myDataTable.NewRow();
                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            r[i] = row[i];
                        }
                        r["X"] = c.lon;
                        r["Y"] = c.lat;
                        MapDataTools.SVCHelper.AddToSvc(r, this.savePath);
                        progressBar.Value = k * 100 / dataTable.Rows.Count;
                        progressBar.Text = k.ToString() + "/" + dataTable.Rows.Count.ToString();
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
            }
            MethodInvoker invokerend = delegate
            {
                progressBar.Value = 100;
                progressBar.Text = "转换完成";
                btnStart.Enabled = true;
                this.btnStop.Enabled = false;
            };
            if ((!base.IsDisposed) && base.InvokeRequired)
            {
                base.Invoke(invokerend);
            }
            else
            {
                invokerend();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要终止转换吗？",
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
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
            }
        }
    }
}
