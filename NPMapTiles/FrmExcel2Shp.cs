using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
namespace NPMapTiles
{
    using MapDataTools;

    public partial class FrmExcel2Shp : Office2007Form
    {
        string type = "";
        string savePath = "";
        string filepath ="";
        private delegate void CoverterEndHandler();
        private CoverterEndHandler coverterEndHandler = null;
        System.Threading.Thread thread = null;
        public FrmExcel2Shp()
        {
            InitializeComponent();
            this.cmbType.Items.Clear();
            this.cmbType.Items.Add("点");
            this.cmbType.Items.Add("线");
            this.cmbType.Items.Add("面");
            this.cmbType.SelectedIndex = 0;
            this.coverterEndHandler += new CoverterEndHandler(this.converteEnd);

            projectCombox.Items.Clear();
            projectCombox.Items.Add("不做变化处理");
            projectCombox.Items.Add("百度转WGS经纬度");
            projectCombox.Items.Add("火星转WGS经纬度");
            projectCombox.Items.Add("高德84转WGS经纬度");
            this.projectCombox.SelectedIndex = 2;
        }

        private void btnScanFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "csv files (*.xls)|*.xls";
            if (of.ShowDialog() == DialogResult.OK)
            {
                this.txbFilePath.Text = of.FileName;
            }
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

        private void btnCoverter_Click(object sender, EventArgs e)
        {
            filepath = this.txbFilePath.Text.Trim();
            if (!System.IO.File.Exists(filepath))
            {
                MessageBox.Show("文件不存在");
                return; 
            }
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            if (file.Extension != ".xls")
            {
                MessageBox.Show("不支持" + file.Extension + "文件");
                return;
            }
            this.savePath = this.txbDirctoryPath.Text.Trim();
            if (!System.IO.Directory.Exists(savePath))
            {
                MessageBox.Show("保存路径不存在");
                return;
            }
            this.savePath = this.savePath + "\\" + file.Name.Split('.')[0] + ".shp";
            type = this.cmbType.SelectedItem.ToString();
            string[] columns = AsposeCellsHelper.GetFileColumns(filepath);
            if (type == "点")
            {
                if (!columns.Contains("X") && !columns.Contains("Y"))
                {
                    MessageBox.Show("文件未同时包含X,Y列");
                    return;
                }
            }
            else if (type == "线"||type == "面")
            {
                if (!columns.Contains("PATH"))
                {
                    MessageBox.Show("文件不未包含PATH列");
                    return;
                }
            }
            btnCoverter.Enabled = false;
            btnStop.Enabled = true;
            var project = this.projectCombox.SelectedItem.ToString();

            switch (project)
            {
                case "百度转WGS经纬度":
                    convert = ProjectConvert.BAIDU_WGS;
                    break;
                case "火星转WGS经纬度":
                    convert = ProjectConvert.GCJ_WGS;
                    break;
                case "高德84转WGS经纬度":
                    convert = ProjectConvert.GAODE84_WGS;
                    break;
                default:
                    convert = ProjectConvert.NONE;
                    break;
            }
            this.progressBar.Text = "正在转换...";
            this.progressBar.ProgressType = eProgressItemType.Marquee;
            this.thread = new System.Threading.Thread(this.convertor);
            this.thread.Start();
        }
        ProjectConvert convert;
        private void convertor()
        {
            if (type == "点")
            {           
                DataTable dataTable = MapDataTools.SVCHelper.ReadCsvFile(filepath);
                MapDataTools.ShpFileHelper.SaveShpFile(dataTable, savePath, OSGeo.OGR.wkbGeometryType.wkbPoint, convert);
            }
            else if (type == "线")
            {
                DataTable dataTable = MapDataTools.SVCHelper.ReadCsvFile(filepath);
                MapDataTools.ShpFileHelper.SaveShpFile(dataTable, savePath, OSGeo.OGR.wkbGeometryType.wkbLineString, convert);
            }
            else if (type == "面")
            {
                DataTable dataTable = MapDataTools.SVCHelper.ReadCsvFile(filepath);
                MapDataTools.ShpFileHelper.SaveShpFile(dataTable, savePath, OSGeo.OGR.wkbGeometryType.wkbPolygon, convert);
            }
            if (this.coverterEndHandler != null)
                this.coverterEndHandler();
        }
        private void converteEnd()
        {
            MethodInvoker invoker = delegate
            {
                this.progressBar.Text = "转换成功";
                this.progressBar.ProgressType = eProgressItemType.Standard;
                this.progressBar.Value = 100;
                this.btnCoverter.Enabled = true;
                this.btnStop.Enabled = false;
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
                }
                catch { }
            }
            btnCoverter.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}
