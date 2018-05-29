﻿using System;
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
using Word = Microsoft.Office.Interop.Word;

namespace CheckList_Konstruktor
{
    public partial class Constructor : Form
    {
        int n = 1; //нумератор строк таблицы
        public Constructor()
        {
            InitializeComponent();
        }

        private void Constructor_Load(object sender, EventArgs e)
        {
            if (DataChekList.Check == null)
            {
                label5.Visible = false;
                tableLayoutPanel1.Visible = false;
                button1.Visible = false;
            }
            else
            {
                label5.Visible = true;
                label5.Text = DataChekList.Check.Inform.Name;
                tableLayoutPanel1.Visible = true;
                button1.Visible = true;
            }
            //DataChekList.LoadEncrypt();
            шифроватьToolStripMenuItem.Checked = DataChekList.Encrypt;
            if (DataChekList.SaveTrack == "")
            {
                DataChekList.LoadSaveTrack(DataChekList.Encrypt);
                while (DataChekList.SaveTrack == "")
                {
                    MessageBox.Show("Так как программа запущена в первый раз, обязательно укажите путь сохранения файлов в следующем диалоговом окне!");
                    RenameSaveTrack form = new RenameSaveTrack();
                    form.ShowDialog();
                }
            }
            CreateNewDirectory();
            if (DataChekList.Cource == null)
            {
                DataChekList.Cource = Subjects.LoadSubList(DataChekList.Encrypt);
            }
            if (DataChekList.Platoons == null)
            {
                DataChekList.Platoons = Platoons.LoadPlatList(DataChekList.Encrypt);
            }
        }

        private void Constructor_Enter(object sender, EventArgs e)
        {
            if (DataChekList.Check == null)
            {
                label5.Visible = false;
                tableLayoutPanel1.Visible = false;
                button1.Visible = false;
            }
            else
            {
                label5.Visible = true;
                label5.Text = DataChekList.Check.Inform.Name;
                tableLayoutPanel1.Visible = true;
                button1.Visible = true;
                label5.Left = (this.Width - label5.Width)/2;
            }
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
            if (DataChekList.Check != null)
            {
                ReadToCheckList();
                String Data = JsonConvert.SerializeObject(DataChekList.Check);
                if (DataChekList.Encrypt) Data = Sini4ka.Flying(Data, "синяя синичка");
                try
                {
                    File.WriteAllText(DataChekList.SaveTrack + @"\CheckList\" + DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test", Data);
                    if (!DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Contains(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test"))//*/
                    {
                        DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Add(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test");
                        DataChekList.Cource.SaveSubList(DataChekList.Encrypt);
                    }
                }
                catch (Exception a)
                {
                    MessageBox.Show("Ошибка сохранения чек листа. " + a.Message);
                }
                DataChekList.Check.Tasks.Clear();
            }
            else
            {
                MessageBox.Show("Карточка задания не создана!");
            }
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
                                    task.Image = ImageToString(Lab.BackgroundImage, tableLayoutPanel1.GetRow(control));
                                }
                                else
                                {
                                    task.Image = null;
                                }
                                DataChekList.Check.Tasks.Add(task);
                                task = new Task();
                            } break;
                        }
                    }
                }
            }
        }

        private string ImageToString(Image Pic, int Number) //сохраняет картинку в папке Pictures, возвращает ее имя
        {
            string Name = "";
            int MaxSize = 300;
            Name = DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + Number.ToString() + ".bin";
            try
            {
                float piece = 1;
                if (Pic.Width > MaxSize || Pic.Height > MaxSize)
                {
                    if (Pic.Width >= Pic.Height)
                    {
                        piece = (float)MaxSize/Pic.Width;
                    }
                    else
                    {
                        piece = (float)MaxSize/Pic.Height;
                    }
                }
                Pic = new Bitmap(Pic, (int)(Pic.Width * piece), (int)(Pic.Height * piece));
                Pic.Save(DataChekList.SaveTrack + @"\CheckList\Pictures\" + Name, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сохранения изображения. " + e.Message);
                Name = null;
            }
            return Name;
        }

        private void экспортВWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //тестовый блок

            //конец тестового блока
            if (DataChekList.Check != null)
            {
                try
                {
                    FolderBrowserDialog Save = new FolderBrowserDialog();
                    if (Save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ReadToCheckList();
                        DataChekList.Check.ExportToWord(Save.SelectedPath);
                        DataChekList.Check.Tasks.Clear();
                    }
                }
                catch (Exception k)
                {
                    MessageBox.Show(k.Message);
                }
            }
            else
            {
                MessageBox.Show("Карточка задания не создана!");
            }
        }

        private void добавитьудалитьПолеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPole Pole = new AddPole(tableLayoutPanel1);
            Pole.Show();
        }

        private void добавитьКарточкЗаданияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 Form = new Form1();
            Form.ShowDialog();
        }

        private void редактироватьПредметыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCources Cources = new AddCources();
            Cources.ShowDialog();
        }

        private void редактироватьВзводаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPlatoons Platoon = new AddPlatoons();
            Platoon.ShowDialog();
        }

        private void изменитьПутьСохраненияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameSaveTrack Save = new RenameSaveTrack();
            Save.ShowDialog();
            CreateNewDirectory();
        }

        private void CreateNewDirectory() //проверяет нахождение папки с чек листами, если нет, то создает их
        {
            bool create = true; //на случай, если потребуется отключить сохранение извне
            if (create)
            {
                DirectoryInfo dirinfo = new DirectoryInfo(DataChekList.SaveTrack + @"\CheckList");
                if (!dirinfo.Exists)
                {
                    dirinfo.Create();
                    dirinfo.CreateSubdirectory(@"Inform");
                    dirinfo.CreateSubdirectory(@"Pictures");
                }
            }
        }

        private void шифроватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*шифроватьToolStripMenuItem.Checked = !шифроватьToolStripMenuItem.Checked;
            DataChekList.Encrypt = шифроватьToolStripMenuItem.Checked;*/
        }

        private void Constructor_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DataChekList.SaveEncrypt();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
    }
}