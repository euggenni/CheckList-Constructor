using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckList_Konstruktor
{
    public partial class WordProgress : Form
    {
        public WordProgress()
        {
            InitializeComponent();
        }

        private void WordProgress_Load(object sender, EventArgs e)
        {

        }

        public void ProgressBarForm(int max)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
        }

        public void ProgressBarInc()
        {
            progressBar1.PerformStep();
        }
    }
}
