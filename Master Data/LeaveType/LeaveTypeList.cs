using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class LeaveTypeList : MaterialForm
    {
        readonly Helper help = new Helper();

        string idUser, dept;

        public LeaveTypeList()
        {
            InitializeComponent();
        }

        private void LeaveTypeList_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;

            //icon
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            LoadData();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");

                (dataGridViewLeaveTypeList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'or description LIKE '%" + search + "%'");

                // to display total data
                totalLbl.Text = dataGridViewLeaveTypeList.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            help.dateTimeNow(dateTimeNow);
        }
        private void refreshLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            tbSearch.Clear();

            // remove data in datagridview result
            dataGridViewLeaveTypeList.DataSource = null;
            dataGridViewLeaveTypeList.Refresh();

            while (dataGridViewLeaveTypeList.Columns.Count > 0)
            {
                dataGridViewLeaveTypeList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewLeaveTypeList.Update();
            dataGridViewLeaveTypeList.Refresh();
        }

        private void LoadData()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT name,description from tbl_masterleavetype ORDER BY id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewLeaveTypeList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewLeaveTypeList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewLeaveTypeList.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                connectionDB.connection.Close();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connectionDB.connection.Dispose();
            }
        }


        private void clearBtn_Click(object sender, EventArgs e)
        {
            tbLeaveType.Clear();
            tbDesc.Clear();
            tbLeaveType.Focus();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbLeaveType.Text == "" || tbDesc.Text == "")
            {
                MessageBox.Show(this, "Unable Add Leave Type with let name or description blank", "Add Leave Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                try
                {
                    var cmd = new MySqlCommand("", connectionDB.connection);
                    string lType = tbLeaveType.Text;
                    string desc = tbDesc.Text;

                    string cek = "SELECT * FROM tbl_masterleavetype WHERE name = '" + lType + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, connectionDB.connection))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Leave Type, Leave Type already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbLeaveType.Clear();
                            tbDesc.Clear();
                            tbLeaveType.Focus();
                        }
                        else
                        {
                            connectionDB.connection.Open();
                            string queryAdd = "INSERT INTO tbl_masterleavetype (name, description, createDate, createBy) VALUES " +
                                "('" + lType + "', '" + desc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            connectionDB.connection.Close();
                            MessageBox.Show(this, "Leave Type Successfully Added", "Add Leave Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tbLeaveType.Clear();
                            tbDesc.Clear();
                            refresh();
                            tbLeaveType.Focus();
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

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu mm = new MainMenu();
            mm.toolStripUsername.Text = toolStripUsername.Text;
            mm.userdetail.Text = userdetail.Text;
            mm.Show();
            this.Hide();
        }

        private void LeaveTypeList_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = "Are you sure you want to close this application?";
            string title = "Confirm Close";
            MaterialDialog materialDialog = new MaterialDialog(this, title, message, "OK", true, "Cancel");
            DialogResult result = materialDialog.ShowDialog(this);
            if (result.ToString() == "OK")
            {
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
                MaterialSnackBar SnackBarMessage = new MaterialSnackBar(result.ToString(), 750);
                SnackBarMessage.Show(this);
            }
        }

        private void dataGridViewLeaveTypeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewLeaveTypeList.SelectedCells[0].RowIndex;
            string depslctd = dataGridViewLeaveTypeList.Rows[i].Cells[0].Value.ToString();

            if (e.ColumnIndex == 2)
            {
                string message = "Do you want to delete this Leave Type " + depslctd + "?";
                string title = "Delete Leave Type";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    ConnectionDB connectionDB = new ConnectionDB();
                    try
                    {
                        var cmd = new MySqlCommand("", connectionDB.connection);

                        string querydeletePO = "DELETE FROM tbl_masterleavetype WHERE name = '" + depslctd + "'";
                        connectionDB.connection.Open();

                        string[] allQuery = { querydeletePO };
                        for (int j = 0; j < allQuery.Length; j++)
                        {
                            cmd.CommandText = allQuery[j];
                            //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                            cmd.ExecuteNonQuery();
                            //Jalankan perintah / query dalam CommandText pada database
                        }

                        connectionDB.connection.Close();
                        LeaveTypeList leaveTypeList = new LeaveTypeList();
                        leaveTypeList.toolStripUsername.Text = toolStripUsername.Text;
                        leaveTypeList.Show();
                        this.Hide();
                        MessageBox.Show("Record Deleted successfully", "Leave Type List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        connectionDB.connection.Close();
                        MessageBox.Show("Unable to remove selected Leave Type", "Leave Type List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connectionDB.connection.Dispose();
                    }
                }
            }
        }

        private void dataGridViewLeaveTypeList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Description" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewLeaveTypeList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewLeaveTypeList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewLeaveTypeList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewLeaveTypeList, e);
        }
    }
}
