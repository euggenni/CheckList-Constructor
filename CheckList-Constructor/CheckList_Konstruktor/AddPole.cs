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
    public partial class AddPole : Form
    {
        TableLayoutPanel Table;
        public AddPole(TableLayoutPanel  table)
        {
            Table = table;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) //удаление указанной строки
        {
            
            //// не работает
            /*int number = 0;
            try
            {
                number = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception) { }
            if (number != 0 && number <= Table.RowStyles.Count)
            {
                for (int i = 0; i<Table.ColumnCount;i++)
                {
                    if (Table.GetControlFromPosition(i, number)!=null)
                    {
                        Table.Controls.Remove(Table.GetControlFromPosition(i, number));
                    }
                }
                for (int i = number+1; i < Table.RowCount; i++)
                {
                    for (int j = 0; j < Table.ColumnCount; j++)
                    {
                        var control = Table.GetControlFromPosition(j, i);
                        if (control != null)
                        {
                            Table.Controls.Add(control, j, i-1);
                        }
                    }
                }
                Table.RowStyles.RemoveAt(Table.RowCount - 1);
                Table.RowCount--;
                Table.Height-=200;
                Renumbered();
            }//*/
        }

        private void button1_Click(object sender, EventArgs e)//добавление указанной строки 
        {
            
            ///// не работает 
            /*int number = 0;
            try
            {
                number = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception) { }
            if (number != 0)
            {
                Label label = new Label();
                label.Dock = DockStyle.Fill;
                label.Text = "";
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                Table.Controls.Add(label, 0, number);
                Table.RowStyles.Insert(number, new RowStyle(SizeType.Absolute, 50));
                ///////////////////////////////////////////////
                RichTextBox TextBox = new RichTextBox();
                TextBox.Dock = DockStyle.Fill;
                Table.Controls.Add(TextBox, 1, number);
                ///////////////////////////////////////////////
                RichTextBox TextBox2 = new RichTextBox();
                TextBox2.Dock = DockStyle.Fill;
                Table.Controls.Add(TextBox2, 2, number);
                ///////////////////////////////////////////////
                Button button = new Button();
                button.Text = "Добавить изображение";
                button.Width = 100;
                button.Height = 60;
                button.Anchor = AnchorStyles.Top;
                button.Click += AddPictureClicked;
                Table.Controls.Add(button, 3, number);
                ///////////////////////////////////////////////
                if (Table.RowCount > 8)
                {
                    Table.AutoSize = false;
                    Table.Height = 445;
                    Table.Width += 15;
                    Table.AutoScroll = true;
                }
            }
            Renumbered();*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //потом будет
        }
        private void Renumbered() // перезаписывает номера строк
        {
            int n = 1;
            foreach (Control Control in Table.Controls)
            {
                if (Table.GetRow(Control) != 0)
                {
                    if (Table.GetColumn(Control) == 0)
                    {
                        Control.Text = n.ToString();
                        n++;
                    }
                }
            }
        }
        private void AddPictureClicked(object sender, EventArgs e)
        {
            OpenFileDialog Open = new OpenFileDialog();
            Open.Title = "Выберите изображение";
            Open.Filter = "Изображения (*.jpg)|*.jpg";
            if (Open.ShowDialog() != DialogResult.OK) return;
            Button b = (Button)sender;
            b.Text = "";
            b.BackgroundImage = new Bitmap(Open.FileName);
            b.BackgroundImageLayout = ImageLayout.Zoom;
        }
    }
}
