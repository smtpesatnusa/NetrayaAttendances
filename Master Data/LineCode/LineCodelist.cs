using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class LineCodelist : MaterialForm
    {
        readonly Helper help = new Helper();

        string idUser, dept;
        string query;
        MySqlConnection myConn;

        public LineCodelist()
        {
            InitializeComponent();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");
                (dataGridViewLineCodeList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'or description LIKE '%" + search + "%'");
                totalLbl.Text = dataGridViewLineCodeList.Rows.Count.ToString();
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
            dataGridViewLineCodeList.DataSource = null;
            dataGridViewLineCodeList.Refresh();

            while (dataGridViewLineCodeList.Columns.Count > 0)
            {
                dataGridViewLineCodeList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewLineCodeList.Update();
            dataGridViewLineCodeList.Refresh();
        }

        private void LoadData()
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);
            try
            {
                //get line code based on dept user
                if (dept == "All")
                {
                    query = "SELECT name,description, dept from tbl_masterlinecode ORDER BY id DESC";
                }
                else
                {
                    query = "SELECT name,description, dept from tbl_masterlinecode where dept = '" + dept + "' ORDER BY id DESC";
                }

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewLineCodeList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewLineCodeList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewLineCodeList.Rows.Count.ToString();
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
            tbLineCode.Clear();
            tbDesc.Clear();
            tbLineCode.Focus();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbLineCode.Text == "" || tbDesc.Text == "")
            {
                MessageBox.Show(this, "Unable Add Line Code with any field blank", "Add Line Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string koneksi = ConnectionDB.strProvider;
                myConn = new MySqlConnection(koneksi);
                try
                {
                    var cmd = new MySqlCommand("", myConn);
                    string lineCode = tbLineCode.Text;
                    string desc = tbDesc.Text;
                    string dept = cmbDepartment.Text;

                    string cek = "SELECT * FROM tbl_masterlinecode WHERE name = '" + lineCode + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, myConn))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Line Code, Line Code already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbLineCode.Clear();
                            tbDesc.Clear();
                            tbLineCode.Focus();
                        }
                        else
                        {
                            myConn.Open();
                            string queryAdd = "INSERT INTO tbl_masterlinecode (name, description, dept, createDate, createBy) VALUES " +
                                "('" + lineCode + "', '" + desc + "', '" + dept + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            myConn.Close();
                            MessageBox.Show(this, "Line Code Successfully Added", "Add Line Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tbLineCode.Clear();
                            tbDesc.Clear();
                            refresh();
                            tbLineCode.Focus();
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

        private void LineCodelist_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;

            //icon
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            //get employee  user based on dept user
            if (dept == "All")
            {
                help.displayCmbList("SELECT * FROM tbl_masterdepartment ORDER BY id ", "name", "name", cmbDepartment);
            }
            else
            {
                help.displayCmbList("SELECT * FROM tbl_masterdepartment where name = '" + dept + "' ORDER BY id ", "name", "name", cmbDepartment);
            }

            LoadData();
        }

        private void LineCodelist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridViewLineCodeList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Description", "Dept." };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewLineCodeList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewLineCodeList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void dataGridViewLineCodeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewLineCodeList.SelectedCells[0].RowIndex;
            string slctd = dataGridViewLineCodeList.Rows[i].Cells[0].Value.ToString();

            try
            {
                if (e.ColumnIndex == 3)
                {
                    string message = "Do you want to delete this Line Code " + slctd + "?";
                    string title = "Delete Line Code";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    MessageBoxIcon icon = MessageBoxIcon.Information;
                    DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                    if (result == DialogResult.Yes)
                    {
                        string koneksi = ConnectionDB.strProvider;
                        myConn = new MySqlConnection(koneksi);

                        var cmd = new MySqlCommand("", myConn);

                        string querydelete = "DELETE FROM tbl_masterlinecode WHERE name = '" + slctd + "'";
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
                        LineCodelist lineCodelist = new LineCodelist();
                        lineCodelist.toolStripUsername.Text = toolStripUsername.Text;
                        lineCodelist.Show();
                        this.Hide();
                        MessageBox.Show("Record Deleted successfully", "Line Code List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                myConn.Close();
                MessageBox.Show("Unable to remove selected Line Code", "Line Code List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myConn.Dispose();
            }            
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewLineCodeList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewLineCodeList, e);
        }
    }
}
