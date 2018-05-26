using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    public class Subject
    {
        private int[] checkListIndexes;
        private string name;

        public Subject()
        {
            this.name = "";
        }
        public Subject(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
