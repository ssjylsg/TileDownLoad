using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPMapTiles
{
    using System.Threading;

    using MapDataTools;
    using MapDataTools.Tile;
    using MapDataTools.Util;

    public partial class EzMapTileDownLoadFrm : Form
    {
        private class EzMapConfig
        {
            public Extent CurrentExtent { get; set; }

            public int MinZoom { get; set; }

            public int MaxZoom { get; set; }

            public string ServerUrl { get; set; }

            public string ServerVersion { get; set; }
            
            [NonSerialized]
            private WorkInfo _WorkInfo;

            public WorkInfo WorkInfo
            {
                get
                {
                    return _WorkInfo;
                }
                set
                {
                    this._WorkInfo = value;
                }
            }

            public void DownLoad(int index,int zoom)
            {
                var config = this;
                var ezMap = new EZMapTile(this.ServerUrl, this.ServerVersion);
                var rowColumns = config.WorkInfo.rcList[index];
                ezMap.DownLoad(
                    rowColumns.minRow,
                    rowColumns.maxRow,
                    rowColumns.minCol,
                    rowColumns.maxCol,
                    zoom,
                    config.WorkInfo);
            }
        }

        private log4net.ILog log;
        public EzMapTileDownLoadFrm()
        {
            InitializeComponent();
            log = log4net.LogManager.GetLogger(this.GetType());
            if (this.DesignMode)
            {
                return;
            }
            this.richTextBox1.Text = @"1.首先在用户给定的PGIS地址上找到地图DEMO
2.找到EzMapApi.js文件，在文件中搜索： 
MapSrcURL为服务地址，如果有多个，请取矢量地图 
CenterPoint为地图中心点， 
MapInitLevel 为最小层级，MapMaxLevel 为最大层级 ,
版本号，请查看地图请求URL中Version值，一般为0.3
如果ZoomOffset 不为0，最小层级调整为原最小层级+ZoomOffset，最大层级调整为原最大层级+ZoomOffset";
            this.richTextBox1.ReadOnly = true;
            this.mapControl1.overlayEditedHandler += (e) =>
            {
                this.extentTxb.Text = e.ToString();
            };
        }

        private Extent currentExtent()
        {
            return  Extent.Resolve(this.extentTxb.Text);
        }

        private EzMapConfig config = null;
        private void saveConfigBtn_Click(object sender, EventArgs e)
        {
            SetConfig();

            // 预览地图
            this.mapControl1.LoadEzMap(JsonHelper.ToJson(config));
        }

        private void SetConfig()
        {
            var extent = currentExtent();
            var ezMap = new EZMapTile(this.urlTxb.Text, this.versionTxb.Text);
            int maxZoom = int.Parse(this.maxZoomTxb.Text);
            int minZoom = int.Parse(this.minZoomTxb.Text);

            var list = new System.Collections.Generic.List<RowColumns>();
            for (int i = minZoom; i <= maxZoom; i++)
            {
                list.Add(ezMap.GetRowColomns(extent, i));
            }
            WorkInfo workInfo = new WorkInfo();
            workInfo.maxX = extent.maxX;
            workInfo.maxY = extent.maxY;
            workInfo.minX = extent.minX;
            workInfo.minY = extent.minY;
            workInfo.mapType = MapType.PGIS;
            workInfo.rcList = list;
            workInfo.filePath = this.txbSavePath.Text.Trim();
            workInfo.workName = "PIS 地图下载";
            workInfo.processDownImage.count = 0;
            workInfo.id = System.Guid.NewGuid().ToString();
            workInfo.isAusterityFile = true;

            config = new EzMapConfig();
            config.CurrentExtent = extent;
            config.WorkInfo = workInfo;
            config.MaxZoom = maxZoom;
            config.MinZoom = minZoom;
            config.ServerUrl = this.urlTxb.Text;
            config.ServerVersion = this.versionTxb.Text;
        }
        private void downLoadTxb_Click(object sender, EventArgs e)
        {
            SetConfig();
            if (string.IsNullOrEmpty(config.WorkInfo.filePath))
            {
                MessageBox.Show("请选择切片保存地址");
                return;
            }
            if (!System.IO.Directory.Exists(config.WorkInfo.filePath))
            {
                MessageBox.Show("目录无效");
                return;
            }
            if (config.CurrentExtent == null)
            {
                MessageBox.Show("地图范围无效");
                return;
            }
            var main = this.Owner as FrmMain;
            System.Configuration.ConfigurationManager.AppSettings.Set("ezmapUrl", config.ServerUrl);
            System.Configuration.ConfigurationManager.AppSettings.Set("serviceVersion", config.ServerVersion);
            
            if (main != null)
            {
                var dataGridViewWorks = main.dataGridViewWorks;
                var count = config.WorkInfo.rcList.Sum(m => m.GetCount());
                int index =  dataGridViewWorks.Rows.Add(
              config.WorkInfo.workName,
              "0/" + count.ToString(),
              "0/" + count.ToString(),
              "0/" + count.ToString(),
              "0/" + count.ToString(),
              count.ToString(),
              "暂停下载");
                
                dataGridViewWorks.Rows[index].Tag = config.WorkInfo;

                config.WorkInfo.processDownImage.count = count;
                WorkConfig.GetInstance().Commandconfig.workInfoList.Add(config.WorkInfo);
                WorkConfig.GetInstance().saveConfig();

                main.StartDownLoad(config.WorkInfo, dataGridViewWorks.Rows[index]);
                main.BringToFront();
                main.SetTileDownShow();
                this.Close(); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.txbSavePath.Text = folderBrowser.SelectedPath;
            }
        }

        private void maxZoomTxb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.mapControl1.drawRectangle();
        }

        private void 清除覆盖物ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl1.clearOverlay();
        }
    }
}
