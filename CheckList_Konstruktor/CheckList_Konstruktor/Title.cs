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
        private int classNum = 0; //номер занятия
        private string topic = ""; //тема занятия
        private string purpose = ""; //цель занятия
        private int time = 0; //время на занятие 
        private string place = ""; //место проведения занятия
        private string material = ""; //материальное обеспечение занятия
        private string literature = ""; //литература
        private string comand = ""; //команда к началу действий
        private string decreace = ""; //оценка снижается...

        //секция конструкторов
        public Title(string name, string course, int classNum, string topic, string purpose, int time, string place, string material, string literature, string comand, string decreace)
        {
            this.Name = name;
            this.Course = course;
            this.ClassNum = classNum;
            this.Topic = topic;
            this.Purpose = purpose;
            this.Time = time;
            this.Place = place;
            this.Material = material;
            this.Literature = literature;
            this.Comand = Comand;
            this.Decreace = decreace;
        }
        public Title(string name, string course)
        {
            this.Name = name;
            this.Course = course;
        }
        public Title()
        {
            this.Name = "";
            this.Course = "";
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
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
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
        public string Comand
        {
            get { return comand; }
            set { comand = value; }
        }
        public string Decreace
        {
            get { return decreace; }
            set { decreace = value; }
        }
    }
}
