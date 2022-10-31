using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class LeaveApproval : MaterialForm
    {
        readonly Helper help = new Helper();
        string idUser, dept;

        public LeaveApproval()
        {
            InitializeComponent();
        }

        private void LeavelistApproval_Load(object sender, EventArgs e)
        {
            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            LoadData();
            LoadDataApprove();
            LoadDataDecline();
        }

        private void LoadData()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT id, (SELECT b.name FROM tbl_employee b WHERE b.badgeID= a.badgeID) AS employee, " +
                    "(SELECT c.name FROM tbl_masterleavetype c WHERE c.id = a.leavetype) AS typeLeave," +
                    " DATE_FORMAT(startDate, '%a, %Y-%m-%e') AS startDate, DATE_FORMAT(endDate, '%a, %Y-%m-%e') AS endDate," +
                    " (SELECT d.name FROM tbl_masterstatus d WHERE d.id= a.status) AS status FROM tbl_leave a WHERE STATUS = '1' AND confirmBy = '" + idUser+"' ORDER BY id DESC";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        dataGridViewLeaveList.DataSource = dset.Tables[0];

                        // add button approve in datagridview table
                        DataGridViewButtonColumn btnApprove = new DataGridViewButtonColumn();
                        dataGridViewLeaveList.Columns.Add(btnApprove);
                        btnApprove.HeaderText = "";
                        btnApprove.Text = "Approve";
                        btnApprove.Name = "btnApprove";
                        btnApprove.UseColumnTextForButtonValue = true;

                        // add button decline in datagridview table
                        DataGridViewButtonColumn btnDecline = new DataGridViewButtonColumn();
                        dataGridViewLeaveList.Columns.Add(btnDecline);
                        btnDecline.HeaderText = "";
                        btnDecline.Text = "Decline";
                        btnDecline.Name = "btnDecline";
                        btnDecline.UseColumnTextForButtonValue = true;
                    }                        
                }
                totalLbl.Text = dataGridViewLeaveList.Rows.Count.ToString();
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

        private void LoadDataApprove()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT (SELECT b.name FROM tbl_employee b WHERE b.badgeID= a.badgeID) AS employee, " +
                    "(SELECT c.name FROM tbl_masterleavetype c WHERE c.id = a.leavetype) AS typeLeave," +
                    " DATE_FORMAT(startDate, '%a, %Y-%m-%e') AS startDate, DATE_FORMAT(endDate, '%a, %Y-%m-%e') AS endDate" +
                    " FROM tbl_leave a WHERE STATUS = '2' AND confirmBy = '" + idUser + "' ORDER BY id DESC";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewApprove.DataSource = dset.Tables[0];
                }

                totalApprove.Text = dataGridViewApprove.Rows.Count.ToString();
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

        private void LoadDataDecline()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT (SELECT b.name FROM tbl_employee b WHERE b.badgeID= a.badgeID) AS employee, " +
                    "(SELECT c.name FROM tbl_masterleavetype c WHERE c.id = a.leavetype) AS typeLeave," +
                    " DATE_FORMAT(startDate, '%a, %Y-%m-%e') AS startDate, DATE_FORMAT(endDate, '%a, %Y-%m-%e') AS endDate" +
                    " FROM tbl_leave a WHERE STATUS = '3' AND confirmBy = '" + idUser + "' ORDER BY id DESC";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewDecline.DataSource = dset.Tables[0];
                }

                totalDecline.Text = dataGridViewDecline.Rows.Count.ToString();
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

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");

                (dataGridViewLeaveList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("employee LIKE '%" + search + "%'or typeLeave LIKE '%" + search + "%'or startDate LIKE '%" + search + "%' " +
                    "or endDate LIKE '%" + search + "%'or STATUS LIKE '%" + search + "%'");

                // to display total data
                totalLbl.Text = dataGridViewLeaveList.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refresh()
        {
            // refresh request leave
            tbSearch.Clear();
            // refresh approve leave
            tbSearchApprove.Clear();
            // refresh decline leave
            tbSearchDecline.Clear();

            // reset datagridview
            DataGridView[] dgv = { dataGridViewLeaveList, dataGridViewApprove , dataGridViewDecline};
            for (int i = 0; i < dgv.Length; i++)
            {
                // remove data in datagridview result
                dgv[i].DataSource = null;
                dgv[i].Refresh();

                while (dgv[i].Columns.Count > 0)
                {
                    dgv[i].Columns.RemoveAt(0);
                }
                dgv[i].Update();
                dgv[i].Refresh();
            }            

            LoadData();
            LoadDataApprove();
            LoadDataDecline();
        }        

        private void dataGridViewScheduleList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewLeaveList, e);
        }

        private void dataGridViewLeaveList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewLeaveList.SelectedCells[0].RowIndex;
            string idLeave = dataGridViewLeaveList.Rows[i].Cells[0].Value.ToString();
            string employeeslctd = dataGridViewLeaveList.Rows[i].Cells[1].Value.ToString();
            ConnectionDB connectionDB = new ConnectionDB();

            if (e.ColumnIndex == 6)
            {
                string message = "Do you want to approve this leave request, employee with name " + employeeslctd + "?";
                string title = "Approve Leave Request";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var cmd = new MySqlCommand("", connectionDB.connection);

                        string queryApprove = "UPDATE tbl_leave SET STATUS = '2' WHERE id = '"+idLeave+"'";
                        connectionDB.connection.Open();

                        string[] allQuery = { queryApprove };
                        for (int j = 0; j < allQuery.Length; j++)
                        {
                            cmd.CommandText = allQuery[j];
                            //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                            cmd.ExecuteNonQuery();
                            //Jalankan perintah / query dalam CommandText pada database
                        }

                        connectionDB.connection.Close();
                        MessageBox.Show("Leave Approved successfully", "Leave Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        refresh();
                    }
                    catch (Exception ex)
                    {
                        connectionDB.connection.Close();
                        MessageBox.Show("Unable to approve selected leave request", "Leave Approval", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connectionDB.connection.Dispose();
                    }
                }
            }
            if (e.ColumnIndex == 7)
            {
                string message = "Do you want to decline this leave request, employee with name " + employeeslctd + "?";
                string title = "Decline Leave Request";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var cmd = new MySqlCommand("", connectionDB.connection);

                        string queryDecline = "UPDATE tbl_leave SET STATUS = '3' WHERE id = '" + idLeave + "'";
                        connectionDB.connection.Open();

                        string[] allQuery = { queryDecline };
                        for (int j = 0; j < allQuery.Length; j++)
                        {
                            cmd.CommandText = allQuery[j];
                            //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                            cmd.ExecuteNonQuery();
                            //Jalankan perintah / query dalam CommandText pada database
                        }

                        connectionDB.connection.Close();
                        MessageBox.Show("Leave Declined successfully", "Leave Decline", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        refresh();
                    }
                    catch (Exception ex)
                    {
                        connectionDB.connection.Close();
                        MessageBox.Show("Unable to decline selected leave request", "Leave Decline", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connectionDB.connection.Dispose();
                    }
                }
            }
        }

        private void dataGridViewLeaveList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewLeaveList, e);
        }

        private void dataGridViewApprove_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewApprove, e);
        }

        private void tbSearchApprove_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearchApprove.Text.Replace("'", "''");

                (dataGridViewApprove.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("employee LIKE '%" + search + "%'or typeLeave LIKE '%" + search + "%'or startDate LIKE '%" + search + "%' " +
                    "or endDate LIKE '%" + search + "%'");

                // to display total data
                totalApprove.Text = dataGridViewApprove.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbSearchDecline_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearchDecline.Text.Replace("'", "''");

                (dataGridViewDecline.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("employee LIKE '%" + search + "%'or typeLeave LIKE '%" + search + "%'or startDate LIKE '%" + search + "%' " +
                    "or endDate LIKE '%" + search + "%'");

                // to display total data
                totalDecline.Text = dataGridViewDecline.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewLeaveList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridViewLeaveList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewLeaveList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
    }
}
