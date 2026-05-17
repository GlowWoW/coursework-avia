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
    public partial class f02_Line : Form
    {
        public f02_Line()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        string con1, com1;
        SqlDataAdapter da;

        private void f02_Line_Load(object sender, EventArgs e)
        {
            this.Text = "Авиакомпании";
            dgvload();
        }

        public void dgvload() {
            dataGridView1.Rows.Clear();

            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;
            try
            {
                con.Open();
            }
            catch { MessageBox.Show("Нет соединения"); Close(); return; }
            con.Close();

            com.CommandType = CommandType.Text;
            //com.CommandText = "select * from Postavki";
            com.CommandText = "select * from Line";
            try { con.Open(); }
            catch
            {MessageBox.Show("Нет соединения");Close(); return;}
            try
            {
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read()) dataGridView1.Rows.Add(rd[0].ToString(), rd[1].ToString());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            con.Close();
        }

        public bool canedit()
        {
            DataTable Flight = new DataTable();
            com1 = "select distinct c_line from Flight"; da = new SqlDataAdapter(com1, con); da.Fill(Flight);
            //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

            int c_line = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            if (Flight.Select("c_line = " + c_line.ToString()).Count() != 0)
                return false;
            else return true;
        }

        private void button1_Click(object sender, EventArgs e) //Добавить
        {
            f02_Line01 f2 = new f02_Line01();
            f2.Owner = this;
            f2.butt = 1;
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {Close();}

        private void button3_Click(object sender, EventArgs e) //Удалить
        {
            if (!canedit())
            {
                MessageBox.Show("Удаление невозможно. Авиакомпания есть в расписании.");
                return;
            }
            DialogResult res = MessageBox.Show("Удалить?", "Внимание", MessageBoxButtons.YesNo);
            if (DialogResult.Yes == res)
            {
                com.CommandText = "delete from Line where c_line=@c_line";
                com.Parameters.Add("@c_line", SqlDbType.Int);
                com.Parameters["@c_line"].Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                try { con.Open(); } catch { MessageBox.Show("Нет соединения"); Close(); return; }
                try { com.ExecuteNonQuery(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close(); dataGridView1.Rows.Remove(dataGridView1.CurrentRow); com.Parameters.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)//РЕДАКТИРОВАТЬ
        {
            if (!canedit())
            {
                MessageBox.Show("Редактирование невозможно. Авиакомпания есть в расписании.");
                return;
            }
            f02_Line01 f2 = new f02_Line01();
            f2.Owner = this;
            f2.c_line = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            f2.butt = 2;
            f2.ShowDialog();
        }
    }
}
