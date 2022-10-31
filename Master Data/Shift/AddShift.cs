using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class AddShift : MaterialForm
    {
        Helper help = new Helper();
        string idUser;
        string idshift;
        MySqlConnection myConn;
        public AddShift()
        {
            InitializeComponent();
        }

        private void AddShift_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = usernameLbl.Text.Split(' ');
            int idPosition = usernameLbl.Text.Split(' ').Length - 3;
            idUser = userId[idPosition].Replace(",", "");

            //menampilkan data combobox 
            help.displayCmbList("SELECT * FROM tbl_mastershift ORDER BY id ", "name", "name", cmbShift);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "" || cmbShift.Text == "" || dateTimePickerFrom.Text == "" || dateTimePickerTo.Text == "" )
            {
                MessageBox.Show(this, "Unable Add Shift with let any field blank", "Add Shift", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);
                    string name = tbName.Text;
                    string category = cmbShift.Text;
                    string from = dateTimePickerFrom.Text;
                    string to = dateTimePickerTo.Text;
                    string totalBrteak = tbTotalBreak.Text;

                    from = from.Remove(from.Length - 3);
                    to = to.Remove(to.Length - 3);

                    string cek = "SELECT * FROM tbl_mastershiftdetail WHERE name = '" + name + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, myConn))
                    {
                        DataTable dt = new DataTable();
                        adpt.Fill(dt);

                        // cek jika modelno tsb sudah di upload
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Shift, Shift already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbName.Clear();
                            tbName.Focus();
                        }
                        else
                        {
                            myConn.Open();
                            string queryAdd = "INSERT INTO tbl_mastershiftdetail (name, category, intime, outtime, totalBreak, createDate, createBy) VALUES " +
                                "('" + name + "', '" + category + "', '" + from + "', '" + to + "', '" + totalBrteak + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            cmd.CommandText = queryAdd;
                            cmd.ExecuteNonQuery();

                            myConn.Close();
                            MessageBox.Show(this, "Shift successfully added", "add shift", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    myConn.Close();
                    //MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    myConn.Dispose();
                }
            }
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            if (cmbShift.Text == "normal")
            {
                dateTimePickerTo.Value = dateTimePickerFrom.Value.AddHours(+9);
            }
            if (cmbShift.Text == "shift")
            {
                dateTimePickerTo.Value = dateTimePickerFrom.Value.AddHours(+8);
            }
        }

        private void tbTotalBreak_TextChanged(object sender, EventArgs e)
        {
            //if user type alphabet
            if (System.Text.RegularExpressions.Regex.IsMatch(tbTotalBreak.Text, "[^0-9]"))
            {
                tbTotalBreak.Text = tbTotalBreak.Text.Remove(tbTotalBreak.Text.Length - 1);
            }
        }
    }
}
