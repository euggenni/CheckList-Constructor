using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    public class CheckList
    {
        private string course; //предмет, для которого создан чек лист
        private string name; //название чек листа
        private List<Task> tasks; //список всех тасков
        private Marks notes; //критерии оценки

        public CheckList(string Course, string Name, List<Task> Tasks, Marks Notes)

        {
            this.Course = Course;
            this.Name = Name;
            this.Tasks = Tasks;
            this.Notes = Notes;
        }

        public CheckList(string Course, string Name, Task Task_One, Marks Notes)
        {
            this.Course = Course;
            this.Name = Name;
            this.Tasks = new List<Task>();
            this.Tasks.Add(Task_One);
            this.Notes = Notes;
        }

        public CheckList(string Course, string Name)
        {
            this.Course = Course;
            this.Name = Name;
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
        }

        public CheckList()
        {
            this.Course = "Unknown";
            this.Name = "Unknown";
            this.Tasks = new List<Task>();
            this.Notes = new Marks();
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

        public Task ReadTaskAt(int i) //читаем пункт по номеру в списке 
        {
            return this.Tasks.ElementAt(i);
        }

        public int CountTasks() //возвращает число пунктов в списке
        {
            return this.Tasks.Count;
        }
    }
}
