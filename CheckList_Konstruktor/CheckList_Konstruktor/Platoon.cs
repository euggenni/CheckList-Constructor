using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace CheckList_Konstruktor
{
    class Platoon
    {
        private int platNum;
        List<Student> students;

        public Platoon()
        {
            this.PlatNum = 0;
            this.students = new List<Student>();
        }
        public Platoon(int platNum, List<Student> students)
        {
            this.PlatNum = platNum;
            this.students = students;
        }
        public Platoon(int platNum)
        {
            this.platNum = platNum;
            this.students = new List<Student>();
        }

        public int PlatNum
        {
            get { return platNum; }
            set { platNum = value; }
        }
        public List<Student> Students
        {
            get { return students; }
            set { students = value; }
        }
    }
}
