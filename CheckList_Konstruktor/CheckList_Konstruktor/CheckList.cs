using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace CheckList_Konstruktor
{
    public class CheckList
    {
        private string course; //предмет, для которого создан чек лист
        private string name; //название чек листа
        private List<Task> tasks; //список всех тасков
        private Marks notes; //критерии оценки
        private bool hasTextField;//имеет ли лист поля для ввода ответов

        public CheckList(string Course, string Name, List<Task> Tasks, Marks Notes, bool hasTextField)
        {
            this.Course = Course;
            this.Name = Name;
            this.Tasks = Tasks;
            this.Notes = Notes;
            this.hasTextField = hasTextField;
        }

        public CheckList(string Course, string Name, Task Task_One, Marks Notes, bool hasTextField)
        {
            this.Course = Course;
            this.Name = Name;
            this.Tasks = new List<Task>();
            this.Tasks.Add(Task_One);
            this.Notes = Notes;
            this.hasTextField = hasTextField;
        }

        public CheckList(string Course, string Name, bool hasTextField)
        {
            this.Course = Course;
            this.Name = Name;
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
            this.hasTextField = hasTextField;
        }

        public CheckList()
        {
            this.Course = "Unknown";
            this.Name = "Unknown";
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
            this.hasTextField = false;
        }

        public string Course
        {
            get { return course; }
            set { course = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Task> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        /*public Task this[int key]
        {
            get { return tasks.ElementAt<Task>(key); }
            set { tasks.ElementAt<Task>(key) = value; }
        }*/

        public Marks Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public void AddTask(Task task) //добавление пункта в чек лист
        {
            this.Tasks.Add(task);
        }

        public void RemoveTask(int i)//удаление строки из листа
        {
            this.Tasks.RemoveAt(i);
        }

        public Task ReadTaskAt(int i) //читаем пункт по номеру в списке 
        {
            return this.Tasks.ElementAt(i);
        }

        public int CountTasks() //возвращает число пунктов в списке
        {
            return this.Tasks.Count;
        }

        public void ExportToWord()
        {
            //открытие приложения Word
            var app = new Word.Application();
            app.Visible = false;
            object missing = System.Reflection.Missing.Value;

            //создание нового документа Word
            var doc = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            doc.PageSetup.TopMargin = 35;
            doc.PageSetup.RightMargin = 35;
            doc.PageSetup.BottomMargin = 35;
            doc.PageSetup.LeftMargin = 35;
            string font = "Times New Roman";

            //Вставка названия чек-листа
            Word.Paragraph p1 = doc.Content.Paragraphs.Add(ref missing);
            p1.Range.Font.Bold = 1;
            p1.Range.Font.Name = font;
            p1.Range.Font.Size = 14;
            p1.Range.Font.Color = Word.WdColor.wdColorBlack;
            p1.Range.Text = Name;
            p1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            p1.Range.InsertParagraphAfter();

            if (!this.hasTextField)
            {
                //Создание таблицы
                var table = doc.Tables.Add(p1.Range, this.Tasks.Count + 1, 5, ref missing, ref missing);
                table.Borders.Enable = 1;
                table.Columns[1].Width = 30;
                table.Columns[2].Width = 75;
                table.Columns[3].Width = 165;
                table.Columns[4].Width = 165;
                table.Columns[5].Width = 75;

                //Заполнение заголовков таблицы
                for (int i = 1; i <= 5; i++)
                {
                    Word.Cell cell = table.Cell(1, i);
                    string sname = "";
                    switch (i)
                    {
                        case 1: sname = "№"; break;
                        case 2: sname = "Название действия"; break;
                        case 3: sname = "Описание последовательности действий"; break;
                        case 4: sname = "Фото"; break;
                        case 5: sname = "Отметка"; break;
                    }
                    cell.Range.Text = sname;
                    cell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                    cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    cell.Range.ParagraphFormat.SpaceAfter = 0;
                    cell.Range.Font.Bold = 1;
                    cell.Range.Font.Name = font;
                    cell.Range.Font.Size = 12;
                }

                //Запись листа в таблицу
                for (int i = 0; i < this.Tasks.Count; i++)
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        Word.Cell cell = table.Cell(i + 2, j);
                        cell.Range.Font.Name = font;
                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                        cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        cell.Range.ParagraphFormat.SpaceAfter = 0;
                        cell.Range.Font.Size = 11;
                        switch (j)
                        {
                            case 1:
                                {
                                    cell.Range.Text = "" + (i + 1);
                                    cell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                                    cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                    cell.Range.Font.Bold = 1;
                                    cell.Range.Font.Size = 12;
                                }
                                break;
                            case 2: cell.Range.Text = this.Tasks[i].Name; break;
                            case 3: cell.Range.Text = this.Tasks[i].Description; break;
                            case 4:
                                {
                                    if (this.Tasks[i].Image != null)
                                    {
                                        cell.Range.InlineShapes.AddPicture(Application.StartupPath + @"\CheckList\Pictures\" + this.Tasks[i].Image);
                                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    var box = cell.Tables.Add(cell.Range, 1, 1, ref missing, ref missing);
                                    box.Borders.Enable = 1;
                                    box.Columns[1].Width = 15;
                                    box.Rows.Alignment = Word.WdRowAlignment.wdAlignRowCenter;
                                }
                                break;
                        }
                    }
                }

                //Сохранение файла
                doc.SaveAs2(Application.StartupPath + @"\" + Name + ".docx");
                doc.Close(ref missing, ref missing, ref missing);
                doc = null;
                app.Quit(ref missing, ref missing, ref missing);
                app = null;
            }
        }
    }
}
