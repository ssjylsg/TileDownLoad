using System;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using MapDataTools;
namespace NPMapTiles
{
    public partial class FrmPOIDown : Office2007Form
    {
        DataTable dataTable = null;
        string path = "";
        string keyWords = "";
        System.Threading.Thread thread = null;
        Extent extent = null;
        bool isCreatShp = false;
        public FrmPOIDown(Extent extent)
        {
            InitializeComponent();
            this.extent = extent;
            checkBoxCreateShp.CheckState = CheckState.Unchecked;
            this.Closed += (sender, obj) =>
            {
                if (this.dataTable != null && this.dataTable.Rows.Count > 0)
                {
                    this.dataTable.Rows.Clear();
                    this.dataTable.Dispose();
                }
            };
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

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(this.txbPath.Text.Trim()))
            {
                MessageBox.Show("当前文件夹不存在");
                return;
            }
            this.path = this.txbPath.Text.ToString();
            this.keyWords = txbKeyWord.Text.Trim();
            if (checkBoxCreateShp.Checked)
                this.isCreatShp = true;
            this.keyWords = this.txbKeyWord.Text.Trim();
            this.InitDataTable();
            thread = new System.Threading.Thread(this.DoSomething);
            thread.Start();
            this.btnDown.Enabled = false;
            this.btnStop.Enabled = true;
        }
        private void DoSomething()
        {
            InitDataTable();
            if (this.extent == null)
                return;
            GaodeMap gaodeMap = new GaodeMap();
            gaodeMap.DowningEvent += new DowningEventHandler(poiDownHandler);
            gaodeMap.DownEndEvent += new DownEndEventHandler(poiDownEndHandler);
            gaodeMap.DowningMessageEvent += new DowningMessageHandler(poiDownMessageHandler);
            gaodeMap.GetPoIbyExtent(this.extent, this.keyWords);
        }
        /// <summary>
        /// 初始化表格式，
        /// </summary>
        private void InitDataTable()
        {
            this.dataTable = new DataTable();
            DataColumn dc = null;
           
           this.dataTable.Columns.Add("name", Type.GetType("System.String"));
           this.dataTable.Columns.Add("X", Type.GetType("System.String"));
           this.dataTable.Columns.Add("Y", Type.GetType("System.String"));
          
           this.dataTable.Columns.Add("r_addr", Type.GetType("System.String"));
           this.dataTable.Columns.Add("type", Type.GetType("System.String"));
           this.dataTable.Columns.Add("phone", Type.GetType("System.String"));
           
        }
        int k = 0;
        private void poiDownHandler(POIInfo poi, int index, int count)
        {
            MethodInvoker invoker = delegate
            {
                k++;
                DataRow row = this.dataTable.NewRow();
                row["name"] = poi.name;
                row["X"] = poi.cx;
                row["Y"] = poi.cy;
              
                row["r_addr"] = poi.address.Replace(",", "");
                row["type"] = poi.type;
                row["phone"] = poi.phone.Replace(",", ";");
                this.dataTable.Rows.Add(row);
                if (this.ckbExcel.Checked)
                {
                    if (this.dataTable.Rows.Count % 10 == 0 || index == count)
                    {
                        string name = this.keyWords.Trim() != "" ? this.keyWords.Trim() : "_兴趣点";
                        SVCHelper.ExportToSvc(this.dataTable, this.path + "\\" + name + ".csv");
                    }
                }
                if (index > count)
                    index = count;
                this.progressBar.Value = index * 100 / count;
                this.labMessage.Text = "提示:已下载兴趣点：" + k.ToString() + "条,"+index.ToString()+"/"+count.ToString();
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
        private void btnStop_Click(object sender, EventArgs e)
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

                    this.Invoke(new MethodInvoker(() =>
                    {
                        SavePoiData();
                    }));
                }
                catch {
                    if (thread != null && thread.ThreadState == System.Threading.ThreadState.Running)
                        thread.Abort();
                }
                this.btnDown.Enabled = true;
                this.btnStop.Enabled = false;
            }
        }
        private void SavePoiData()
        {
            string name = this.keyWords.Trim() != "" ? this.keyWords.Trim() : "_兴趣点";
            SVCHelper.ExportToSvc(this.dataTable, this.path + "\\" + name + ".csv");
            this.progressBar.Value = 100;
            this.labMessage.Text = "提示:已下载完成";
            this.progressBar.Update();
            if (this.isCreatShp)
            {
                this.labMessage.Text = "提示:已下载完成,生成shp...";
                ShpFileHelper.SaveShpFile(this.dataTable, this.path + "\\" + name + ".shp", OSGeo.OGR.wkbGeometryType.wkbPoint,ProjectConvert.NONE);
                this.labMessage.Text = "提示:生成shp成功";
            }
        }
        private void poiDownEndHandler(string message)
        {
            MethodInvoker invoker = delegate
            {
                SavePoiData();
                this.dataTable.Dispose();
                this.btnDown.Enabled = true;
                this.btnStop.Enabled = false;
                try
                {
                    if (thread != null && thread.ThreadState == System.Threading.ThreadState.Running)
                        thread.Abort();
                    thread = null;
                }
                catch { }
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
        private void poiDownMessageHandler(int index, int count)
        {
            MethodInvoker invoker = delegate
            {
                if (index > count)
                    index = count;
                this.progressBar.Value = index * 100 / count;
                this.labMessage.Text = "提示:已下载兴趣点：" + k.ToString() + "条," + index.ToString() + "/" + count.ToString();
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
    }
}
