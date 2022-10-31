using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class Shiftlist : MaterialForm
    {
        readonly Helper help = new Helper();
        string idUser, dept;
        MySqlConnection myConn;

        public Shiftlist()
        {
            InitializeComponent();
        }

        private void Shiftlist_Load(object sender, EventArgs e)
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

                (dataGridViewShiftList.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("name LIKE '%" + search + "%'");

                totalLbl.Text = dataGridViewShiftList.Rows.Count.ToString();
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
            dataGridViewShiftList.DataSource = null;
            dataGridViewShiftList.Refresh();

            while (dataGridViewShiftList.Columns.Count > 0)
            {
                dataGridViewShiftList.Columns.RemoveAt(0);
            }

            LoadData();

            dataGridViewShiftList.Update();
            dataGridViewShiftList.Refresh();
        }

        private void LoadData()
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            try
            {
                //string query = "SELECT a.name, CONCAT(LEFT(a.intime, 5),'-', LEFT(a.outtime, 5)) AS WorkHour, " +
                //    "(SELECT CONCAT(LEFT(b.timein, 5), '-', LEFT(b.timeout, 5)) FROM tbl_mastershiftbreak b WHERE b.descr = 'meal' AND a.id = b.shiftId) meal," +
                //    "(SELECT CONCAT(LEFT(b.timein, 5), '-', LEFT(b.timeout, 5)) FROM tbl_mastershiftbreak b WHERE b.descr = 'coffe break 1' AND a.id = b.shiftId) coffebreak1," +
                //    "(SELECT CONCAT(LEFT(b.timein, 5), '-', LEFT(b.timeout, 5)) FROM tbl_mastershiftbreak b WHERE b.descr = 'coffe break 2' AND a.id = b.shiftId) coffebreak2" +
                //    "   FROM tbl_mastershiftdetail a ORDER BY id DESC";

                string query = "SELECT a.name, a.category, CONCAT(LEFT(a.intime, 5),'-', LEFT(a.outtime, 5)) AS WorkHour, totalBreak " +
                    "FROM tbl_mastershiftdetail a ORDER BY id DESC";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewShiftList.DataSource = dset.Tables[0];

                    // add button delete in datagridview table
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    dataGridViewShiftList.Columns.Add(btnDelete);
                    btnDelete.HeaderText = "";
                    btnDelete.Text = "Delete";
                    btnDelete.Name = "btnDelete";
                    btnDelete.UseColumnTextForButtonValue = true;
                }
                totalLbl.Text = dataGridViewShiftList.Rows.Count.ToString();
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

        private void Shiftlist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridViewShiftList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewShiftList.SelectedCells[0].RowIndex;
            string slctd = dataGridViewShiftList.Rows[i].Cells[0].Value.ToString();

            try
            {
                if (e.ColumnIndex == 4)
                {
                    string message = "Do you want to delete this Shift " + slctd + "?";
                    string title = "Delete Shift";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    MessageBoxIcon icon = MessageBoxIcon.Information;
                    DialogResult result = MessageBox.Show(this, message, title, buttons, icon);

                    if (result == DialogResult.Yes)
                    {
                        string koneksi = ConnectionDB.strProvider;
                        myConn = new MySqlConnection(koneksi);

                        var cmd = new MySqlCommand("", myConn);

                        string querydelete = "DELETE FROM tbl_mastershiftdetail WHERE name = '" + slctd + "'";
                        myConn.Open();

                        cmd.CommandText = querydelete;
                        cmd.ExecuteNonQuery();

                        myConn.Close();
                        MessageBox.Show("Record Deleted successfully", "Shift List Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                myConn.Close();
                MessageBox.Show("Unable to remove selected Shift", "Shift List Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                myConn.Dispose();
            }
            
        }

        private void dataGridViewShiftList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Name", "Category", "Work Hour", "Total Break (minute)" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewShiftList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewShiftList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddShift addShift = new AddShift();
            addShift.usernameLbl.Text = toolStripUsername.Text;
            addShift.ShowDialog();
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewShiftList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewShiftList, e);
        }
    }
}
