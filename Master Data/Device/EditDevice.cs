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
        MySqlConnection myConn;
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

            //menampilkan combobox workArea
            help.displayCmbList("SELECT * FROM tbl_masterworkarea ORDER BY id ", "name", "name", cmbWorkArea);

            //menampilkan combobox department
            help.displayCmbList("SELECT * FROM tbl_masterdepartment ORDER BY id ", "name", "name", cmbDepartment);

            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);
            try
            {                
                string query = "SELECT id, workarea, dept, ipAddress, PORT, indicator, isactive FROM tbl_masterdevice WHERE ipaddress = '"+tbipAddress.Text+"'";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
                {
                    DataTable dset = new DataTable();
                    adpt.Fill(dset);
                    if (dset.Rows.Count > 0)
                    {
                        string id = dset.Rows[0]["id"].ToString();
                        string workarea = dset.Rows[0]["workarea"].ToString();
                        string dept = dset.Rows[0]["dept"].ToString();
                        string ipAddress = dset.Rows[0]["ipAddress"].ToString();
                        string port = dset.Rows[0]["port"].ToString();
                        string indicator = dset.Rows[0]["indicator"].ToString();
                        string isactive = dset.Rows[0]["isactive"].ToString();

                        cmbWorkArea.SelectedIndex = cmbWorkArea.FindStringExact(workarea);
                        cmbDepartment.SelectedIndex = cmbDepartment.FindStringExact(dept);
                        tbipAddress.Text = ipAddress;
                        cmbInout.SelectedIndex = cmbInout.FindStringExact(indicator);                       

                    }
                }
            }
            catch (Exception ex)
            {
                myConn.Close();
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                myConn.Dispose();
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (cmbWorkArea.Text == "" || cmbDepartment.Text == "" || tbipAddress.Text == "" || tbPort.Text == "" || cmbInout.Text == "")
            {
                MessageBox.Show(this, "Unable Edit Device with let data blank", "Edit Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);
                    //string device = tbDeviceName.Text;
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
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, myConn))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to Edit Device, Device with selected IP already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //tbDeviceName.Clear();
                            tbipAddress.Clear();
                            //tbDeviceName.Focus();
                        }
                        else
                        {
                            //myConn.Open();
                            //string queryAdd = "INSERT INTO tbl_masterdevice (name, dept, ipAddress, isActive, createDate, createBy) VALUES " +
                            //    "('" + device + "', '" + department + "', '" + ipAddress + "', '" + isCheck + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            //string[] allQuery = { queryAdd };
                            //for (int j = 0; j < allQuery.Length; j++)
                            //{
                            //    cmd.CommandText = allQuery[j];
                            //    //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                            //    cmd.ExecuteNonQuery();
                            //    //Jalankan perintah / query dalam CommandText pada database
                            //}
                            //myConn.Close();
                            //MessageBox.Show(this, "Device Successfully Updated", "Edit Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //tbDeviceName.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    myConn.Close();
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    myConn.Dispose();
                }
            }
        }
    }
}
