using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    /// статичный класс данных чек листа для связи между формами
    public static class DataChekList
    {
        private static CheckList check = null; //хранит чек листы
        private static Subjects cource = null; //хранит предметы
        private static Platoons platoons = null; //хранит взвода

        public static CheckList Check
        {
            get { return DataChekList.check; }
            set { DataChekList.check = value; }
        }

        public static Subjects Cource
        {
            get { return DataChekList.cource; }
            set { DataChekList.cource = value; }
        }

        internal static Platoons Platoons
        {
            get { return DataChekList.platoons; }
            set { DataChekList.platoons = value; }
        }

    }
}
