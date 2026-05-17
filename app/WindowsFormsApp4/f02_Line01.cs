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
    public partial class f02_Line01 : Form
    {
        public f02_Line01()
        {
            InitializeComponent();
        }
        public int butt;
        public int c_line;
        public string name;
        //===
        public SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();

        private void f02_Line01_Load(object sender, EventArgs e)
        {
            if (butt == 2)
            {
                textBox1.Text = name;
                this.Text = "Редактирование Авиакомпания";
            }
            else this.Text = "Добавление Авиакомпания";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f02_Line main = this.Owner as f02_Line;
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
                com.CommandText = "insert into Line values (@name)";
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
                com.CommandText = "update Line set name=@name where c_line=@c_line";
                com.Parameters.Add("@c_line", SqlDbType.Int);
                com.Parameters.Add("@name", SqlDbType.VarChar);
                com.Parameters["@c_line"].Value = c_line;
                com.Parameters["@name"].Value = textBox1.Text;
                con.Open(); try { com.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close();
                com.Parameters.Clear();
                main.dgvload();
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
