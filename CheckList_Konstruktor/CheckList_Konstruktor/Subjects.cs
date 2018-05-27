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
    public class Subjects
    {
        private List<Subject> subList;

        public Subjects()
        {
            this.subList = new List<Subject>();
        }
        public Subjects(List<Subject> subList)
        {
            this.subList = subList;
        }
        public Subjects(Subject sub)
        {
            this.subList = new List<Subject>();
            this.subList.Add(sub);
        }

        public List<Subject> SubList
        {
            get { return subList; }
            set { subList = value; }
        }

        //секция методов
        public void AddSubject(Subject sub) //добавление предмета в список
        {
            this.subList.Add(sub);
        }
        public void RemoveTask(int i) //удаление предмета из списка
        {
            this.subList.RemoveAt(i);
        }
        public Subject ReadSubAt(int i) //получаем предмет по номеру в списке
        {
            return this.subList.ElementAt(i);
        }
        public int CountTasks() //возвращает число предметов
        {
            return this.subList.Count;
        }

        public void SaveSubList(bool encrypt)
        {
            string data = JsonConvert.SerializeObject(this);
            if (encrypt) data = Sini4ka.Flying(data, "синяя синичка");
            try
            {
                File.WriteAllText(@DataChekList.SaveTrack + /*Application.StartupPath +*/@"\CheckList\Inform\subjects.sub", data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static Subjects LoadSubList(bool encrypt) //чтение предметов
        {
            String data = "";
            try
            {
                data = File.ReadAllText(@DataChekList.SaveTrack + /*Application.StartupPath +*/@"\CheckList\Inform\subjects.sub");
                if (encrypt) data = Sini4ka.Landing(data, "синяя синичка");
                return JsonConvert.DeserializeObject<Subjects>(data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new Subjects();
            }
        }
    }
}
