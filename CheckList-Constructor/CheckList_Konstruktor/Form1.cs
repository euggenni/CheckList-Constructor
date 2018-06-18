using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace CheckList_Konstruktor
{
    public partial class Form1 : Form
    {
        string State = "";
        Constructor form = null;
        public Form1(string state)
        {
            State = state;
            InitializeComponent();
        }

        public Form1(string state, Constructor Form)
        { 
            State = state;
            InitializeComponent();
            form = Form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int classNum = 0;
            try
            {
                classNum = Convert.ToInt32(textBox6.Text);
            }
            catch (Exception){}
            int time = 0;
            try
            {
                time = Convert.ToInt32(textBox7.Text);
            }
            catch (Exception){}
            try
            {
                Title title = new Title(textBox2.Text, comboBox1.SelectedItem.ToString(), classNum, richTextBox1.Text, richTextBox2.Text, time, textBox8.Text, richTextBox6.Text, richTextBox3.Text, richTextBox5.Text, richTextBox4.Text);
                if (State == "Create")
                {
                    DataChekList.Check = new CheckList(comboBox1.SelectedIndex, title, new List<Task>(), new Marks(textBox3.Text, textBox4.Text, textBox5.Text), checkBox1.Checked);
                }
                if (State == "Update")
                {
                    DataChekList.Cource.SubList.ElementAt<Subject>(DataChekList.Check.Index).CheckListIndexes.Remove(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test"); //удаляет в предмете ссылку на старую версию названия
                    if (File.Exists(DataChekList.SaveTrack + @"\CheckList\" + DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test"))
                    {
                        File.Delete(DataChekList.SaveTrack + @"\CheckList\" + DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + ".test");
                    }
                    ///// изменение имен изображений
                    try
                    {
                        String[] Files = Directory.GetFiles(DataChekList.SaveTrack + @"\CheckList\Pictures\", DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name + "*.bin");
                        foreach(string file in Files)
                        {
                            string newFile = file.Replace(DataChekList.Check.Inform.Course + " " + DataChekList.Check.Inform.Name, comboBox1.SelectedItem.ToString() + " " + textBox2.Text);
                            File.Move(file, newFile);
                        }
                    }
                    catch (Exception b)
                    {
                        MessageBox.Show("Ошибка сохранения" + b.Message);
                    }
                    //////изменение ссылок на изображения в тесте
                    foreach (Task line in DataChekList.Check.Tasks)
                    {
                        if (line.Image != null)
                        { 
                            
                        }
                    }
                    //////1
                    DataChekList.Check.Index = comboBox1.SelectedIndex;
                    DataChekList.Check.Inform = title;
                    DataChekList.Check.Notes = new Marks(textBox3.Text, textBox4.Text, textBox5.Text);
                    DataChekList.Check.HasTimer = checkBox1.Checked;
                    ///// сохранение нового файла
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
                    form.UpdateListTests();
                }
            }
            catch (Exception a)
            {
                if (State == "Create")
                {
                    MessageBox.Show("Ошибка создания чек листа" + a.Message);
                    DataChekList.Check = new CheckList();
                }
                if (State == "Update")
                {
                    MessageBox.Show("Ошибка изменения чек листа" + a.Message);
                }
            }
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrintComboBox();
            if (State == "Update")
            {
                button1.Text = "Сохранить";
                comboBox1.SelectedIndex = DataChekList.Check.Index;
                textBox2.Text = DataChekList.Check.Inform.Name;
                textBox3.Text = DataChekList.Check.Notes.Excellent.ToString();
                textBox4.Text = DataChekList.Check.Notes.Good.ToString();
                textBox5.Text = DataChekList.Check.Notes.Satisfactory.ToString();
                richTextBox5.Text = DataChekList.Check.Inform.Comand;
                richTextBox4.Text = DataChekList.Check.Inform.Decreace;
                checkBox1.Checked = DataChekList.Check.HasTimer;
                textBox6.Text = DataChekList.Check.Inform.ClassNum.ToString();
                textBox7.Text = DataChekList.Check.Inform.Time.ToString();
                textBox8.Text = DataChekList.Check.Inform.Place;
                richTextBox1.Text = DataChekList.Check.Inform.Topic;
                richTextBox2.Text = DataChekList.Check.Inform.Purpose;
                richTextBox3.Text = DataChekList.Check.Inform.Literature;
                richTextBox6.Text = DataChekList.Check.Inform.Material;
            }
        }

        private void PrintComboBox()
        {
            comboBox1.Items.Clear();
            foreach (Subject Cource in DataChekList.Cource.SubList)
            {
                comboBox1.Items.Add(Cource.Name);
            }
        }
    }
}
