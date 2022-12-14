using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class OnlineStatus : MaterialForm
    {
        readonly Helper help = new Helper();
        string idUser, dept;

        readonly LoadForm lf = new LoadForm();
        private DataSet ds;
        private DataTable dtSource;
        private int PageCount;
        private int maxRec;
        private int pageSize;
        private int currentPage;
        private int recNo;

        DateTime dt1, dt2;
        MySqlConnection myConn;

        public OnlineStatus()
        {
            InitializeComponent();
        }

        private void LeavelistApproval_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;

            //icon
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            dtBegin.Value = DateTime.Now;
            dtEnd.Value = DateTime.Now;

            LoadDataOnline();
            LoadDataOffline();

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
            while (dataGridViewOnline.Columns.Count > 0)
            {
                dataGridViewOnline.Columns.RemoveAt(0);
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

            dataGridViewOnline.DataSource = dtTemp;
            DisplayPageInfo();
        }

        private void DisplayPageInfo()
        {
            txtDisplayPageNo.Text = "Page " + currentPage.ToString() + "/ " + PageCount.ToString();
        }

        private void LoadDS(string SQL)
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(SQL, myConn);
                ds = new DataSet();

                // Fill the DataSet.
                da.Fill(ds, "Items");

                // Set the source table.
                dtSource = ds.Tables["Items"];
            }
            catch (Exception ex)
            {
                myConn.Close();
                //MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myConn.Dispose();
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

        private void LoadDataOnline()
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            try
            {
                dt1 = dtBegin.Value;
                dt2 = dtEnd.Value;

                myConn.Open();

                string query = "SELECT b.badgeId, b.name, b.linecode FROM tbl_attendance a, tbl_employee b WHERE a.emplid = b.id AND b.dept = '"+dept+"' AND " +
                    "(a.date BETWEEN '" + dt1.ToString("yyyy-MM-dd") + "' AND '" + dt2.ToString("yyyy-MM-dd") + "') GROUP BY b.badgeId, b.name, b.linecode";

                StartProgress("Loading...");

                LoadDS(query);
                FillGrid();

                string record = dtSource.Rows.Count.ToString();

                CloseProgress();
                myConn.Close();
                totalLblAll.Text = dtSource.Rows.Count.ToString();
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

        private void LoadDataOffline()
        {
            string koneksi = ConnectionDB.strProvider;
            myConn = new MySqlConnection(koneksi);

            try
            {
                dt1 = dtBegin.Value;
                dt2 = dtEnd.Value;

                string query = "SELECT badgeID, NAME, linecode FROM tbl_employee WHERE badgeID NOT IN(SELECT b.badgeId FROM tbl_attendance a, " +
                    "tbl_employee b WHERE a.emplid = b.id AND b.dept = '" + dept + "' AND (a.date BETWEEN '" + dt1.ToString("yyyy-MM-dd") + "' AND '" + dt2.ToString("yyyy-MM-dd") + "'))";
                   
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, myConn))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        dataGridViewOffline.DataSource = dset.Tables[0];
                    }
                }
                totalLbl.Text = dataGridViewOffline.Rows.Count.ToString();
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
        

        private void refresh()
        {
            dtBegin.Value = DateTime.Now;
            dtEnd.Value = DateTime.Now;

            // refresh request leave
            tbSearchOnline.Clear();
            // refresh request leave
            tbSearchOffline.Clear();

            // reset datagridview
            DataGridView[] dgv = { dataGridViewOnline, dataGridViewOffline};
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

            LoadDataOnline();
            LoadDataOffline();
        }        


        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            MainMenu mm = new MainMenu();
            mm.toolStripUsername.Text = toolStripUsername.Text;
            mm.userdetail.Text = userdetail.Text;
            mm.Show();
            this.Hide();
        }


        private void dataGridViewOntimeList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Badge ID", "Name", "Line Code"};
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewOffline.Columns[i].HeaderText = "" + title[i];
            }
        }

       

        private void dataGridViewOntimeList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewOffline, e);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            help.dateTimeNow(dateTimeNow);
        }

        private void dataGridViewAttendanceList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewOnline.SelectedCells[0].RowIndex;
            string badgeslctd = dataGridViewOnline.Rows[i].Cells[0].Value.ToString();
            string employeeslctd = dataGridViewOnline.Rows[i].Cells[1].Value.ToString();
            string statusslctd = dataGridViewOnline.Rows[i].Cells[6].Value.ToString();
            //string dateslctd = dateSelected;

            if (e.ColumnIndex == 7)
            {
                if (statusslctd != "Absent")
                {
                    DetailPosition detailPosition = new DetailPosition();
                    detailPosition.tbBadge.Text = badgeslctd;
                    detailPosition.tbName.Text = employeeslctd;

                    //detailPosition.tbDate.Text = dateslctd;
                    detailPosition.ShowDialog();
                }
                else
                {
                    MessageBox.Show(this, "No any detail for absent employee!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        private void btnView_Click(object sender, EventArgs e)
        {
            // reset datagridview
            DataGridView[] dgv = { dataGridViewOnline, dataGridViewOffline };
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

            LoadDataOnline();
            LoadDataOffline();
        }

        private void dataGridViewLeaveList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridViewOffline.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewOffline.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void tbSearchOnline_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewOnline.Rows.Count > 0)
                {
                    string search = tbSearchOnline.Text.Replace("'", "''");

                    if (tbSearchOnline.Text == "")
                    {
                        LoadDataOnline();
                    }
                    else
                    {
                        string query = "SELECT b.badgeId, b.name, b.linecode FROM tbl_attendance a, tbl_employee b WHERE a.emplid = b.id AND b.dept = '" + dept + "' AND " +
                    "(a.date BETWEEN '" + dt1.ToString("yyyy-MM-dd") + "' AND '" + dt2.ToString("yyyy-MM-dd") + "') GROUP BY b.badgeId, b.name, b.linecode";

                        //    string query = "SELECT badgeID, NAME, linecode, ScheduleIn, intime, outtime, Sttus FROM (SELECT e.badgeID, e.name, e.linecode, " +
                        //"DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, " +
                        //"IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' " +
                        //"AND a.date = '" + dateSelected + "' AND a.ScheduleIn IS NOT NULL ORDER BY a.ScheduleIn ASC)  AS a where" +
                        //" (badgeID  LIKE '%" + search + "%' OR NAME LIKE '%" + search + "%' OR linecode LIKE '%" + search + "%' OR ScheduleIn LIKE '%" + search + "%' " +
                        //"OR intime LIKE '%" + search + "%' OR outtime LIKE '%" + search + "%' OR Sttus LIKE '%" + search + "%')" +
                        //"UNION SELECT badgeID, NAME, linecode, ScheduleIn, intime, outtime, Sttus FROM (SELECT badgeID, NAME, linecode, ScheduleIn, intime, outtime, Sttus FROM (SELECT badgeID, NAME, linecode, '-' AS ScheduleIn, " +
                        //"'-' AS intime, '-' AS outtime, 'Absent' AS Sttus FROM tbl_employee WHERE badgeID NOT IN(SELECT b.badgeID FROM tbl_attendance a, tbl_employee b " +
                        //"WHERE a.EmplId = b.id AND a.date = '" + dateSelected + "' AND b.dept = '" + dept + "' AND intime IS NOT NULL) ) AS A ) AS a where" +
                        //" (badgeID  LIKE '%" + search + "%' OR NAME LIKE '%" + search + "%' OR linecode LIKE '%" + search + "%' OR ScheduleIn LIKE '%" + search + "%' " +
                        //"OR intime LIKE '%" + search + "%' OR outtime LIKE '%" + search + "%' OR Sttus LIKE '%" + search + "%')";


                        //"SELECT b.badgeId, b.name, b.dept, b.linecode, RIGHT(MIN(a.datetimeAbsent),8) AS clockIn FROM tbl_inout a, tbl_employee b WHERE a.indicator = 'in' " +
                        //"AND a.rfidNo = b.rfidNo AND(a.datetimeAbsent BETWEEN '" + dateSelected + " 00:00:01' AND '" + dateSelected + " 23:59:59') AND b.dept = '" + dept + "' " +
                        //"AND (b.badgeId LIKE '%" + search + "%' OR b.name LIKE '%" + search + "%' OR b.dept LIKE '%" + search + "%'OR b.linecode LIKE '%" + search + "%'OR a.datetimeAbsent LIKE '%" + search + "%' ) GROUP BY a.rfidNo, b.badgeId, b.name, b.dept, b.linecode, a.indicator  ";

                        //LoadDS(query);
                        FillGrid();
                        totalLblAll.Text = dtSource.Rows.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void tbSearchOffline_TextChanged(object sender, EventArgs e)
        {

        }

        private void OnlineStatus_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
