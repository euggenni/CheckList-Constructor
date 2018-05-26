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
    public partial class AddPlatoons : Form
    {
        public AddPlatoons()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddPlatoons_Load(object sender, EventArgs e)
        {
            PrintToolStrip();
            if (DataChekList.Platoons.PlatList.Count != 0)
            {
                bindingSource1.DataSource = DataChekList.Platoons.PlatList.Last<Platoon>().Students;
                toolStripComboBox1.SelectedIndex = 0;
            }
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Fio";
            column.HeaderText = "Студент";
            column.Name = "Студент";
            column.Width = 320;
            dataGridView1.Columns.Add(column);
        }

        private void PrintToolStrip() //переносит в список коллекцию взводов
        {
            toolStripComboBox1.Items.Clear();
            foreach (Platoon Plat in DataChekList.Platoons.PlatList)
            {
                toolStripComboBox1.Items.Add(Plat.PlatNum.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddPlatoon Plat = new AddPlatoon();
            Plat.ShowDialog();
            PrintToolStrip();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataChekList.Platoons.SavePlatList("Platoons", false);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindingSource1.DataSource = DataChekList.Platoons.PlatList.ElementAt<Platoon>(toolStripComboBox1.SelectedIndex).Students;
        }
    }
}
