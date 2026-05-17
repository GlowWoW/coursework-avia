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
    public partial class f05_Period01 : Form
    {
        public f05_Period01()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        public int butt;
        public int c_period;
        public int wday;
        //bool delthis=false;
        string con1, com1;
        SqlDataAdapter da;

        private void button1_Click(object sender, EventArgs e)
        {
            f05_Period main = this.Owner as f05_Period;
            if (textBox1.Text.Trim() == "" || comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Не все поля заполнены!"); textBox1.Focus(); return;
            }
            int res;
            bool isInt = Int32.TryParse(textBox1.Text, out res);
            if (!isInt)
            {
                MessageBox.Show("В коде посторонние символы"); textBox1.Focus(); return;
            }
            if ((int.Parse(textBox1.Text))<1){ MessageBox.Show("Код не может быть <=0"); textBox1.Focus(); return; }


            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;
            if (butt == 1)
            {
                DataTable Flight = new DataTable();
                com1 = "select distinct wday from Period where c_period="+ textBox1.Text; da = new SqlDataAdapter(com1, con); da.Fill(Flight);
                //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);

         
                if (Flight.Select("wday = " + getwday().ToString()).Count() != 0) {
                    MessageBox.Show("Такое сочетание код периода + день уже есть.");
                    return;
                }
                com.Parameters.Clear();
                com.CommandText = "insert into Period values (@c_period,@wday)";
                com.Parameters.Add("@c_period", SqlDbType.Int);
                com.Parameters.Add("@wday", SqlDbType.Int);
                com.Parameters["@c_period"].Value = textBox1.Text;
                com.Parameters["@wday"].Value = getwday().ToString();
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
                com.CommandText = "update Period set wday=@wday2 where c_period=@c_period and wday=@wday";
                com.Parameters.Add("@c_period", SqlDbType.Int);
                com.Parameters.Add("@wday", SqlDbType.Int);
                com.Parameters.Add("@wday2", SqlDbType.Int);
                com.Parameters["@c_period"].Value = textBox1.Text;
                com.Parameters["@wday"].Value = wday;
                com.Parameters["@wday2"].Value = getwday().ToString();
                con.Open(); try { com.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close();
                com.Parameters.Clear();
                main.dgvload();
                Close();
            }
        }

        public int getwday() {
            if (comboBox1.SelectedItem.ToString() == "Понедельник") return 1;
            if (comboBox1.SelectedItem.ToString() == "Вторник") return 2;
            if (comboBox1.SelectedItem.ToString() == "Среда") return 3;
            if (comboBox1.SelectedItem.ToString() == "Четверг") return 4;
            if (comboBox1.SelectedItem.ToString() == "Пятница") return 5;
            if (comboBox1.SelectedItem.ToString() == "Суббота") return 6;
            if (comboBox1.SelectedItem.ToString() == "Воскресенье") return 7;
            else return 0;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {       }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {       }

        private void button2_Click(object sender, EventArgs e)
        { Close(); }

        private void f05_Period01_Load(object sender, EventArgs e)
        {
            con.ConnectionString = Properties.Settings.Default.con;
            comboBox1.Items.Add("Понедельник");
            comboBox1.Items.Add("Вторник");
            comboBox1.Items.Add("Среда");
            comboBox1.Items.Add("Четверг");
            comboBox1.Items.Add("Пятница");
            comboBox1.Items.Add("Суббота");
            comboBox1.Items.Add("Воскресенье");
            if (butt == 2)
            {
                textBox1.Text = c_period.ToString();
                textBox1.Enabled = false;
                //comboBox1.SelectedIndex = 2;
                comboBox1.SelectedIndex = wday - 1;
                //comboBox1.Items.RemoveAt(1);
                this.Text = "Редактирование Период";
                DataTable Period = new DataTable();
                com1 = "select wday from Period where c_period=" + c_period; da = new SqlDataAdapter(com1, con); da.Fill(Period);
                //com = "SELECT * FROM postavki"; da = new SqlDataAdapter(com, con); da.Fill(dt);
                //label3.Text = Period.Rows[1][0].ToString();
                int k = 0;
                for (int i = 0; i < Period.Rows.Count; i++)
                {
                    if (wday != (int.Parse(Period.Rows[i][0].ToString())))
                    { comboBox1.Items.RemoveAt((int.Parse(Period.Rows[i][0].ToString())) - k - 1); k++; }
                }
            }
            else { this.Text = "Добавление Период"; }
        }
    }
}
