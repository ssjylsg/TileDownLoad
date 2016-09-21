using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MapDataTools;
using System.IO;
using System.Runtime.InteropServices;
namespace NPMapTiles
{
    public partial class FrmGoogleRectify : Office2007Form
    {
        private string path = "";
        private System.Threading.Thread thread = null;
        private delegate void ConverteHandler(DataRow row, int index, int count);
        private ConverteHandler convertHandler = null;
        private delegate void ConverteEndHandler();
        private ConverteEndHandler converteEndHandler = null;
        private string outPutPath = "";
        private string outputFileName = "";
        private bool isCreateShp = false;
        private bool isStop = true;
        DataTable dataTable = null;

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        public FrmGoogleRectify()
        {
            InitializeComponent();
            this.convertHandler += new ConverteHandler(this.converteMessage);
            this.converteEndHandler += new ConverteEndHandler(this.converteEnd);
        }
        private void InitDataTable()
        {
            this.dataTable = SVCHelper.ReadCsvFile(this.path);
            dataTable.Columns.Add("WGS84_X", Type.GetType("System.String"));
            dataTable.Columns.Add("WGS84_Y", Type.GetType("System.String"));
            dataTable.Rows.Clear();
        }
        private void converteMessage(DataRow row, int index, int count)
        {
            MethodInvoker invoker = delegate
            {
                if (index < count + 1)
                {
                    SVCHelper.AddToSvc(row, this.outPutPath + "\\" + this.outputFileName);
                    this.progressBar.Value = index * 100 / count;
                }
                if (index >= count)
                {
                    this.btnCoverter.Enabled = true;
                    this.btnStop.Enabled = false;
                    this.progressBar.Value = 100;
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
        private void converteEnd()
        {
            MethodInvoker invoker = delegate
            {
                if (isCreateShp)
                {
                    if (File.Exists(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shp"))
                    {
                        File.Delete(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shp");
                    }
                    if (File.Exists(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".dbf"))
                    {
                        File.Delete(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".dbf");
                    }
                    if (File.Exists(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shx"))
                    {
                        File.Delete(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shx");
                    }
                    ShpFileHelper.SaveShpFile(this.dataTable, this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shp", OSGeo.OGR.wkbGeometryType.wkbPoint,ProjectConvert.GCJ_WGS);
                }
                if(!this.isStop)
                    MessageBox.Show("转换成功");
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
        private void btnScan_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "csv files (*.xls)|*.xls";
            if (of.ShowDialog() == DialogResult.OK)
            {
                this.txbPath.Text = of.FileName;
            }
        }

        private void btnCoverter_Click(object sender, EventArgs e)
        {
            this.path = this.txbPath.Text.Trim();
            if (!System.IO.File.Exists(this.path))
            {
                MessageBox.Show("文件不存在");
                return;
            }
            FileInfo file = new FileInfo(this.path);
            //if (file.Extension != ".csv")
            //{
            //    MessageBox.Show("不支持改文件类型");
            //    return;
            //}
            IntPtr vHandle = _lopen(this.path, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                MessageBox.Show(this.path + "被其他程序占用！");
                return;
            }
            CloseHandle(vHandle);
            string[] columns = AsposeCellsHelper.GetFileColumns(this.path);
            //if (!SVCHelper.isContainsCloumnName(this.path, "x") || !SVCHelper.isContainsCloumnName(this.path, "y"))
            //{
            //    MessageBox.Show("文件中没有同时包含x和y列，大小写不区分");
            //    return;
            //}
            if (!columns.Contains("X") || !columns.Contains("Y"))
            // (!SVCHelper.isContainsCloumnName(this.path, "x") || !SVCHelper.isContainsCloumnName(this.path, "y"))
            {
                MessageBox.Show("文件中没有同时包含x和y列，大小写不区分");
                return;
            }
            btnCoverter.Enabled = false;
            btnStop.Enabled = true;
            if (checkBoxX1.Checked)
                this.isCreateShp = true;
            this.isStop = false;
            this.InitDataTable();
            this.outPutPath = file.DirectoryName;
            if (this.outPutPath.Substring(this.outPutPath.Length - 1, 1) == "\\")
                this.outPutPath = this.outPutPath.Substring(0, this.outPutPath.Length - 1);
            this.outputFileName = file.Name.Split('.')[0] + "_WGS84.csv";
            SVCHelper.ExportToSvc(this.dataTable, this.outPutPath + "\\" + this.outputFileName);
            this.thread = new System.Threading.Thread(this.converteData);
            thread.Start();
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
                    this.isStop = true;
                    if (thread != null && thread.ThreadState == System.Threading.ThreadState.Running)
                        thread.Abort();
                    thread = null;
                }
                catch { }
                btnCoverter.Enabled = true;
                btnStop.Enabled = false;
            }
        }
        private void converteData()
        {
            DataTable tempDataTable = SVCHelper.ReadCsvFile(this.path);
            int k = 0;
            foreach (DataRow row in tempDataTable.Rows)
            {
                if (!this.isStop)
                {
                    k++;
                    string xString = row["X"] != null ? row["X"].ToString() : "";
                    string yString = row["Y"] != null ? row["Y"].ToString() : "";
                    double x = 0.0, y = 0.0;
                    double.TryParse(xString, out x);
                    double.TryParse(yString, out y);
                    Coord coord = new Coord(x, y);
                    coord = CoordHelper.Gcj2Wgs(coord.lon, coord.lat);
                    DataRow newRow = this.dataTable.NewRow();
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        newRow[i] = row[i];
                    }
                    newRow["WGS84_X"] = coord.lon;
                    newRow["WGS84_Y"] = coord.lat;
                    if (this.convertHandler != null)
                    {
                        this.convertHandler(newRow, k, tempDataTable.Rows.Count);
                    }
                }
            }
            if (this.converteEndHandler!=null)
            {
                this.converteEndHandler();
            }
        }
    }
}
