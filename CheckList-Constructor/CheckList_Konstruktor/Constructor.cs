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
using Word = Microsoft.Office.Interop.Word;

namespace CheckList_Konstruktor
{
    public partial class Constructor : Form
    {
        int n = 1; //нумератор строк таблицы
        List<CheckList> CheckLists = new List<CheckList>();
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
            DataChekList.LoadEncrypt();
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
            ReadChekLists();
            PaintChekLists();
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

        private void AddLabel(String Text, int Col) //добавляет ячейки в header
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Text = Text;
            label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            //label.BackColor = SystemColors.ActiveBorder;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label.TextAlign = ContentAlignment.MiddleCenter;
            tableLayoutPanel1.Controls.Add(label, Col, 0);
            if (Col == 0) tableLayoutPanel1.RowStyles.Insert(0, new RowStyle(SizeType.Absolute, 50));
        }

        private void AddRichTextBox(int Column)
        {
            RichTextBox TextBox = new RichTextBox();
            TextBox.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(TextBox, Column, n-1);
        }
        private void AddRichTextBox(int Column, string Text)
        {
            RichTextBox TextBox = new RichTextBox();
            TextBox.Dock = DockStyle.Fill;
            TextBox.Text = Text;
            tableLayoutPanel1.Controls.Add(TextBox, Column, n - 1);
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

        private void AddButton(string image)
        {
            Button button = new Button();
            if (image != null) button.Text = "";
            else button.Text = "Добавить изображение";
            button.Name = n.ToString();
            button.Width = 100;
            button.Height = 60;
            button.Anchor = AnchorStyles.Top;
            button.Click += AddPictureClicked;
            if (image != null)
            {
                try
                {
                    CheckList.Rename(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + image, true);
                    Image pic = Image.FromFile(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + string.Concat(image.Remove(image.LastIndexOf('.')), ".jpeg"));
                    CheckList.Rename(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + string.Concat(image.Remove(image.LastIndexOf('.')), ".jpeg"), false);
                    button.BackgroundImage = pic;
                }
                catch (Exception) { }
            }
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
            DataChekList.Check.Tasks.Clear();
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
                try
                {
                    DirectoryInfo dirinfo = new DirectoryInfo(DataChekList.SaveTrack + @"\CheckList");
                    if (!dirinfo.Exists)
                    {
                        dirinfo.Create();
                        dirinfo.CreateSubdirectory(@"Inform");
                        dirinfo.CreateSubdirectory(@"Pictures");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка создания директории. " + e.Message);
                }
            }
        }

        private void шифроватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            шифроватьToolStripMenuItem.Checked = !шифроватьToolStripMenuItem.Checked;
            DataChekList.Encrypt = шифроватьToolStripMenuItem.Checked;//*/
        }

        private void Constructor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataChekList.SaveEncrypt();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        //тестовый блок
        private void секретнаяКнопкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*Control c;
            List<HoldingCell> temHolding = new List<HoldingCell>();
            HoldingCell cell;
            int index = 1;
            {
                var withBlock = tableLayoutPanel1;

                for (int col = 0; col <= withBlock.ColumnCount - 1; col++)
                {
                    c = withBlock.GetControlFromPosition(column: col, row: index);
                    if (c != null)
                    {
                        withBlock.Controls.RemoveByKey(c.Name);
                        c.Dispose();
                    }
                }

                for (int row = index + 1; row <= tableLayoutPanel1.RowCount - 1; row++)
                {
                    for (int col = 0; col <= tableLayoutPanel1.ColumnCount - 1; col++)
                    {
                        cell = new HoldingCell();
                        cell.cntrl = withBlock.GetControlFromPosition(col, row);
                        cell.pos = new TableLayoutPanelCellPosition(col, row - 1);
                        temHolding.Add(cell);
                    }
                }

                withBlock.RowStyles.RemoveAt(index: index);
                withBlock.RowCount -= 1;

                foreach (var cel in temHolding)
                {
                    if (cel.cntrl != null)
                    {
                        withBlock.SetCellPosition(cel.cntrl, cel.pos);
                    }
                }
            }*/
        }
        struct HoldingCell
        {
            public Control cntrl;
            public TableLayoutPanelCellPosition pos;
        }
        private void PaintChekLists() //формирует элементы для вывода чек листов
        {
            panel2.Visible = true;
            panel2.Dock = DockStyle.Fill;
            int n = 0;
            foreach (CheckList Test in CheckLists)
            {
                n++;
                Panel Pan = new Panel();
                Label TestName = new Label();
                Button TestOpen = new Button();

                Pan.BackColor = SystemColors.ControlLight;
                Pan.Name = "Pan" + n;
                Pan.Size = new Size(700, 51);
                Pan.Location = new Point(3, 15+51*(n-1));
                Pan.Tag = "panelTestInTests";

                TestName.AutoSize = true;
                TestName.Font = new Font("Century Gothic", 11.25F);
                TestName.Location = new Point(3, 15);
                TestName.Name = "" + n;
                TestName.Size = new Size(146, 20);
                TestName.Text = Test.Inform.Name;
                TestName.TabStop = true;
                TestName.Tag = n;
                TestName.TextAlign = ContentAlignment.MiddleCenter;

                TestOpen.FlatStyle = FlatStyle.Flat;
                TestOpen.Font = new Font("Century Gothic", 11.25F);
                TestOpen.Location = new Point(485, 5);
                TestOpen.Name = n.ToString();
                TestOpen.Size = new Size(150, 40);
                TestOpen.Text = "Открыть тест";
                TestOpen.UseVisualStyleBackColor = true;
                TestOpen.Tag = n;
                TestOpen.TextAlign = ContentAlignment.MiddleCenter;
                TestOpen.Click += OpenTest;
                Pan.Show();
                TestName.Show();
                TestOpen.Show();

                panel2.Controls.Add(Pan);
                Pan.Controls.Add(TestName);
                Pan.Controls.Add(TestOpen);
            }
        }

        private void OpenTest(object sender, EventArgs e) //событие на открытие теста
        {
            Button b = (Button)sender;
            try
            {
                DataChekList.Check = CheckLists.ElementAt<CheckList>(Convert.ToInt32(b.Name)-1);
                //OpenCheckList
            }
            catch (Exception) { return; }
            OpenCheckList();
        }

        private void ReadChekLists() //читает из сохраненной директории все чек листы
        {
            try
            {
                String[] Files = Directory.GetFiles(DataChekList.SaveTrack + @"\CheckList\", "*.test");
                if (Files.Length != 0)
                {
                    foreach (string file in Files)
                    {
                        string Data = "";
                        try
                        {
                            Data = File.ReadAllText(file);
                            if (DataChekList.Encrypt) Data = Sini4ka.Landing(Data, "синяя синичка");
                            CheckLists.Add(JsonConvert.DeserializeObject<CheckList>(Data));
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Ошибка чтения теста. " + e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки чек листов. " + e.Message);
            }
        }

        private void OpenCheckList() //инициирует открытие чек листа для редактирования полей
        {
            panel2.Visible = false;
            label5.Visible = true;
            label5.Left = (this.Width - label5.Width) / 2;
            label5.Text = DataChekList.Check.Inform.Name;
            /////////////////////////////////////////////пересоздаем таблицу
            tableLayoutPanel1.Dispose();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.Location = new Point(23, 60);
            tableLayoutPanel1.Height = 59;
            tableLayoutPanel1.Width = 655;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BackColor = SystemColors.ActiveBorder;
            tableLayoutPanel1.AutoSize = true;
            /*TableLayoutColumnStyleCollection columnStyle;
            columnStyle.Add(new ColumnStyle(SizeType.Percent, (float)0.4737));
            columnStyle.Add(new ColumnStyle(SizeType.Percent, (float)0.5263));
            columnStyle.Add(new ColumnStyle(SizeType.Absolute, 245));
            columnStyle.Add(new ColumnStyle(SizeType.Absolute, 188));*/
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 245));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 188));
            CreateTableHeader();
            /////////////////////////////////////////////
            button1.Visible = true;
            int n = 1;
            foreach (Task task in DataChekList.Check.Tasks)
            {
                AddLabel(n.ToString());
                AddRichTextBox(1, task.Name);
                AddRichTextBox(2, task.Description);
                AddButton(task.Image);
                if (n > 8)
                {
                    tableLayoutPanel1.AutoSize = false;
                    tableLayoutPanel1.Height = 445;
                    tableLayoutPanel1.Width += 15;
                    tableLayoutPanel1.AutoScroll = true;
                }
                n++;
            }
            this.Controls.Add(tableLayoutPanel1);
            tableLayoutPanel1.Show();
        }
        private void CreateTableHeader() //добавляет шапку таблицы
        { 
            AddLabel("№ действия", 0);
            AddLabel("Название действия", 1);
            AddLabel("Порядок выполнения", 2);
            AddLabel("Контроль", 3);
        }
    }
}
