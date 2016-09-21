using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NPMapTiles
{
    using MapDataTools.Util;

    using NpgsqlTypes;

    public partial class FrmBussiness : Form
    {
        public FrmBussiness()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

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
            this.dbcon = new DbHelper(connString);
            BindCity("1", this.cmbProvice);
        }
        private DbHelper dbcon = null;
        private void btnOk_Click(object sender, EventArgs e)
        {
            var gid = (((ComboboxItem)((ComboBox)cmbBussiness).SelectedItem).Value);
            UpdateDes(gid);
        }

        private void UpdateDes(object gid)
        {
            this.dbcon.CreateParametersCommand(
                (command) =>
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            string.Format("update bussiness_area set description=@des where gid = @gid");
                        command.Parameters.Add("@des", NpgsqlDbType.Text).Value = this.Description;
                        command.Parameters.Add("@gid", NpgsqlDbType.Integer).Value = int.Parse(gid.ToString());
                        command.ExecuteNonQuery();
                        command.Dispose();
                    });
            MessageBox.Show("跟新成功!");
        }

        private void GetDes(string bussinessId)
        {
            if (string.IsNullOrEmpty(bussinessId))
            {
                this.Description = string.Empty;
                return;
            }
            var read =
                this.dbcon.ExecuteReader(
                    string.Format("select description from bussiness_area  where  gid = {0}", bussinessId));
            if (read.Read())
            {
                this.Description = (read.GetValue(0) ?? string.Empty).ToString();
            }
            read.Close();
        }
        private void BindCity(string code, ComboBox comboBox)
        {
            var read =
               this.dbcon.ExecuteReader(string.Format("SELECT area_code,area_name FROM CITY WHERE parent_code = '{0}'", code));
            comboBox.Items.Clear();
           
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
        private void BindBussiness(string code, ComboBox comboBox)
        {
           
            comboBox.Items.Clear();
            var read = this.dbcon.ExecuteReader(string.Format("select gid,area_name,description from bussiness_area  where city_code =  '{0}'", code));
            comboBox.DisplayMember = "Text";
            comboBox.ValueMember = "Value";
            while (read.Read())
            {
                comboBox.Items.Add(
                    new ComboboxItem() { Text = read.GetString(1), Value = read.GetValue(0), Tag = read.GetValue(2) });
            }
            read.Close();
            comboBox.Items.Insert(0, new ComboboxItem() { Text = "请选择", Value = string.Empty });
        }
        #region
        private void cmbProvice_SelectedValueChanged(object sender, EventArgs e)
        {
            Description = string.Empty;
            if (((ComboBox)sender).SelectedItem == null)
            {
                return;
            }
            var hotCity = new string[] { "131", "332", "132", "289", };
            var selectItem = ((ComboboxItem)((ComboBox)sender).SelectedItem);
            var code =  selectItem.Value.ToString();
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

        public string Description
        {
            get
            {
                return this.rtbDes.Text;
            }
            set
            {
                this.rtbDes.Text = value;
            }
        }
        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Description = string.Empty;
            this.BindCity(((ComboboxItem)((ComboBox)sender).SelectedItem).Value.ToString(),this.cmbDistrict);
        }

        private void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            Description = string.Empty;
            this.BindBussiness(((ComboboxItem)((ComboBox)sender).SelectedItem).Value.ToString(), this.cmbBussiness);
        }

        private void cmbBussiness_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Description = string.Empty;
            if (((ComboBox)sender).SelectedItem == null)
            {
                return;
            }
            GetDes(((ComboboxItem)((ComboBox)sender).SelectedItem).Value.ToString());
            //this.Description = (((ComboboxItem)((ComboBox)sender).SelectedItem).Tag ?? string.Empty).ToString();
        }

        #endregion
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public object Tag { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
