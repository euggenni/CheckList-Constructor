using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    public class Marks
    {
        private int excellent; //отлично
        private int good; //хорошо
        private int satisfactory; //удовлетворительно

        public Marks(int Excellent, int Good, int Satisfactory)
        {
            this.Excellent = Excellent;
            this.Good = Good;
            this.Satisfactory = Satisfactory;
        }

        public Marks(string Excellent, string Good, string Satisfactory)
        {
            try
            {
                this.Excellent = Convert.ToInt32(Excellent);
            }
            catch(Exception)
            {
                this.Excellent = 0;
            }
            try
            {
                this.Good = Convert.ToInt32(Good);
            }
            catch (Exception)
            {
                this.Good = 0;
            }
            try
            {
                this.Satisfactory = Convert.ToInt32(Satisfactory);
            }
            catch (Exception)
            {
                this.Satisfactory = 0;
            }
        }

        public Marks()
        {
            this.Excellent = 0;
            this.Good = 0;
            this.Satisfactory = 0;
        }

        public int Excellent
        {
            get { return excellent; }
            set { excellent = value; }
        }

        public int Good
        {
            get { return good; }
            set { good = value; }
        }

        public int Satisfactory
        {
            get { return satisfactory; }
            set { satisfactory = value; }
        }
    }
}
