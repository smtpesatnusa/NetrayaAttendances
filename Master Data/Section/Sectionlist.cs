using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class Sectionlist : MaterialForm
    {
        readonly Helper help = new Helper();
                
        string idUser, dept;

        public Sectionlist()
        {
            InitializeComponent();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");
                (dataGridViewSectionList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'or description LIKE '%" + search + "%'");

                totalLbl.Text = dataGridViewSectionList.Rows.Count.ToString();
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
            dataGridViewSectionList.DataSource = null;
            dataGridViewSectionList.Refresh();

            while (dataGridViewSectionList.Columns.Count > 0)
            {
                dataGridViewSectionList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewSectionList.Update();
            dataGridViewSectionList.Refresh();
        }

        private void LoadData()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT name,description from tbl_mastersection ORDER BY id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewSectionList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewSectionList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewSectionList.Rows.Count.ToString();
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
            tbSection.Clear();
            tbDesc.Clear();
            tbSection.Focus();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tbSection.Text == "" || tbDesc.Text == "" )
            {
                MessageBox.Show(this, "Unable Add Section with let name or description blank", "Add Section", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                try
                {
                    var cmd = new MySqlCommand("", connectionDB.connection);
                    string section = tbSection.Text;
                    string desc = tbDesc.Text;

                    string cek = "SELECT * FROM tbl_mastersection WHERE name = '" + section + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, connectionDB.connection))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add section, Section already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbSection.Clear();
                            tbDesc.Clear();
                            tbSection.Focus();
                        }
                        else
                        {
                            connectionDB.connection.Open();
                            string queryAdd = "INSERT INTO tbl_mastersection (name, description, createDate, createBy) VALUES " +
                                "('" + section + "', '" + desc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            connectionDB.connection.Close();
                            MessageBox.Show(this, "Section Successfully Added", "Add Section", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tbSection.Clear();
                            tbDesc.Clear();
                            refresh();
                            tbSection.Focus();
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

        private void Sectionlist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Sectionlist_Load(object sender, EventArgs e)
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

        private void dataGridViewSectionList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Description" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewSectionList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewSectionList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void dataGridViewSectionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewSectionList.SelectedCells[0].RowIndex;
            string slctd = dataGridViewSectionList.Rows[i].Cells[0].Value.ToString();

            if (e.ColumnIndex == 2)
            {
                string message = "Do you want to delete this Section " + slctd + "?";
                string title = "Delete Section";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    ConnectionDB connectionDB = new ConnectionDB();
                    try
                    {
                        var cmd = new MySqlCommand("", connectionDB.connection);

                        string querydelete = "DELETE FROM tbl_mastersection WHERE name = '" + slctd + "'";
                        connectionDB.connection.Open();

                        string[] allQuery = { querydelete };
                        for (int j = 0; j < allQuery.Length; j++)
                        {
                            cmd.CommandText = allQuery[j];
                            //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                            cmd.ExecuteNonQuery();
                            //Jalankan perintah / query dalam CommandText pada database
                        }

                        connectionDB.connection.Close();
                        Sectionlist sectionlist = new Sectionlist();
                        sectionlist.toolStripUsername.Text = toolStripUsername.Text;
                        sectionlist.userdetail.Text = userdetail.Text;
                        sectionlist.Show();
                        this.Hide();
                        MessageBox.Show("Record Deleted successfully", "Section List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        connectionDB.connection.Close();
                        MessageBox.Show("Unable to remove selected Section", "Section List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridViewSectionList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewSectionList, e);
        }
    }
}
