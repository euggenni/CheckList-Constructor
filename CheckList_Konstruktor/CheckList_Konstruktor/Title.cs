using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    public class Title
    {
        //секция полей титульного листа
        private string name; //название карточки задания
        private string course; //предмет, для которого создана карточка задания
        private int classNum; //номер занятия
        private string purpose; //цель занятия
        private int time; //время на занятие 
        private string place; //место проведения занятия
        private string material; //материальное обеспечение занятия
        private string literature; //литература
        private string decreace; //оценка снижается...

        //секция конструкторов
        public Title()
        {
            this.name = "";
            this.course = "";
            this.classNum = 0;
            this.purpose = "";
            this.time = 0;
            this.place = "";
            this.material = "";
            this.literature = "";
            this.decreace = "";
        }
        public Title(string name, string course)
        {
            this.name = name;
            this.course = course;
        }
        //секция свойств полей титульного листа
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Course
        {
            get { return course; }
            set { course = value; }
        }
        public int ClassNum
        {
            get { return classNum; }
            set { classNum = value; }
        }
        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }
        public int Time
        {
            get { return time; }
            set { time = value; }
        }
        public string Place
        {
            get { return place; }
            set { place = value; }
        }
        public string Material
        {
            get { return material; }
            set { material = value; }
        }
        public string Literature
        {
            get { return literature; }
            set { literature = value; }
        }
        public string Decreace
        {
            get { return decreace; }
            set { decreace = value; }
        }
    }
}
