using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Threading;

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

        //не трогать
        private Word.Application app = null;
        private Word.Document doc = null;
        object missing = System.Reflection.Missing.Value;

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
            this.index = 0;
            this.inform = new Title("Unknown", "Unknown");
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
            this.hasTimer = false;
        }


        //секция свойств полей карточки задания
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
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
        public void PrintPar(string text, int align, bool subPar)//Вывод параграфа
        {
            Word.Paragraph p = doc.Content.Paragraphs.Add(ref missing);
            p.Range.Font.Bold = 1;
            p.Range.Font.Size = 14;
            p.Range.Text = text;
            switch (align)
            {
                case 0: p.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; break;
                case 1: p.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; break;
                case 2: p.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight; break;
            }
            if (subPar)
            {
                object oStart = null;
                object oEnd = null;
                if (text.Contains("Занятие №"))
                {
                    oStart = p.Range.Start + p.Range.Text.IndexOf('\"');
                    oEnd = p.Range.Start + p.Range.Text.LastIndexOf('\"');
                }
                else
                {
                    oStart = p.Range.Start + p.Range.Text.IndexOf(':');
                    oEnd = p.Range.Start + p.Range.Text.Length;
                }
                Word.Range pl = doc.Range(ref oStart, ref oEnd);
                pl.Bold = 0;
            }
            p.Range.InsertParagraphAfter();
        }
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
            this.inform.Time = 90;
            this.inform.Topic = "Сборка и разборка оружия";*/
            //конец тестового блока
            WordProgress progress = new WordProgress();
            progress.ProgressBarForm(((this.tasks.Count + 1) * 5) + 21);
            progress.label2.Text = SaveTrack + @"\" + this.Inform.Course + " " + this.inform.Name + ".doc";
            progress.Show();

            //открытие приложения Word
            app = new Word.Application();
            app.Visible = false;
            progress.ProgressBarInc();

            //создание нового документа Word
            doc = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            progress.ProgressBarInc();
            doc.PageSetup.TopMargin = 35;
            doc.PageSetup.RightMargin = 35;
            doc.PageSetup.BottomMargin = 35;
            doc.PageSetup.LeftMargin = 35;
            object oStart = doc.Content.Start;
            object oEnd = doc.Content.End;
            Word.Range docRange = doc.Range(ref oStart, ref oEnd);
            docRange.Font.Name = "Times New Roman";
            doc.Paragraphs.SpaceAfter = 0;
            progress.ProgressBarInc();

            //блок титульного листа1
            //строка "Карточка задания"
            this.PrintPar("КАРТОЧКА ЗАДАНИЯ", 1, false);
            progress.ProgressBarInc();
            //Строка с названием предмета
            this.PrintPar(this.inform.Course, 1, false);
            progress.ProgressBarInc();
            //Строка с номером и темой занятия
            this.PrintPar("Занятие №" + this.inform.ClassNum + " \"" + this.inform.Topic + "\"", 1, true);
            progress.ProgressBarInc();
            //Строка с названием карточки задания
            this.PrintPar("\"" + this.inform.Name + "\"", 1, false);
            progress.ProgressBarInc();

            //блок титульного листа2
            //Строка с целью занятия
            this.PrintPar("\nЦель: " + this.inform.Purpose, 0, true);
            progress.ProgressBarInc();
            //Строка с временем занятия
            this.PrintPar("Время: " + this.inform.Time + " мин.", 0, true);
            progress.ProgressBarInc();
            //Строка с местом проведения занятия
            this.PrintPar("Место: " + this.inform.Place, 0, true);
            progress.ProgressBarInc();
            //Строка с материальным обеспечением занятия
            this.PrintPar("Материальное обеспечение: " + this.inform.Material, 0, true);
            progress.ProgressBarInc();
            //Строка с литературой
            this.PrintPar("Литература: " + this.inform.Literature, 0, true);
            progress.ProgressBarInc();

            //Блок титульного листа 3
            //Строка команды к началу действий
            this.PrintPar("\nНачало по команде: \"" + this.inform.Comand + "\"", 0, true);
            progress.ProgressBarInc();
            //Строка с критериями оценки
            this.PrintPar("Критерии оценки: Отлично - " + this.notes.Excellent + " c, Хорошо - " + this.notes.Good + " c, Удовлетворительно - " + this.notes.Satisfactory + " с.", 0, true);
            progress.ProgressBarInc();
            //Строка с критериями снижения оценки
            this.PrintPar("Оценка снижается: " + this.inform.Decreace, 0, true);
            progress.ProgressBarInc();

            //Создание таблицы
            Word.Paragraph p = doc.Content.Paragraphs.Add(ref missing);
            var table = doc.Tables.Add(p.Range, this.Tasks.Count + 5, 5, ref missing, ref missing);
            table.Borders.Enable = 1;
            table.Columns[1].Width = 30;
            table.Columns[2].Width = 75;
            table.Columns[3].Width = 165;
            table.Columns[4].Width = 165;
            table.Columns[5].Width = 75;
            progress.ProgressBarInc();

            Word.Cell cell;
            //Заполнение заголовков таблицы
            for (int i = 1; i <= 5; i++)
            {
                cell = table.Cell(1, i);
                string sname = "";
                switch (i)
                {
                    case 1: sname = "№"; break;
                    case 2: sname = "Название действия"; break;
                    case 3: sname = "Порядок выполнения"; break;
                    case 4: sname = "Контроль"; break;
                    case 5: sname = "Выполнено"; break;
                }
                cell.Range.Text = sname;
                cell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cell.Range.Font.Bold = 1;
                cell.Range.Font.Size = 12;
                progress.ProgressBarInc();
            }

            //Запись листа в таблицу
            for (int i = 0; i < this.Tasks.Count; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    cell = table.Cell(i + 2, j);
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
                                    try
                                    {
                                        Rename(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + this.Tasks[i].Image, true);
                                        cell.Range.InlineShapes.AddPicture(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + string.Concat(this.Tasks[i].Image.Remove(this.Tasks[i].Image.LastIndexOf('.')), ".jpeg"));
                                        Rename(DataChekList.SaveTrack + @"\" +/*Application.StartupPath +*/@"\CheckList\Pictures\" + string.Concat(this.Tasks[i].Image.Remove(this.Tasks[i].Image.LastIndexOf('.')), ".jpeg"), false);
                                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show(e.Message);
                                    }
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
                    progress.ProgressBarInc();
                }
            }

            //Вставка поля с оценкой
            for (int i = 0; i < 4; i++)
            {
                cell = table.Cell(table.Rows.Count - i, 1);
                cell.Merge(table.Cell(table.Rows.Count - i, 4));
                switch (i)
                {
                    case 0: cell.Range.Text = "Итоговая оценка: "; break;
                    case 1: cell.Range.Text = "Оценка снижена на количество баллов: "; break;
                    case 2: cell.Range.Text = "Время выполнения: "; break;
                    case 3: cell.Range.Text = "Процент выполнения: "; break;
                }
                cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                cell.Range.Font.Size = 14;
                cell.Range.Font.Bold = 1;
                progress.ProgressBarInc();
            }

            //Сохранение файла
            try
            {
                doc.SaveAs2000(SaveTrack + @"\" + this.Inform.Course + " " + this.inform.Name + ".doc");
                progress.ProgressBarInc();
                doc.Close(ref missing, ref missing, ref missing);
                doc = null;
                app.Quit(ref missing, ref missing, ref missing);
                app = null;
                progress.ProgressBarInc();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            progress.Visible = false;
        }

        public static void Rename(string fileName, bool avers) //меняет тип файла из bin в jpeg и обратно
        {
            try
            {
                string newType = string.Copy(fileName);
                if (avers) //в jpeg
                {
                    newType = string.Concat(newType.Remove(newType.LastIndexOf('.')), ".jpeg");
                }
                else //в bin
                {
                    newType = string.Concat(newType.Remove(newType.LastIndexOf('.')), ".bin");
                }
                File.Move(fileName, newType);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
