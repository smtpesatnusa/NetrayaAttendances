using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class WorkArealist : MaterialForm
    {
        readonly Helper help = new Helper();

        string idUser, dept;
        MySqlConnection myConn;

        public WorkArealist()
        {
            InitializeComponent();
        }


        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");

                (dataGridViewWorkAreaList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'or description LIKE '%" + search + "%'");
                
                // to display total data
                totalLbl.Text = dataGridViewWorkAreaList.Rows.Count.ToString();
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
            tbWorkArea.Clear();
            tbDesc.Clear();

            // remove data in datagridview result
            dataGridViewWorkAreaList.DataSource = null;
            dataGridViewWorkAreaList.Refresh();

            while (dataGridViewWorkAreaList.Columns.Count > 0)
            {
                dataGridViewWorkAreaList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewWorkAreaList.Update();
            dataGridViewWorkAreaList.Refresh();
        }

        private void LoadData()
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);
            try
            {
                string query = "SELECT name,description from tbl_masterworkarea ORDER BY id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewWorkAreaList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewWorkAreaList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewWorkAreaList.Rows.Count.ToString();
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
            tbWorkArea.Clear();
            tbDesc.Clear();
            tbWorkArea.Focus();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbWorkArea.Text == "" || tbDesc.Text == "" )
            {
                MessageBox.Show(this, "Unable Add Work Area with let name or description blank", "Add Work Area", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);
                    string workarea = tbWorkArea.Text;
                    string desc = tbDesc.Text;

                    string cek = "SELECT * FROM tbl_masterworkarea WHERE name = '" + workarea + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, myConn))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Work Area, Work Area already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbWorkArea.Clear();
                            tbDesc.Clear();
                            tbWorkArea.Focus();
                        }
                        else
                        {
                            myConn.Open();
                            string queryAdd = "INSERT INTO tbl_masterworkarea (name, description, createDate, createBy) VALUES " +
                                "('" + workarea + "', '" + desc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd};
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            myConn.Close();
                            MessageBox.Show(this, "Work Area Successfully Added", "Add Work Area", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            refresh();
                            tbWorkArea.Focus();
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


        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu mm = new MainMenu();
            mm.toolStripUsername.Text = toolStripUsername.Text;
            mm.userdetail.Text = userdetail.Text;
            mm.Show();
            this.Hide();
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewWorkAreaList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewWorkAreaList, e);
        }

        private void dataGridViewWorkAreaList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewWorkAreaList.SelectedCells[0].RowIndex;
            string workareaslctd = dataGridViewWorkAreaList.Rows[i].Cells[0].Value.ToString();

            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            if (e.ColumnIndex == 2)
            {
                string message = "Do you want to delete this Work Area " + workareaslctd + "?";
                string title = "Delete Work Area";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var cmd = new MySqlCommand("", myConn);

                        string querydelete = "DELETE FROM tbl_masterworkarea WHERE name = '" + workareaslctd + "'";
                        myConn.Open();

                        string[] allQuery = { querydelete };
                        for (int j = 0; j < allQuery.Length; j++)
                        {
                            cmd.CommandText = allQuery[j];
                            //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                            cmd.ExecuteNonQuery();
                            //Jalankan perintah / query dalam CommandText pada database
                        }

                        myConn.Close();
                        MessageBox.Show("Record Deleted successfully", "Work Area List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        refresh();
                    }
                    catch (Exception ex)
                    {
                        myConn.Close();
                        MessageBox.Show("Unable to remove selected Work Area", "Work Area List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myConn.Dispose();
                    }
                }
            }
        }

        private void WorkArealist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridViewWorkAreaList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Description" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewWorkAreaList.Columns[i].HeaderText = "" + title[i];
            }
            dataGridViewWorkAreaList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void WorkArealist_Load(object sender, EventArgs e)
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

    }
}
