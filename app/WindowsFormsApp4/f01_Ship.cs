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
    public partial class f01_Ship : Form
    {
        public f01_Ship()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        string con1, com1;
        SqlDataAdapter da;

        public void var1()
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
            com.CommandText = "select * from Ship";
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
        }

        public bool canedit()
        {
            DataTable Flight = new DataTable();
            com1 = "select distinct c_ship from Flight"; da = new SqlDataAdapter(com1, con); da.Fill(Flight);
            //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

            int c_ship = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            if (Flight.Select("c_ship = " + c_ship.ToString()).Count() != 0)
                return false;
            else return true;
        }

            private void f01_Ship_Load(object sender, EventArgs e)
        {
            this.Text = "Самолёты";
            var1();
        }

        private void button1_Click(object sender, EventArgs e) //Добавить
        {
            f01_Ship01 f2 = new f01_Ship01();
            f2.Owner = this;
            f2.butt = 1;
            f2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e) //РЕДАКТИРОВАТЬ
        {
            if (!canedit())
            {
                MessageBox.Show("Редактирование невозможно. Самолёт есть в расписании.");
                return;
            }
            f01_Ship01 f2 = new f01_Ship01();
            f2.Owner = this;
            f2.c_ship = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            f2.capacity = int.Parse(dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.butt = 2;
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!canedit())
            {
                MessageBox.Show("Удаление невозможно. Самолёт есть в расписании.");
                return;
            }
            DialogResult res = MessageBox.Show("Удалить?", "Внимание", MessageBoxButtons.YesNo);
            if (DialogResult.Yes == res)
            {
                com.CommandText = "delete from Ship where c_ship=@c_ship";
                com.Parameters.Add("@c_ship", SqlDbType.Int);
                com.Parameters["@c_ship"].Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                try { con.Open(); } catch { MessageBox.Show("Нет соединения"); Close(); return; }
                try { com.ExecuteNonQuery(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close(); dataGridView1.Rows.Remove(dataGridView1.CurrentRow); com.Parameters.Clear();
                if (dataGridView1.Rows.Count == 0)
                {
                    //button2.Enabled = false; button3.Enabled = false;
                }

            }

        }
    }
}
