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
    public partial class AddPlatoon : Form
    {
        public AddPlatoon()
        {
            InitializeComponent();
        }

        private void AddPlatoon_Load(object sender, EventArgs e)
        {
            bindingSource1.DataSource = DataChekList.Platoons.PlatList;
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "PlatNum";
            column.HeaderText = "Взвод";
            column.Name = "Взвод";
            column.Width = 320;
            dataGridView1.Columns.Add(column);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataChekList.Platoons.SavePlatList(DataChekList.Encrypt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
