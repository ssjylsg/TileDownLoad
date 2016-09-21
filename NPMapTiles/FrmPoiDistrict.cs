using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NPMapTiles
{
    using System.Threading;

    using log4net;

    using MapDataTools.Util;

    public partial class FrmPoiDistrict : Form
    {
        private ILog log;
        public FrmPoiDistrict()
        {
            InitializeComponent();
            log = log4net.LogManager.GetLogger(this.GetType());
            this.Closed += (sender, obj) =>
                {
                    if (this.dbcon != null)
                    {
                        this.dbcon.Dispose();
                    }
                };
        }
        private DbHelper dbcon = null;
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
            this.dbcon = new DbHelper(connString);
            BindCity("1", this.cmbProvice);
            LoadDataTable(this.cmbPoi);
        }

        private void LoadDataTable(ComboBox cmbTableName)
        {
            cmbTableName.Items.Clear();
            cmbTableName.DisplayMember = "Text";
            cmbTableName.ValueMember = "Value";

            var dr =
                this.dbcon.ExecuteReader(
                    "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='public' order by TABLE_NAME");
            while (dr.Read())
            {
               cmbTableName.Items.Add(new ComboboxItem(){Text = dr.GetString(0),Value = dr.GetString(0)});
            }
            dr.Close();
            if (cmbTableName.Items.Count > 0) cmbTableName.SelectedIndex = 0;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {

            var poiTable = this.cmbPoi.SelectedItem as ComboboxItem;

            this.dbcon.AddColumn("districtName", poiTable.Value.ToString());

            var city = this.cmbCity.SelectedItem as ComboboxItem;
            if (poiTable != null && city != null)
            {
                var districts = new Dictionary<string,string>(); // code name
                var read = GetCityReader(city.Value.ToString());
                while (read.Read())
                {
                    districts[read.GetInt32(2).ToString()] = read.GetString(1);
                }
                read.Close();
                var tableName = poiTable.Value;
                var sql = @" UPDATE {0} set districtName = '{1}' WHERE GID IN  (
                                            SELECT m.gid FROM {0} as m  , city as c
                                            where st_intersects(st_setsrid(m.geom,4326),st_setsrid(c.geom,4326))   and c.gid = {2}
                                            )  ";
                int totalCount = districts.Keys.Count;
                int currentCount = 0;
              var t =  new Thread(
                    () => districts.Keys.ToList().ForEach(
                        m =>
                            {
                                currentCount++;
                                this.BeginInvoke(
                                   new MethodInvoker(
                                       () =>
                                       {
                                           this.lblStatus.Text = districts[m];
                                       }));
                                this.ExecuteSql(string.Format(sql, tableName, districts[m],m));
                                if (currentCount == totalCount)
                                {
                                    this.BeginInvoke(
                                        new MethodInvoker(
                                            () =>
                                                {
                                                    this.lblStatus.Text = "行政区跟新完成！";
                                                }));
                                }
                               
                            }));
                t.IsBackground = true;
                t.Start();
            }
        }

        private void ExecuteSql(string sql)
        {
            log.Info(sql);
            this.dbcon.ExecuteNonQuery(sql);
        }

        private int GetRowCount(string tableName)
        { 
            var result = this.dbcon.ExecuteScalar("SELECT COUNT(1) FROM " + tableName);
            var count = int.Parse(result.ToString());
            return count;
        }

        private void cmbProvice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null)
            {
                return;
            }
            var hotCity = new string[] { "131", "332", "132", "289", };
            var selectItem = ((ComboboxItem)((ComboBox)sender).SelectedItem);
            var code = selectItem.Value.ToString();
            if (hotCity.Contains(code))
            {
                this.cmbCity.Items.Clear();
                this.cmbCity.Items.Add(selectItem);
                this.cmbCity.SelectedIndex = 0;
            }
            else
            {
                this.BindCity(code, this.cmbCity);
            }
        }

        private IDataReader GetCityReader(string code)
        {
            return this.dbcon.ExecuteReader(
                string.Format("SELECT area_code,area_name,gid FROM CITY WHERE parent_code = '{0}'", code));
        }
        private void BindCity(string code, ComboBox comboBox)
        {
            comboBox.Items.Clear();
            var read = GetCityReader(code);
            comboBox.DisplayMember = "Text";
            comboBox.ValueMember = "Value";
            while (read.Read())
            {
                comboBox.Items.Add(new ComboboxItem() { Text = read.GetString(1), Value = read.GetString(0) });
            }
            read.Close();
            comboBox.Items.Insert(0, new ComboboxItem() { Text = "请选择", Value = string.Empty });
            comboBox.SelectedIndex = 0;
        }
        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
