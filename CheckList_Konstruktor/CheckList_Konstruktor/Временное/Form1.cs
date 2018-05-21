using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckList_Konstruktor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckList Checks = new CheckList(textBox1.Text, textBox2.Text, new List<Task>(), new Marks(textBox3.Text, textBox4.Text, textBox5.Text));
            Constructor Constr = new Constructor(Checks);
            this.Visible = false;
            Constr.ShowDialog();
            this.Close();
        }
    }
}
