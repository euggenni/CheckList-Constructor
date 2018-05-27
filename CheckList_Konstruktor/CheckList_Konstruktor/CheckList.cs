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
        //секция полей карточки задания
        private int index;//принадлежность листа предмету
        private Title inform; //информация титульного листа
        private List<Task> tasks; //список всех тасков
        private Marks notes; //критерии оценки
        private bool hasTimer; //засечка времени выполнения 



        //секция конструкторов
        public CheckList(int Index, Title Inform, List<Task> Tasks, Marks Notes, bool hasTimer)
        {
            this.index = Index;
            this.Inform = Inform;
            this.Tasks = Tasks;
            this.Notes = Notes;
            this.hasTimer = hasTimer;
        }
        public CheckList(Title Inform, List<Task> Tasks, Marks Notes, bool hasTimer)
        {
            this.Inform = Inform;
            this.Tasks = Tasks;
            this.Notes = Notes;
            this.hasTimer = hasTimer;
        }
        public CheckList(string Course, string Name, List<Task> Tasks, Marks Notes, bool hasTimer)
        {
            this.inform = new Title(Name, Course);
            this.Tasks = Tasks;
            this.Notes = Notes;
            this.hasTimer = hasTimer;
        }
        public CheckList(string Course, string Name, Task Task_One, Marks Notes, bool hasTimer)
        {
            this.inform = new Title(Name, Course);
            this.Tasks = new List<Task>();
            this.Tasks.Add(Task_One);
            this.Notes = Notes;
            this.hasTimer = hasTimer;
        }
        public CheckList(string Course, string Name, bool hasTimer)
        {
            this.inform = new Title(Name, Course);
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
            this.hasTimer = hasTimer;
        }
        public CheckList()
        {
            this.inform = new Title("Unknown", "Unknown");
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
            this.hasTimer = false;
        }


        //секция свойств полей карточки задания
        public Title Inform
        {
            get { return inform; }
            set { inform = value; }
        }
        public List<Task> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }
        public Marks Notes
        {
            get { return notes; }
            set { notes = value; }
        }
        public bool HasTimer
        {
            get { return hasTimer; }
            set { hasTimer = value; }
        }

        //секция методов
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

        //особый гость
        public void ExportToWord(string SaveTrack)
        {
            //тестовый блок
            /*this.inform.ClassNum = 3;
            this.inform.Comand = "к неполной разборке автомата приступить!";
            this.inform.Decreace = "потеря автомата - ссылка в дизбад";
            this.inform.Literature = "Учебник \"Военная топография\", стр 94-95";
            this.inform.Material = "Топографические карты. Клей. Ножницы (нож). Карандаши. Офицерские линейки. Курвиметры. Клей.";
            this.inform.Place = "класс";
            this.inform.Purpose = "привить навык по разборке оружия";
            this.inform.Time = 90;*/
            //this.inform.Topic = "Сборка и разборка оружия";

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
            doc.Paragraphs.SpaceAfter = 0;

            //блок титульного листа1
            Word.Paragraph p1 = doc.Content.Paragraphs.Add(ref missing);
            p1.Range.Font.Bold = 1;
            p1.Range.Font.Name = font;
            p1.Range.Font.Size = 14;
            p1.Range.Text = "КАРТОЧКА ЗАДАНИЯ";
            p1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            p1.Range.InsertParagraphAfter();

            Word.Paragraph p2 = doc.Content.Paragraphs.Add(ref missing);
            p2.Range.Font.Bold = 1;
            p2.Range.Font.Name = font;
            p2.Range.Font.Size = 14;
            p2.Range.Text = this.inform.Course;
            p2.Range.InsertParagraphAfter();

            Word.Paragraph p3 = doc.Content.Paragraphs.Add(ref missing);
            p3.Range.Font.Bold = 1;
            p3.Range.Font.Name = font;
            p3.Range.Font.Size = 14;
            p3.Range.Text = "Занятие №" + this.inform.ClassNum;
            p3.Range.InsertParagraphAfter();

            Word.Paragraph p4 = doc.Content.Paragraphs.Add(ref missing);
            p4.Range.Font.Bold = 1;
            p4.Range.Font.Name = font;
            p4.Range.Font.Size = 14;
            p4.Range.Text = "\"" + this.inform.Name + "\"";
            p4.Range.InsertParagraphAfter();

            //блок титульного листа2
            Word.Paragraph p5 = doc.Content.Paragraphs.Add(ref missing);
            p5.Range.Font.Bold = 1;
            p5.Range.Font.Name = font;
            p5.Range.Font.Size = 14;
            p5.Range.Text = "\nЦель: " + this.inform.Purpose;
            object oStart = p5.Range.Start + p5.Range.Text.IndexOf(':');
            object oEnd = p5.Range.Start + p5.Range.Text.Length;
            Word.Range p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p5.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            p5.Range.InsertParagraphAfter();

            Word.Paragraph p6 = doc.Content.Paragraphs.Add(ref missing);
            p6.Range.Font.Bold = 1;
            p6.Range.Font.Name = font;
            p6.Range.Font.Size = 14;
            p6.Range.Text = "Время: " + this.inform.Time + " мин.";
            oStart = p6.Range.Start + p6.Range.Text.IndexOf(':');
            oEnd = p6.Range.Start + p6.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p6.Range.InsertParagraphAfter();

            Word.Paragraph p7 = doc.Content.Paragraphs.Add(ref missing);
            p7.Range.Font.Bold = 1;
            p7.Range.Font.Name = font;
            p7.Range.Font.Size = 14;
            p7.Range.Text = "Место: " + this.inform.Place;
            oStart = p7.Range.Start + p7.Range.Text.IndexOf(':');
            oEnd = p7.Range.Start + p7.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p7.Range.InsertParagraphAfter();

            Word.Paragraph p8 = doc.Content.Paragraphs.Add(ref missing);
            p8.Range.Font.Bold = 1;
            p8.Range.Font.Name = font;
            p8.Range.Font.Size = 14;
            p8.Range.Text = "Материальное обеспечение: " + this.inform.Material;
            oStart = p8.Range.Start + p8.Range.Text.IndexOf(':');
            oEnd = p8.Range.Start + p8.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p8.Range.InsertParagraphAfter();

            Word.Paragraph p9 = doc.Content.Paragraphs.Add(ref missing);
            p9.Range.Font.Bold = 1;
            p9.Range.Font.Name = font;
            p9.Range.Font.Size = 14;
            p9.Range.Text = "Литература: " + this.inform.Literature;
            oStart = p9.Range.Start + p9.Range.Text.IndexOf(':');
            oEnd = p9.Range.Start + p9.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p9.Range.InsertParagraphAfter();

            Word.Paragraph p10 = doc.Content.Paragraphs.Add(ref missing);
            p10.Range.Font.Bold = 1;
            p10.Range.Font.Name = font;
            p10.Range.Font.Size = 14;
            p10.Range.Text = "\nНачало по команде: \"" + this.inform.Comand + "\"";
            oStart = p10.Range.Start + p10.Range.Text.IndexOf(':');
            oEnd = p10.Range.Start + p10.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p10.Range.InsertParagraphAfter();

            Word.Paragraph p11 = doc.Content.Paragraphs.Add(ref missing);
            p11.Range.Font.Bold = 1;
            p11.Range.Font.Name = font;
            p11.Range.Font.Size = 14;
            p11.Range.Text = "Критерии оценки: Отлично - " + this.notes.Excellent + " c, Хорошо - " + this.notes.Good + " c, Удовлетворительно - " + this.notes.Satisfactory + " с.";
            oStart = p11.Range.Start + p11.Range.Text.IndexOf(':');
            oEnd = p11.Range.Start + p11.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p11.Range.InsertParagraphAfter();

            Word.Paragraph p12 = doc.Content.Paragraphs.Add(ref missing);
            p12.Range.Font.Bold = 1;
            p12.Range.Font.Name = font;
            p12.Range.Font.Size = 14;
            p12.Range.Text = "Оценка снижается: " + this.inform.Decreace;
            oStart = p12.Range.Start + p12.Range.Text.IndexOf(':');
            oEnd = p12.Range.Start + p12.Range.Text.Length;
            p = doc.Range(ref oStart, ref oEnd);
            p.Bold = 0;
            p12.Range.InsertParagraphAfter();

            //Создание таблицы
            var table = doc.Tables.Add(p12.Range, this.Tasks.Count + 5, 5, ref missing, ref missing);
            table.Borders.Enable = 1;
            table.Columns[1].Width = 30;
            table.Columns[2].Width = 75;
            table.Columns[3].Width = 165;
            table.Columns[4].Width = 165;
            table.Columns[5].Width = 75;

            //Заполнение заголовков таблицы
            for (int i = 1; i <= 5; i++)
            {
                Word.Cell hcell = table.Cell(1, i);
                string sname = "";
                switch (i)
                {
                    case 1: sname = "№"; break;
                    case 2: sname = "Название действия"; break;
                    case 3: sname = "Описание последовательности действий"; break;
                    case 4: sname = "Фото"; break;
                    case 5: sname = "Отметка"; break;
                }
                hcell.Range.Text = sname;
                hcell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                hcell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                hcell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                hcell.Range.Font.Bold = 1;
                hcell.Range.Font.Name = font;
                hcell.Range.Font.Size = 12;
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

            //Вставка поля с оценкой
            for (int i = 0; i < 4; i++)
            {
                Word.Cell fcell = table.Cell(table.Rows.Count - i, 1);
                fcell.Merge(table.Cell(table.Rows.Count - i, 4));
                fcell.Range.Font.Name = font;
                switch (i)
                {
                    case 0: fcell.Range.Text = "Итоговая оценка: "; break;
                    case 1: fcell.Range.Text = "Оценка снижена на количество баллов: "; break;
                    case 2: fcell.Range.Text = "Время выполнения: "; break;
                    case 3: fcell.Range.Text = "Процент выполнения: "; break;
                }
                fcell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                fcell.Range.Font.Size = 14;
                fcell.Range.Font.Bold = 1;
            }

            //Сохранение файла
            doc.SaveAs2(SaveTrack+@"\"/*Application.StartupPath + @"\"*/ + this.Inform.Course +" "+ this.inform.Name + ".docx");
            doc.Close(ref missing, ref missing, ref missing);
            doc = null;
            app.Quit(ref missing, ref missing, ref missing);
            app = null;
        }
    }
}
