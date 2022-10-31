using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class Devicelist : MaterialForm
    {
        readonly Helper help = new Helper();
        string idUser, dept;

        public Devicelist()
        {
            InitializeComponent();
        }

        private void Devicelist_Load(object sender, EventArgs e)
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

            //menampilkan combobox workArea
            help.displayCmbList("SELECT * FROM tbl_masterworkarea ORDER BY id ", "name", "name", cmbWorkArea);

            //menampilkan combobox department
            help.displayCmbList("SELECT * FROM tbl_masterdepartment ORDER BY id ", "name", "name", cmbDepartment);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");
                (dataGridViewDeviceList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("workarea LIKE '%" + search + "%' or dept LIKE '%" + search + "%' " +
                    "or ipAddress LIKE '%" + search + "%' or indicator LIKE '%" + search + "%'");

                totalLbl.Text = dataGridViewDeviceList.Rows.Count.ToString();
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

            cmbWorkArea.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
            tbipAddress.Clear();
            tbPort.Clear();
            isActiveCheckBox.Checked = true;

            // remove data in datagridview result
            dataGridViewDeviceList.DataSource = null;
            dataGridViewDeviceList.Refresh();

            while (dataGridViewDeviceList.Columns.Count > 0)
            {
                dataGridViewDeviceList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewDeviceList.Update();
            dataGridViewDeviceList.Refresh();
        }

        private void LoadData()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT workarea, dept, ipAddress, port, indicator, isActive FROM tbl_masterdevice ORDER BY id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewDeviceList.DataSource = dset.Tables[0];

                    // add button edit in datagridview table
                    DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                    dataGridViewDeviceList.Columns.Add(btnEdit);
                    btnEdit.HeaderText = "";
                    btnEdit.Text = "Edit";
                    btnEdit.Name = "btnEdit";
                    btnEdit.UseColumnTextForButtonValue = true;

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewDeviceList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }

                totalLbl.Text = dataGridViewDeviceList.Rows.Count.ToString();
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
            cmbWorkArea.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
            tbipAddress.Clear();
            isActiveCheckBox.Checked = true;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (cmbWorkArea.Text == "" || cmbDepartment.Text == "" || tbipAddress.Text == "" || tbPort.Text == "" || cmbInout.Text == "")
            {
                MessageBox.Show(this, "Unable Add Device with let data blank", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                try
                {
                    var cmd = new MySqlCommand("", connectionDB.connection);
                    string workArea = cmbWorkArea.Text;
                    string department = cmbDepartment.Text;
                    string ipAddress = tbipAddress.Text;
                    string port = tbPort.Text;
                    string indicator = cmbInout.Text;
                    string isCheck;

                    //get checkbox value
                    if (isActiveCheckBox.CheckState == CheckState.Checked)
                    {
                        isCheck = "1";
                    }
                    else
                    {
                        isCheck = "0";
                    }

                    string cek = "SELECT * FROM tbl_masterdevice WHERE ipAddress = '" + ipAddress + "'";
                    using (MySqlDataAdapter adpt = new MySqlDataAdapter(cek, connectionDB.connection))
                    {
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        // cek jika modelno tsb sudah di upload
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(this, "Unable to add Device, Device with selected IP already insert", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cmbWorkArea.SelectedIndex = -1;
                            cmbDepartment.SelectedIndex = -1;
                            tbipAddress.Clear();
                            isActiveCheckBox.Checked = true;
                        }
                        else
                        {
                            connectionDB.connection.Open();
                            string queryAdd = "INSERT INTO tbl_masterdevice (workarea, dept, ipAddress, port,  indicator, isActive, createDate, createBy) VALUES " +
                                "('" + workArea + "', '" + department + "', '" + ipAddress + "', '" + port + "', '" + indicator + "', '" + isCheck + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + idUser + "')";

                            string[] allQuery = { queryAdd };
                            for (int j = 0; j < allQuery.Length; j++)
                            {
                                cmd.CommandText = allQuery[j];
                                //Masukkan perintah/query yang akan dijalankan ke dalam CommandText
                                cmd.ExecuteNonQuery();
                                //Jalankan perintah / query dalam CommandText pada database
                            }
                            connectionDB.connection.Close();
                            MessageBox.Show(this, "Device Successfully Added", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            refresh();
                            cmbWorkArea.Focus();
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
            string[] title = { "Work Area", "Dept", "IP address", "Indicator", "isActive" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewDeviceList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewDeviceList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void dataGridViewLineCodeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewDeviceList.SelectedCells[0].RowIndex;
            string nameslctd = dataGridViewDeviceList.Rows[i].Cells[0].Value.ToString();
            string deptslctd = dataGridViewDeviceList.Rows[i].Cells[1].Value.ToString();
            string ipslctd = dataGridViewDeviceList.Rows[i].Cells[2].Value.ToString();
            string indicslctd = dataGridViewDeviceList.Rows[i].Cells[3].Value.ToString();
            string isactiveslctd = dataGridViewDeviceList.Rows[i].Cells[4].Value.ToString();

            if (e.ColumnIndex == 6)
            {
                EditDevice editDevice = new EditDevice();
                editDevice.usernameLbl.Text = toolStripUsername.Text;
                editDevice.tbDeviceName.Text = nameslctd;
                editDevice.cmbDepartment.SelectedItem = deptslctd;
                editDevice.tbipAddress.Text = ipslctd;
                editDevice.cmbInout.SelectedItem = indicslctd;
                editDevice.ShowDialog();
            }
            if (e.ColumnIndex == 7)
            {
                string message = "Do you want to delete this Device with IP " + ipslctd + "?";
                string title = "Delete Device";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                if (result == DialogResult.Yes)
                {
                    ConnectionDB connectionDB = new ConnectionDB();
                    try
                    {
                        var cmd = new MySqlCommand("", connectionDB.connection);

                        string querydelete = "DELETE FROM tbl_masterdevice WHERE ipAddress = '" + ipslctd + "'";
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
                        Devicelist devicelist = new Devicelist();
                        devicelist.toolStripUsername.Text = toolStripUsername.Text;
                        devicelist.userdetail.Text = userdetail.Text;
                        devicelist.Show();
                        this.Hide();
                        MessageBox.Show("Record Deleted successfully", "Device List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        connectionDB.connection.Close();
                        MessageBox.Show(ex.Message);
                        //MessageBox.Show("Unable to remove selected Device", "Device List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridViewDeviceList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewDeviceList, e);
        }
    }
}
