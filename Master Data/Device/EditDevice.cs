using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class EditDevice : MaterialForm
    {
        readonly Helper help = new Helper();

        string idUser, dept;

        public EditDevice()
        {
            InitializeComponent();
        }

        private void Devicelist_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = usernameLbl.Text.Split(' ');
            int idPosition = usernameLbl.Text.Split(' ').Length - 3;
            idUser = userId[idPosition].Replace(",", "");

            //menampilkan combobox department
            help.displayCmbList("SELECT * FROM tbl_masterdepartment ORDER BY id ", "name", "name", cmbDepartment);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbDeviceName.Text == "" || cmbDepartment.Text == "")
            {
                MessageBox.Show(this, "Unable Add Device with let data blank", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                try
                {
                    var cmd = new MySqlCommand("", connectionDB.connection);
                    string device = tbDeviceName.Text;
                    string department = cmbDepartment.Text;
                    string ipAddress = tbipAddress.Text;
                    string isCheck;

                    //get checkbox value
                    if (isActiveCheckBox.CheckState == CheckState.Checked)
                    {
                        isCheck = "1";
                    }
                    else
                    {
                        isCheck = "0";
                    }

                    string cek = "SELECT * FROM tbl_masterdevice WHERE ipAddress = '" + ipAddress + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, connectionDB.connection))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Device, Device with selected IP already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbDeviceName.Clear();
                            tbipAddress.Clear();
                            tbDeviceName.Focus();
                        }
                        else
                        {
                            connectionDB.connection.Open();
                            string queryAdd = "INSERT INTO tbl_masterdevice (name, dept, ipAddress, isActive, createDate, createBy) VALUES " +
                                "('" + device + "', '" + department + "', '" + ipAddress + "', '" + isCheck + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            connectionDB.connection.Close();
                            MessageBox.Show(this, "Device Successfully Added", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tbDeviceName.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    connectionDB.connection.Close();
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    connectionDB.connection.Dispose();
                }
            }
        }
    }
}
