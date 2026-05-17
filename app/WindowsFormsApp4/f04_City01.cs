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
    public partial class f04_City01 : Form
    {
        public f04_City01()
        {
            InitializeComponent();
        }

        public int butt;
        public int c_city;
        public string name;
        //===
        public SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();

        private void button1_Click(object sender, EventArgs e) //ОК
        {
            f04_City main = this.Owner as f04_City;
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Не все поля заполнены!"); textBox1.Focus(); return;
            }
            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;
            if (butt == 1)
            {
                com.Parameters.Clear();
                com.CommandText = "insert into City values (@name)";
                com.Parameters.Add("@name", SqlDbType.VarChar);
                com.Parameters["@name"].Value = textBox1.Text;
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
            if (butt == 2)
            {
                com.Parameters.Clear();
                com.CommandText = "update City set name=@name where c_city=@c_city";
                com.Parameters.Add("@c_city", SqlDbType.Int);
                com.Parameters.Add("@name", SqlDbType.VarChar);
                com.Parameters["@c_city"].Value = c_city;
                com.Parameters["@name"].Value = textBox1.Text;
                con.Open(); try { com.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close();
                com.Parameters.Clear();
                main.dgvload();
                Close();
            }
        }

        private void f04_City01_Load(object sender, EventArgs e)
        {
            if (butt == 2)
            {
                textBox1.Text = name; this.Text = "Редактирование Город";
            }
            else this.Text = "Добавление Город";
        }

        private void button2_Click(object sender, EventArgs e)
        {  Close(); }
    }
}
