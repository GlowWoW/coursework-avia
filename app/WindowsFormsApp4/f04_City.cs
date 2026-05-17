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
    public partial class f04_City : Form
    {
        public f04_City()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        string con1, com1;
        SqlDataAdapter da;

        private void f04_City_Load(object sender, EventArgs e)
        {
            this.Text = "Города";
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
            com.CommandText = "select * from City";
            try { con.Open(); }
            catch
            {
                MessageBox.Show("Нет соединения");
                Close(); return;
            }
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
            com1 = "select distinct DepPoint, DestPoint from Flight"; da = new SqlDataAdapter(com1, con); da.Fill(Flight);
            //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

            int c_city = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            if ((Flight.Select("DepPoint = " + c_city.ToString()).Count() != 0) || (Flight.Select("DestPoint = " + c_city.ToString()).Count() != 0))
                return false;
            else return true;
        }

        private void button1_Click(object sender, EventArgs e)//Добавить
        {
            f04_City01 f2 = new f04_City01();
            f2.Owner = this;
            f2.butt = 1;
            f2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e) //Удалить
        {
            if (!canedit())
            {
                MessageBox.Show("Удаление невозможно. Город есть в расписании.");
                return;
            }
            DialogResult res = MessageBox.Show("Удалить?", "Внимание", MessageBoxButtons.YesNo);
            if (DialogResult.Yes == res)
            {
                com.CommandText = "delete from City where c_city=@c_city";
                com.Parameters.Add("@c_city", SqlDbType.Int);
                com.Parameters["@c_city"].Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                try { con.Open(); } catch { MessageBox.Show("Нет соединения"); Close(); return; }
                try { com.ExecuteNonQuery(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close(); dataGridView1.Rows.Remove(dataGridView1.CurrentRow); com.Parameters.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)//РЕДАКТИРОВАТЬ
        {
            if (!canedit())
            {
                MessageBox.Show("Редактирование невозможно. Город есть в расписании.");
                return;
            }
            f04_City01 f2 = new f04_City01();
            f2.Owner = this;
            f2.c_city = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            f2.butt = 2;
            f2.ShowDialog();
        }
    }
}
