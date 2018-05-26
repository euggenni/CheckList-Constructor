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
        public static CheckList Check = null; //хранит чек листы
        public static Subjects Cource = null; //хранит предметы
    }
}
