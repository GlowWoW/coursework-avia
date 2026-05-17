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
    public partial class f06_Flight01 : Form
    {
        public f06_Flight01()
        {
            InitializeComponent();
        }

        public SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        string con1, com1;
        SqlDataAdapter da, da1, da2;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        DataTable dt4 = new DataTable();

        private void f06_Flight01_Load(object sender, EventArgs e)
        {

            this.Text = "Добавление Расписание";

            con1 = Properties.Settings.Default.con;
            com1 = "select * from City order by Name";
            da = new SqlDataAdapter(com1, con1);
            dt.Rows.Clear();
            dt1.Rows.Clear();
            da.Fill(dt);
            da.Fill(dt1);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "C_City";

            comboBox2.DataSource = dt1;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "C_City";
            //Самолёт
            com1 = "select * from Ship order by Name";
            da = new SqlDataAdapter(com1, con1);
            dt2.Rows.Clear();
            da.Fill(dt2);
            comboBox3.DataSource = dt2;
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "C_ship";
            //Авиакомпания
            com1 = "select * from Line order by Name";
            da = new SqlDataAdapter(com1, con1);
            dt3.Rows.Clear();
            da.Fill(dt3);
            comboBox4.DataSource = dt3;
            comboBox4.DisplayMember = "Name";
            comboBox4.ValueMember = "C_Line";
            //Авиакомпания
            com1 = "  select C_Period, Wday2  from " +
                "  (select  distinct C_Period, stuff " +
                "  ((select ', ' + CAST(CASE Wday " +
                "    WHEN 1 THEN 'Пн' WHEN 2 THEN 'Вт'" +
                "    WHEN 3 THEN 'Ср' WHEN 4 THEN 'Чт'" +
                "    WHEN 5 THEN 'Пт' WHEN 6 THEN 'Сб'" +
                "    WHEN 7 THEN 'Вс' END  AS VARCHAR)" +
                "    from Period where C_Period = t.C_Period" +
                "    order by C_Period for XML path('')),1,1,'') " +
                "	Wday2 from Period t) t ";
            da = new SqlDataAdapter(com1, con1);
            dt4.Rows.Clear();
            da.Fill(dt4);
            comboBox5.DataSource = dt4;
            comboBox5.DisplayMember = "Wday2";
            comboBox5.ValueMember = "C_Period";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //label7.Text = dateTimePicker1.ToString();
            //label7.Text = (TimeSpan.ParseExact(dateTimePicker1.ToString(), "HH:mm", null)).ToString();
            
            f06_Flight main = this.Owner as f06_Flight;
            if (comboBox1.Text.Trim() == comboBox2.Text.Trim())
            {
                MessageBox.Show("Города одинаковые!"); comboBox1.Focus(); return;
            }
            con.ConnectionString = Properties.Settings.Default.con;
            com.Connection = con;
            com.CommandType = CommandType.Text;

            com.Parameters.Clear();
            com.CommandText = "insert into Flight values (@C_Period,@DepTime,@DepPoint,@DestPoint,@C_Ship,@C_Line)";
            com.Parameters.Add("@C_Period", SqlDbType.Int);
            com.Parameters.Add("@DepTime", SqlDbType.Time);
            com.Parameters.Add("@DepPoint", SqlDbType.Int);
            com.Parameters.Add("@DestPoint", SqlDbType.Int);
            com.Parameters.Add("@C_Ship", SqlDbType.Int);
            com.Parameters.Add("@C_Line", SqlDbType.Int);
            com.Parameters["@C_Period"].Value = comboBox5.SelectedValue.ToString();
            //com.Parameters["@DepTime"].Value = TimeSpan.ParseExact(dateTimePicker1.ToString(), "HH:mm",CultureInfo.InvariantCulture).TimeOfDay; 
            //TimeSpan t = TimeSpan.Parse(dateTimePicker1)
            //com.Parameters["@DepTime"].Value = DateTime.ParseExact(s, "HH.mm", CultureInfo.InvariantCulture).TimeOfDay;
            com.Parameters["@DepTime"].Value = TimeSpan.Parse(dateTimePicker1.Text.ToString());
            //com.Parameters["@DepTime"].Value = TimeSpan.ParseExact(dateTimePicker1.Text.ToString(), "HH:mm",null);
            com.Parameters["@DepPoint"].Value = comboBox1.SelectedValue.ToString();
            com.Parameters["@DestPoint"].Value = comboBox2.SelectedValue.ToString();
            com.Parameters["@C_Ship"].Value = comboBox3.SelectedValue.ToString();
            com.Parameters["@C_Line"].Value = comboBox4.SelectedValue.ToString();
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
    }
}
