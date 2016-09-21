using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DevComponents.DotNetBar;

using System.Threading;
using System.IO;
using System.Diagnostics;
using MapDataTools;

namespace NPMapTiles
{
    using System.Linq;

    using log4net;

    using MapDataTools.Tile;
    using MapDataTools.Util;

    using ThreadState = System.Threading.ThreadState;

    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class FrmMain : Office2007RibbonForm
    {
        #region private filed

        private Handlers.ProcessNotifyHandler OnProcessNotify;

        private Handlers.ProcessNotifyHandler OnProcessNotify2;

        private Handlers.LogHandler OnLog;

        private Handlers.DownImageHandler DownImage;

        private double maxExtent = 20037508.34;

        private double maxResolution = 156543.03390625;

        private double minX = 0.0;

        private double minY = 0.0;

        private double maxX = 0.0;

        private double maxY = 0.0;

        private MapType mapType = MapType.Google;

        private MapDataTools.MapControl mapControl = null;


        private ILog log;

        private Object obj = new object();

        #endregion

        internal DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewWorks
        {
            get
            {
                return this.dataGridViewX;
            }
            set
            {
                this.dataGridViewX = value;
            }
        }

        #region from

        public FrmMain()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
            InitializeComponent();
            if (this.DesignMode)
            {
                return;
            }
            System.Reflection.Assembly ma = System.Reflection.Assembly.GetEntryAssembly();
            FileInfo fi = new FileInfo(ma.Location);
            FileVersionInfo mfv = FileVersionInfo.GetVersionInfo(ma.Location);
            this.Text += mfv.FileVersion;
            this.tsbPinTu.Visible = false;
            this.stbProgressBar.Visible = false;
            this.stbProgressText.Visible = false;
            this.toolStripLabel1.Visible = false;

            List<ButtonItem> list = new List<ButtonItem>();

            list.Add(btnRoadDown);
            list.Add(btnRoadCrossDown);
            list.Add(btnAdminDivision);
            list.Add(btnRoadNameUpdata);
            list.Add(btnBaiduPoi);
            list.Add(btnPoiDown);
            list.Add(btnPinYing);
            //btnPinYing.Visible = true;
            string authkey = System.Configuration.ConfigurationManager.AppSettings["authkey"].ToString();
            var isPass = authkey.Equals("35615D9551BFA71950119F5688EBE35D");
            list.ForEach(m => m.Visible = isPass);
            Dictionary<string, string> otherForms = new Dictionary<string, string>();
            otherForms.Add("PGIS下载", "NPMapTiles.EzMapTileDownLoadFrm");
            if (isPass)
            {
                otherForms.Add("全数据下载", "NPMapTiles.FrmAllData");
            }

            foreach (KeyValuePair<string, string> keyValuePair in otherForms)
            {
                ButtonItem item = new ButtonItem();
                item.Text = keyValuePair.Key;
                this.ribbonBar2.Items.Add(item);
                item.Click += (sender, args) =>
                    {
                        try
                        {
                            var from = System.Activator.CreateInstance(Type.GetType(keyValuePair.Value)) as Form;
                            if (from != null)
                            {
                                from.Show(this);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            MessageBox.Show("程序出错！");
                        }
                    };
            }


            this.FormClosing += FrmMain_FormClosing;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TaskQueue.GeTaskQueue().GetTaskCount() != 0)
            {
                if (MessageBox.Show("还有运行在后台的紧凑任务，是否关闭程序?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    log.Info("程序退出");
                    TaskQueue.GeTaskQueue().IsRunning = false;
                    Thread.Sleep(500);
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                log.Info("程序退出");
                TaskQueue.GeTaskQueue().IsRunning = false;
                Thread.Sleep(50);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void AddCityNode()
        {
            MethodInvoker invoker = delegate
                {
                    this.myTreeView.Nodes.Clear();
                    TreeNode rootNode = new TreeNode();
                    rootNode.Text = "中国";
                    List<Province> provinces = MapDataTools.CityConfig.GetInstance().Countryconfig.countries;
                    foreach (Province p in provinces)
                    {
                        var pnode = new TreeNode();
                        pnode.Text = p.name;
                        foreach (City c in p.cities)
                        {
                            TreeNode cnode = new TreeNode();
                            cnode.Text = c.name;
                            foreach (District d in c.districts)
                            {
                                TreeNode dnode = new TreeNode();
                                dnode.Text = d.name;
                                cnode.Nodes.Add(dnode);
                            }
                            pnode.Nodes.Add(cnode);
                        }
                        rootNode.Nodes.Add(pnode);
                    }
                    this.myTreeView.Nodes.Add(rootNode);
                    rootNode.Expand();
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

        private void mapMovingHandler(ArcMapEvent arcMapEvent)
        {
            this.labLon.Text = arcMapEvent.lon.ToString();
            this.labLat.Text = arcMapEvent.lat.ToString();
        }

        private void mapZoomedHandler(ArcMapEvent arcMapEvent)
        {
            this.labZoom.Text = arcMapEvent.zoom.ToString();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(this.AddCityNode);
            t.Start();

            mapPanelNew.Controls.Clear();
            mapControl = new MapControl();
            mapControl.Dock = DockStyle.Fill;
            mapControl.mapMovingHandler += new MapControl.MapMovingHandler(this.mapMovingHandler);
            mapControl.mapZoomedHandler += new MapControl.MapZoomedHandler(this.mapZoomedHandler);
            mapControl.overlayEditedHandler += (extent) =>
                {
                    this.maxX = extent.maxX;
                    this.maxY = extent.maxY;
                    this.minX = extent.minX;
                    this.minY = extent.minY;
                };
            mapPanelNew.Controls.Add(mapControl);


            cmbMapType.DisplayMember = "Text";
            cmbMapType.Items.Clear();
            cmbMapType.Items.AddRange(
                new ComboxItem[]
                    {
                        new ComboxItem() { Tag = "Google", Value = MapType.Google, Text = "谷歌矢量图" },
                        new ComboxItem() { Tag = "GoogleImage", Value = MapType.GoogleImage, Text = "谷歌影像图" },
                        new ComboxItem() { Tag = "TianDi", Value = MapType.Tiandi, Text = "天地矢量图" },
                        new ComboxItem() { Tag = "TianDiImage", Value = MapType.TiandiImage, Text = "天地影像图" },
                        new ComboxItem() { Tag = "GaoDe", Value = MapType.Gaode, Text = "高德矢量图" },
                        new ComboxItem() { Tag = "GaoDeImage", Value = MapType.GaodeImage, Text = "高德影像图" },
                         new ComboxItem() { Tag = "initOSMMap", Value = MapType.OpenStreetMap, Text = "OpenStreet" },
                        new ComboxItem() { Tag = "BaiduImage", Value = MapType.Baidu, Text = "百度矢量图" },
                        new ComboxItem(){Tag = "BaiduImageTile",Value = MapType.BaiduImageTile,Text = "百度影像图"},
                       // new ComboxItem(){Tag = "InitQQMap",Value = MapType.QQMap,Text = "腾讯地图"}, 
                    });
            cmbCoverteType.Items.Clear();
            this.cmbCoverteType.DisplayMember = "Text";
            this.cmbCoverteType.Items.AddRange(
                new ComboxItem[]
                    {
                        new ComboxItem() { Text = "WGS84-->GCJ-02", Value = CovertType.WGS2GCJ02 },
                        new ComboxItem() { Text = "WGS84-->百度", Value = CovertType.WGS2BAIDU },
                        new ComboxItem() { Text = "GCJ-02-->百度", Value = CovertType.GCJ022BAIDU },
                        new ComboxItem() { Text = "墨卡托平面-->经纬度", Value = CovertType.MOCTOR2LONLAT },
                        new ComboxItem() { Text = "经纬度(火星)-->墨卡托平面", Value = CovertType.LONLAT2MOCTOR },
                        new ComboxItem() { Text = "百度-->GCJ02", Value = CovertType.BAIDU2GCJ02},
                        new ComboxItem(){Text = "WGS84-->墨卡托平面",Value = CovertType.WGS2MOCTOR}, 
                    });


            if (this.cmbMapType.Items.Count > 0)
            {
                cmbMapType.SelectedIndex = 2;
            }
            if (this.cmbCoverteType.Items.Count > 0)
            {
                this.cmbCoverteType.SelectedIndex = 0;
            }
            this.OnProcessNotify += new Handlers.ProcessNotifyHandler(this.ShowMsg);
            this.OnProcessNotify2 += new Handlers.ProcessNotifyHandler(this.ShowMsg2);
            this.OnLog += new Handlers.LogHandler(this.WriteLog);
            this.DownImage += new Handlers.DownImageHandler(this.doNewThings);
            this.myTreeView.ExpandAll();
            this.tabControl.SelectedTab = tabItemMap;
            this.WriteLog("启动结束");
            List<WorkInfo> workInfos = WorkConfig.GetInstance().Commandconfig.workInfoList;
            foreach (WorkInfo workInfo in workInfos)
            {
                string x = "未完成下载：" + workInfo.processDownImage.processIndex.ToString() + "/"
                           + workInfo.processDownImage.count.ToString();
                string sta = "停止下载";
                if (workInfo.downStates == DownStates.start)
                {
                    sta = "暂停下载";
                }
                else if (workInfo.downStates == DownStates.pause)
                {
                    sta = "暂停下载";
                }
                else if (workInfo.downStates == DownStates.stop)
                {
                    sta = "停止下载";
                }
                else if (workInfo.downStates == DownStates.ready)
                {
                    sta = "完成下载";
                }
                if (workInfo.processDownImage.processIndex == workInfo.processDownImage.count)
                {
                    x = "已完成下载：" + workInfo.processDownImage.processIndex.ToString() + "/"
                        + workInfo.processDownImage.count.ToString();
                }
                int index = this.dataGridViewWorks.Rows.Add(
                    workInfo.workName,
                    x,
                    workInfo.processDownImage.secess.ToString() + "/" + workInfo.processDownImage.count.ToString(),
                    workInfo.processDownImage.lose.ToString() + "/" + workInfo.processDownImage.count.ToString(),
                    workInfo.processDownImage.lose.ToString() + "/" + workInfo.processDownImage.count.ToString(),
                    workInfo.processDownImage.count.ToString(),
                    sta);
                this.dataGridViewWorks.Rows[index].Tag = workInfo;
            }
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion

        private void btnGetRange_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (TaskQueue.GeTaskQueue().GetTaskCount() != 0)
            {
                if (MessageBox.Show("还有运行在后台的紧凑任务，是否关闭程序?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    TaskQueue.GeTaskQueue().IsRunning = false;
                    Thread.Sleep(500);
                    this.Close();
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        #region 

        private void doNewThings(string workID, int i, int count, int sucess, int lose)
        {
            string threadName = Thread.CurrentThread.Name;
            MethodInvoker invoker = delegate
                {
                    foreach (DataGridViewRow row in this.dataGridViewWorks.Rows)
                    {
                        WorkInfo workInfo = row.Tag as WorkInfo;
                        if (workInfo != null)
                        {
                            string temp = workInfo.id;
                            if (temp == workID)
                            {
                                if (i == count)
                                {
                                    row.Cells[1].Value = "已下载完成：" + i.ToString() + "/" + count.ToString();
                                    row.Cells["workStatues"].Value = "完成下载";
                                }
                                else
                                {
                                    row.Cells[1].Value = "未下载完成：" + i.ToString() + "/" + count.ToString();
                                }
                                row.Cells[2].Value = sucess.ToString() + "/" + count.ToString();
                                row.Cells[3].Value = lose.ToString() + "/" + count.ToString();
                                row.Cells[4].Value = lose;
                            }
                        }
                    }

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

        //private List<int> GetRowAndCols()
        //{
        //    List<int> rowCols = new List<int>();
        //    double minX = 0;
        //    string strMinRow = this.txbMinX.Text.Trim();
        //    double.TryParse(strMinRow, out minX);

        //    double maxX = 0;
        //    string strMaxRow = this.txbMaxX.Text.Trim();
        //    double.TryParse(strMaxRow, out maxX);

        //    double minY = 0;
        //    string strMinCol = this.txbMinY.Text.Trim();
        //    double.TryParse(strMinCol, out minY);

        //    double maxY = 0;
        //    string strMaxCol = this.txbMaxY.Text.Trim();
        //    double.TryParse(strMaxCol, out maxY);

        //    int minZoom = 0;
        //    string strminZoom = this.txbMinZoom.Text.Trim();
        //    int.TryParse(strminZoom, out minZoom);

        //    int minRow = (int)Math.Floor((minX + maxExtent) / (maxResolution / (Math.Pow(2, minZoom)) * 256.0));
        //    int maxRow = (int)Math.Ceiling((maxX + maxExtent) / (maxResolution / (Math.Pow(2, minZoom)) * 256.0));
        //    int minCel = (int)Math.Floor((maxExtent - maxY) / (maxResolution / (Math.Pow(2, minZoom)) * 256.0));
        //    int maxCel = (int)Math.Ceiling((maxExtent - minY) / (maxResolution / (Math.Pow(2, minZoom)) * 256.0));
        //    rowCols.Add(minRow);
        //    rowCols.Add(maxRow);
        //    rowCols.Add(minCel);
        //    rowCols.Add(maxCel);
        //    return rowCols;
        //}

        //private void GetTiles(int minRow, int maxRow, int minCol, int maxCol, int zoom)
        //{

        //}


        private void ShowMsg(string msg1, int process)
        {
            string threadName = Thread.CurrentThread.Name;
            MethodInvoker invoker = delegate
                {
                    this.labMessge.Text = msg1;
                    this.progressBar.Value = process;
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

        private void ShowMsg2(string msg1, int process)
        {
            string threadName = Thread.CurrentThread.Name;
            MethodInvoker invoker = delegate
                {
                    this.stbProgressText.Text = msg1;
                    this.stbProgressBar.Value = process;
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

        private void WriteLog(string msg)
        {
            log.Info(msg);
        }

        #endregion

        private void btnGetURL_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbUrl.Text = folder.SelectedPath;
            }
        }
        private void tsbNewThing_Click(object sender, EventArgs e)
        {
            FrmNewThing frmNewThing = new FrmNewThing(
                this.minX,
                this.minY,
                this.maxX,
                this.maxY,
                this.mapType,
                dataGridViewWorks);
            frmNewThing.ShowDialog();
        }
        
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            for (int i = 0; i < this.dataGridViewWorks.SelectedRows.Count; i++)
            {
                rows.Add(this.dataGridViewWorks.SelectedRows[i]);
            }
            if (rows.Count > 0)
            {
                if (MessageBox.Show(
                    "确定要删除该任务？",
                    "提示",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in rows)
                    {
                        WorkInfo workInfo = row.Tag as WorkInfo;
                        workInfo.downStates = DownStates.stop;
                        if (workInfo != null && myDicThreads.ContainsKey(workInfo.id))
                        {
                            workInfo.rcList.ForEach(
                                m =>
                                    {
                                        var t = m.GetData() as Thread;
                                        if (t != null)
                                        {
                                            t.Abort();
                                        }
                                    });
                            myDicThreads[workInfo.id].Abort();
                            myDicThreads[workInfo.id] = null;
                            myDicThreads.Remove(workInfo.id);
                        }
                        lock (obj)
                        {
                            WorkConfig.GetInstance().Commandconfig.workInfoList.Remove(workInfo);
                            WorkConfig.GetInstance().saveConfig();
                        }
                        this.dataGridViewWorks.Rows.Remove(row);
                    }
                }
            }
        }

        private void tsbOpenDirectory_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWorks.SelectedRows.Count == 0) return;
            DataGridViewRow row = this.dataGridViewWorks.SelectedRows[0];
            WorkInfo workInfo = row.Tag as WorkInfo;
            if (workInfo != null)
            {
                string path = workInfo.filePath;
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }

        private Dictionary<string, Thread> myDicThreads = new Dictionary<string, Thread>();

        private void tsbStart_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWorks.SelectedRows.Count == 0) return;
            bool isLincence = LisenceManager.IsKeyUsing();
            if (!isLincence)
            {
                if (MessageBox.Show(
                    "您没有许可权限,是否需要注册？",
                    "提示",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    FrmLinence frmLinence = new FrmLinence();
                    frmLinence.ShowDialog();
                }
                return;
            }
            DataGridViewRow row = this.dataGridViewWorks.SelectedRows[0];
            WorkInfo workInfo = row.Tag as WorkInfo;
            this.StartDownLoad(workInfo, row);
        }

        internal void StartDownLoad(WorkInfo workInfo, DataGridViewRow row)
        {
            if (workInfo != null)
            {
                workInfo.downStates = DownStates.start;
                ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ThreadMethod);
                Thread myThread = new Thread(ParStart);
                if (myDicThreads.ContainsKey(workInfo.id))
                {
                    if (myDicThreads[workInfo.id].ThreadState == System.Threading.ThreadState.Suspended)
                    {
                        myDicThreads[workInfo.id].Resume();
                        workInfo.rcList.ForEach(
                            m =>
                            {
                                var t = m.GetData() as Thread;
                                if (t != null && t.ThreadState == System.Threading.ThreadState.Suspended)
                                {
                                    t.Resume();
                                }
                            });
                    }
                    else if (myDicThreads[workInfo.id].ThreadState == System.Threading.ThreadState.Stopped)
                    {
                        myDicThreads[workInfo.id] = myThread;
                        myThread.Start(workInfo);
                        workInfo.rcList.ForEach(
                            m =>
                            {
                                var t = m.GetData() as Thread;
                                if (t != null && t.ThreadState == System.Threading.ThreadState.Stopped)
                                {
                                    t.Start();
                                }
                            });
                    }
                }
                else
                {
                    myDicThreads.Add(workInfo.id, myThread);
                    myThread.Start(workInfo);
                }
                MethodInvoker invoker = delegate
                {
                    row.Cells["workStatues"].Value = "正在下载";
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
        public void ThreadMethod(object ParObject)
        {
            WorkInfo workInfo = ParObject as WorkInfo;
            workInfo = WorkConfig.GetInstance().GetWorkInfoByID(workInfo.id);
            if (workInfo != null)
            {
                ProcessDownImage processDown = new ProcessDownImage();
                foreach (RowColumns rc in workInfo.rcList)
                {
                    processDown.count = processDown.count + (rc.maxRow - rc.minRow + 1) * (rc.maxCol - rc.minCol + 1);
                }
                //if (workInfo.mapType == MapType.Tiandi)
                //{
                //    processDown.count = processDown.count;
                //}
                workInfo.processDownImage = processDown;
                int minZoom = 0;
                int maxZoom = 0;
                if (workInfo.rcList.Count > 0)
                {
                    minZoom = workInfo.rcList[0].zoom;
                    maxZoom = workInfo.rcList[0].zoom;
                }
                foreach (RowColumns rc in workInfo.rcList)
                {
                    if (minZoom > rc.zoom) minZoom = rc.zoom;
                    if (maxZoom < rc.zoom) maxZoom = rc.zoom;
                }
                workInfo.CreateTitle().CreateHtmlDemo(workInfo,minZoom,maxZoom);
                workInfo.CreateConfig();
                //FileTools.HtmlTool.CreateHtmlDemo(workInfo, minZoom, maxZoom);
                //if (workInfo.isAusterityFile)
                //{
                //    string austerityFilePath = Path.Combine(workInfo.filePath, "Layers");
                //    if (!Directory.Exists(austerityFilePath))
                //    {
                //        Directory.CreateDirectory(austerityFilePath);
                //    }
                //    workInfo.createAusterityConfig(austerityFilePath);
                //}
                var isMutilThread =
                    WorkConfig.GetInstance()
                        .Commandconfig.workInfoList.FirstOrDefault(
                            m => m.downStates == DownStates.start && m.id != workInfo.id) != null;
                foreach (RowColumns rc in workInfo.rcList)
                {
                    if (workInfo.downStates != DownStates.stop)
                    {

                        if (isMutilThread)
                        {
                            NewGetTiles(rc.minRow, rc.maxRow, rc.minCol, rc.maxCol, rc.zoom, workInfo);
                        }
                        else
                        {
                            Thread t = rc.GetData() as Thread;
                            if (t != null && t.ThreadState == ThreadState.Stopped)
                            {
                                t.Resume();
                            }
                            else
                            {
                                var result = System.Threading.ThreadPool.QueueUserWorkItem(
                                    new WaitCallback(
                                        o =>
                                            {
                                                NewGetTiles(
                                                    rc.minRow,
                                                    rc.maxRow,
                                                    rc.minCol,
                                                    rc.maxCol,
                                                    rc.zoom,
                                                    workInfo);
                                            }));
                                if (!result) // 加入线程池失败，则直接下载
                                {
                                    NewGetTiles(rc.minRow, rc.maxRow, rc.minCol, rc.maxCol, rc.zoom, workInfo);
                                }
                            }

                        }
                    }
                }
                if (workInfo.downStates == DownStates.start)
                {
                    workInfo.downStates = DownStates.ready;
                }
                else if (workInfo.downStates == DownStates.stop)
                {
                    if (this.DownImage != null)
                    {
                        this.DownImage(workInfo.id, 0, workInfo.processDownImage.count, 0, 0);
                    }
                }
                workInfo.WriteConfigurationToFile();
                lock (obj)
                {
                    WorkConfig.GetInstance().saveConfig();
                }
            }
        }

        private void NewGetTiles(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo)
        {
            MapTile tile = workInfo.CreateTitle();
            if (tile != null)
            {
                tile.DownImage += doNewThings;
                tile.OnLog += o =>
                    {
                        this.log.Error(o);
                    };
                tile.DownLoad(minRow, maxRow, minCol, maxCol, zoom, workInfo);
            }
        }

        #region 按钮事件

        private void tsbPause_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWorks.SelectedRows.Count == 0) return;
            DataGridViewRow row = this.dataGridViewWorks.SelectedRows[0];
            WorkInfo workInfo = row.Tag as WorkInfo;
            if (workInfo != null && myDicThreads.ContainsKey(workInfo.id))
            {
                if (myDicThreads[workInfo.id].ThreadState == System.Threading.ThreadState.Running)
                {
                    myDicThreads[workInfo.id].Suspend();
                    MethodInvoker invoker = delegate
                        {
                            workInfo.downStates = DownStates.pause;
                            row.Cells["workStatues"].Value = "暂停下载";
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
                else
                {
                    workInfo.rcList.ForEach(
                        m =>
                            {
                                var t = m.GetData() as Thread;
                                if (t != null && t.ThreadState == System.Threading.ThreadState.Running)
                                {
                                    t.Suspend();
                                }
                            });
                    MethodInvoker invoker = delegate
                        {
                            workInfo.downStates = DownStates.stop;
                            row.Cells["workStatues"].Value = "暂停下载";
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

        private void tsbPan_Click(object sender, EventArgs e)
        {
            mapControl.pan();
        }

        private void tsbSelectZoomIn_Click(object sender, EventArgs e)
        {
            mapControl.selectZoomIn();
        }

        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            mapControl.zoomIn();
        }

        private void tsbZoomOut_Click(object sender, EventArgs e)
        {
            mapControl.zoomOut();
        }

        private void tsbDraw_Click(object sender, EventArgs e)
        {
            mapControl.drawRectangle();
        }

        private void btnSelectZoomOut_Click(object sender, EventArgs e)
        {
            this.mapControl.selectZoomOut();
        }

        private void tsbMapNewThing_Click(object sender, EventArgs e)
        {
            if (mapControl.extent != null)
            {
                this.maxX = mapControl.extent.maxX;
                this.maxY = mapControl.extent.maxY;
                this.minX = mapControl.extent.minX;
                this.minY = mapControl.extent.minY;
            }
            else
            {
                MessageBox.Show("请绘制选区");
                return;
            }
            //if (this.minX > 0 && this.minY > 0 && this.maxX > 0 && this.maxY > 0)
            {
                FrmNewThing frmNewThing = new FrmNewThing(
                    this.minX,
                    this.minY,
                    this.maxX,
                    this.maxY,
                    this.mapType,
                    dataGridViewWorks);
                if (frmNewThing.ShowDialog() == DialogResult.OK)
                {
                    frmNewThing.Close();
                    this.tabControl.SelectedTab = this.tabItemWork;
                }
            }
            //else MessageBox.Show("请绘制选区");
        }

        internal void SetTileDownShow()
        {
            this.tabControl.SelectedTab = this.tabItemWork;
        }
        private void tsbClearBounds_Click(object sender, EventArgs e)
        {
            this.mapControl.clearOverlay();
            this.minX = 0.0;
            this.minY = 0.0;
            this.maxX = 0.0;
            this.maxY = 0.0;
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridViewWorks.SelectedRows.Count == 0) return;
                DataGridViewRow row = this.dataGridViewWorks.SelectedRows[0];
                WorkInfo workInfo = row.Tag as WorkInfo;
                if (workInfo != null && myDicThreads.ContainsKey(workInfo.id))
                {

                    workInfo.downStates = DownStates.stop;
                    myDicThreads.Remove(workInfo.id);

                    //workInfo.rcList.ForEach(
                    //    m =>
                    //        {
                    //        });

                    if (this.DownImage != null)
                    {
                        this.DownImage(workInfo.id, 0, workInfo.processDownImage.count, 0, 0);
                    }
                    MethodInvoker invoker = delegate
                        {
                            row.Cells["workStatues"].Value = "停止下载";
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
            catch (Exception ex)
            {

            }
        }

        private void dataGridViewX_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.dataGridViewWorks.SelectedRows.Count == 0)
            {
                return;
            }
            DataGridViewRow row = this.dataGridViewWorks.SelectedRows[0];
            WorkInfo workInfo = row.Tag as WorkInfo;
            if (workInfo != null)
            {
                string path = workInfo.filePath;
                path = System.IO.Path.Combine(path, "temp.html");
                if (File.Exists(path))
                {
                    webBrowser.Navigate(path);
                }
            }
        }

        private void tsbPinTu_Click(object sender, EventArgs e)
        {

        }

        private void tsbProperty_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWorks.SelectedRows.Count == 0)
            {
                return;
            }
            DataGridViewRow row = this.dataGridViewWorks.SelectedRows[0];
            WorkInfo workInfo = row.Tag as WorkInfo;
            if (workInfo != null)
            {
                FrmProperty frmProperty = new FrmProperty(workInfo);
                frmProperty.ShowDialog();
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        public class ComboxItem
        {
            public string Text { get; set; }

            public object Value { get; set; }

            public object Tag { get; set; }
        }

        /// <summary>
        /// 当地图类型选择改变时，出发，切换相应的地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMapTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Coord coord = this.mapControl.GetCenter();
            Coord result = coord;
            int oldZoom = 0;
            MapType oldMapType = MapType.Google;
            if (coord != null)
            {
                result = coord;
                oldZoom = this.mapControl.GetZoom();
                oldMapType = this.mapType;
            }
            var selectItem = (ComboxItem)this.cmbMapType.SelectedItem;
            this.mapControl.setMap(selectItem.Tag.ToString());
            this.mapType = (MapType)selectItem.Value;

            if (coord == null)
            {
                return;
            }
            switch (oldMapType)
            {
                case MapType.Baidu:
                case MapType.BaiduImageTile:
                    result = CoordHelper.WebMercator2lonLat(coord);
                    result = CoordHelper.BdDecrypt(result.lat, result.lon);
                    result = CoordHelper.Gcj2Wgs(result.lon, result.lat);
                    break;
                case MapType.Google:
                case MapType.GoogleImage:
                    result = CoordHelper.WebMercator2lonLat(coord);
                    result = CoordHelper.Gcj2Wgs(result.lon, result.lat);
                    break;
                case MapType.Gaode:
                case MapType.GaodeImage:
                case MapType.QQMap:
                case MapType.QQImage:
                    result = CoordHelper.WebMercator2lonLat(coord);
                    result = CoordHelper.Gcj2Wgs(result.lon, result.lat);
                    break;
                case MapType.OpenStreetMap:
                    result = CoordHelper.WebMercator2lonLat(coord);
                    break;
                case MapType.Tiandi:
                case MapType.TiandiImage:
                    result = coord;
                    break;
            }
            switch (this.mapType)
            {
                case MapType.Baidu:
                case MapType.BaiduImageTile:
                    result = CoordHelper.Transform(result.lon, result.lat);
                    result = CoordHelper.BdEncrypt(result.lat, result.lon);
                    result = CoordHelper.WebMoctorJw2Pm(result.lon, result.lat);
                    break;
                case MapType.Google:
                case MapType.GoogleImage:
                    result = CoordHelper.Transform(result.lon, result.lat);
                    result = CoordHelper.WebMoctorJw2Pm(result.lon, result.lat);
                    break;
                case MapType.Gaode:
                case MapType.GaodeImage:
                case MapType.QQImage:
                case MapType.QQMap:
                    result = CoordHelper.Transform(result.lon, result.lat);
                    result = CoordHelper.WebMoctorJw2Pm(result.lon, result.lat);
                    break;
                case MapType.OpenStreetMap:
                    result = CoordHelper.WebMoctorJw2Pm(result.lon, result.lat);
                    break;
                case MapType.Tiandi:
                case MapType.TiandiImage:
                    break;
            }
            this.mapControl.SetZoom(oldZoom);
            this.mapControl.SetCenter(result);
            tsbClearBounds_Click(null, null);
        }

        private void myTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text != "中国" && e.Node.Text != "")
            {
                this.mapControl.locationCity(e.Node.Text);
            }
        }

        private void btnAdminDivision_Click(object sender, EventArgs e)
        {
            FrmDownAdminDivision frmDownAdminDivision = new FrmDownAdminDivision();
            frmDownAdminDivision.ShowDialog();
        }

        private void btnRoadCrossDown_Click(object sender, EventArgs e)
        {
            FrmDownRoadCross frmDownRoadCross = new FrmDownRoadCross();
            frmDownRoadCross.ShowDialog();
        }

        private void btnRoadDown_Click(object sender, EventArgs e)
        {
            FrmDownRoadLine frmDownRoadLine = new FrmDownRoadLine();
            frmDownRoadLine.ShowDialog();
        }

        private void btnBaiduPosition_Click(object sender, EventArgs e)
        {
            FrmBaiduRectify frmBaiduRectify = new FrmBaiduRectify();
            frmBaiduRectify.ShowDialog();
        }

        private void btnGooglePosition_Click(object sender, EventArgs e)
        {
            FrmGoogleRectify frmGoogleRectify = new FrmGoogleRectify();
            frmGoogleRectify.ShowDialog();
        }

        private void btnGaoDePosition_Click(object sender, EventArgs e)
        {
            FrmGoogleRectify frmGoogleRectify = new FrmGoogleRectify();
            frmGoogleRectify.Text = "高德坐标纠偏";
            frmGoogleRectify.ShowDialog();
        }

        private void btnTengXunPosition_Click(object sender, EventArgs e)
        {
            FrmGoogleRectify frmGoogleRectify = new FrmGoogleRectify();
            frmGoogleRectify.Text = "腾讯坐标纠偏";
            frmGoogleRectify.ShowDialog();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            var selectItem = (ComboxItem)this.cmbCoverteType.SelectedItem;
            FrmCoverteRectify frmCovertRectify = new FrmCoverteRectify((CovertType)selectItem.Value);
            frmCovertRectify.Text = selectItem.Text;
            frmCovertRectify.ShowDialog();
        }

        private void btnPoiDown_Click(object sender, EventArgs e)
        {
            if (this.mapType != MapType.Gaode && this.mapType != MapType.GaodeImage)
            {
                MessageBox.Show("请选择高德地图");
                return;
            }
            if (this.mapControl.extent == null)
            {
                MessageBox.Show("请选择下载区域");
                return;
            }
            FrmPOIDown frmPOIDown = new FrmPOIDown(this.mapControl.extent);
            frmPOIDown.ShowDialog();
        }

        private void btnRoadNameUpdata_Click(object sender, EventArgs e)
        {
            FrmRoadNameUpdata frmRoadNameUpdata = new FrmRoadNameUpdata();
            frmRoadNameUpdata.ShowDialog();
        } 
        private void btnGetAdress_Click(object sender, EventArgs e)
        {
            FrmGetAddressByPoint frmGetAddressByPoint = new FrmGetAddressByPoint();
            frmGetAddressByPoint.ShowDialog();
        }

        private void btnGetPosition_Click(object sender, EventArgs e)
        {
            FrmGetpointByAddress frmGetpointByAddress = new FrmGetpointByAddress();
            frmGetpointByAddress.ShowDialog();
        }

        private void btnBaiduPoi_Click(object sender, EventArgs e)
        {
            if (this.mapControl.extent == null)
            {
                MessageBox.Show("请选择下载区域");
                return;
            }
            Extent extent = this.mapControl.extent;
            Extent newExtent = new Extent();
            if (this.mapType == MapType.Gaode || this.mapType == MapType.GaodeImage || this.mapType == MapType.Google
                || this.mapType == MapType.GoogleImage)
            {
                Coord leftTop = CoordHelper.Mercator2lonLat(extent.minX, extent.minY);
                leftTop = CoordHelper.BdEncrypt(leftTop.lat, leftTop.lon);
                Coord rightBottum = CoordHelper.Mercator2lonLat(extent.maxX, extent.maxY);
                rightBottum = CoordHelper.BdEncrypt(rightBottum.lat, rightBottum.lon);
                newExtent.minX = leftTop.lon;
                newExtent.minY = leftTop.lat;
                newExtent.maxX = rightBottum.lon;
                newExtent.maxY = rightBottum.lat;
            }
            else if (this.mapType == MapType.Tiandi || this.mapType == MapType.TiandiImage)
            {
                Coord leftTop = CoordHelper.Transform(extent.minX, extent.minY);
                leftTop = CoordHelper.BdEncrypt(leftTop.lat, leftTop.lon);
                Coord rightBottum = CoordHelper.Transform(extent.maxX, extent.maxY);
                rightBottum = CoordHelper.BdEncrypt(rightBottum.lat, rightBottum.lon);
                newExtent.minX = leftTop.lon;
                newExtent.minY = leftTop.lat;
                newExtent.maxX = rightBottum.lon;
                newExtent.maxY = rightBottum.lat;
            }
            else
            {
                MessageBox.Show("该地图类型不支持兴趣点下载,选择其他类型");
                return;
            }
            FrmPOIDownForBaidu frmPOIDown = new FrmPOIDownForBaidu(newExtent);
            frmPOIDown.Show();
        }

        private void btnTileCoverter_Click(object sender, EventArgs e)
        {
            FrmTileCoverter frmTileCoverter = new FrmTileCoverter();
            frmTileCoverter.Show();
        }

        private void btnLincence_Click(object sender, EventArgs e)
        {
            FrmLinence frmLinence = new FrmLinence();
            frmLinence.Show();
        }

        private void btnWaiWang_Click(object sender, EventArgs e)
        {
            Process.Start("http://113.200.159.100:9500");
        }

        private void btnNeiWang_Click(object sender, EventArgs e)
        {
            Process.Start("http://192.168.60.242:1080");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
             Process.Start(Application.StartupPath + "\\help.chm");
        }

        private void btnQuestion_Click(object sender, EventArgs e)
        {
            FrmCaption frmCaption = new FrmCaption();
            frmCaption.Show();
        }

        private void btnExcel2Shp_Click_1(object sender, EventArgs e)
        {
            new FrmExcel2Shp().Show(this);
        }

        private void btnShp2Excel_Click_1(object sender, EventArgs e)
        {
            new FrmShp2Excel().Show(this);
        }

        private void btnRegion2Line_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            new FrmChnCharInfo().Show(this);
        }

        #endregion
    }
}
