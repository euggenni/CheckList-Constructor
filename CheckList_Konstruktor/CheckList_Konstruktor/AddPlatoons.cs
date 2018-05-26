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
            bindingSource1.DataSource = DataChekList.Platoons;
        }
    }
}
