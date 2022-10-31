using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class Departmentlist : MaterialForm
    {
        readonly Helper help = new Helper();
        string idUser, dept;
        MySqlConnection myConn;

        public Departmentlist()
        {
            InitializeComponent();
        }

        private void Departmentlist_Load(object sender, EventArgs e)
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

                (dataGridViewDepartmentList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'or description LIKE '%" + search + "%'");

                // to display total data
                totalLbl.Text = dataGridViewDepartmentList.Rows.Count.ToString();
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

        private void refresh()
        {
            tbSearch.Clear();

            // remove data in datagridview result
            dataGridViewDepartmentList.DataSource = null;
            dataGridViewDepartmentList.Refresh();

            while (dataGridViewDepartmentList.Columns.Count > 0)
            {
                dataGridViewDepartmentList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewDepartmentList.Update();
            dataGridViewDepartmentList.Refresh();
        }

        private void LoadData()
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);
            try
            {
                string query = "SELECT name,description from tbl_masterdepartment ORDER BY id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewDepartmentList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewDepartmentList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewDepartmentList.Rows.Count.ToString();
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


        private void clearBtn_Click(object sender, EventArgs e)
        {
            tbDepartment.Clear();
            tbDesc.Clear();
            tbDepartment.Focus();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbDepartment.Text == "" || tbDesc.Text == "")
            {
                MessageBox.Show(this, "Unable Add Department with let name or description blank", "Add Department", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);
                    string department = tbDepartment.Text;
                    string desc = tbDesc.Text;

                    string cekdept = "SELECT * FROM tbl_masterdepartment WHERE name = '" + department + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cekdept, myConn))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Department, Department already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbDepartment.Clear();
                            tbDesc.Clear();
                            tbDepartment.Focus();
                        }
                        else
                        {
                            myConn.Open();
                            string queryAdddepartment = "INSERT INTO tbl_masterdepartment (name, description, createDate, createBy) VALUES " +
                                "('" + department + "', '" + desc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdddepartment };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            myConn.Close();
                            MessageBox.Show(this, "Department Successfully Added", "Add Department", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tbDepartment.Clear();
                            tbDesc.Clear();
                            refresh();
                            tbDepartment.Focus();
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


        private void UserLevellist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu mm = new MainMenu();
            mm.toolStripUsername.Text = toolStripUsername.Text;
            mm.userdetail.Text = userdetail.Text;
            mm.Show();
            this.Hide();
        }

        private void dataGridViewDepartmentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewDepartmentList.SelectedCells[0].RowIndex;
            string depslctd = dataGridViewDepartmentList.Rows[i].Cells[0].Value.ToString();

            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            try
            {
                if (e.ColumnIndex == 2)
                {
                    string message = "Do you want to delete this Department " + depslctd + "?";
                    string title = "Delete Department";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    MessageBoxIcon icon = MessageBoxIcon.Information;
                    DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            var cmd = new MySqlCommand("", myConn);

                            string querydeletePO = "DELETE FROM tbl_masterdepartment WHERE name = '" + depslctd + "'";
                            myConn.Open();

                            string[] allQuery = { querydeletePO };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }

                            myConn.Close();
                            MessageBox.Show("Record Deleted successfully", "Department List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            refresh();
                        }
                        catch (Exception ex)
                        {
                            myConn.Close();
                            MessageBox.Show("Unable to remove selected Department", "Department List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            myConn.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                myConn.Close();
            }
            finally
            {
                myConn.Dispose();
            }            
        }

        private void dataGridViewDepartmentList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Description" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewDepartmentList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewDepartmentList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewDepartmentList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewDepartmentList, e);
        }
    }
}
