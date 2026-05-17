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
    public partial class f03_Client01 : Form
    {
        public f03_Client01()
        {
            InitializeComponent();
        }

        public int butt,c_client;
        public string passport, fam, im, otch;
        public DateTime dr;

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();


        private void button1_Click(object sender, EventArgs e)
        {
            f03_Client main = this.Owner as f03_Client;
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || maskedTextBox1.Text.Trim() == "" ||  dateTimePicker1.Text.Trim() == "")
            {
                MessageBox.Show("Не все поля заполнены!"); textBox1.Focus(); return;
            }

            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;
            if (butt == 1)
            {
                com.Parameters.Clear();
                com.CommandText = "insert into Client values (@passport,@fam, @im, @otch, @dr)";
                com.Parameters.Add("@passport", SqlDbType.VarChar);
                com.Parameters.Add("@fam", SqlDbType.VarChar);
                com.Parameters.Add("@im", SqlDbType.VarChar);
                com.Parameters.Add("@otch", SqlDbType.VarChar);
                com.Parameters.Add("@dr", SqlDbType.DateTime);
                com.Parameters["@passport"].Value = maskedTextBox1.Text;
                com.Parameters["@fam"].Value = textBox1.Text;
                com.Parameters["@im"].Value = textBox2.Text;
                com.Parameters["@otch"].Value = textBox3.Text;
                com.Parameters["@dr"].Value = dateTimePicker1.Text;
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
                main.upd1();
                Close();
            }
            if (butt == 2)
            {

                com.Parameters.Clear();
                com.CommandText = "update Client set passport=@passport, fam=@fam, im=@im, otch=@otch, dr=@dr where c_client=@c_client";
                com.Parameters.Add("@c_client", SqlDbType.Int);
                com.Parameters.Add("@passport", SqlDbType.VarChar);
                com.Parameters.Add("@fam", SqlDbType.VarChar);
                com.Parameters.Add("@im", SqlDbType.VarChar);
                com.Parameters.Add("@otch", SqlDbType.VarChar);
                com.Parameters.Add("@dr", SqlDbType.DateTime);
                com.Parameters["@c_client"].Value = c_client;
                com.Parameters["@passport"].Value = maskedTextBox1.Text;
                com.Parameters["@fam"].Value = textBox1.Text;
                com.Parameters["@im"].Value = textBox2.Text;
                com.Parameters["@otch"].Value = textBox3.Text;
                com.Parameters["@dr"].Value = dateTimePicker1.Text;
                

                con.Open(); try { com.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close();
                com.Parameters.Clear();
                main.upd1();
                Close();
            }
        }



        private void f03_Client01_Load(object sender, EventArgs e)
        {
            if (butt == 2)
            {
                maskedTextBox1.Text = passport;
                textBox1.Text = fam;
                textBox2.Text = im;
                textBox3.Text = otch;
                dateTimePicker1.Text = dr.ToString();
                this.Text = "Редактирование Клиенты";
            }
            else this.Text = "Добавление Клиенты";
        }
    }
}
