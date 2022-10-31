using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class AddShifts : MaterialForm
    {
        Helper help = new Helper();
        string idUser;
        string idshift;

        MySqlConnection myConn;

        public AddShifts()
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
            if (tbName.Text == "" || cmbShift.Text == "" || dateTimePickerFrom.Text == "" || dateTimePickerTo.Text == "" ||
                dateTimePickerMealIn.Text == "" || dateTimePickerMealOut.Text == "")
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
                    string cb1in = dateTimePickercf1In.Text;
                    string cb1out = dateTimePickercf1Out.Text;
                    string cb2in = dateTimePickercf2In.Text;
                    string cb2out = dateTimePickercf2Out.Text;
                    string mealin = dateTimePickerMealIn.Text;
                    string mealout = dateTimePickerMealOut.Text;

                    from = from.Remove(from.Length - 3);
                    to = to.Remove(to.Length - 3);
                    mealin = mealin.Remove(mealin.Length - 3);
                    mealout = mealout.Remove(mealout.Length - 3);

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
                            string queryAdd = "INSERT INTO tbl_mastershiftdetail (name, category, intime, outtime, createDate, createBy) VALUES " +
                                "('" + name + "', '" + category + "', '" + from + "', '" + to + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }

                            
                            //get id new shift
                            DataTable dt2 = new DataTable();
                            adpt.Fill(dt2);

                            if (dt2.Rows.Count > 0)
                            {
                                //get id that already insert
                                idshift = dt2.Rows[0]["id"].ToString();                               
                            }

                            // insert lunch
                            string querymeal = "insert into tbl_mastershiftbreak (shiftId, timein, timeout, descr, createdate, createby) values " +
                                "('" + idshift + "', '" + mealin + "', '" + mealout + "', 'meal','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            cmd.CommandText = querymeal;
                            cmd.ExecuteNonQuery();

                            // insert coffe break 1
                            if (checkBoxCF1.CheckState == CheckState.Checked)
                            {
                                cb1in = cb1in.Remove(cb1in.Length - 3);
                                cb1out = cb1out.Remove(cb1out.Length - 3);

                                string querycf1 = "insert into tbl_mastershiftbreak (shiftId, timein, timeout, descr, createdate, createby) values " +
                                "('" + idshift + "', '" + cb1in + "', '" + cb1out + "', 'coffe break 1','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                                cmd.CommandText = querycf1;
                                cmd.ExecuteNonQuery();
                            }

                            // insert coffe break 1
                            if (checkBoxCF2.CheckState == CheckState.Checked)
                            {
                                cb2in = cb2in.Remove(cb2in.Length - 3);
                                cb2out = cb2out.Remove(cb2out.Length - 3);

                                string querycf2 = "insert into tbl_mastershiftbreak (shiftId, timein, timeout, descr, createdate, createby) values " +
                                "('" + idshift + "', '" + cb2in + "', '" + cb2out + "', 'coffe break 2','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                                cmd.CommandText = querycf2;
                                cmd.ExecuteNonQuery();
                            }

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

        private void checkBoxCF1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCF1.CheckState == CheckState.Checked)
            {
                dateTimePickercf1In.Visible = true;
                dateTimePickercf1Out.Visible = true;
                label4.Visible = true;
            }
            else
            {
                dateTimePickercf1In.Visible = false;
                dateTimePickercf1Out.Visible = false;
                label4.Visible = false;
            }
        }

        private void checkBoxCF2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCF2.CheckState == CheckState.Checked)
            {
                dateTimePickercf2In.Visible = true;
                dateTimePickercf2Out.Visible = true;
                label5.Visible = true;
            }
            else
            {
                dateTimePickercf2In.Visible = false;
                dateTimePickercf2Out.Visible = false;
                label5.Visible = false;
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

        private void dateTimePickerLunchIn_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerMealIn.Value > dateTimePickerTo.Value)
            {
                MessageBox.Show(this, "Unable to fill meal time out than work hour", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePickerMealIn.Value = dateTimePickerTo.Value.AddHours(-1);
            }

            if (dateTimePickerMealIn.Value < dateTimePickerFrom.Value)
            {
                MessageBox.Show(this, "Unable to fill meal time out than work hour", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePickerMealIn.Value = dateTimePickerFrom.Value.AddHours(1); 
            }
            dateTimePickerMealOut.Value = dateTimePickerMealIn.Value.AddMinutes(+45);
        }
        private void dateTimePickercf1In_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickercf1In.Value > dateTimePickerTo.Value)
            {
                MessageBox.Show(this, "Unable to fill coffe break time out than work hour", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePickercf1In.Value = dateTimePickerTo.Value.AddHours(-1);
            }

            if (dateTimePickercf1In.Value < dateTimePickerFrom.Value)
            {
                MessageBox.Show(this, "Unable to fill coffe break time out than work hour", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePickercf1In.Value = dateTimePickerFrom.Value.AddHours(1);
            }

            dateTimePickercf1Out.Value = dateTimePickercf1In.Value.AddMinutes(+10);
        }
        private void dateTimePickercf2In_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickercf2In.Value > dateTimePickerTo.Value)
            {
                MessageBox.Show(this, "Unable to fill coffe break time out than work hour", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePickercf2In.Value = dateTimePickerTo.Value.AddHours(-1);
            }

            if (dateTimePickercf2In.Value < dateTimePickerFrom.Value)
            {
                MessageBox.Show(this, "Unable to fill coffe break time out than work hour", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePickercf2In.Value = dateTimePickerFrom.Value.AddHours(1);
            }
            dateTimePickercf2Out.Value = dateTimePickercf2In.Value.AddMinutes(+10);
        }
    }
}
