using System;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
namespace NPMapTiles
{
    using MapDataTools;

    public partial class FrmShp2Excel : Office2007Form
    {
        string type = "";
        string savePath = "";
        string filepath ="";
        private delegate void CoverterEndHandler();
        private CoverterEndHandler coverterEndHandler = null;
        System.Threading.Thread thread = null;
        public FrmShp2Excel()
        {
            InitializeComponent();
            this.coverterEndHandler += new CoverterEndHandler(this.converteEnd);
        }

        private void btnScanFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "shp files (*.shp)|*.shp";
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
            if (file.Extension != ".shp")
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
            this.savePath = this.savePath + "\\" + file.Name.Split('.')[0] + ".csv";
            btnCoverter.Enabled = false;
            btnStop.Enabled = true;
            this.progressBar.Text = "正在转换...";
            this.progressBar.ProgressType = eProgressItemType.Marquee;
            this.thread = new System.Threading.Thread(this.convertor);
            this.thread.Start();
        }
        private void convertor()
        {
            DataTable dataTable = MapDataTools.ShpFileHelper.GetData(this.filepath);
            //MapDataTools.SVCHelper.ExportToSvc(dataTable, savePath);
            AsposeCellsHelper.ExportToExcel(dataTable,savePath);
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
                this.Close();
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
