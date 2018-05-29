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
    public partial class RenameSaveTrack : Form
    {
        public RenameSaveTrack()
        {
            InitializeComponent();
        }

        private void RenameSaveTrack_Load(object sender, EventArgs e)
        {
            textBox1.Text = DataChekList.SaveTrack;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Track = new FolderBrowserDialog();
            if (Track.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataChekList.SaveTrack = Track.SelectedPath;
                textBox1.Text = DataChekList.SaveTrack;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataChekList.SaveTrack = textBox1.Text;
            DataChekList.SaveSaveTrack(DataChekList.Encrypt);
            DataChekList.Cource = Subjects.LoadSubList(DataChekList.Encrypt);
            DataChekList.Platoons = Platoons.LoadPlatList(DataChekList.Encrypt);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
