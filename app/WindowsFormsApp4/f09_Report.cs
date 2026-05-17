using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;


namespace WindowsFormsApp4
{
    public partial class f09_Report : Form
    {
        public f09_Report()
        {
            InitializeComponent();
        }

        string con1, com1;
        SqlDataAdapter da;
        DataTable dt4 = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {        }

        DataView dv2;

        private void button1_Click(object sender, EventArgs e)
        {
            slct();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { comboBox1.Enabled = false; comboBox2.Enabled = false; }
            else { comboBox1.Enabled = true; comboBox2.Enabled = true; }
        }


        public void slct()
        {
            con1 = Properties.Settings.Default.con;
            if (!checkBox1.Checked)
            {
                com1 = "select f.C_Flight,DepPoint,DestPoint,C_Ship,cnt1,cnt2,CAST((cnt1 * 100.0 / cnt2 ) as numeric(6, 2)) as v1 " +
                    "from Flight as f " +
                    "join Tickets  as t on f.C_Flight = t.C_Flight " +
                    "join (select t2.C_Flight as cnt0, SUM(Tickets_cnt) as cnt1, " +
                    "SUM(Tickets_cnt0 + Tickets_cnt) as cnt2 from Tickets as t2 " +
                    "where DepDate >= '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy") + "' and DepDate<='" + Convert.ToDateTime(dateTimePicker2.Value).ToString("dd/MM/yyyy") + "' group by t2.C_Flight ) " +
                    "as dt01 on f.C_Flight = dt01.cnt0 where DepPoint =" + comboBox1.SelectedValue.ToString() + " and DestPoint = " + comboBox2.SelectedValue.ToString() +
                    " group by f.C_Flight,DepTime,DepPoint,DestPoint,C_Ship,cnt1,cnt2";
            }
            else {
                com1 = "select f.C_Flight,DepPoint,DestPoint,C_Ship,cnt1,cnt2,CAST((cnt1 * 100.0 / cnt2 ) as numeric(6, 2)) as v1 " +
                "from Flight as f " +
                "join Tickets  as t on f.C_Flight = t.C_Flight " +
                "join (select t2.C_Flight as cnt0, SUM(Tickets_cnt) as cnt1, " +
                "SUM(Tickets_cnt0 + Tickets_cnt) as cnt2 from Tickets as t2 " +
                "where DepDate >= '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy") + "' and DepDate<='" + Convert.ToDateTime(dateTimePicker2.Value).ToString("dd/MM/yyyy") + "' group by t2.C_Flight ) " +
                "as dt01 on f.C_Flight = dt01.cnt0 " +
                " group by f.C_Flight,DepTime,DepPoint,DestPoint,C_Ship,cnt1,cnt2";
            }
            //com1 = "select * from Flight";
            da = new SqlDataAdapter(com1, con1);
            dt4.Rows.Clear();
            /*
            label5.Text = comboBox1.SelectedValue.ToString();
            MessageBox.Show("a");
            label5.Text = comboBox2.SelectedValue.ToString();
            MessageBox.Show("a");
            label5.Text = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");
            MessageBox.Show("a");
            label5.Text = Convert.ToDateTime(dateTimePicker2.Value).ToString("dd/MM/yyyy");
            MessageBox.Show("a");*/
            
            da.Fill(dt4);

            //dv2 = new DataView(dt4);

            if (dt4.Rows.Count > 0)
            {
                label6.Text = "";
                //TextBox.Text = dataGridView1.RowCount.ToString();
                dv2 = new DataView(dt4);
                dataGridView1.DataSource = dv2;

                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;

                dataGridView1.Columns[0].HeaderText = "Номер рейса";
                dataGridView1.Columns[4].HeaderText = "Пассажиров";
                dataGridView1.Columns[5].HeaderText = "Мест всего";
                dataGridView1.Columns[6].HeaderText = "% заполняемости рейса";
                button2.Visible = true;
            }
            else { label6.Text = "За выбранный промежуток не было рейсов"; button2.Visible = false; }

            //canfly3();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileTest = Application.StartupPath.ToString() + "\\test5.xlsx";
            string fileTest2 = Application.StartupPath.ToString() + "\\test7.xlsx";

            Excel.Application oApp;
            Excel.Workbook oBook;
            Excel.Worksheet oSheet;

            oApp = new Excel.Application();
            
            oBook = oApp.Workbooks.Open(fileTest);
            oApp.DisplayAlerts = false;
            oBook.SaveAs(fileTest2);
            oApp.DisplayAlerts = true;
            oBook.Close();
            oBook = oApp.Workbooks.Open(fileTest2);
            //oSheet = (Excel.Worksheet)oBook.Worksheets.get_Item(1);
            oSheet = oBook.Worksheets[1];

            if (checkBox1.Checked) { oSheet.Cells[3, 4].Value2 = ""; oSheet.Cells[3, 1].Value2 = "Все направления"; }
            else { oSheet.Cells[3, 4].Value2 = comboBox2.Text; oSheet.Cells[3, 1].Value2 = comboBox1.Text; }

            //Даты
            oSheet.Cells[6, 1].Value2 = (dateTimePicker1.Value).ToString("dd/MM/yyyy"); oSheet.Cells[6, 4].Value2 = (dateTimePicker2.Value).ToString("dd/MM/yyyy");

            //string d = "D" + dataGridView1.RowCount.ToString();
            Excel.Range rng = oSheet.Range["A9","D"+ (dataGridView1.RowCount+8).ToString()];
            // Выделяем границы у этой ячейки
            Excel.Borders border = rng.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;//xlLineStyleNone;

            for (int i = 9; i < dataGridView1.RowCount+9; i++) {

                oSheet.Cells[i, 1].Value2 = dataGridView1[0, i-9].Value.ToString();
                oSheet.Cells[i, 2].Value2 = dataGridView1[4, i - 9].Value.ToString();
                oSheet.Cells[i, 3].Value2 = dataGridView1[5, i - 9].Value.ToString();
                oSheet.Cells[i, 4].Value2 = dataGridView1[6, i - 9].Value.ToString();
            }

            oBook.Save(); 
            oBook.Close();
            oBook = oApp.Workbooks.Open(fileTest2);
            oApp.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void f09_Report_Load(object sender, EventArgs e)
        {
            this.Text = "Отчёт";

            con1 = Properties.Settings.Default.con;
            com1 = "select * from City order by Name";
            da = new SqlDataAdapter(com1, con1);
            dt.Rows.Clear();
            dt1.Rows.Clear();
            da.Fill(dt);
            da.Fill(dt1);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "C_City";
            comboBox2.DataSource = dt1;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "C_City";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
        }
    }
}
