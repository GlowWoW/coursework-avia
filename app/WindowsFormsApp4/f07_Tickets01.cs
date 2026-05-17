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
    public partial class f07_Tickets01 : Form
    {
        public f07_Tickets01()
        {
            InitializeComponent();
        }


        string con1, com1;
        SqlDataAdapter da;
        DataTable dt = new DataTable();
        public int C_Flight;

        public SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();

        private void button1_Click(object sender, EventArgs e)
        {
            f07_Tickets main = this.Owner as f07_Tickets;
            //if (textBox1.Text.Trim() == "")
            // {
            //     MessageBox.Show("Не все поля заполнены!"); textBox1.Focus(); return;
            // }

            //label1.Text = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");
           
            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;

            com.Parameters.Clear();
            com.CommandText = "insert into Tickets (C_Flight, DepDate)  values (@C_Flight,@DepDate)";
            com.Parameters.Add("@C_Flight", SqlDbType.Int);
            com.Parameters.Add("@DepDate", SqlDbType.Date);
            com.Parameters["@C_Flight"].Value = comboBox1.SelectedValue;
            com.Parameters["@DepDate"].Value = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");

            try { con.Open(); }
            catch { MessageBox.Show("Нет соединения"); Close(); return; }
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close(); con.Close(); return;
            }
            con.Close();
            com.Parameters.Clear();
            main.dgvload();
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void f07_Tickets01_Load(object sender, EventArgs e)
        {
            this.Text = "Добавление Билеты (дата вылета)";

            con1 = Properties.Settings.Default.con;
            com1 = "select distinct C_Flight from Flight";
            da = new SqlDataAdapter(com1, con1);
            dt.Rows.Clear();
            da.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "C_Flight";
            comboBox1.ValueMember = "C_Flight";
            comboBox1.SelectedValue = C_Flight;
            dateTimePicker1.MinDate = DateTime.Now;
        }
    }
}
