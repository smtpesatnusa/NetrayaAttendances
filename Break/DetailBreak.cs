using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SMTAttendance
{
    public partial class DetailBreak : Form
    {
        readonly Helper help = new Helper();
        string dateNow = DateTime.Now.ToString("yyyy-MM-dd");

        MySqlConnection myConn;

        public DetailBreak()
        {
            InitializeComponent();
        }

        private void DetailAttendance_Load(object sender, EventArgs e)
        {
            tbDate.Text = dateNow;
            string badge = tbBadge.Text;
            string date = tbDate.Text;
            try
            {
                // detail break
                string detail = "SELECT a.timeOut, a.timeIn, a.duration FROM tbl_durationbreak a, tbl_employee b " +
                "WHERE a.date = '" + date + "' AND b.badgeId = '" + badge + "' AND a.rfidNo = b.rfidNo ORDER BY a.id DESC";
                using (MySqlDataAdapter adpt = new MySqlDataAdapter(detail, myConn))
                {
                    DataSet dset = new DataSet();
                    adpt.Fill(dset);
                    dataGridViewBreakList.DataSource = dset.Tables[0];
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("LoadDataLate; " + ex.Message);
            }
            finally
            {
                myConn.Dispose();
            }
        }
                        
        private void dataGridViewScheduleList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set table title
            string[] title = { "Time Out", "Time In", "Duration (In Minute)" };

            for (int i = 0; i < title.Length; i++)
            {
                dataGridViewBreakList.Columns[i].HeaderText = "" + title[i];
            }

            dataGridViewBreakList.Columns[0].DefaultCellStyle.Format = "HH:mm";
            dataGridViewBreakList.Columns[1].DefaultCellStyle.Format = "HH:mm";
        }


        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewAttendanceList_Paint(object sender, PaintEventArgs e)
        {
            help.norecord_dgv(dataGridViewBreakList, e);
        }
    }
}
