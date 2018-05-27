using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace CheckList_Konstruktor
{
    class Platoons
    {
        List<Platoon> platList;

        public Platoons()
        {
            this.platList = new List<Platoon>();
        }
        public Platoons(List<Platoon> platList)
        {
            this.platList = platList;
        }
        public Platoons(Platoon plat)
        {
            this.platList = new List<Platoon>();
            this.platList.Add(plat);
        }

        public List<Platoon> PlatList
        {
            get { return platList; }
            set { platList = value; }
        }

        //секция методов
        public void AddPlatoon(Platoon plat) //добавление предмета в список
        {
            this.platList.Add(plat);
        }
        public void RemovePlatoon(int i) //удаление предмета из списка
        {
            this.platList.RemoveAt(i);
        }
        public Platoon ReadPlatAt(int i) //получаем предмет по номеру в списке
        {
            return this.platList.ElementAt(i);
        }
        public int CountPlatoons() //возвращает число предметов
        {
            return this.platList.Count;
        }

        public void SavePlatList(bool encrypt)
        {
            string data = JsonConvert.SerializeObject(this);
            if (encrypt) data = Sini4ka.Flying(data, "синяя синичка");
            try
            {
                File.WriteAllText(DataChekList.SaveTrack + /*Application.StartupPath +*/@"\CheckList\Inform\Platoons.plat", data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static Platoons LoadPlatList(bool encrypt) //чтение предметов
        {
            String data = "";
            try
            {
                data = File.ReadAllText(DataChekList.SaveTrack + /*Application.StartupPath +*/@"\CheckList\Inform\Platoons.plat");
                if (encrypt) data = Sini4ka.Landing(data, "синяя синичка");
                return JsonConvert.DeserializeObject<Platoons>(data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new Platoons();
            }
        }
    }
}
