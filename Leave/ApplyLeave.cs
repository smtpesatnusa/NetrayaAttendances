using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class ApplyLeave : MaterialForm
    {
        Helper help = new Helper();
        MySqlConnection myConn;
        string idUser, dept;

        public ApplyLeave()
        {
            InitializeComponent();
        }
        private void ApplyLeave_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            //display combobox data
            
            help.displayCmbList("SELECT NAME, id FROM tbl_masterleavetype ORDER BY NAME ", "name", "id", cmbLeaveType);

            //get employee  user based on dept user
            if (dept == "All")
            {
                help.displayCmbList("SELECT CONCAT(badgeID, ' | ', NAME) AS NAMES, badgeID FROM tbl_employee ORDER BY NAME", "names", "badgeID", cmbEmployee);
                help.displayCmbList("SELECT CONCAT(username, ' | ', NAME) AS NAMES, username FROM tbl_user ORDER BY NAME", "names", "username", cmbConfirm);
            }
            else
            {
                help.displayCmbList("SELECT CONCAT(badgeID, ' | ', NAME) AS NAMES, badgeID FROM tbl_employee WHERE dept = '"+dept+"' ORDER BY NAME", "names", "badgeID", cmbEmployee);
                help.displayCmbList("SELECT CONCAT(username, ' | ', NAME) AS NAMES, username FROM tbl_user WHERE dept = '"+dept+"' ORDER BY NAME", "names", "username", cmbConfirm);
            }

            tbDuration.Text = help.totalDay(dateTimePickerStart, dateTimePickerEnd) + " Day";
        }


        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (cmbEmployee.Text == "" || cmbLeaveType.Text == "" )
            {
                MessageBox.Show(this, "Unable Apply leave with let employee or leave type blank", "Apply Leave", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);

                    //get employee id
                    var employee = cmbEmployee.Text.Split('|');
                    string employeeId = employee[0].Replace(" ", "");

                    //get confirm by id
                    var confirm = cmbConfirm.Text.Split('|');
                    string confirmId = confirm[0].Replace(" ", "");

                    string type = cmbLeaveType.SelectedValue.ToString();
                    string start = dateTimePickerStart.Text;
                    string end = dateTimePickerEnd.Text;
                    string desc = tbDesc.Text;
                    string status = "1";
                    
                    string cek = "SELECT* FROM tbl_leave WHERE badgeID = '"+employeeId+ "' AND(startdate = '" + start + "' OR endDate = '" + end + "')";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, myConn))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to apply leave, leave already submit", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cmbEmployee.SelectedIndex = -1;
                            cmbLeaveType.SelectedIndex = -1;
                            tbDesc.Clear();
                        }
                        else
                        {
                            myConn.Open();
                            string queryAdd = "INSERT INTO tbl_leave (badgeID, leavetype, startdate, enddate, descr, confirmBy, status, createDate, createBy) " +
                                "VALUES ('" + employeeId + "', '" + type + "','" + start + "','" + end + "','" + desc + "','" + confirmId + "','" + status + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            myConn.Close();
                            MessageBox.Show(this, "Leave Successfully Submit", "Apply Leave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    myConn.Close();
                    //MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerEnd.Value.Date < dateTimePickerStart.Value.Date)
            {
                MessageBox.Show("Start Date is greater than End Date");
                dateTimePickerEnd.Value = dateTimePickerStart.Value.AddDays(+1);
            }

            tbDuration.Text = help.totalDay(dateTimePickerStart, dateTimePickerEnd) + " Day";
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerEnd.Value.Date < dateTimePickerStart.Value.Date)
            {
                MessageBox.Show("Start Date is greater than End Date");
                dateTimePickerEnd.Value = dateTimePickerStart.Value.AddDays(+1);
            }

            tbDuration.Text = help.totalDay(dateTimePickerStart, dateTimePickerEnd) + " Day";
        }
    }
}
