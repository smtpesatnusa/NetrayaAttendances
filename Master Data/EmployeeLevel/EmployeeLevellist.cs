using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class EmployeeLevellist : MaterialForm
    {
        readonly Helper help = new Helper();

        string idUser, dept;

        public EmployeeLevellist()
        {
            InitializeComponent();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");

                (dataGridViewEmployeeLevelList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'or description LIKE '%" + search + "%'");

                totalLbl.Text = dataGridViewEmployeeLevelList.Rows.Count.ToString();
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
            dataGridViewEmployeeLevelList.DataSource = null;
            dataGridViewEmployeeLevelList.Refresh();

            while (dataGridViewEmployeeLevelList.Columns.Count > 0)
            {
                dataGridViewEmployeeLevelList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewEmployeeLevelList.Update();
            dataGridViewEmployeeLevelList.Refresh();
        }

        private void LoadData()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT name,description from tbl_masteremployeelevel ORDER BY id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewEmployeeLevelList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewEmployeeLevelList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewEmployeeLevelList.Rows.Count.ToString();
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
            tbEmployeeLevel.Clear();
            tbDesc.Clear();
            tbEmployeeLevel.Focus();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbEmployeeLevel.Text == "" || tbDesc.Text == "")
            {
                MessageBox.Show(this, "Unable Add Employee Level with let Level or description blank", "Add Employee Level", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                try
                {
                    var cmd = new MySqlCommand("", connectionDB.connection);
                    string userlevel = tbEmployeeLevel.Text;
                    string desc = tbDesc.Text;

                    string cekmodel = "SELECT * FROM tbl_masteremployeelevel WHERE name = '" + userlevel + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cekmodel, connectionDB.connection))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add employee level, Employee level already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbEmployeeLevel.Clear();
                            tbDesc.Clear();
                            tbEmployeeLevel.Focus();
                        }
                        else
                        {
                            connectionDB.connection.Open();
                            string queryAddmodel = "INSERT INTO tbl_masteremployeelevel (name, description, createDate, createBy) VALUES " +
                                "('" + userlevel + "', '" + desc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAddmodel };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            connectionDB.connection.Close();
                            MessageBox.Show(this, "Employeee Level Successfully Added", "Add Employee Level", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tbEmployeeLevel.Clear();
                            tbDesc.Clear();
                            refresh();
                            tbEmployeeLevel.Focus();
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

        private void EmployeeLevellist_Load(object sender, EventArgs e)
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

        private void EmployeeLevellist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridViewEmployeeLevelList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Description" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewEmployeeLevelList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewEmployeeLevelList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void dataGridViewEmployeeLevelList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewEmployeeLevelList.SelectedCells[0].RowIndex;
            string levelslctd = dataGridViewEmployeeLevelList.Rows[i].Cells[0].Value.ToString();
            ConnectionDB connectionDB = new ConnectionDB();

            if (e.ColumnIndex == 2)
            {
                string message = "Do you want to delete this Employee Level " + levelslctd + "?";
                string title = "Delete Employee Level";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var cmd = new MySqlCommand("", connectionDB.connection);

                        string querydeletePO = "DELETE FROM tbl_masteremployeelevel WHERE name = '" + levelslctd + "'";
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
                        EmployeeLevellist employeeLevellist = new EmployeeLevellist();
                        employeeLevellist.toolStripUsername.Text = toolStripUsername.Text;
                        employeeLevellist.Show();
                        this.Hide();
                        MessageBox.Show("Record Deleted successfully", "Employee Level List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        connectionDB.connection.Close();
                        MessageBox.Show("Unable to remove selected employee level", "Employee Level List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connectionDB.connection.Dispose();
                    }
                }
            }
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewEmployeeLevelList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewEmployeeLevelList, e);
        }
    }
}
