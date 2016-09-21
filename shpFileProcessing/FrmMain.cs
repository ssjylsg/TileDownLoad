using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Data.OleDb;
using System.IO;
using System.Threading;
namespace ShpFileProcessing
{
    public partial class FrmMain : Form
    {
        NpgsqlConnection dbcon = null;
        private delegate void ProcessNotifyHandler(string msg1, int process);
        private ProcessNotifyHandler OnProcessNotify;
        public FrmMain()
        {
            InitializeComponent();
            this.OnProcessNotify += new ProcessNotifyHandler(this.ShowMsg);
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
            string connString = "Server = " + txbServer.Text.Trim() + ";Port=5432;user id = " + txbUser.Text.Trim() + ";password = " + txbPassWord.Text.Trim() + ";Database = " + txbDataBase.Text.Trim() + ";";
            dbcon = new NpgsqlConnection(connString);
            try
            {
                dbcon.Open();
                this.cmbTableName.Items.Clear();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btnCloseTable_Click(object sender, EventArgs e)
        {
            this.dbcon.Close();
            this.cmbTableName.Items.Clear();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {

        }

        private void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbHanZi.Items.Clear();
            string tableName = this.cmbTableName.SelectedItem.ToString();
            string sqlString = " SELECT column_name FROM information_schema.columns WHERE table_schema='public' AND table_name='" + tableName + "'";
            NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlString, dbcon);
            NpgsqlDataReader MyReader = sqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string field = MyReader.GetString(0);
                this.cmbHanZi.Items.Add(field);
            }
            if (this.cmbHanZi.Items.Count > 0)
                this.cmbHanZi.SelectedIndex = 0;
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
                ParameterizedThreadStart pt = new ParameterizedThreadStart(DoSomthing);
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
            bool loop = true;
            string tableName = this.cmbTableName.SelectedItem.ToString();
            string quanPinName = columnName;
            int count = 0;
            string sqlString = " SELECT column_name FROM information_schema.columns WHERE table_schema='public' AND table_name='" + tableName + "'";
            NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlString, dbcon);
            NpgsqlDataReader MyReader = sqlCommand.ExecuteReader();
            List<string> fieldList = new List<string>();
            while (MyReader.Read())
            {
                string field = MyReader.GetString(0);
                fieldList.Add(field);
            }
            while (loop)
            {
                count = 0;
                foreach (string item in fieldList)
                {
                    if (item == quanPinName)
                    {
                        quanPinName = quanPinName + "_1";
                        break;
                    }
                    count++;
                }
                if (count == fieldList.Count)
                    loop = false;
            }
            string sqlString1 = "alter table " + tableName + " add " + quanPinName + " character varying(254)";
            NpgsqlCommand objCommand = new NpgsqlCommand(sqlString1, dbcon);
            objCommand.ExecuteNonQuery();
            return quanPinName;
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
                string sqlString = "SELECT gid," + hanzi + "," + quanpin + "," + shouZim + " FROM " + tableName+" where "+hanzi+" is not null";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlString, dbcon);
                da.UpdateCommand = new NpgsqlCommand();
                da.UpdateCommand.Parameters.Add(new NpgsqlParameter("@" + quanpin, DbType.String, 254, quanpin));
                da.UpdateCommand.Parameters.Add(new NpgsqlParameter("@" + shouZim, DbType.String, 254, shouZim));
                NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(da);
                DataSet dataset = new DataSet();
                da.Fill(dataset);
                msg2 = "完成获取所有数据信息";
                if (this.OnProcessNotify != null)
                {
                    this.OnProcessNotify(msg2, 0);
                }
                DataColumn hanziCol = dataset.Tables[0].Columns[hanzi];
                DataColumn quanpinCol = dataset.Tables[0].Columns[quanpin];
                DataColumn shouZimCol = dataset.Tables[0].Columns[shouZim];
                int j = 0;
                int count = dataset.Tables[0].Rows.Count;
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    j++;
                    string sql = "";
                    try
                    {
                        string hanziValue = row[hanziCol].ToString().Trim();
                        row[quanpinCol] = Hz2Py.GetPinyin(hanziValue);
                        row[shouZimCol] = Hz2Py.GetFirstPinyin(hanziValue);
                        sql = "update " + tableName + " set " + quanpin + "='" + row[quanpinCol].ToString() + "'," + shouZim + "='" + row[shouZimCol] + "' where gid=" + row[0].ToString();
                        NpgsqlCommand objCommand = new NpgsqlCommand(sql, dbcon);
                        objCommand.ExecuteNonQuery();
                    }
                    catch 
                    {
                        LogManager.writeLog("异常：<" + sql + ">语法错误");
                    }
                    string msg = "已处理路网数据" + j.ToString() + "条,共" + count.ToString() + "条";
                    if (this.OnProcessNotify != null)
                    {
                        this.OnProcessNotify(msg, (j * 100) / count);
                    }
                }
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
    public class LogManager
    {
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="str"></param>
        public static void writeLog(string str)
        {
            string ErrPath = AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(ErrPath))
            {
                Directory.CreateDirectory(ErrPath);
            }
            using (StreamWriter sw = new StreamWriter(System.Windows.Forms.Application.StartupPath + @"\ErrLog.txt", true))
            {
                sw.WriteLine(str);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }
    }
}
