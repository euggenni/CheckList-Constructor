using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace CheckList_Konstruktor
{
    public partial class Constructor : Form
    {
        CheckList Checks;
        int n = 1; //нумератор строк таблицы
        public Constructor(CheckList Checks)
        {
            InitializeComponent();
            this.Checks = Checks;
        }

        private void Constructor_Load(object sender, EventArgs e)
        {
            label5.Text = Checks.Name;
            /*CheckList Checker = new CheckList("Гкчп", "Ектенчукс", new List<Task>(), new Marks(1, 2, 3));
            Checker.AddTask(new Task("Велосипед","Краксеньпукс", null));
            Checker.AddTask(new Task("Анчоус", "Семен бородач", null));*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddLabel(n.ToString());
            AddRichTextBox(1);
            AddRichTextBox(2);
            AddButton();
            if (n > 8)
            {
                tableLayoutPanel1.AutoSize = false;
                tableLayoutPanel1.Height = 445;
                tableLayoutPanel1.Width += 15;
                tableLayoutPanel1.AutoScroll = true;
            }
        }
//////////////////////////////////////////////////////////////////////////
        private void AddLabel(String Text)
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Text = Text;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tableLayoutPanel1.Controls.Add(label, 0, n);
            tableLayoutPanel1.RowStyles.Insert(n, new RowStyle(SizeType.Absolute, 50));
            n++;
        }

        private void AddRichTextBox(int Column)
        {
            RichTextBox TextBox = new RichTextBox();
            TextBox.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(TextBox, Column, n-1);
        }
        private void AddButton()
        {
            Button button = new Button();
            button.Text = "Добавить изображение";
            button.Name = n.ToString();
            button.Width = 100;
            button.Height = 60;
            button.Anchor = AnchorStyles.Top;
            //tableLayoutPanel1.Controls.Add(button, 3, n - 1); //попытка добавить еще одну кнопку в одну ячейку
            /*Button button2 = new Button();
            button2.Text = "Вставить";
            button2.Width = 100;
            button2.Height = 60;
            button2.Anchor = AnchorStyles.Top;*/
            button.Click += AddPictureClicked;
            tableLayoutPanel1.Controls.Add(button, 3, n - 1);
        }

        private void AddPictureClicked(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Выберите изображение";
            openFileDialog1.Filter = "Изображения (*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            Button b = (Button)sender;
            b.Text = "";
            b.BackgroundImage = new Bitmap(openFileDialog1.FileName);
            b.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e) //сохранение
        {
            ReadToCheckList();
            String Data = JsonConvert.SerializeObject(Checks);
            try
            {
                File.WriteAllText("CheckList\\" + Checks.Course + " " + Checks.Name + ".test", Data);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения чек листа.");
            }
            Checks.Tasks.Clear();
        }

        private void ReadToCheckList() //собирает информацию из Control таблицы в чек лист
        {
            Task task = new Task();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (tableLayoutPanel1.GetRow(control) > 0)
                {
                    if (tableLayoutPanel1.GetColumn(control) > 0)
                    {
                        switch (tableLayoutPanel1.GetColumn(control))
                        {
                            case 1:
                            {
                                RichTextBox Lab = control as RichTextBox;
                                task.Name = Lab.Text;
                            } break;
                            case 2:
                            {
                                RichTextBox Lab = control as RichTextBox;
                                task.Description = Lab.Text;
                            } break;
                            case 3:
                            {
                                Button Lab = control as Button;
                                if (Lab.BackgroundImage != null)
                                {
                                    task.Image = ImageToString(Lab.BackgroundImage, tableLayoutPanel1.GetRow(control));/*Lab.BackgroundImage as String;*/
                                }
                                else
                                {
                                    task.Image = null;
                                }
                                Checks.Tasks.Add(task);
                                task = new Task();
                            } break;
                        }
                    }
                }
            }
        }

        private string ImageToString(Image Pic, int Number) //сохраняет картинку в папке Picture, возвращает ее имя
        {
            string Name = "";
            Name = Checks.Course + " " + Checks.Name + Number.ToString() + ".bmp";
            try
            {
                Pic.Save("CheckList\\Pictures\\" + Name, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения изображения.");
                Name = null;
            }
            return Name;
        }

        private void экспортВWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadToCheckList();
            Checks.ExportToWord();
            Checks.Tasks.Clear();
        }
    }
}
