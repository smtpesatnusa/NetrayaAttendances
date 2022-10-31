using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class EditSchedule : MaterialForm
    {
        readonly Helper help = new Helper();
        
        string idUser, dept;
        string dateSchdule;

        MySqlConnection myConn;

        public EditSchedule()
        {
            InitializeComponent();
        }

        private void AddSchedule_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            // display shift combo box
            help.displayCmbList("SELECT  id, CONCAT(NAME,' (', CONCAT(LEFT(intime, 5),'-', LEFT(outtime, 5)),')') AS workShift FROM tbl_mastershiftdetail order by category, intime", "workShift", "id", cmbShift);

            // select cmb shift if not -
            if (tbSchedule.Text != "-")
            {
                cmbShift.SelectedIndex = cmbShift.FindString(tbSchedule.Text);
            }

            // convert date format
            string _Date = tbDateSchedule.Text;
            DateTime dt = Convert.ToDateTime(_Date);
            dateSchdule = dt.ToString("yyyy-MM-dd");
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            SelectedGroupEmployee selectedGroupEmployee = new SelectedGroupEmployee();
            selectedGroupEmployee.ShowDialog();
        }


        private void submitBtn_Click(object sender, EventArgs e)
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);
            try
            {
                if (tbBadgeId.Text == "" ||  cmbShift.Text == "")
                {
                    MessageBox.Show(this, "Unable Update Schedule with let any field blank", "Edit Schedule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var cmd = new MySqlCommand("", myConn);

                    string createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string createBy = idUser;
                    string shiftID = cmbShift.SelectedValue.ToString();
                    string badgeId = tbBadgeId.Text;

                    // menentukan jadwal schedule in dan out
                    var scheduleTime = cmbShift.Text.Split('(');
                    string scheduleIn = dateSchdule+" " +scheduleTime[1].Trim().Substring(0, 5);
                    string scheduleOut;
                    // jika jam masuk diatas jam 22 maka tanggal keluar adalah hri berikutnya
                    if (Convert.ToInt32(scheduleTime[1].Trim().Substring(0, 2)) < 22 )
                    {
                        scheduleOut = dateSchdule + " " + scheduleTime[1].Trim().Substring(6, 5);
                    }
                    else
                    {
                        scheduleOut = DateTime.Parse(dateSchdule).AddDays(1).ToString("yyyy-MM-dd") + " " + scheduleTime[1].Trim().Substring(6, 5);
                    }

                    //string cek = "SELECT * FROM tbl_masterdevice WHERE ipAddress = '" + ipAddress + "'";
                    //using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, connectionDB.connection))
                    //{
                    //    DataSet ds = new DataSet();
                    //    adpt.Fill(ds);

                    //    // cek jika modelno tsb sudah di upload
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        MessageBox.Show(this, "Unable to add Device, Device with selected IP already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        tbDeviceName.Clear();
                    //        tbipAddress.Clear();
                    //        tbDeviceName.Focus();
                    //    }
                    //    else
                    //    {
                    //        connectionDB.connection.Open();
                    //        string queryAdd = "INSERT INTO tbl_masterdevice (name, dept, ipAddress, isActive, createDate, createBy) VALUES " +
                    //            "('" + device + "', '" + department + "', '" + ipAddress + "', '" + isCheck + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                    //        string[] allQuery = { queryAdd };
                    //        for (int j = 0; j < allQuery.Length; j++)
                    //        {
                    //            cmd.CommandText = allQuery[j];
                    //            //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                    //            cmd.ExecuteNonQuery();
                    //            //Jalankan perintah / query dalam CommandText pada database
                    //        }
                    //        connectionDB.connection.Close();
                    //        MessageBox.Show(this, "Device Successfully Added", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        tbDeviceName.Focus();
                    //    }
                    //}


                    // select shift = - insert schedule
                    if (tbSchedule.Text == "-")
                    {
                        myConn.Open();
                        string queryUpdate = "INSERT INTO tbl_attendance (rfidNo, DATE, EmplID, DayType, ShiftId, ScheduleIn, ScheduleOut) VALUES " +
                            "('-', '" + dateSchdule + "', (SELECT id FROM tbl_employee WHERE badgeID = '" + badgeId + "'),'WorkDay', '" + shiftID + "', '" + scheduleIn + "', '" + scheduleOut + "')";

                        cmd.CommandText = queryUpdate;
                        //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                        cmd.ExecuteNonQuery();
                        //Jalankan perintah / query dalam CommandText pada database

                        myConn.Close();
                        MessageBox.Show(this, "Schedule Successfully Updated", "Edit Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        myConn.Open();
                        string queryUpdate = " UPDATE tbl_attendance SET shiftId = '" + shiftID + "', ScheduleIn = '" + scheduleIn + "', ScheduleOut = '" + scheduleOut + "', intime = Null, outtime = Null  " +
                            ", BreakOut = Null, BreakIn = Null, TotalBreak = 0, LateIn = 0, EarlyOut = 0 WHERE EmplId = (SELECT id FROM tbl_employee WHERE badgeID = '" + badgeId + "') AND DATE = '" + dateSchdule + "'";

                        cmd.CommandText = queryUpdate;
                        //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                        cmd.ExecuteNonQuery();
                        //Jalankan perintah / query dalam CommandText pada database

                        myConn.Close();
                        MessageBox.Show(this, "Schedule Successfully Updated", "Edit Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
