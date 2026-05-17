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
    public partial class f07_Tickets : Form
    {
        public f07_Tickets()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();

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
            com.CommandText = "select t.C_Flight,DepDate,Tickets_cnt0,c1.Name, c2.Name,DepTime," +
                "s.Name,l.Name,Tickets_cnt from Tickets as t join Flight as f on t.C_Flight=f.C_Flight " +
                "join City as c1 on f.DepPoint = c1.C_City join City as c2 on f.DestPoint = c2.C_City " +
                "join Ship as s on f.C_Ship = s.C_Ship " +
                "join Line as l on f.C_Line = l.C_Line " +
                "where DepDate>(select getdate())";
            try { con.Open(); }
            catch
            {
                MessageBox.Show("Нет соединения");
                Close(); return;
            }
            try
            {
                //Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read()) dataGridView1.Rows.Add(rd[0].ToString(), string.Format("{0:dd/MM/yyyy}", DateTime.Parse(rd[1].ToString())), rd[2].ToString(), rd[3].ToString(),
                    rd[4].ToString(), string.Format("{0:t}", DateTime.Parse(rd[5].ToString())), rd[6].ToString(), rd[7].ToString(), rd[8].ToString());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            con.Close();

        }

            private void f07_Tickets_Load(object sender, EventArgs e)
        {
            this.Text = "Билеты";
            dgvload();
        }

        private void button1_Click(object sender, EventArgs e)//Добавить
        {
            f07_Tickets01 f2 = new f07_Tickets01();
            f2.Owner = this;
            f2.C_Flight = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()); ;
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
