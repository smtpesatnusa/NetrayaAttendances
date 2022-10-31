using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class Leavelist : MaterialForm
    {
        readonly LoadForm lf = new LoadForm();
        readonly Helper help = new Helper();
        private DataSet ds;
        private DataTable dtSource;
        private int PageCount;
        private int maxRec;
        private int pageSize;
        private int currentPage;
        private int recNo;
        private string Sql;

        string idUser, dept;

        public Leavelist()
        {
            InitializeComponent();
        }

        //The below is the key for showing Progress bar
        private void StartProgress(String strStatusText)
        {
            LoadForm lf = new LoadForm();
            ShowProgress();
        }
        private void CloseProgress()
        {
            //Thread.Sleep(200);
            while (!this.IsHandleCreated)
                System.Threading.Thread.Sleep(200);
            lf.Invoke(new Action(lf.Close));
        }
        private void ShowProgress()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    try
                    {
                        lf.ShowDialog();
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    Thread th = new Thread(ShowProgress);
                    th.IsBackground = false;
                    th.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CheckFillButton()
        {
            // Check if the user clicks the "Fill Grid" button.
            if (pageSize == 0)
            {
                MessageBox.Show("Set the Page Size, and then click the \"Fill Grid\" button!");
                return false;
            }
            else
                return true;
        }

        private void LoadPage()
        {
            int startRec;
            int endRec;
            DataTable dtTemp;

            // Duplicate or clone the source table to create the temporary table.
            dtTemp = dtSource.Clone();

            if (currentPage == PageCount)
                endRec = maxRec;
            else
                endRec = pageSize * currentPage;

            startRec = recNo;

            //remove button
            while (dataGridViewLeaveList.Columns.Count > 0)
            {
                dataGridViewLeaveList.Columns.RemoveAt(0);
            }

            if (dtSource.Rows.Count > 0)
            {
                // Copy the rows from the source table to fill the temporary table.
                for (int i = startRec; i <= endRec - 1; i++)
                {
                    dtTemp.ImportRow(dtSource.Rows[i]);
                    recNo++;
                }
            }

            dataGridViewLeaveList.DataSource = dtTemp;
            DisplayPageInfo();
        }

        private void DisplayPageInfo()
        {
            txtDisplayPageNo.Text = "Page " + currentPage.ToString() + "/ " + PageCount.ToString();
        }

        private void LoadDS(string SQL)
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(SQL, connectionDB.connection);
                ds = new DataSet();

                // Fill the DataSet.
                da.Fill(ds, "Items");

                // Set the source table.
                dtSource = ds.Tables["Items"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connectionDB.connection.Dispose();
            }
        }

        private void FillGrid()
        {
            // Set the start and max records. 
            pageSize = 50; // txtPageSize.Text
            maxRec = dtSource.Rows.Count;
            PageCount = maxRec / pageSize;

            // Adjust the page number if the last page contains a partial page.
            if ((maxRec % pageSize) > 0)
                PageCount++;

            // Initial seeings
            currentPage = 1;
            recNo = 0;

            // Display the content of the current page.
            LoadPage();
        }

        private void refresh()
        {
            tbSearch.Clear();

            // remove data in datagridview result
            dataGridViewLeaveList.DataSource = null;
            dataGridViewLeaveList.Refresh();

            while (dataGridViewLeaveList.Columns.Count > 0)
            {
                dataGridViewLeaveList.Columns.RemoveAt(0);
            }

            loadData();

            dataGridViewLeaveList.Update();
            dataGridViewLeaveList.Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            help.dateTimeNow(dateTimeNow);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            MainMenu mm = new MainMenu();
            mm.toolStripUsername.Text = toolStripUsername.Text;
            mm.userdetail.Text = userdetail.Text;
            mm.Show();
            this.Hide();
        }

        private void loadData()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                connectionDB.connection.Open();

                //get dept depend on user dept
                if (dept == "All")
                {
                    Sql = "SELECT a.badgeID, b.name , c.name, DATE_FORMAT(startDate, '%b, %e %Y') AS startDate,DATE_FORMAT(endDate, '%b, %e %Y') AS endDate," +
                        "d.name FROM tbl_leave a, tbl_employee b, tbl_masterleavetype c, tbl_masterstatus d " +
                        "WHERE b.badgeID = a.badgeID AND c.id = a.leavetype AND d.id = a.status ORDER BY a.id DESC";
                }
                else
                {
                    Sql = "SELECT a.badgeID, b.name , c.name, DATE_FORMAT(startDate, '%b, %e %Y') AS startDate,DATE_FORMAT(endDate, '%b, %e %Y') AS endDate," +
                        "d.name FROM tbl_leave a, tbl_employee b, tbl_masterleavetype c, tbl_masterstatus d " +
                        "WHERE b.dept = '"+dept+"' and b.badgeID = a.badgeID AND c.id = a.leavetype AND d.id = a.status ORDER BY a.id DESC";
                }

                StartProgress("Loading...");

                LoadDS(Sql);
                FillGrid();

                string record = dtSource.Rows.Count.ToString();

                CloseProgress();

                connectionDB.connection.Close();

                totalLbl.Text = dtSource.Rows.Count.ToString();
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


        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (!CheckFillButton())
                return;

            // Check if you are already at the first page.
            if (currentPage == 1)
            {
                MessageBox.Show("You are at the First Page!");
                return;
            }

            currentPage = 1;
            recNo = 0;

            LoadPage();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            currentPage--;

            // Check if you are already at the first page.
            if (currentPage < 1)
            {
                MessageBox.Show("You are at the First Page!");
                currentPage = 1;
                return;
            }
            else
                recNo = pageSize * (currentPage - 1);

            LoadPage();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            // Check if the user clicked the "Fill Grid" button.
            if (pageSize == 0)
            {
                MessageBox.Show("Set the Page Size, and then click the \"Fill Grid\" button!");
                return;
            }

            currentPage++;

            if (currentPage > PageCount)
            {
                currentPage = PageCount;

                // Check if you are already at the last page.
                if (recNo == maxRec)
                {
                    MessageBox.Show("You are at the Last Page!");
                    return;
                }
            }
            LoadPage();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (!CheckFillButton())
                return;

            // Check if you are already at the last page.
            if (recNo == maxRec)
            {
                MessageBox.Show("You are at the Last Page!");
                return;
            }

            currentPage = PageCount;

            recNo = pageSize * (currentPage - 1);

            LoadPage();
        }


        private void Employeelist_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;

            //icon
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            loadData();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            ApplyLeave applyLeave = new ApplyLeave();
            applyLeave.userdetail.Text = userdetail.Text;
            applyLeave.ShowDialog();
        }

        private void dataGridViewScheduleList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewLeaveList, e);
        }


        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string search = tbSearch.Text.Replace("'", "''");

                //get dept depend on user dept
                if (dept == "All")
                {
                    if (tbSearch.Text == "")
                    {
                        Sql = "SELECT a.badgeID, b.name , c.name, DATE_FORMAT(startDate, '%b, %e %Y') AS startDate,DATE_FORMAT(endDate, '%b, %e %Y') AS endDate," +
                        "d.name FROM tbl_leave a, tbl_employee b, tbl_masterleavetype c, tbl_masterstatus d " +
                        "WHERE b.badgeID = a.badgeID AND c.id = a.leavetype AND d.id = a.status ORDER BY a.id DESC";
                    }
                    else
                    {
                        Sql = "SELECT a.badgeID, b.name , c.name, DATE_FORMAT(startDate, '%b, %e %Y') AS startDate,DATE_FORMAT(endDate, '%b, %e %Y') AS endDate," +
                            "d.name FROM tbl_leave a, tbl_employee b, tbl_masterleavetype c, tbl_masterstatus d WHERE b.badgeID = a.badgeID AND c.id = a.leavetype AND d.id = a.status " +
                            "AND(a.badgeID LIKE '%" + search + "%' OR b.name LIKE '%" + search + "%' OR c.name LIKE '%" + search + "%' OR startdate LIKE '%" + search + "%' OR enddate LIKE '%" + search + "%' OR d.name LIKE '%" + search + "%')";
                    }
                }
                else
                {
                    if (tbSearch.Text == "")
                    {
                        Sql = "SELECT a.badgeID, b.name , c.name, DATE_FORMAT(startDate, '%b, %e %Y') AS startDate,DATE_FORMAT(endDate, '%b, %e %Y') AS endDate," +
                        "d.name FROM tbl_leave a, tbl_employee b, tbl_masterleavetype c, tbl_masterstatus d " +
                        "WHERE b.dept = '" + dept + "' and b.badgeID = a.badgeID AND c.id = a.leavetype AND d.id = a.status ORDER BY a.id DESC";
                    }
                    else
                    {
                        Sql = "SELECT a.badgeID, b.name , c.name, DATE_FORMAT(startDate, '%b, %e %Y') AS startDate,DATE_FORMAT(endDate, '%b, %e %Y') AS endDate," +
                            "d.name FROM tbl_leave a, tbl_employee b, tbl_masterleavetype c, tbl_masterstatus d WHERE b.dept = '" + dept + "' and b.badgeID = a.badgeID AND c.id = a.leavetype AND d.id = a.status " +
                            "AND(a.badgeID LIKE '%" + search + "%' OR b.name LIKE '%" + search + "%' OR c.name LIKE '%" + search + "%' OR startdate LIKE '%" + search + "%' OR enddate LIKE '%" + search + "%' OR d.name LIKE '%" + search + "%')";
                    }
                }

                LoadDS(Sql);
                FillGrid();
                totalLbl.Text = dtSource.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Leavelist_FormClosing(object sender, FormClosingEventArgs e)
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

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewLeaveList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridViewLeaveList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            // Set table title
            string[] title = { "Badge ID",  "Name", "Leave Type", "Start Date", "End Date", "Status"};

            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewLeaveList.Columns[i].HeaderText = "" + title[i];
            }
        }
    }
}
