using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using System.IO;

namespace NPMapTiles
{
    using MapDataTools.Util;

    public partial class FrmNewThing : Office2007Form
    {
        private double maxExtent = 20037508.34;

        private double maxResolution = 156543.03390625;

        private double minX = 0.0;

        private double minY = 0.0;

        private double maxX = 0.0;

        private double maxY = 0.0;

        private MapType mapType = MapType.Google;

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewWorks;

        public FrmNewThing(
            double minX,
            double minY,
            double maxX,
            double maxY,
            MapType mapType,
            DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewWorks)
        {
            InitializeComponent();
            btnOk.DialogResult = DialogResult.OK;
            btnCanel.DialogResult = DialogResult.Cancel;
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.mapType = mapType;
            this.txbLeftBottom.Text = this.minX.ToString() + "," + this.minY.ToString();
            this.txbRightTop.Text = this.maxX.ToString() + "," + this.maxY.ToString();
            this.dataGridViewWorks = dataGridViewWorks;
            AddLevels();
        }

        private void AddLevels()
        {
            var tile = WorkInfo.CreateTitle(this.mapType);
            for (int i = 6; i < tile.MaxTileCount; i++)
            {
                RowColumns rc = tile.GetRowColomns(this.minX, this.minY, this.maxX, this.maxY, i);
                int count = (rc.maxRow - rc.minRow + 1) * (rc.maxCol - rc.minCol + 1);
                double memorySize = count * tile.TileSize / 1024.0;
                int index =
                    this.dataGridViewX.Rows.Add(
                        new object[]
                                {
                                    false, "第" + i.ToString() + "级", rc.maxRow - rc.minRow + 1, rc.maxCol - rc.minCol + 1,
                                    count, memorySize.ToString("0.00") + "MB"
                                });
                this.dataGridViewX.Rows[index].Tag = i; 
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridViewX.Rows.Count; i++)
            {
                this.dataGridViewX.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnReverseSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridViewX.Rows.Count; i++)
            {
                if ((bool)this.dataGridViewX.Rows[i].Cells[0].Value)
                {
                    this.dataGridViewX.Rows[i].Cells[0].Value = false;
                }
                else this.dataGridViewX.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridViewX.Rows.Count; i++)
            {
                this.dataGridViewX.Rows[i].Cells[0].Value = false;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbSavePath.Text = folder.SelectedPath;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txbSavePath.Text))
            {
                MessageBox.Show("请输入有效存储路径");
                this.DialogResult = DialogResult.None;
                return;
            }
            if (txbName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入任务名称");
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                this.minX = double.Parse(this.txbLeftBottom.Text.Split(',')[0]);
                this.minY = double.Parse(this.txbLeftBottom.Text.Split(',')[1]);
                this.maxX = double.Parse(this.txbRightTop.Text.Split(',')[0]);
                this.maxY = double.Parse(this.txbRightTop.Text.Split(',')[1]);
            }
            catch (Exception)
            {

            }
           

            int count = 0;
            NPMapTiles.ImageTools.MapTool mapTool = new ImageTools.MapTool();
            List<RowColumns> rcList = new List<RowColumns>();
            var tile = WorkInfo.CreateTitle(this.mapType);
            for (int i = 0; i < this.dataGridViewX.Rows.Count; i++)
            {
                if ((bool)this.dataGridViewX.Rows[i].Cells[0].Value)
                {
                    int z = (int)this.dataGridViewX.Rows[i].Tag;
                    count = count + (int)this.dataGridViewX.Rows[i].Cells[4].Value;
                    RowColumns rc = tile.GetRowColomns(this.minX, this.minY, this.maxX, this.maxY, z);
                    rcList.Add(rc);
                }
            }
            if (rcList.Count == 0)
            {
                MessageBox.Show("请至少选择一个层级");
                this.DialogResult = DialogResult.None;
                return;
            }
            string name = this.txbName.Text.Trim();
            int index = this.dataGridViewWorks.Rows.Add(
                name,
                "0/" + count.ToString(),
                "0/" + count.ToString(),
                "0/" + count.ToString(),
                "0/" + count.ToString(),
                count.ToString(),
                "暂停下载");
            WorkInfo workInfo = new WorkInfo();
            if (checkBoxAusterityFile.Checked)
            {
                workInfo.isAusterityFile = true;
            }
            else
            {
                workInfo.isAusterityFile = false;
            }
            workInfo.maxX = this.maxX;
            workInfo.maxY = this.maxY;
            workInfo.minX = this.minX;
            workInfo.minY = this.minY;
            workInfo.mapType = this.mapType;
            workInfo.rcList = rcList;
            workInfo.filePath = this.txbSavePath.Text.Trim();
            workInfo.workName = name;
            workInfo.processDownImage.count = count;
            workInfo.id = System.Guid.NewGuid().ToString();
            this.dataGridViewWorks.Rows[index].Tag = workInfo;
            WorkConfig.GetInstance().Commandconfig.workInfoList.Add(workInfo);
            WorkConfig.GetInstance().saveConfig();
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbVectorMap_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbVectorMap.Checked)
            {
                this.checkBoxAusterityFile.Enabled = true;
            }
        }
    }
}
