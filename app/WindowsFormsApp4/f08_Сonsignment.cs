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
    public partial class f08_Сonsignment : Form
    {
        public f08_Сonsignment()
        {
            InitializeComponent();
        }



        SqlDataAdapter da, da1, da2;
        string con1, com1;
        DataTable dt2 = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable();
        DataView dv;
        DataView dv1;
        DataView dv2;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() != "System.Data.DataRowView")
            {

                if (!canfly2()) { label3.Text = "Для данных городов рейсов нет"; panel1.Visible = false; }
                else {
                    upd2();
                    panel1.Visible = true;
                    if (!canfly3())
                    {
                        label5.Text = "На данную дату рейсов нет. Посмотрите другие";
                        button2.Enabled = false;
                    }
                    else
                    {
                        label5.Text = "";
                        button2.Enabled = true;
                    }
                }
                if (!canfly()) label3.Text = "Города одинаковые";
                if (canfly2() && canfly()) label3.Text = "";

            }
            //label3.Text = comboBox1.SelectedValue.ToString();
            //if (!canfly2()) label3.Text = "Для данных городов рейсов нет";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                if (!canfly2()) { label3.Text = "Для данных городов рейсов нет"; panel1.Visible = false; }
                else
                {
                    upd2();
                    panel1.Visible = true;
                    if (!canfly3())
                    {
                        label5.Text = "На данную дату рейсов нет. Посмотрите другие";
                        button2.Enabled = false;
                    }
                    else
                    {
                        label5.Text = "";
                        button2.Enabled = true;
                    }
                    //label4.Text=canfly3().ToString();

                }
                if (!canfly()) label3.Text = "Города одинаковые";
                if (canfly2() && canfly()) label3.Text = "";

            }
            //label3.Text = comboBox2.SelectedValue.ToString();
            //if (!canfly2()) label3.Text = "Для данных городов рейсов нет";
        }

        public void upd1()
        {
            con1 = Properties.Settings.Default.con;
            dt2.Rows.Clear();
            com1 = "select * from Flight";
            da2 = new SqlDataAdapter(com1, con1);
            da2.Fill(dt2);
            //bsMaster.DataSource = dt2;



            //dataGridView2.DataSource = bsMaster;
            //if (dataGridView2.Rows.Count != 0) { dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[5]; }

        }
        public void upd2()
        {
            con1 = Properties.Settings.Default.con;
            com1 = "select t.C_Flight,DepDate,DepPoint,DestPoint,DepTime  from Tickets as t join Flight as f on t.C_Flight = f.C_Flight where " +
                "DepPoint="+comboBox1.SelectedValue.ToString()+ " and DestPoint="+comboBox2.SelectedValue.ToString();
            da = new SqlDataAdapter(com1, con1);
            dt3.Rows.Clear();
            da.Fill(dt3);

        }

        public void upd3()
        {
            com1 = " select f.C_Flight,C_Period,CONVERT(VARCHAR(5),DepTime,108),DepPoint,DestPoint,f.C_Ship,f.C_line, " +
    " DepDate, s.Name,l.Name,Tickets_cnt0 from Flight as f " +
    " join Tickets as t on f.C_Flight = t.C_Flight " +
    " join Ship as s on f.C_Ship = s.C_Ship " +
    "join Line as l on f.C_line = l.C_line " +
    "  where Tickets_cnt0> 0";
            da = new SqlDataAdapter(com1, con1);
            dt4.Rows.Clear();
            da.Fill(dt4);

            canfly3();
        }

        // public void canfly3()
        // {label4.Text = dateTimePicker1.Value.ToString();}

        public bool canfly3()
        {
            if (dt3.Columns.Count > 0) { 
            dv1 = new DataView(dt3);
            dv1.RowFilter = "DepDate='" + Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy") + "'"; 
            //label4.Text = dateTimePicker1.Value.ToString();
            if (dv1.Count == 0) return false;
            else return true;
            }
            if (dt3.Columns.Count > 0) { return true; }
            else return false;

        }

        public bool canfly4()
        {
            dv2 = new DataView(dt4);
            dv2.RowFilter = "DepPoint=" + comboBox1.SelectedValue.ToString() + " and DestPoint=" + comboBox2.SelectedValue.ToString()+" and DepDate='"+ Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy") + "'";
            //label4.Text = dateTimePicker1.Value.ToString();
            //dataGridView1.DataSource = dv2;
            if (dv2.Count == 0) return false;
            else { dataGridView1.DataSource = dv2;
                panel2.Visible = true;
                //dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;

                dataGridView1.Columns[0].HeaderText = "Номер рейса";
                dataGridView1.Columns[0].Width = 75;
                dataGridView1.Columns[2].HeaderText = "Время отправления";
                dataGridView1.Columns[0].Width = 75;
                dataGridView1.Columns[8].HeaderText = "Самолёт";
                dataGridView1.Columns[9].HeaderText = "Авиакомпания";
                dataGridView1.Columns[10].HeaderText = "Осталось билетов";
                dataGridView1.Columns[10].Width = 75;

                return true; }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!canfly3())
            {
                label5.Text = "На данную дату рейсов нет. Посмотрите другие";
                button2.Enabled = false;
            }
            else
            {
                label5.Text = "";
                button2.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e) //Выбрать этот рейс
        {
            f03_Client f2 = new f03_Client();
            f2.Owner = this;
            f2.C_Flight = int.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.DepTime = DateTime.Parse(dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString());
            f2.DepPointName = comboBox1.Text;
            f2.DestPointName = comboBox2.Text;
            f2.ShipName = dataGridView1[8, dataGridView1.CurrentRow.Index].Value.ToString();
            f2.LineName = dataGridView1[9, dataGridView1.CurrentRow.Index].Value.ToString();
            f2.DepDate = dataGridView1[7, dataGridView1.CurrentRow.Index].Value.ToString(); 

            f2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            canfly4();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        public bool canfly()
        {
            if (comboBox1.SelectedValue.ToString() == comboBox2.SelectedValue.ToString()) return false;
            else return true;
        }

        public bool canfly2()
        {
            dv = new DataView(dt2);
            dv.RowFilter = "DepPoint="+ comboBox1.SelectedValue.ToString()+" and DestPoint="+comboBox2.SelectedValue.ToString();
            if (dv.Count==0) return false;
            else return true;
        }

            private void f08_Сonsignment_Load(object sender, EventArgs e)
        {
            upd1();
            //DateTime end = DateTime.Now;
            dateTimePicker1.MinDate = DateTime.Now.AddDays(1);
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
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            panel2.Visible = false;


            com1 = " select f.C_Flight,C_Period,CONVERT(VARCHAR(5),DepTime,108),DepPoint,DestPoint,f.C_Ship,f.C_line, " +
                " DepDate, s.Name,l.Name,Tickets_cnt0 from Flight as f " +
                " join Tickets as t on f.C_Flight = t.C_Flight " +
                " join Ship as s on f.C_Ship = s.C_Ship " +
                "join Line as l on f.C_line = l.C_line " +
                "  where Tickets_cnt0> 0";
            da = new SqlDataAdapter(com1, con1);
            dt4.Rows.Clear();
            da.Fill(dt4);
            
        }
    }
}
