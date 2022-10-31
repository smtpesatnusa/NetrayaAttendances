using MaterialSkin.Controls;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.Mail;
using System.Net;

namespace SMTAttendance
{
    public partial class MainMenus : MaterialForm
    {
        readonly Helper help = new Helper();

        string idUser, dept;
        string dateNow = DateTime.Now.ToString("yyyy-MM-dd");

        // date
        string dt2 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        string dt3 = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");

        string totalLessBreak;
        string totalOverBreak;

        string late3, late2, late1;
        string ontime3, ontime2, ontime1;
        string over3, over2, over1;
        string break3, break2, break1;

        bool sidebarExpand;

        public MainMenus()
        {
            InitializeComponent();
            CustomizeDesign();
        }

        private void CustomizeDesign()
        {
            panelEmployeeMenu.Visible = false;
            panelLeave.Visible = false;
        }

        private void showSubMenu(Panel SubMenu)
        {
            if (SubMenu.Visible == false)
            {
                hideSubMenu();
                SubMenu.Visible = true;
            }
            else
            {
                SubMenu.Visible = false;
            }
        }


        private void MainMenu_Load(object sender, System.EventArgs e)
        {
            //set full with taskbar below
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;

            //get user id
            var userId = userdetail.Text.Split('|');
            idUser = userId[0].Trim();
            dept = userId[1].Trim();

            dateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            // to display menu based on user role
            CreateMenu();

            // display data
            LoadDataOverBreak3Day();
            LoadDataBreakOK3Day();

            // display data to main menu
            displayData();

            // load pie chart
            LoadPieChart();

            timerRefresh.Start();
        }

        private void hideSubMenu()
        {
            if (panelEmployeeMenu.Visible == true)
            {
                panelEmployeeMenu.Visible = false;
            }
            if (panelLeave.Visible == true)
            {
                panelLeave.Visible = false;
            }
        }

        private void defaultColorPanel()
        {
            panel8.BackColor = Color.FromArgb(48, 63, 159);
            panel8.ForeColor = Color.White;

            panel9.BackColor = Color.FromArgb(48, 63, 159);
            panel9.ForeColor = Color.White;

            panel10.BackColor = Color.FromArgb(48, 63, 159);
            panel10.ForeColor = Color.White;

            panel11.BackColor = Color.FromArgb(48, 63, 159);
            panel11.ForeColor = Color.White;

            panel12.BackColor = Color.FromArgb(48, 63, 159);
            panel12.ForeColor = Color.White;

            panel15.BackColor = Color.FromArgb(48, 63, 159);
            panel15.ForeColor = Color.White;
        }


        private void LoadPieChart()
        {
            LoadDataOverBreak3Day();
            LoadDataBreakOK3Day();

            chartBreak.Series["Ontime"].Points.AddXY("", break3);
            chartBreak.Series["Ontime"].Points.AddXY("", break2);
            chartBreak.Series["Ontime"].Points.AddXY("", break1);
            chartBreak.Series["Over Break"].Points.AddXY(DateTime.Now.AddDays(-2).ToString("dd-MMM"), over3);
            chartBreak.Series["Over Break"].Points.AddXY(DateTime.Now.AddDays(-1).ToString("dd-MMM"), over2);
            chartBreak.Series["Over Break"].Points.AddXY(DateTime.Now.ToString("dd-MMM"), over1);

            // show total Label
            chartBreak.Series["Ontime"].IsValueShownAsLabel = true;
            chartBreak.Series["Over Break"].IsValueShownAsLabel = true;

            //chart title  
            chartBreak.Titles.Add("Break Time Summary");

            chartBreak.Series["Ontime"].ChartType = SeriesChartType.Column;
            chartBreak.Series["Over Break"].ChartType = SeriesChartType.Column;
        }


        //void LoadPieChart()
        //{
        //    chartBreak.Series.Clear();
        //    chartBreak.Titles.Clear();
        //    chartBreak.Palette = ChartColorPalette.Fire;
        //    chartBreak.Titles.Add("Break Time Summary");
        //    chartBreak.ChartAreas[0].BackColor = Color.Transparent;
        //    Series series1 = new Series
        //    {
        //        Name = "series1",
        //        IsVisibleInLegend = true,
        //        Color = Color.Green,
        //        ChartType = SeriesChartType.Doughnut
        //    };
        //    chartBreak.Series.Add(series1);
        //    series1.Points.Add(Convert.ToInt32(break1));
        //    series1.Points.Add(Convert.ToInt32(over1));
        //    var p1 = series1.Points[0];
        //    p1.AxisLabel = break1;
        //    p1.LegendText = "Break less than 80 min";
        //    p1.Color = Color.ForestGreen;
        //    var p2 = series1.Points[1];
        //    p2.AxisLabel = over1;
        //    p2.LegendText = "Break more than 80 min";
        //    p2.Color = Color.Crimson;
        //    chartBreak.Invalidate();
        //    //pnlPie.Controls.Add(pieChart);
        //}


        private void sendEmail()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {              

                var from = new MailAddress("yunindafaranika@gmail.com");
                var to = new MailAddress("yunindafaranika@gmail.com");
                var subject = "Email with attachment";
                var body = "Email body";

                var username = "yunindafaranika@gmail.com"; // get from Mailtrap
                var password = "@Luphu4ever"; // get from Mailtrap

                var host = "smtp.gmail.com";
                var port = 587;

                var client = new SmtpClient(host, port);
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;

                var mail = new MailMessage();
                mail.Subject = subject;
                mail.From = from;
                mail.To.Add(to);
                mail.Body = body;

                //var attachment = new Attachment("\\\\192.168.192.254\\SystemSupport\\Attendance-SMT\\13-10-2022\\Summary.xlsx");
                //mail.Attachments.Add(attachment);

                client.Send(mail);

                Console.WriteLine("Email sent");
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


        private void displayData()
        {
            // get total ontime
            string queryTotalOntime = "SELECT COUNT(*)AS total FROM (SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, " +
                "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dateNow + "' AND a.DayType = 'WorkDay' " +
                "AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Ontime') AS A";

            help.resultQuery(queryTotalOntime, totalOntime, "total");

            // get total late
            string queryTotalLate =
                "SELECT COUNT(*)AS total FROM (SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, " +
                "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dateNow + "' AND a.DayType = 'WorkDay' " +
                "AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Late') AS A";

            help.resultQuery(queryTotalLate, totalLate, "total");

            totalOver.Text = over1;

            fillChart();

            // load data late in datagridview c#
            LoadDataLate();

            // load data break > 3 in datagridview c#
            LoadDataBreakTime();
        }

        private void LoadDataLate3Day()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {

                string query = "SELECT COUNT(*)AS total, '" + dt3 + "' AS DATE  FROM (SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                    "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, " +
                    "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                    "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dt3 + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL " +
                    "ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Late') AS A UNION " +
                    "SELECT COUNT(*)AS total, '" + dt2 + "' AS DATE  FROM(SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                    "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, " +
                    "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                    "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dt2 + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL " +
                    "ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Late') AS A " +
                    "UNION SELECT COUNT(*)AS total, '" + dateNow + "' AS DATE  FROM(SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                    "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut," +
                    "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                    "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dateNow + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL " +
                    "ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Late') AS A";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        late3 = dt.Rows[0]["total"].ToString();
                        late2 = dt.Rows[1]["total"].ToString();
                        late1 = dt.Rows[2]["total"].ToString();
                    }
                }

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

        private void LoadDataOntime3Day()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT COUNT(*)AS total, '" + dt3 + "' AS DATE  FROM (SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                    "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, " +
                    "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                    "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dt3 + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL " +
                    "ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Ontime') AS A UNION " +
                    "SELECT COUNT(*)AS total, '" + dt2 + "' AS DATE  FROM(SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                    "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, " +
                    "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                    "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dt2 + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL " +
                    "ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Ontime') AS A " +
                    "UNION SELECT COUNT(*)AS total, '" + dateNow + "' AS DATE  FROM(SELECT EmplID, NAME, ScheduleIn, ScheduleOut, intime, outtime, Sttus FROM " +
                    "(SELECT a.EmplID, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut," +
                    "DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus " +
                    "FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' AND a.date = '" + dateNow + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL AND a.intime IS NOT NULL " +
                    "ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Ontime') AS A";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        ontime3 = dt.Rows[0]["total"].ToString();
                        ontime2 = dt.Rows[1]["total"].ToString();
                        ontime1 = dt.Rows[2]["total"].ToString();
                    }
                }
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

        private void LoadDataOverBreak3Day()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT COUNT(*)AS total, '" + dt3 + "' AS DATE FROM (SELECT b.badgeId, b.name, " +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dt3 + "' AND b.dept = '" + dept + "' " +
                    "GROUP BY b.badgeId, b.name) AS a WHERE totalbreak > 90 UNION " +
                    "SELECT COUNT(*)AS total, '" + dt2 + "' AS DATE FROM(SELECT b.badgeId, b.name, " +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dt2 + "' AND b.dept = '" + dept + "' " +
                    "GROUP BY b.badgeId, b.name) AS a WHERE totalbreak > 90 UNION SELECT COUNT(*)AS total, '" + dateNow + "' AS DATE FROM(SELECT b.badgeId, b.name, " +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dateNow + "' AND b.dept = '" + dept + "'" +
                    " GROUP BY b.badgeId, b.name) AS a WHERE totalbreak > 90";
                    
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        over3 = dt.Rows[0]["total"].ToString();
                        over2 = dt.Rows[1]["total"].ToString();
                        over1 = dt.Rows[2]["total"].ToString();
                    }
                }
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

        private void LoadDataBreakOK3Day()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT COUNT(*)AS total, '" + dt3 + "' AS DATE FROM (SELECT b.badgeId, b.name, " +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dt3 + "' AND b.dept = '" + dept + "' " +
                    "GROUP BY b.badgeId, b.name) AS a WHERE totalbreak <= 90 UNION " +
                    "SELECT COUNT(*)AS total, '" + dt2 + "' AS DATE FROM(SELECT b.badgeId, b.name, " +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dt2 + "' AND b.dept = '" + dept + "' " +
                    "GROUP BY b.badgeId, b.name) AS a WHERE totalbreak <= 90 UNION SELECT COUNT(*)AS total, '" + dateNow + "' AS DATE FROM(SELECT b.badgeId, b.name, " +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dateNow + "' AND b.dept = '" + dept + "'" +
                    " GROUP BY b.badgeId, b.name) AS a WHERE totalbreak <= 90";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        break3 = dt.Rows[0]["total"].ToString();
                        break2 = dt.Rows[1]["total"].ToString();
                        break1 = dt.Rows[2]["total"].ToString();
                    }
                }
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

        private void LoadDataLate()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "(SELECT linecode, NAME, ScheduleIn, ScheduleOut, intime, outtime FROM (SELECT e.linecode, e.name, DATE_FORMAT(a.ScheduleIn, '%H:%i') AS ScheduleIn, " +
                    "DATE_FORMAT(a.ScheduleOut, '%H:%i') AS ScheduleOut, DATE_FORMAT(a.intime, '%H:%i') AS intime, DATE_FORMAT(a.outtime, '%H:%i') AS outtime, " +
                    "IF(a.intime > a.ScheduleIn, 'Late', 'Ontime') AS Sttus FROM tbl_attendance a, tbl_employee e WHERE e.id = a.emplid AND e.dept = '" + dept + "' " +
                    "AND a.date = '" + dateNow + "' AND a.DayType = 'WorkDay' AND a.ScheduleIn IS NOT NULL ORDER BY a.ScheduleIn ASC) AS A WHERE Sttus = 'Late' ORDER BY intime desc)"; 
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewLate.DataSource = dset.Tables[0];
                }
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

        // to display breaktime more than 60
        private void LoadDataBreakTime()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            try
            {
                string query = "SELECT badgeid, NAME, totalbreak FROM (SELECT b.badgeId, b.name," +
                    "SUM(a.duration) AS totalBreak FROM tbl_durationbreak a, tbl_employee b WHERE a.emplid = b.id AND a.date = '" + dateNow + "' " +
                    "GROUP BY b.badgeId, b.name) AS a WHERE totalbreak > 90 ORDER BY totalbreak";

                using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, connectionDB.connection))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewBreak.DataSource = dset.Tables[0];

                    DataGridViewButtonColumn btnDetail = new DataGridViewButtonColumn();
                    dataGridViewBreak.Columns.Add(btnDetail);
                    btnDetail.HeaderText = "";
                    btnDetail.Text = "Detail";
                    btnDetail.Name = "btnDetail";
                    btnDetail.UseColumnTextForButtonValue = true;
                }
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


        //fillChart method  
        private void fillChart()
        {
            LoadDataLate3Day();
            LoadDataOntime3Day();

            //AddXY value in chart1  
            chartAttendance.Series["Ontime"].Points.AddXY("", ontime3);
            chartAttendance.Series["Ontime"].Points.AddXY("", ontime2);
            chartAttendance.Series["Ontime"].Points.AddXY("", ontime1);
            chartAttendance.Series["Late"].Points.AddXY(DateTime.Now.AddDays(-2).ToString("dd-MMM"), late3);
            chartAttendance.Series["Late"].Points.AddXY(DateTime.Now.AddDays(-1).ToString("dd-MMM"), late2);
            chartAttendance.Series["Late"].Points.AddXY(DateTime.Now.ToString("dd-MMM"), late1);
            chartAttendance.Series["Over Break"].Points.AddXY("", over3);
            chartAttendance.Series["Over Break"].Points.AddXY("", over2);
            chartAttendance.Series["Over Break"].Points.AddXY("", over1);

            // show total Label
            chartAttendance.Series["Ontime"].IsValueShownAsLabel = true;
            chartAttendance.Series["Late"].IsValueShownAsLabel = true;
            chartAttendance.Series["Over Break"].IsValueShownAsLabel = true;

            //chart title  
            chartAttendance.Titles.Add("Attendances Summary");         
        }


        private void timer_Tick(object sender, System.EventArgs e)
        {
            help.dateTimeNow(dateTimeNow);
            //dataGridViewLate.FirstDisplayedCell = dataGridViewLate.Rows[dataGridViewLate.Rows.Count - 1].Cells[0];
            
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
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
                MaterialSnackBar SnackBarMessage = new MaterialSnackBar(result.ToString(), 750);
                SnackBarMessage.Show(this);
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string message = "Are you sure you want to logout?";
            string title = "Confirm Logout";
            MaterialDialog materialDialog = new MaterialDialog(this, title, message, "OK", true, "Cancel");
            DialogResult result = materialDialog.ShowDialog(this);
            if (result.ToString() == "OK")
            {
                this.Hide();
                Login login = new Login();
                login.Show();
            }
            else
            {
                MaterialSnackBar SnackBarMessage = new MaterialSnackBar(result.ToString(), 750);
                SnackBarMessage.Show(this);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.usernameLbl.Text = toolStripUsername.Text;
            changePassword.ShowDialog();
        }

        private void userToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Userlist userlist = new Userlist();
            userlist.toolStripUsername.Text = toolStripUsername.Text;
            userlist.userdetail.Text = userdetail.Text;
            userlist.Show();
            this.Hide();
        }

        private void userRoleToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            UserRole userRole = new UserRole();
            userRole.toolStripUsername.Text = toolStripUsername.Text;
            userRole.userdetail.Text = userdetail.Text;
            userRole.Show();
            this.Hide();
        }

        private void departmentToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Departmentlist departmentlist = new Departmentlist();
            departmentlist.toolStripUsername.Text = toolStripUsername.Text;
            departmentlist.userdetail.Text = userdetail.Text;
            departmentlist.Show();
            this.Hide();
        }

        private void employeeLevelToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            EmployeeLevellist employeeLevellist = new EmployeeLevellist();
            employeeLevellist.toolStripUsername.Text = toolStripUsername.Text;
            employeeLevellist.userdetail.Text = userdetail.Text;
            employeeLevellist.Show();
            this.Hide();
        }

        private void genderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Genderlist genderlist = new Genderlist();
            genderlist.toolStripUsername.Text = toolStripUsername.Text;
            genderlist.userdetail.Text = userdetail.Text;
            genderlist.Show();
            this.Hide();
        }

        private void lineCodeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LineCodelist lineCodelist = new LineCodelist();
            lineCodelist.toolStripUsername.Text = toolStripUsername.Text;
            lineCodelist.userdetail.Text = userdetail.Text;
            lineCodelist.Show();
            this.Hide();
        }

        private void shiftToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Shiftlist shiftlist = new Shiftlist();
            shiftlist.toolStripUsername.Text = toolStripUsername.Text;
            shiftlist.userdetail.Text = userdetail.Text;
            shiftlist.Show();
            this.Hide();
        }

        private void deviceToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Devicelist devicelist = new Devicelist();
            devicelist.toolStripUsername.Text = toolStripUsername.Text;
            devicelist.userdetail.Text = userdetail.Text;
            devicelist.Show();
            this.Hide();
        }

        private void scheduleToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Schedule schedule = new Schedule();
            schedule.toolStripUsername.Text = toolStripUsername.Text;
            schedule.userdetail.Text = userdetail.Text;
            schedule.Show();
            this.Hide();
        }

        private void sectionToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Sectionlist sectionlist = new Sectionlist();
            sectionlist.toolStripUsername.Text = toolStripUsername.Text;
            sectionlist.userdetail.Text = userdetail.Text;
            sectionlist.Show();
            this.Hide();
        }

        private void groupToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            EmployeeGrouplist employeeGrouplist = new EmployeeGrouplist();
            employeeGrouplist.toolStripUsername.Text = toolStripUsername.Text;
            employeeGrouplist.userdetail.Text = userdetail.Text;
            employeeGrouplist.Show();
            this.Hide();
        }

        private void listToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Employeelist employeelist = new Employeelist();
            employeelist.toolStripUsername.Text = toolStripUsername.Text;
            employeelist.userdetail.Text = userdetail.Text;
            employeelist.Show();
            this.Hide();
        }

        private void leaveTypeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LeaveTypeList leaveTypeList = new LeaveTypeList();
            leaveTypeList.toolStripUsername.Text = toolStripUsername.Text;
            leaveTypeList.userdetail.Text = userdetail.Text;
            leaveTypeList.Show();
            this.Hide();
        }

        private void listToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            Leavelist leavelist = new Leavelist();
            leavelist.toolStripUsername.Text = toolStripUsername.Text;
            leavelist.userdetail.Text = userdetail.Text;
            leavelist.Show();
            this.Hide();
        }

        private void approvalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LeaveApproval leaveApproval = new LeaveApproval();
            leaveApproval.userdetail.Text = userdetail.Text;
            leaveApproval.ShowDialog();
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
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

        private void leaveRequestCard_Click(object sender, System.EventArgs e)
        {
            LeaveApproval leaveApproval = new LeaveApproval();
            leaveApproval.userdetail.Text = userdetail.Text;
            leaveApproval.ShowDialog();
        }

        private void employeeCard_Click(object sender, System.EventArgs e)
        {
            Employeelist employeelist = new Employeelist();
            employeelist.toolStripUsername.Text = toolStripUsername.Text;
            employeelist.userdetail.Text = userdetail.Text;
            employeelist.Show();
            this.Hide();
        }

        private void employeeShiftBoardToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            EmployeeShiftBoard employeeShiftBoard = new EmployeeShiftBoard();
            employeeShiftBoard.toolStripUsername.Text = toolStripUsername.Text;
            employeeShiftBoard.userdetail.Text = userdetail.Text;
            employeeShiftBoard.Show();
            this.Hide();
        }

        private void publicHolidayToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            PublicHoliday publicHoliday = new PublicHoliday();
            publicHoliday.toolStripUsername.Text = toolStripUsername.Text;
            publicHoliday.userdetail.Text = userdetail.Text;
            publicHoliday.Show();
            this.Hide();
        }

        private void totalOntime_Click(object sender, System.EventArgs e)
        {
            Attendance attendance = new Attendance();
            attendance.toolStripUsername.Text = toolStripUsername.Text;
            attendance.userdetail.Text = userdetail.Text;
            attendance.Show();
            attendance.materialTabControl2.SelectedIndex = 1;
            this.Hide();
        }

        private void totalLate_Click(object sender, System.EventArgs e)
        {
            Attendance attendance = new Attendance();
            attendance.toolStripUsername.Text = toolStripUsername.Text;
            attendance.userdetail.Text = userdetail.Text;
            attendance.Show();
            attendance.materialTabControl2.SelectedIndex = 2;
            this.Hide();
        }

        private void totalAbsent_Click(object sender, System.EventArgs e)
        {
            Attendance attendance = new Attendance();
            attendance.toolStripUsername.Text = toolStripUsername.Text;
            attendance.userdetail.Text = userdetail.Text;
            attendance.Show();
            attendance.materialTabControl2.SelectedIndex = 3;
            this.Hide();
        }

        private void workAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkArealist workArealist = new WorkArealist();
            workArealist.toolStripUsername.Text = toolStripUsername.Text;
            workArealist.userdetail.Text = userdetail.Text;
            workArealist.Show();
            this.Hide();
        }

        private void logEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogEmployee logEmployee = new LogEmployee();
            logEmployee.toolStripUsername.Text = toolStripUsername.Text;
            logEmployee.userdetail.Text = userdetail.Text;
            logEmployee.Show();
            this.Hide();
        }

        private void attendancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Attendance attendance = new Attendance();
            attendance.toolStripUsername.Text = toolStripUsername.Text;
            attendance.userdetail.Text = userdetail.Text;
            attendance.Show();
            this.Hide();
        }

        private void positionImage_Click(object sender, EventArgs e)
        {
            EmployeePosition employeePosition = new EmployeePosition();
            employeePosition.toolStripUsername.Text = toolStripUsername.Text;
            employeePosition.userdetail.Text = userdetail.Text;
            employeePosition.Show();
            this.Hide();
        }

        private void refreshLbl_Click(object sender, EventArgs e)
        {
            refresh();
        }


        private void refresh()
        {
            //sendEmail();

            dateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");

            // remove data in datagridview result
            dataGridViewLate.DataSource = null;
            dataGridViewLate.Refresh();

            while (dataGridViewLate.Columns.Count > 0)
            {
                dataGridViewLate.Columns.RemoveAt(0);
            }

            dataGridViewLate.Update();
            dataGridViewLate.Refresh();

            dataGridViewBreak.DataSource = null;
            dataGridViewBreak.Refresh();

            while (dataGridViewBreak.Columns.Count > 0)
            {
                dataGridViewBreak.Columns.RemoveAt(0);
            }

            dataGridViewBreak.Update();
            dataGridViewBreak.Refresh();

            // remove chart
            chartAttendance.Series["Ontime"].Points.Clear();
            chartAttendance.Series["Late"].Points.Clear();
            chartAttendance.Series["Over Break"].Points.Clear();

            //chart title  
            chartAttendance.Titles.Clear();

            // remove chart
            chartBreak.Series["Ontime"].Points.Clear();
            chartBreak.Series["Over Break"].Points.Clear();

            //chart title  
            chartBreak.Titles.Clear();

            displayData();
            LoadPieChart();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridViewTotalBreak_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewBreak.SelectedCells[0].RowIndex;
            string badgeslctd = dataGridViewBreak.Rows[i].Cells[0].Value.ToString();
            string employeeslctd = dataGridViewBreak.Rows[i].Cells[1].Value.ToString();
            string totalBreak = dataGridViewBreak.Rows[i].Cells[2].Value.ToString();

            if (e.ColumnIndex == 3)
            {

                DetailPosition detailPosition = new DetailPosition();
                detailPosition.tbBadge.Text = badgeslctd;
                detailPosition.tbName.Text = employeeslctd;
                detailPosition.tbDate.Text = dateNow;
                detailPosition.Show();
                detailPosition.tabControl1.SelectedIndex = 1;

                //DetailBreak detailBreak = new DetailBreak();
                //detailBreak.tbBadge.Text = badgeslctd;
                //detailBreak.tbName.Text = employeeslctd;
                //detailBreak.tbTotalBreak.Text = totalBreak;

                //detailBreak.ShowDialog();
            }
        }

        private void dataGridViewTotalBreak_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Badge ID", "Name", "Total Break (minute)" };
            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewBreak.Columns[i].HeaderText = "" + title[i];
            }

            //dataGridViewBreak.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewBreak.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
        }

        private void buttonEmployee_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel9.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel9.ForeColor = System.Drawing.Color.White;

            hideSubMenu();
            showSubMenu(panelEmployeeMenu);
        }

        private void buttonLeave_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel15.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel15.ForeColor = System.Drawing.Color.White;

            hideSubMenu();
            showSubMenu(panelLeave);
        }

        private void buttonDashboard_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel8.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel8.ForeColor = System.Drawing.Color.White;
            hideSubMenu();

        }

        private void buttonSchedule_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel11.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel11.ForeColor = System.Drawing.Color.White;

            hideSubMenu();
            Schedule schedule = new Schedule();
            schedule.toolStripUsername.Text = toolStripUsername.Text;
            schedule.userdetail.Text = userdetail.Text;
            schedule.Show();
            this.Hide();
        }

        private void buttonAttendance_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel12.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel12.ForeColor = System.Drawing.Color.White;

            hideSubMenu();
            Attendance attendance = new Attendance();
            attendance.toolStripUsername.Text = toolStripUsername.Text;
            attendance.userdetail.Text = userdetail.Text;
            attendance.Show();
            this.Hide();
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel10.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel10.ForeColor = System.Drawing.Color.White;

            hideSubMenu();
            LogEmployee logEmployee = new LogEmployee();
            logEmployee.toolStripUsername.Text = toolStripUsername.Text;
            logEmployee.userdetail.Text = userdetail.Text;
            logEmployee.Show();
            this.Hide();
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            defaultColorPanel();
            panel1.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            panel1.ForeColor = System.Drawing.Color.White;

            hideSubMenu();
            Status status = new Status();
            status.toolStripUsername.Text = toolStripUsername.Text;
            status.userdetail.Text = userdetail.Text;
            status.Show();
            this.Hide();
        }

        private void statusTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status status = new Status();
            status.toolStripUsername.Text = toolStripUsername.Text;
            status.userdetail.Text = userdetail.Text;
            status.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void buttonEmployeeList_Click(object sender, EventArgs e)
        {
            Employeelist employeelist = new Employeelist();
            employeelist.toolStripUsername.Text = toolStripUsername.Text;
            employeelist.userdetail.Text = userdetail.Text;
            employeelist.Show();
            this.Hide();
        }

        private void buttonEmployeeGroup_Click(object sender, EventArgs e)
        {
            EmployeeGrouplist employeeGrouplist = new EmployeeGrouplist();
            employeeGrouplist.toolStripUsername.Text = toolStripUsername.Text;
            employeeGrouplist.userdetail.Text = userdetail.Text;
            employeeGrouplist.Show();
            this.Hide();
        }

        private void buttonLeaveList_Click(object sender, EventArgs e)
        {
            Leavelist leavelist = new Leavelist();
            leavelist.toolStripUsername.Text = toolStripUsername.Text;
            leavelist.userdetail.Text = userdetail.Text;
            leavelist.Show();
            this.Hide();
        }

        private void buttonLeaveApproval_Click(object sender, EventArgs e)
        {
            LeaveApproval leaveApproval = new LeaveApproval();
            leaveApproval.userdetail.Text = userdetail.Text;
            leaveApproval.ShowDialog();
        }

        

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void dataGridViewLate_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewLate, e);
        }

        private void dataGridViewBreak_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewBreak, e);
        }

        private void CreateMenu()
        {
            ConnectionDB connectionDB = new ConnectionDB();
            string menu = "SELECT a.roleID, b.parentID, b.nodetext, b.toolStripMenu FROM tbl_userrole a, tbl_menu b " +
                "WHERE a.userid = '" + idUser + "' AND a.roleID = b.NodeID";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(menu, connectionDB.connection))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                // cek jika ada selected menu 
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["roleID"].ToString() == "1")
                        {
                            employeeToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "2")
                        {
                            scheduleToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "3")
                        {
                            attendancesToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "4")
                        {
                            masterDataToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "5")
                        {
                            administrationToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "6")
                        {
                            listToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "7")
                        {
                            groupToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "8")
                        {
                            genderToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "9")
                        {
                            employeeLevelToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "10")
                        {
                            departmentToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "11")
                        {
                            lineCodeToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "12")
                        {
                            sectionToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "13")
                        {
                            shiftToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "14")
                        {
                            deviceToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "15")
                        {
                            userToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "16")
                        {
                            userRoleToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "17")
                        {
                            leaveToolStripMenuItem.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "18")
                        {
                            listToolStripMenuItem1.Visible = true;
                        }
                        if (dt.Rows[i]["roleID"].ToString() == "19")
                        {
                            approvalToolStripMenuItem.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
