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
    public partial class AddCources : Form
    {
        public AddCources()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataChekList.Cource.SaveSubList();
            this.Close();
        }

        private void AddCources_Load(object sender, EventArgs e)
        {
            bindingSource1.DataSource = DataChekList.Cource.SubList;
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Name";
            column.HeaderText = "Предмет";
            column.Name = "Предмет";
            column.Width = 300;
            dataGridView1.Columns.Add(column);
        }
    }
}
