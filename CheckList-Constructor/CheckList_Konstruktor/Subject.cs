using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    public class Subject
    {
        private List<string> checkListIndexes = new List<string>();
        private string name;

        public Subject()
        {
            this.Name = "";
            this.CheckListIndexes = new List<string>();
        }
        public Subject(string name)
        {
            this.Name = name;
            this.CheckListIndexes = new List<string>();
        }

        public Subject(string name, List<string> checkListIndexes)
        {
            this.Name = name;
            this.CheckListIndexes = checkListIndexes;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> CheckListIndexes
        {
            get { return checkListIndexes; }
            set { checkListIndexes = value; }
        }
    }
}
