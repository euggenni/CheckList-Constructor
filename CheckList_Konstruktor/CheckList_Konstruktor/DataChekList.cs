using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CheckList_Konstruktor
{
    /// статичный класс данных чек листа для связи между формами
    public static class DataChekList
    {
        private static CheckList check = null; //хранит чек листы
        private static Subjects cource = null; //хранит предметы
        private static Platoons platoons = null; //хранит взвода
        private static string saveTrack = ""; //хранит путь сохранения
        private static bool encrypt = false; //проверяет, шифровать или нет

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

        public static string SaveTrack
        {
            get { return DataChekList.saveTrack; }
            set { DataChekList.saveTrack = value; }
        }

        public static bool Encrypt
        {
            get { return DataChekList.encrypt; }
            set { DataChekList.encrypt = value; }
        }

        public static void LoadSaveTrack(bool encrypt)
        {
            try
            {
                SaveTrack = File.ReadAllText(Application.StartupPath + @"\SaveTrack.tra");
                if (encrypt) SaveTrack = Sini4ka.Landing(SaveTrack, "синяя синичка");
            }
            catch (Exception e)
            {
                SaveTrack = "";
                MessageBox.Show(e.Message);
            }
        }
        public static void SaveSaveTrack(bool encrypt)
        {
            if (encrypt) SaveTrack = Sini4ka.Flying(SaveTrack, "синяя синичка");
            try
            {
                File.WriteAllText(Application.StartupPath + @"\SaveTrack.tra", SaveTrack);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            if (encrypt) SaveTrack = Sini4ka.Landing(SaveTrack, "синяя синичка");
        }

        public static void LoadEncrypt()
        { 
            
        }
    }
}
