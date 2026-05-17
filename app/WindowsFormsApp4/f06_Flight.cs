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
    public partial class f06_Flight : Form
    {
        public f06_Flight()
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
            com.CommandText = "select C_Flight, f.C_Period , c1.Name, c2.Name,Wday2,DepTime, s.Name,l.Name,DepPoint,DestPoint,f.C_Ship,f.C_line " +
                "from Flight as f join (select  distinct C_Period, " +
                "	stuff((select ', ' + CAST( " +
                "			CASE Wday" +
                "			WHEN 1 THEN 'Пн' " +
                "			WHEN 2 THEN 'Вт' " +
                "			WHEN 3 THEN 'Ср' " +
                "			WHEN 4 THEN 'Чт' " +
                "			WHEN 5 THEN 'Пт' " +
                "			WHEN 6 THEN 'Сб' " +
                "			WHEN 7 THEN 'Вс' " +
                "			END  AS VARCHAR) " +
                "		from Period " +
                "		where C_Period=t.C_Period " +
                "		order by Wday " +
                "		for XML path('') " +
                "		) " +
                "	,1,1,'' " +
                "	) " +
                "Wday2 " +
                "from Period t) t on f.C_Period=t.C_Period " +
                "join City as c1 on f.DepPoint=c1.C_City join City as c2 on f.DestPoint=c2.C_City " +
                "join Ship as s on f.C_Ship=s.C_Ship " +
                "join Line as l on f.C_line=l.C_line ";
            try { con.Open(); }
            catch
            {
                MessageBox.Show("Нет соединения");
                Close(); return;
            }
            try
            {
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read()) dataGridView1.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(),
                    rd[3].ToString(), rd[4].ToString(), string.Format("{0:t}", DateTime.Parse(rd[5].ToString())), rd[6].ToString(), rd[7].ToString(),
                    rd[8].ToString(), rd[9].ToString(), rd[10].ToString(), rd[11].ToString()
                    );
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            con.Close();


        }

            private void f06_Flight_Load(object sender, EventArgs e)
        {
            this.Text = "Расписание";
            dgvload();
        }

        private void button1_Click(object sender, EventArgs e) //Добавить
        {
            f06_Flight01 f2 = new f06_Flight01();
            f2.Owner = this;
            //f2.butt = 1;
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
