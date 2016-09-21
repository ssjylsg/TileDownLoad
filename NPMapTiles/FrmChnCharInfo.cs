namespace NPMapTiles
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    using log4net;

    using MapDataTools.Util;

    public partial class FrmChnCharInfo : Form
    {
        private DbHelper dbcon = null;

        private delegate void ProcessNotifyHandler(string msg1, int process);

        private ProcessNotifyHandler OnProcessNotify;

        private ILog log;

        public FrmChnCharInfo()
        {
            this.InitializeComponent();
            this.OnProcessNotify += new ProcessNotifyHandler(this.ShowMsg);
            log = LogManager.GetLogger(this.GetType());
            this.Closed += (sender, obj) =>
            {
                if (this.dbcon != null )
                {
                    this.dbcon.Dispose();
                }
            };
            this.txbDataBase.Text = "routing";
        }

        private void ShowMsg(string msg1, int process)
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
            string connString = "Server = " + this.txbServer.Text.Trim() + ";Port=5432;user id = "
                                + this.txbUser.Text.Trim() + ";password = " + this.txbPassWord.Text.Trim()
                                + ";Database = " + this.txbDataBase.Text.Trim() + ";";

            try
            {
                this.dbcon = new DbHelper(connString);
                this.cmbTableName.Items.Clear();
                var dr =
                    dbcon.ExecuteReader(
                        "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public' order by TABLE_NAME");
                while (dr.Read())
                {
                    this.cmbTableName.Items.Add(dr.GetString(0));
                }
                dr.Close();
                if (this.cmbTableName.Items.Count > 0) this.cmbTableName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开数据库失败；" + ex.Message);
            }
        }

        private void btnCloseTable_Click(object sender, EventArgs e)
        {
            
            this.cmbTableName.Items.Clear();
        }

        private void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbHanZi.Items.Clear();
            string tableName = this.cmbTableName.SelectedItem.ToString();
            string sqlString =
                " SELECT column_name FROM information_schema.columns WHERE table_schema='public' AND table_name='"
                + tableName + "' order by column_name";

            var read = this.dbcon.ExecuteReader(sqlString);
            while (read.Read())
            {
                string field = read.GetString(0);
                this.cmbHanZi.Items.Add(field);
            }
            read.Close();
            if (this.cmbHanZi.Items.Count > 0) this.cmbHanZi.SelectedIndex = 0;
        }

        private void btnCreatPinYin_Click(object sender, EventArgs e)
        {
            try
            {
                string quanpin = this.addColumn("quanpin");
                string shouZim = this.addColumn("szm");
                string hanzi = this.cmbHanZi.SelectedItem.ToString();
                NameObject nobject = new NameObject();
                nobject.hanzi = hanzi;
                nobject.shouZim = shouZim;
                nobject.quanpin = quanpin;
                nobject.tableName = this.cmbTableName.SelectedItem.ToString();
                ParameterizedThreadStart pt = new ParameterizedThreadStart(this.DoSomthing);
                Thread t = new Thread(pt);
                t.Start(nobject);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string addColumn(string columnName)
        {

            string tableName = this.cmbTableName.SelectedItem.ToString();
            this.dbcon.AddColumn(columnName, tableName);
            return columnName;
        }

        private void DoSomthing(object nobject)
        {
            try
            {
                NameObject nameObject = nobject as NameObject;
                string quanpin = nameObject.quanpin;
                string shouZim = nameObject.shouZim;
                string hanzi = nameObject.hanzi;
                string tableName = nameObject.tableName;
                string msg2 = "开始获取所有数据信息";
                if (this.OnProcessNotify != null)
                {
                    this.OnProcessNotify(msg2, 0);
                }
                string sqlString = "SELECT gid," + hanzi + "," + quanpin + "," + shouZim + " FROM " + tableName
                                   + " where " + hanzi + " is not null and (szm is  null or szm = '')";

                if (tableName.Contains("roadnet") || tableName.Contains("road"))
                {

                    sqlString = string.Format(
                        "select  distinct name from {0}   where name is not null and name != ''",
                        tableName);
                }
                msg2 = "完成获取所有数据信息";
                if (this.OnProcessNotify != null)
                {
                    this.OnProcessNotify(msg2, 0);
                }
                int count =
                    int.Parse(
                        this.dbcon.ExecuteScalar(string.Format("Select Count(1) From ({0}) as t", sqlString)).ToString());
                var read = this.dbcon.ExecuteReader(sqlString);
                int j = 0;
                PinyinHelper helper;
                while (read.Read())
                { 
                    j++;
                    string sql = "";
                    try
                    {
                        if (tableName.Contains("roadnet") || tableName.Contains("road"))
                        {
                            var name = read.GetString(0);
                            string hanziValue = name.Trim().Replace('\'', ' ').Replace('（', '(').Replace('）', ')');
                            helper = TextToPinyin.Convert(hanziValue);
                            //sql = "update " + tableName + " set " + quanpin + "='" + helper.Pinyin + "',"
                            //      + shouZim + "='" + helper.Szm + "' where name ='" + read.GetInt32(0) + "'";

                            sql = string.Format(
                                "update {0} set quanpin='{1}',szm='{2}' where name ='{3}'",
                                tableName,
                                helper.Pinyin,
                                helper.Szm, name);

                            this.dbcon.ExecuteNonQuery(sql);
                        }
                        else
                        {
                            string hanziValue = read.GetString(1).Trim().Replace('\'', ' ').Replace('（', '(').Replace('）', ')');
                            helper = TextToPinyin.Convert(hanziValue);
                            sql = "update " + tableName + " set " + quanpin + "='" + helper.Pinyin + "',"
                                  + shouZim + "='" + helper.Szm + "' where gid=" + read.GetInt32(0);

                            this.dbcon.ExecuteNonQuery(sql);
                        }
                    }
                    catch(Exception e)
                    {
                        log.Error(e);
                    }
                    string msg = "已处理路网数据" + j.ToString() + "条,共" + count.ToString() + "条";
                    if (this.OnProcessNotify != null)
                    {
                        this.OnProcessNotify(msg, (j * 100) / count);
                    }
                }
                read.Close();
                string msg1 = "处理完成";
                if (this.OnProcessNotify != null)
                {
                    this.OnProcessNotify(msg1, 100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class NameObject
    {
        public string quanpin;
        public string shouZim;
        public string hanzi;
        public string tableName;
    }
     
}
