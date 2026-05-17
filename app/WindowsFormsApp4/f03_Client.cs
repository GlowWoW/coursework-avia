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
    public partial class f03_Client : Form
    {
        public f03_Client()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        public int C_Flight;
        public string DepDate;
        public string DepPointName,DestPointName, ShipName, LineName;
        public DateTime DepTime;
        string con1, com1;
        SqlDataAdapter da;

        public void upd1()
        {
            dataGridView1.Rows.Clear();
            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;

            try
            {
                con.Open();
                //max = int.Parse(com.ExecuteScalar().ToString());
            }
            catch { MessageBox.Show("Нет соединения"); Close(); return; }
            con.Close();

            com.CommandType = CommandType.Text;
            //com.CommandText = "select * from Postavki";
            com.CommandText = "select * from Client";
            try { con.Open(); }
            catch
            {
                MessageBox.Show("Нет соединения");
                Close(); return;
            }
            try
            {
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read()) dataGridView1.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(), rd[3].ToString(), rd[4].ToString(), Convert.ToDateTime(rd[5]).ToString("dd/MM/yyyy"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            con.Close();
        }


        private void f03_Client_Load(object sender, EventArgs e)
        {
            if (C_Flight == 0) { this.Text = "Клиенты"; button5.Visible = false; }
            else { this.Text = "Выбор клиента";
                //button1.Visible = false; button2.Visible = false; button3.Visible = false; button4.Visible = false;
            }

            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;

            try
            {
                con.Open();
                //max = int.Parse(com.ExecuteScalar().ToString());
            }
            catch { MessageBox.Show("Нет соединения"); Close(); return; }
            con.Close();

            com.CommandType = CommandType.Text;
            //com.CommandText = "select * from Postavki";
            com.CommandText = "select * from Client";
            try { con.Open(); }
            catch
            {
                MessageBox.Show("Нет соединения");
                Close(); return;
            }
            try
            {
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read()) dataGridView1.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(), rd[3].ToString(), rd[4].ToString(), Convert.ToDateTime(rd[5]).ToString("dd/MM/yyyy"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            con.Close();
        }

        private void excfnc() {

            string fileTest = Application.StartupPath.ToString() + "\\test4.xlsx";
                       
            Excel.Application oApp;
            Excel.Workbook oBook;
            Excel.Worksheet oSheet;

            oApp = new Excel.Application();
            oBook = oApp.Workbooks.Open(fileTest);
            //oSheet = (Excel.Worksheet)oBook.Worksheets.get_Item(1);
            oSheet = oBook.Worksheets[1];

            oSheet.Cells[3, 1].Value2 = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            oSheet.Cells[3, 2].Value2 = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
            oSheet.Cells[3, 3].Value2 = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
            //Откуда
            oSheet.Cells[6, 1].Value2 = DepPointName;
            //Куда
            oSheet.Cells[9, 1].Value2 = DestPointName;
            //Авиакомпания
            oSheet.Cells[12, 1].Value2 = LineName;
                       

            //Рейс
            oSheet.Cells[3, 4].Value2 = C_Flight.ToString();
            //Дата
            oSheet.Cells[6, 4].Value2 =  Convert.ToDateTime(DepDate).ToString("dd/MM/yyyy");
            //Время
            oSheet.Cells[9, 4].Value2 = string.Format("{0:t}", DepTime);
            //Самолёт
            oSheet.Cells[12, 4].Value2 = ShipName;

            oBook.Save();
            oBook.Close();
            oBook = oApp.Workbooks.Open(fileTest);
            oApp.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Visible = dataGridView1[2, i].Value.ToString().Contains(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            com.Parameters.Clear();
                com.CommandText = "insert into Сonsignment values (@C_Flight,@DepDate, @C_Client)";
                //com.Parameters.Add("@cod_post", SqlDbType.Int);
                com.Parameters.Add("@C_Flight", SqlDbType.Int);
                com.Parameters.Add("@DepDate", SqlDbType.Date);
                com.Parameters.Add("@C_Client", SqlDbType.Int);
                //com.Parameters["@kod_kl"].Value = max + 1;
                com.Parameters["@C_Flight"].Value = C_Flight;//???????????
                com.Parameters["@DepDate"].Value = DepDate.ToString();
                com.Parameters["@C_Client"].Value = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
                try { con.Open(); }
                catch { MessageBox.Show("Нет соединения"); Close(); return; }
                try
                {
                    com.ExecuteNonQuery(); //ed = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); //ed = false;
                    Close(); con.Close(); return;
                }
                con.Close();
                com.Parameters.Clear();
            excfnc();
            f08_Сonsignment main = this.Owner as f08_Сonsignment;
            main.upd3();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f03_Client01 f2 = new f03_Client01();
            f2.Owner = this;
            f2.butt = 1;
            f2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f03_Client01 f2 = new f03_Client01();
            f2.Owner = this;
            f2.c_client = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.passport = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            f2.fam = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            f2.im = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            f2.otch = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            f2.dr = DateTime.Parse((dataGridView1.CurrentRow.Cells[5].Value.ToString()));
            f2.butt = 2;
            f2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e) //Удалить
        {
            DataTable Сonsignment = new DataTable();
            com1 = "select distinct c_client from Сonsignment"; da = new SqlDataAdapter(com1, con); da.Fill(Сonsignment);
            //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

            int c_client = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            if (Сonsignment.Select("c_client = " + c_client.ToString()).Count() != 0)
            {
                MessageBox.Show("Удаление не возможно. Клиент есть в Заказах.");
                return;
            }
            DialogResult res = MessageBox.Show("Удалить?", "Внимание", MessageBoxButtons.YesNo);
            if (DialogResult.Yes == res)
            {
                com.CommandText = "delete from Client where c_client=@c_client";
                com.Parameters.Add("@c_client", SqlDbType.Int);
                com.Parameters["@c_client"].Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                try { con.Open(); } catch { MessageBox.Show("Нет соединения"); Close(); return; }
                try { com.ExecuteNonQuery(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close(); dataGridView1.Rows.Remove(dataGridView1.CurrentRow); com.Parameters.Clear();
            }
        }
    }
}
