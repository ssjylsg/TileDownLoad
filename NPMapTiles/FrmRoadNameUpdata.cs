using System;
using System.Windows.Forms;
using DevComponents.DotNetBar;
namespace NPMapTiles
{
    using MapDataTools;

    public partial class FrmRoadNameUpdata : Office2007Form
    {
        System.Threading.Thread thread = null;
        public FrmRoadNameUpdata()
        {
            InitializeComponent();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            this.btnDown.Enabled = false;
            this.btnStop.Enabled = true;
            this.thread = new System.Threading.Thread(this.UpdataRoadNames);
            this.thread.Start();
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
                this.btnDown.Enabled = true;
                this.btnStop.Enabled = false;
            }
        }
        private void UpdataRoadNames()
        {
            MapDataTools.RoadNameLoad roadNameLoad = new MapDataTools.RoadNameLoad();
            roadNameLoad.cityRoadLoadLog += new CityRoadLoadLogHandler(this.CityRoadLoadhandler);
            roadNameLoad.UpdateRoads();
        }
        private void CityRoadLoadhandler(string message,int i)
        { 
            MethodInvoker invoker = delegate
            {
                this.progressBar.Value = i;
                this.labMessage.Text = "提示:" + message;
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
