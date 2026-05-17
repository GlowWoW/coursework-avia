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

namespace WindowsFormsApp4
{
    public partial class f05_Period : Form
    {
        public f05_Period()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        string con1, com1;
        SqlDataAdapter da;


        private void f05_Period_Load(object sender, EventArgs e)
        {
            this.Text = "Периодичность";
            dgvload();
        }


        public void dgvload()
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
            com.CommandText = "select C_Period,Wday," +
                " CASE Wday" +
                "  WHEN 1 THEN 'Понедельник'" +
                "  WHEN 2 THEN 'Вторник'" +
                "  WHEN 3 THEN 'Среда'" +
                "  WHEN 4 THEN 'Четверг'" +
                "  WHEN 5 THEN 'Пятница'" +
                "  WHEN 6 THEN 'Суббота'" +
                "  WHEN 7 THEN 'Воскресенье'" +
                " END from Period";
            try { con.Open(); }
            catch
            {
                MessageBox.Show("Нет соединения");
                Close(); return;
            }
            try
            {
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read()) dataGridView1.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            con.Close();
            dataGridView1.Columns[1].Visible = false;

        }

            public bool canedit()
        {
            DataTable Flight = new DataTable();
            com1 = "select distinct c_period from Flight"; da = new SqlDataAdapter(com1, con); da.Fill(Flight);
            //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

            int c_period = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            if (Flight.Select("c_period = " + c_period.ToString()).Count() != 0)
                return false;
            else return true;
        }

        private void button1_Click(object sender, EventArgs e)//Добавить
        {
            f05_Period01 f2 = new f05_Period01();
            f2.Owner = this;
            f2.butt = 1;
            f2.c_period = 0;// int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            //f2.wday = int.Parse(dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)//РЕДАКТИРОВАТЬ
        {
            if (!canedit())
            {
                MessageBox.Show("Редактирование невозможно. Периодичность есть в расписании.");
                return;
            }
            DataTable Period = new DataTable();
            com1 = "select c_period from Period"; da = new SqlDataAdapter(com1, con); da.Fill(Period);
            //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

            int c_period = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            if (Period.Select("c_period = " + c_period.ToString()).Count() == 7) {
                MessageBox.Show("Данный период-все дни недели. Изменять нечего.");
                return;
            }
            f05_Period01 f2 = new f05_Period01();
            f2.Owner = this;
            f2.c_period = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.wday = int.Parse(dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.butt = 2;
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)//Удалить
        {
            if (!canedit())
            {
                MessageBox.Show("Удаление невозможно. Периодичность есть в расписании.");
                return;
            }
            DialogResult res = MessageBox.Show("Удалить?", "Внимание", MessageBoxButtons.YesNo);
            if (DialogResult.Yes == res)
            {
                com.CommandText = "delete from Period where c_period=@c_period and wday=@wday";
                com.Parameters.Add("@c_period", SqlDbType.Int);
                com.Parameters.Add("@wday", SqlDbType.Int);
                com.Parameters["@c_period"].Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                com.Parameters["@wday"].Value = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                try { con.Open(); } catch { MessageBox.Show("Нет соединения"); Close(); return; }
                try { com.ExecuteNonQuery(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close(); dataGridView1.Rows.Remove(dataGridView1.CurrentRow); com.Parameters.Clear();
            }
        }
    }
}
