using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    class Student
    {
        private string fio;

        public Student()
        {
            this.fio = "";
        }
        public Student(string fio)
        {
            this.fio = fio;
        }

        public string Fio
        {
            get { return fio; }
            set { fio = value; }
        }
    }
}
