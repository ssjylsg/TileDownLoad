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
    public partial class FrmCoverteRectify : Office2007Form
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
        DataTable dataTable = null;
        private bool isStop=true;
        CovertType covertType = CovertType.WGS2BAIDU;

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        public FrmCoverteRectify(CovertType covertType)
        {
            InitializeComponent();
            this.covertType = covertType;
            this.convertHandler += new ConverteHandler(this.converteMessage);
            this.converteEndHandler += new ConverteEndHandler(this.converteEnd);
        }
        private void InitDataTable()
        {
            this.dataTable = AsposeCellsHelper.ExportToDataTable(this.path,true);
                //SVCHelper.readCSVFile(this.path);
            //dataTable.Columns.Add("WGS84_X", Type.GetType("System.String"));
            //dataTable.Columns.Add("WGS84_Y", Type.GetType("System.String"));
            //dataTable.Rows.Clear();
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
                //if (isCreateShp)
                //{
                //    if (File.Exists(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shp"))
                //    {
                //        File.Delete(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shp");
                //    }
                //    if (File.Exists(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".dbf"))
                //    {
                //        File.Delete(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".dbf");
                //    }
                //    if (File.Exists(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shx"))
                //    {
                //        File.Delete(this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shx");
                //    }
                //    ShpFileHelper.saveShpFile(this.dataTable, this.outPutPath + "\\" + this.outputFileName.Split('.')[0] + ".shp", OSGeo.OGR.wkbGeometryType.wkbPoint,ProjectConvert.NONE);
                //}
                if (!this.isStop)
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
            of.Filter = "Excel文件|*.xls|csv files (*.csv)|*.csv";
            if (of.ShowDialog() == DialogResult.OK)
                this.txbPath.Text = of.FileName;
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
            switch (this.covertType)
            {
                case CovertType.WGS2GCJ02:
                case CovertType.BAIDU2GCJ02:
                    this.outputFileName = file.Name.Split('.')[0] + "_GDJ.csv";
                    dataTable.Columns.Add("GDJ_X", Type.GetType("System.String"));
                    dataTable.Columns.Add("GDJ_Y", Type.GetType("System.String"));
                    break;
                case CovertType.LONLAT2MOCTOR:
                case CovertType.WGS2MOCTOR:
                    this.outputFileName = file.Name.Split('.')[0] + "_PM.csv";
                    dataTable.Columns.Add("PM_X", Type.GetType("System.String"));
                    dataTable.Columns.Add("PM_Y", Type.GetType("System.String"));
                    break;
                case CovertType.GCJ022BAIDU:
                case CovertType.WGS2BAIDU:
                    this.outputFileName = file.Name.Split('.')[0] + "_BD.csv";
                    dataTable.Columns.Add("BD_X", Type.GetType("System.String"));
                    dataTable.Columns.Add("BD_Y", Type.GetType("System.String"));
                    break;
                case CovertType.MOCTOR2LONLAT:
                    this.outputFileName = file.Name.Split('.')[0] + "_JW.csv";
                    dataTable.Columns.Add("JW_X", Type.GetType("System.String"));
                    dataTable.Columns.Add("JW_Y", Type.GetType("System.String"));
                    break;
            }
           // SVCHelper.ExportToSvc(this.dataTable, this.outPutPath + "\\" + this.outputFileName);
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
           // DataTable tempDataTable = AsposeCellsHelper.ExportToDataTable(this.path,true);
                //SVCHelper.readCSVFile(this.path);
            int k = 0;
            foreach (DataRow row in this.dataTable.Rows)
            {
                if (!this.isStop)
                {
                    k++;
                    DataRow newRow = row;
                    //for (int i = 0; i < row.ItemArray.Length; i++)
                    //{
                    //    newRow[i] = row[i];
                    //}
                    string xString = row["X"] != null ? row["X"].ToString() : "0";
                    string yString = row["Y"] != null ? row["Y"].ToString() : "0";
                    double x = 0.0, y = 0.0;
                    double.TryParse(xString, out x);
                    double.TryParse(yString, out y);
                    if (x == 0 || y == 0)
                    {
                        continue;
                    }
                    Coord coord = new Coord(x, y);
                    switch (this.covertType)
                    {
                        case CovertType.WGS2GCJ02:
                            coord = CoordHelper.Transform(x, y);
                            newRow["GDJ_X"] = coord.lon;
                            newRow["GDJ_Y"] = coord.lat;
                            break;
                        case CovertType.BAIDU2GCJ02:
                            coord = CoordHelper.BdDecrypt(y, x);
                            newRow["GDJ_X"] = coord.lon;
                            newRow["GDJ_Y"] = coord.lat;
                            break;
                        case CovertType.LONLAT2MOCTOR:
                            coord = CoordHelper.WebMoctorJw2Pm(x, y);
                            newRow["PM_X"] = coord.lon;
                            newRow["PM_Y"] = coord.lat;
                            break;
                        case CovertType.GCJ022BAIDU:
                            coord = CoordHelper.BdEncrypt(y, x);
                            newRow["BD_X"] = coord.lon;
                            newRow["BD_Y"] = coord.lat;
                            break;
                        case CovertType.WGS2BAIDU:
                            coord = CoordHelper.Transform(x, y);
                            coord = CoordHelper.BdEncrypt(coord.lat, coord.lon);
                            newRow["BD_X"] = coord.lon;
                            newRow["BD_Y"] = coord.lat;
                            break;
                        case CovertType.MOCTOR2LONLAT:
                            coord = CoordHelper.Mercator2lonLat(x, y);
                            newRow["JW_X"] = coord.lon;
                            newRow["JW_Y"] = coord.lat;
                            break;
                        case CovertType.WGS2MOCTOR:
                            coord = CoordHelper.Transform(x, y);
                            coord = CoordHelper.WebMoctorJw2Pm(coord.lon, coord.lat);
                            newRow["PM_X"] = coord.lon;
                            newRow["PM_Y"] = coord.lat;
                            break;
                    }
                    //if (this.convertHandler != null)
                    //{
                    //    this.convertHandler(newRow, k, tempDataTable.Rows.Count);
                    //}
                }
            }
            AsposeCellsHelper.ExportToExcel(this.dataTable, this.outPutPath + "\\" + this.outputFileName);
            if (this.converteEndHandler!=null)
            {
                this.converteEndHandler();
            }
        }
    }
    public enum CovertType
    {
        WGS2GCJ02=1,
        GCJ022BAIDU=2,
        WGS2BAIDU=3,
        LONLAT2MOCTOR=4,
        MOCTOR2LONLAT=5,
        BAIDU2GCJ02=6,
        WGS2MOCTOR=7
    }
}
