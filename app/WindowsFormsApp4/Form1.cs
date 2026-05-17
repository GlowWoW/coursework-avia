using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void самолётыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f01_Ship f2 = new f01_Ship();
            f2.ShowDialog();
        }

        private void авиакомпанииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f02_Line f2 = new f02_Line();
            f2.ShowDialog();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f03_Client f2 = new f03_Client();
            f2.C_Flight = 0;
            f2.ShowDialog();
        }

        private void заказToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f08_Сonsignment f2 = new f08_Сonsignment();
            f2.ShowDialog();
        }

        private void городаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f04_City f2 = new f04_City();
            f2.ShowDialog();
        }

        private void периодичностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f05_Period f2 = new f05_Period();
            f2.ShowDialog();
        }

        private void расписаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f06_Flight f2 = new f06_Flight();
            f2.ShowDialog();
        }

        private void числоБилетовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f07_Tickets f2 = new f07_Tickets();
            f2.ShowDialog();
        }

        private void отчётToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f09_Report f2 = new f09_Report();
            f2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Главная форма";
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point((this.Width / 2) - (pictureBox1.Width / 2), (this.Height / 2) - (pictureBox1.Height / 2));
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
        }
    }
}
