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
        public ComboBox cb1 = new ComboBox();
        public int n = 1; //нумератор строк таблицы
        public int selectedItem = -1;
        List<CheckList> CheckLists = new List<CheckList>();
        List<CheckList> cl;
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
                panel1.Visible = false;
            }
            else
            {
                label5.Visible = true;
                label5.Text = DataChekList.Check.Inform.Name;
                tableLayoutPanel1.Visible = true;
                panel1.Visible = true;
            }
            //DataChekList.LoadEncrypt();
            шифроватьToolStripMenuItem.Checked = DataChekList.Encrypt;
            шифроватьToolStripMenuItem.Enabled = false;
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
            PaintChekLists("");
            cb1.SelectedIndex = -1;
        }

        private void Constructor_Enter(object sender, EventArgs e)
        {
            if (DataChekList.Check == null)
            {
                label5.Visible = false;
                tableLayoutPanel1.Visible = false;
                panel1.Visible = false;
                добавитьудалитьПолеToolStripMenuItem.Enabled = false;
                сохранитьToolStripMenuItem.Enabled = false;
            }
            else
            {
                label5.Visible = true;
                label5.Text = DataChekList.Check.Inform.Name;
                tableLayoutPanel1.Visible = true;
                panel1.Visible = true;
                panel2.Visible = false;
                label5.Left = (this.Width - label5.Width)/2;
                добавитьудалитьПолеToolStripMenuItem.Enabled = true;
                сохранитьToolStripMenuItem.Enabled = true;
                //OpenCheckList();
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
                //if (n == 8)tableLayoutPanel1.Width += 15;
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
            button.MouseUp += AddPictureClicked;
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
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.MouseUp += AddPictureClicked;
            if (image != null)
            {
                try
                {
                    //CheckList.Rename(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + image, true);
                    var fileOfInterest = DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + image/*string.Concat(image.Remove(image.LastIndexOf('.')), ".jpeg")*/;
                    byte[] imageData = new byte[0];
                    byte[] buffer = new byte[255];
                    int total_byte_count = 0;
                    using (FileStream fos = new FileStream(fileOfInterest, FileMode.Open))
                    {
                        int readCount = 0;
                        do
                        {
                            readCount = fos.Read(buffer, 0, buffer.Length);
                            Array.Resize(ref imageData, imageData.Length + readCount);
                            Array.Copy(buffer, 0, imageData, total_byte_count, readCount);
                            total_byte_count += readCount;
                        }
                        while (readCount != 0);
                    }
                    MemoryStream ms = new MemoryStream(imageData);
                    Image pic = Image.FromStream(ms);
                    //Image pic = Image.FromFile(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + string.Concat(image.Remove(image.LastIndexOf('.')), ".jpeg"));
                    button.BackgroundImage = pic;
                    //CheckList.Rename(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + string.Concat(image.Remove(image.LastIndexOf('.')), ".jpeg"), false);
                }
                catch (Exception c) { MessageBox.Show(c.Message); }
            }
            tableLayoutPanel1.Controls.Add(button, 3, n - 1);
        }

        private void AddPictureClicked(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (Clipboard.GetImage() != null)
                {
                    b.Text = "";
                    b.BackgroundImageLayout = ImageLayout.Zoom;
                    b.BackgroundImage = (Image)Clipboard.GetImage();
                }
            }
            else
            {
                openFileDialog1.Title = "Выберите изображение";
                openFileDialog1.Filter = "Изображения (*.jpg)|*.jpg";
                if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
                b.Text = "";
                b.BackgroundImage = new Bitmap(openFileDialog1.FileName);
                b.BackgroundImageLayout = ImageLayout.Zoom;
            }
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
                    if(!DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Contains(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test"))//*/
                    {
                        DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Add(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test");
                        DataChekList.Cource.SaveSubList(DataChekList.Encrypt);
                    }
                    DirectoryInfo dirinfo = new DirectoryInfo(DataChekList.SaveTrack + @"\CheckList\Exports\");
                    if(dirinfo.Exists)
                    {
                        dirinfo.CreateSubdirectory(DataChekList.Check.Inform.Course);
                    }
                    DataChekList.Check.ExportToWord(DataChekList.SaveTrack + @"\CheckList\Exports\" + DataChekList.Check.Inform.Course + "\\");
                }
                catch (Exception a)
                {
                    MessageBox.Show("Ошибка сохранения чек листа. " + a.Message);
                }
                DataChekList.Check.Tasks.Clear();
                CloseCheckList();
            }
            else
            {
                MessageBox.Show("Карточка задания не создана!");
            }
        }

        public void ReadToCheckList() //собирает информацию из Control таблицы в чек лист
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

        /*private void экспортВWordToolStripMenuItem_Click(object sender, EventArgs e)
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
        */
        private void добавитьудалитьПолеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPole Pole = new AddPole(this);
            Pole.Show();
        }

        private void добавитьКарточкЗаданияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 Form = new Form1("Create", this);
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
                        dirinfo.CreateSubdirectory(@"Exports");
                        dirinfo.CreateSubdirectory(@"Results");
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
            /*шифроватьToolStripMenuItem.Checked = !шифроватьToolStripMenuItem.Checked;
            DataChekList.Encrypt = шифроватьToolStripMenuItem.Checked;//*/
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
        
        private void PaintChekLists(string subName) //формирует элементы для вывода чек листов
        {
            panel2.Controls.Clear();
            if (subName != "")
            {
                cl = new List<CheckList>();
                foreach (CheckList Test in CheckLists)
                {
                    if (String.Compare(Test.Inform.Course, subName) == 0)
                    {
                        cl.Add(Test);
                    }
                }
            }
            else
            {
                cl = CheckLists;
            }
            panel2.Visible = true;
            panel2.Dock = DockStyle.Fill;
            panel2.AutoSize = true;
            panel2.HorizontalScroll.Maximum = 0;
            panel2.AutoScroll = false;
            panel2.VerticalScroll.Visible = false;
            panel2.AutoScroll = true;
            int n = 0;
            Panel p1 = new Panel();
            Label l1 = new Label();
            cb1 = new ComboBox();
            p1.BackColor = SystemColors.ControlLight;
            p1.Name = "Pan" + n;
            p1.Size = new Size(700, 51);
            p1.Location = new Point(3, 15);
            p1.Tag = "panelTestInTests";
            l1.AutoSize = true;
            l1.Font = new Font("Century Gothic", 11.25F);
            l1.Location = new Point(3, 15);
            l1.Name = "CbLabel";
            l1.Size = new Size(146, 20);
            l1.Text = "Предмет ";
            l1.TabStop = true;
            l1.Tag = n;
            l1.TextAlign = ContentAlignment.MiddleCenter;
            cb1.FlatStyle = FlatStyle.Flat;
            cb1.Font = new Font("Century Gothic", 11.25F);
            cb1.Location = new Point(300, 5);
            cb1.Name = "cb";
            cb1.Size = new Size(310, 40);
            cb1.Text = "Выберите предмет";
            cb1.Tag = n;
            p1.Show();
            l1.Show();
            cb1.Show();

            panel2.Controls.Add(p1);
            p1.Controls.Add(l1);
            p1.Controls.Add(cb1);
            foreach (Subject Cource in DataChekList.Cource.SubList)
            {
                cb1.Items.Add(Cource.Name);
            }
            cb1.SelectedIndex = selectedItem;
            cb1.SelectedIndexChanged += Sic;
            foreach (CheckList Test in cl)
            {
                n++;
                Panel Pan = new Panel();
                Label TestName = new Label();
                Button TestOpen = new Button();
                Button TestInform = new Button();
                Button TestDelete = new Button();
                //охватывающий блок
                Pan.BackColor = SystemColors.ControlLight;
                Pan.Name = "Pan" + n;
                Pan.Size = new Size(700, 51);
                Pan.Location = new Point(3, 15+51*(n));
                Pan.Tag = "panelTestInTests";
                //название теста
                TestName.AutoSize = true;
                TestName.Font = new Font("Century Gothic", 11.25F);
                TestName.Location = new Point(3, 15);
                TestName.Name = "" + n;
                TestName.Size = new Size(146, 20);
                TestName.Text = Test.Inform.Name;
                TestName.TabStop = true;
                TestName.Tag = n;
                TestName.TextAlign = ContentAlignment.MiddleCenter;
                //кнопка открытия содержимого теста
                TestOpen.FlatStyle = FlatStyle.Flat;
                TestOpen.Font = new Font("Century Gothic", 11.25F);
                TestOpen.Location = new Point(485, 5);
                TestOpen.Name = n.ToString();
                TestOpen.Size = new Size(150, 40);
                //TestOpen.Te
                TestOpen.Text = "Открыть";
                TestOpen.UseVisualStyleBackColor = true;
                TestOpen.Tag = n;
                TestOpen.TextAlign = ContentAlignment.MiddleCenter;
                TestOpen.Click += OpenTest;
                //кнопка открытия информации теста
                TestInform.FlatStyle = FlatStyle.Flat;
                TestInform.Font = new Font("Century Gothic", 11.25F);
                TestInform.Location = new Point(330, 5);
                TestInform.Name = n.ToString()+"0";
                TestInform.Size = new Size(150, 40);
                TestInform.Text = "Заголовок";
                TestInform.UseVisualStyleBackColor = true;
                TestInform.Tag = n+"0";
                TestInform.TextAlign = ContentAlignment.MiddleCenter;
                TestInform.Click += OpenInform;
                //кнопка удаления теста
                TestDelete.FlatStyle = FlatStyle.Flat;
                TestDelete.Font = new Font("Century Gothic", 11.25F);
                TestDelete.Location = new Point(175, 5);
                TestDelete.Name = n.ToString() + "00";
                TestDelete.Size = new Size(150, 40);
                TestDelete.Text = "Удалить";
                TestDelete.UseVisualStyleBackColor = true;
                TestDelete.Tag = n + "00";
                TestDelete.TextAlign = ContentAlignment.MiddleCenter;
                TestDelete.Click += DeleteTest;
                ///////////////////////////////////
                Pan.Show();
                TestName.Show();
                TestOpen.Show();
                TestInform.Show();
                TestDelete.Show();

                panel2.Controls.Add(Pan);
                Pan.Controls.Add(TestName);
                Pan.Controls.Add(TestOpen);
                Pan.Controls.Add(TestInform);
                Pan.Controls.Add(TestDelete);
            }
        }

        private void Sic(object sender, EventArgs e)
        {
            selectedItem = cb1.SelectedIndex;
            PaintChekLists(cb1.SelectedItem.ToString());
        }

        private void OpenTest(object sender, EventArgs e) //событие на открытие теста
        {
            Button b = (Button)sender;
            try
            {
                DataChekList.Check = cl.ElementAt<CheckList>(Convert.ToInt32(b.Name)-1);
            }
            catch (Exception a) { MessageBox.Show(a.Message); return; }
            OpenCheckList();
        }

        private void OpenInform(object sender, EventArgs e) //событие на открытие информации теста
        { 
            Button b = (Button)sender;
            try
            {
                DataChekList.Check = cl.ElementAt<CheckList>((Convert.ToInt32(b.Name)-1) /10);
            }
            catch (Exception a) { MessageBox.Show(a.Message); return; }
            Form1 Form = new Form1("Update", this);
            Form.ShowDialog();
        }

        private void DeleteTest(object sender, EventArgs e) //событие на удаление определенного теста
        {
            Button b = (Button)sender;
            try
            {
                DataChekList.Check = cl.ElementAt<CheckList>((Convert.ToInt32(b.Name) -1) / 100);
            }
            catch (Exception a) { MessageBox.Show(a.Message); return; }
            DeleteCheckList();
        }

        private void ReadChekLists() //читает из сохраненной директории все чек листы
        {
            try
            {
                String[] Files = Directory.GetFiles(DataChekList.SaveTrack + @"\CheckList\", "*.test");
                CheckLists.Clear();
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

        public void OpenCheckList() //инициирует открытие чек листа для редактирования полей
        {
            panel2.Visible = false;
            label5.Visible = true;
            label5.Left = (this.Width - label5.Width) / 2;
            label5.Text = DataChekList.Check.Inform.Name;
            //this.n = 1;
            добавитьудалитьПолеToolStripMenuItem.Enabled = true;
            сохранитьToolStripMenuItem.Enabled = true;
            добавитьКарточкЗаданияToolStripMenuItem.Enabled = false;
            /////////////////////////////////////////////пересоздаем таблицу
            //this.Controls.Remove(tableLayoutPanel1);
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
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.13));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.25));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.47));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.15));
            CreateTableHeader();
            /////////////////////////////////////////////
            panel1.Visible = true;
            int n = 1;
            foreach (Task task in DataChekList.Check.Tasks)
            {
                AddLabel(n.ToString());
                AddRichTextBox(1, task.Name);
                AddRichTextBox(2, task.Description);
                AddButton(task.Image);
                if (n == 8)
                {
                    tableLayoutPanel1.AutoSize = false;
                    tableLayoutPanel1.Height = 445;
                    //tableLayoutPanel1.Width += 15;
                    tableLayoutPanel1.AutoScroll = true;
                }
                n++;
            }
            this.Controls.Add(tableLayoutPanel1);
            tableLayoutPanel1.Show();
            tableLayoutPanel1.Visible = true;
        }

        private void CreateTableHeader() //добавляет шапку таблицы
        { 
            AddLabel("№ действия", 0);
            AddLabel("Название действия", 1);
            AddLabel("Порядок выполнения", 2);
            AddLabel("Контроль", 3);
        }

        private void CloseCheckList() //инициирует закрытие чек листа
        {
            DataChekList.Check = null;
            n = 1;
            tableLayoutPanel1.Visible = false;
            panel1.Visible = false;
            добавитьудалитьПолеToolStripMenuItem.Enabled = false;
            сохранитьToolStripMenuItem.Enabled = false;
            добавитьКарточкЗаданияToolStripMenuItem.Enabled = true;
            ReadChekLists();
            selectedItem = -1;
            PaintChekLists("");
        }

        public void UpdateListTests()//обновляет список тестов
        {
            ReadChekLists();
            selectedItem = -1;
            PaintChekLists("");
            panel1.Visible = false;
        }

        private void DeleteCheckList() //удаляет всю информацию о тесте
        { 
            DialogResult = MessageBox.Show("Вы точно хотите удалить карточку задания и всю информацию о ней?", "Внимание!", MessageBoxButtons.YesNo);
            if (DialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                //непосредственное удаление файла теста
                if (File.Exists(DataChekList.SaveTrack + @"\CheckList\" + DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test"))
                {
                    File.Delete(DataChekList.SaveTrack + @"\CheckList\" + DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test");
                }
                //удаление связи с предметом
                if (DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Contains(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test"))
                {
                    DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Remove(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test");
                }
                DataChekList.Cource.SaveSubList(DataChekList.Encrypt);
                //удаление всех изображений, связанных с тестом
                String[] Files = Directory.GetFiles(DataChekList.SaveTrack + @"\CheckList\Pictures\", DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + "*.bin");
                foreach (string file in Files)
                {
                    File.Delete(file);
                }
                //
                DataChekList.Check = null;
                UpdateListTests();
            }
        }

        private void button2_Click(object sender, EventArgs e) //закрывает чек лист
        {
            CloseCheckList();
        }

        private void button3_Click(object sender, EventArgs e) //готово
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
                    DirectoryInfo dirinfo = new DirectoryInfo(DataChekList.SaveTrack + @"\CheckList\Exports\");
                    if (dirinfo.Exists)
                    {
                        dirinfo.CreateSubdirectory(DataChekList.Check.Inform.Course);
                    }
                    DataChekList.Check.ExportToWord(DataChekList.SaveTrack + @"\CheckList\Exports\" + DataChekList.Check.Inform.Course + "\\");
                }
                catch (Exception a)
                {
                    MessageBox.Show("Ошибка сохранения чек листа. " + a.Message);
                }
                DataChekList.Check.Tasks.Clear();
                CloseCheckList();
            }
            else
            {
                MessageBox.Show("Карточка задания не создана!");
            }
        }
    }
}
