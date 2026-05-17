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
    public partial class f01_Ship01 : Form
    {
        public f01_Ship01()
        {
            InitializeComponent();
        }

        public int butt;
        //===
        public int c_ship;
        public int capacity;
        public string name;
        //===
        public SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();

        private void f01_Ship01_Load(object sender, EventArgs e)
        {
            if (butt == 2)
            {
                textBox1.Text = name;
                textBox2.Text = capacity.ToString();
                this.Text = "Редактирование Самолёт";
            }
            else this.Text = "Добавление Самолёт";
        }

        private void button1_Click(object sender, EventArgs e) //OK
        {
            f01_Ship main = this.Owner as f01_Ship;
            int res;
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Не все поля заполнены!"); textBox1.Focus(); return;
            }
            
            bool isInt = Int32.TryParse(textBox2.Text, out res);
            if (!isInt)
            {
                MessageBox.Show("Во вместимости посторонние символы"); textBox2.Focus(); return;
            }
            if (res<=0)
            {
                MessageBox.Show("Минимальная вместимость-1 человек"); textBox2.Focus(); return;
            }
            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;
            if (butt == 1)
            {
                com.Parameters.Clear();
                com.CommandText = "insert into Ship values (@name,@capacity)";
                com.Parameters.Add("@name", SqlDbType.VarChar);
                com.Parameters.Add("@capacity", SqlDbType.Int);
                com.Parameters["@name"].Value = textBox1.Text;
                com.Parameters["@capacity"].Value = textBox2.Text;
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
                main.var1();
                Close();
            }
            if (butt == 2)
            {

                bool isInt2 = Int32.TryParse(textBox2.Text, out res);
                if (!isInt2)
                {
                    MessageBox.Show("Во вместимости посторонние символы"); textBox2.Focus(); return;
                }
                com.Parameters.Clear();
                com.CommandText = "update Ship set name=@name, capacity=@capacity where c_ship=@c_ship";
                com.Parameters.Add("@c_ship", SqlDbType.Int);
                com.Parameters.Add("@name", SqlDbType.VarChar);
                com.Parameters.Add("@capacity", SqlDbType.Int);
                com.Parameters["@c_ship"].Value = c_ship;
                com.Parameters["@name"].Value = textBox1.Text;
                com.Parameters["@capacity"].Value = textBox2.Text;
                con.Open(); try { com.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close();
                com.Parameters.Clear();
                main.var1();
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
