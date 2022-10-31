using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class AddPublicHoliday : MaterialForm
    {
        Helper help = new Helper();
        string idUser, dept;
        MySqlConnection myConn;
        public AddPublicHoliday()
        {
            InitializeComponent();
        }

        private void AddPublicHoliday_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = usernameLbl.Text.Split(' ');
            int idPosition = usernameLbl.Text.Split(' ').Length - 3;
            idUser = userId[idPosition].Replace(",", "");
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "")
            {
                MessageBox.Show(this, "Unable Add Public Holiday with let any field blank", "Add Public Holiday", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);
                    string name = tbName.Text;
                    string date = dateTimePickerDate.Text;

                    string cekdata = "SELECT * FROM tbl_masterholiday WHERE date = '" + date + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cekdata, myConn))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika tgl tsb sudah di insert
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add public holiday, Public Holiday already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbName.Clear();
                        }
                        else
                        {
                            myConn.Open();
                            string queryAdd = "INSERT INTO tbl_masterholiday (name, date, createDate, createBy) " +
                                "VALUES ('" + name + "', '" + date + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            myConn.Close();
                            MessageBox.Show(this, "Public Holiday Successfully Added", "Add Public Holiday", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    }
}
