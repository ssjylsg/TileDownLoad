using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace CutDataTiles
{
    public partial class MainForm : Form
    {
        NpgsqlConnection dbcon = null;
        object lockObj = new object();
        /// <summary>
        /// 委托，主要执行提示信息，对控件进行操作
        /// </summary>
        /// <param name="msg1">提示信息</param>
        /// <param name="process">进度</param>
        private delegate void ProcessNotifyHandler(string msg1, int process);
        private ProcessNotifyHandler OnProcessNotify;
        double maxY = 0;
        double minX = 0;
        double maxX = 0;
        double minY = 0;
        int minZoom = 0;
        int maxZoom = 0;
        List<double> resolutions=new List<double>();
        List<double> orgions = new List<double>();
        OleDbConnection OleConn = null;
        string tableName = "";
        public MainForm()
        {
            InitializeComponent();
            this.OnProcessNotify += new ProcessNotifyHandler(this.ShowMsg);
        }
        /// <summary>
        /// 对信息进行更新到指定提示控件
        /// </summary>
        /// <param name="msg1">提示信息</param>
        /// <param name="process">进度值</param>
        private void ShowMsg(string msg1, int process)
        {
            try
            {
                MethodInvoker invoker = delegate
                {
                    this.labMessegbox.Text = msg1;
                    this.progressBar.Value = process;
                    this.progressBar.Update();
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
            catch { }
        }
        /// <summary>
        /// 连接postgis数据库
        /// by songjiang 20141113
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.txbDataBase.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入DataBase！");
                return;
            }
            if (this.txbServer.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入Server！");
                return;
            }
            if (this.txbUser.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入User！");
                return;
            }
            if (this.txbPassWord.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入PassWord！");
                return;
            }
            //连接数据库
            string connString = "Server = " + txbServer.Text.Trim() + ";Port=5432;user id = " + txbUser.Text.Trim() + ";password = " + txbPassWord.Text.Trim() + ";Database = " + txbDataBase.Text.Trim() + ";";
            dbcon = new NpgsqlConnection(connString);
            try
            {
                dbcon.Open();
                // NpgsqlCommand command = new NpgsqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", dbcon);
                NpgsqlCommand command = new NpgsqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public'", dbcon);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    this.cmbTableName.Items.Add(dr.GetString(0));
                }
                if (this.cmbTableName.Items.Count > 0)
                    this.cmbTableName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开数据库失败；" + ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.cmbTableName.SelectedItem!=null&&this.cmbTableName.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("确保数据库中有表");
                return;
            }
            this.tableName = this.cmbTableName.SelectedItem.ToString();
            if (this.txbMinX.Text.Trim() == string.Empty || !double.TryParse(this.txbMinX.Text.Trim(), out minX))
            {
                MessageBox.Show("最小维度请输入有效值");
                return;
            }
            if (this.txbMaxX.Text.Trim() == string.Empty || !double.TryParse(this.txbMaxX.Text.Trim(), out maxX))
            {
                MessageBox.Show("最大维度请输入有效值");
                return;
            }
            if (this.txbminY.Text.Trim() == string.Empty || !double.TryParse(this.txbminY.Text.Trim(), out minY))
            {
                MessageBox.Show("最小经度请输入有效值");
                return;
            }

            if (this.txbMaxY.Text.Trim() == string.Empty || !double.TryParse(this.txbMaxY.Text.Trim(), out maxY))
            {
                MessageBox.Show("最大经度请输入有效值");
                return;
            }
            if (this.txbMinZoom.Text.Trim() == string.Empty || !int.TryParse(this.txbMinZoom.Text.Trim(), out minZoom))
            {
                MessageBox.Show("最小级别请输入有效值");
                return;
            }
            if (this.txbMaxZoom.Text.Trim() == string.Empty || !int.TryParse(this.txbMaxZoom.Text.Trim(), out maxZoom))
            {
                MessageBox.Show("最大级别请输入有效值");
                return;
            }
            string[] strOrgin = this.txbOrgin.Text.Trim().Split(',');
            if (strOrgin.Length != 2)
            {
                MessageBox.Show("请确保切片原点格式正确，格式如：300,300");
                return;
            }
            for (int i = 0; i < strOrgin.Length; i++)
            {
                double tempOragin = 0;
                if (!double.TryParse(strOrgin[i], out tempOragin))
                {
                    MessageBox.Show("分别率数组中存在非法字符");
                    return;
                }
                else
                {
                    orgions.Add(tempOragin);
                }
            }
            if (!Directory.Exists(this.txbPath.Text.Trim()))
            {
                MessageBox.Show("请输入有效存储路径");
                return;
            }
            string[] strResolutions= this.txbResolutions.Text.Trim().Split(',');
            //if (strResolutions.Length != maxZoom - minZoom + 1)
            //{
            //    MessageBox.Show("请输入有效分辨率数组");
            //    return;
            //}
            for (int i = 0; i < strResolutions.Length; i++)
            {
                double tempResolution = 0;
                if (!double.TryParse(strResolutions[i], out tempResolution))
                {
                    MessageBox.Show("分别率数组中存在非法字符");
                    return;
                }
                else
                {
                    resolutions.Add(tempResolution);
                }
            }
                new Thread(new ThreadStart(this.DoSomething)).Start();
        }
        int k = 0;
        int count =0;
        private void DoSomething()
        {
            double[] fullExent = new double[]{minX,minY,maxX,maxY};
            for (int i = minZoom; i < maxZoom + 1; i++)
            {
                RowColumns rc = MapTool.GeRowColomns(fullExent, resolutions.ToArray(), i);
                k = 0;
                count = 0;
                this.cutDataJson(i);
                
            }
        }

        /// <summary>
        /// 用Excel Com组件方式读取Excel内容到DataSet(兼容性较高)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataSet ToDataTableEx(string path)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();


            excel.Visible = false;
            excel.ScreenUpdating = false;
            excel.DisplayAlerts = false;

            excel.Workbooks.Add(path);

            DataSet ds = new DataSet();
            try
            {
                //遍历Worksheets中的每张表
                for (int i = 1; i <= excel.Worksheets.Count; i++)
                {
                    //获得指定表
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.Worksheets[i];

                    DataTable dt = new DataTable();

                    //取表明赋值到dt TableName
                    dt.TableName = worksheet.Name;

                    worksheet.Columns.EntireColumn.AutoFit();

                    int row = worksheet.UsedRange.Rows.Count;
                    int col = worksheet.UsedRange.Columns.Count;

                    for (int c = 1; c <= col; c++)
                    {
                        dt.Columns.Add(new DataColumn((String)((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, c]).Text));
                    }

                    for (int r = 2; r <= row; r++)
                    {
                        DataRow newRow = dt.NewRow();
                        for (int c = 1; c <= col; c++)
                        {
                            newRow[c - 1] = ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[r, c]).Text;
                        }
                        dt.Rows.Add(newRow);
                    }
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                excel.Quit();
                System.GC.Collect();
            }
            return ds;
        }

        private void cutDataJson(int zoom)
        {
            string sql = "SELECT to_char(x, '999.999999999') as X,to_char(y, '99.999999999') as Y  FROM " + this.tableName;
            DataSet datasetAllPoint = new DataSet();
            NpgsqlDataAdapter dAllPoint = new NpgsqlDataAdapter(sql, dbcon);
            dAllPoint.Fill(datasetAllPoint);
            DataColumn X = datasetAllPoint.Tables[0].Columns["X"];
            DataColumn Y = datasetAllPoint.Tables[0].Columns["Y"];
            double[] fullExent = new double[]{minX,minY,maxX,maxY};
            count = datasetAllPoint.Tables[0].Rows.Count;
            foreach (DataRow arow in datasetAllPoint.Tables[0].Rows)
            {
                k++;
                double ax = double.Parse(arow[X].ToString().Trim());
                double ay = double.Parse(arow[Y].ToString().Trim());
                GeoPoint point = new GeoPoint();
                point.x = ax;
                point.y = ay;
                double[] bounds = MapTool.GetBoundsByPoint(point,fullExent, this.resolutions[zoom]);
                string sqlString = "SELECT Name,to_char(x, '999.999999999') as X,to_char(y, '99.999999999') as Y FROM " + this.tableName + " WHERE (x>" + bounds[0].ToString()
                        + " AND x< " + bounds[2].ToString() + " AND y>" + bounds[1].ToString() + " AND y<" + bounds[3].ToString() + ")";
                //string sqlString = "SELECT * FROM [" + this.tableName + "] WHERE (X>" + bounds[0].ToString()
                //+ " AND X< " + bounds[2].ToString() + " AND Y>" + bounds[1].ToString() + " AND Y<" + bounds[3].ToString() + ")";
                DataSet dataset = new DataSet();
                lock (lockObj)
                {
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlString, dbcon);
                    da.Fill(dataset);
                    //OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sqlString, OleConn);
                    //OleDaExcel.Fill(dataset);
                }
                DataColumn cX = dataset.Tables[0].Columns["X"];
                DataColumn cY = dataset.Tables[0].Columns["Y"];
                DataColumn cName = dataset.Tables[0].Columns["NAME"];
                TileObj to = new TileObj();
                double res = resolutions[zoom];
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    PointObj po = new PointObj();
                    geoObj geo = new geoObj();
                    proObj pro = new proObj();
                    double x = double.Parse(row[cX].ToString().Trim());
                    double y = double.Parse(row[cY].ToString().Trim());
                    geo.coordinates = new double[] { x, y };
                    pro.Name = row[cName].ToString();
                    po.geometry = geo;
                    po.properties = pro;
                    if (zoom > 8)
                    {
                        to.features.Add(po);
                    }
                    else
                    {
                        if (to.features.Count == 0)
                            to.features.Add(po);
                        else
                        {
                            bool isCluster = false;
                            for (int index = 0; index < to.features.Count; index++)
                            {
                                PointObj p = to.features[index];
                                if (shouldCluster(p, po, res))
                                {
                                    isCluster = true;
                                    break;
                                }
                            }
                            if (!isCluster)
                            {
                                to.features.Add(po);
                            }
                        }
                    }
                }
                if (to.features.Count > 0)
                {
                    RowColumns trc = MapTool.GetTileRowColomns(bounds, orgions.ToArray(), resolutions[zoom]);
                    string id = zoom.ToString() + "_" + trc.Col.ToString() + "_" + trc.Row.ToString();
                    string tos = JsonHelper.JsonSerializer<TileObj>(to);
                    string path = this.txbPath.Text.Trim() + "\\" + zoom.ToString() + "\\" + trc.Col.ToString();
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var file = Path.Combine(path, id + ".json");
                    if (!File.Exists(file))
                    {
                        using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
                        {
                            var content = tos;
                            var buffer = System.Text.ASCIIEncoding.UTF8.GetBytes(content);
                            fileStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                string msg1 = "正在生产第" + zoom.ToString() + "级数据，生成第" + k.ToString() + "条,共" + count.ToString() + "条";
                if (this.OnProcessNotify != null)
                {
                    this.OnProcessNotify(msg1, (k * 100) / count);
                }
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        private bool shouldCluster(PointObj cluster, PointObj p, double resolution)
        {
            double[] cc = cluster.geometry.coordinates;
            double[] fc = p.geometry.coordinates;
            double distance = (
                Math.Sqrt(
                    Math.Pow((cc[0] - fc[0]), 2) + Math.Pow((cc[1] - fc[1]), 2)
                ) / resolution
            );
            return (distance <= 50);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {

        }

        private void btnFindFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Excel   Files(*.xls)|*.xls";
            if (of.ShowDialog() == DialogResult.OK)
            {
                string path = of.FileName;
                txbExcelFile .Text = path;
            }
        }
        /// <summary>
        /// 链接EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenExcel_Click(object sender, EventArgs e)
        {
            string filePath = this.txbExcelFile.Text.Trim();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleConn = new OleDbConnection(strConn); 
            OleConn.Open();
            DataTable tb = OleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            foreach (DataRow row in tb.Rows)
            {
                this.cmbTableName.Items.Add(row["TABLE_NAME"]);
            }
                if (this.cmbTableName.Items.Count > 0)
                    this.cmbTableName.SelectedIndex = 0;
        }

        private void btnCloseExcel_Click(object sender, EventArgs e)
        {
            OleConn.Close();
        }
    }
   [Serializable]
    public class TileObj
    {
       public string type = "FeatureCollection";
       public List<PointObj> features = new List<PointObj>();
    }
    [Serializable]
    public class PointObj
    {
        public string type = "Feature";
        public geoObj geometry = new geoObj();
        public proObj properties = new proObj();
    }
    public class geoObj
    {
        public string type = "Point";
        public double[] coordinates;
    }
    public class proObj
    {
        public string Name = "";
    }
    public class Paremt
    {
        public int processIndex = 0;
        public RowColumns rc = null;
    }
}
