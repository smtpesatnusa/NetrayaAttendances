using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class EmailTemplate : MaterialForm
    {
        Helper help = new Helper();
        MySqlConnection myConn;
        string idUser, dept;

        public EmailTemplate()
        {
            InitializeComponent();
        }

        private void EmailTemplate_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            string query = "SELECT sendto, cc, SUBJECT, message, sendTime FROM tbl_mastertemplateemail WHERE id = 1";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
            {
                DataTable dset = new DataTable();
                adpt.Fill(dset);
                if (dset.Rows.Count > 0)
                {
                    string sendto = dset.Rows[0]["sendto"].ToString();
                    string cc = dset.Rows[0]["cc"].ToString();
                    string subject = dset.Rows[0]["subject"].ToString();
                    string message = dset.Rows[0]["message"].ToString();
                    string sendTime = dset.Rows[0]["sendTime"].ToString();

                    tbTo.Text = sendto;
                    tbCc.Text = cc;
                    tbSubject.Text = subject;
                    tbMessage.Text = message;
                    dateTimePickerSendIn.Text = sendTime;
                }
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbTo.Text == "" || tbSubject.Text == "" || tbMessage.Text == "")
            {
                MessageBox.Show(this, "Unable  Save Email Template with let To, Subject or Message blank", "Report Email Template", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);

                    string to = tbTo.Text;
                    string cc = tbCc.Text;
                    string subject = tbSubject.Text;
                    string message = tbMessage.Text;
                    string time = dateTimePickerSendIn.Text;

                    myConn.Open();
                    string queryUpdate = "UPDATE tbl_mastertemplateemail SET sendto = '"+to+ "' , cc = '" + cc + "' , " +
                        "subject = '" + subject + "' , message = '" + message + "' , sendTime = '" + time + "' , createDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' , createBy = '" + idUser + "' WHERE id = 1";
                        
                    cmd.CommandText = queryUpdate;
                    cmd.ExecuteNonQuery();

                    myConn.Close();
                    MessageBox.Show(this, "Report Email Template Successfully Updated", "Update Report Email Template", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    myConn.Close();
                    //MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
