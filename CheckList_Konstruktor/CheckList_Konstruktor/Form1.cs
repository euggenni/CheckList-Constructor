﻿using System;
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
            int classNum = 0;
            try
            {
                classNum = Convert.ToInt32(textBox6.Text);
            }
            catch (Exception){}
            int time = 0;
            try
            {
                time = Convert.ToInt32(textBox7.Text);
            }
            catch (Exception){}
            Title title = new Title(textBox2.Text, textBox1.Text, classNum, richTextBox1.Text, richTextBox2.Text, time, textBox8.Text, richTextBox6.Text, richTextBox3.Text, richTextBox5.Text, richTextBox4.Text);
            DataChekList.Check = new CheckList(title, new List<Task>(), new Marks(textBox3.Text, textBox4.Text, textBox5.Text), false);
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
