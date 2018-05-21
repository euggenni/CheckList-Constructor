using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList_Konstruktor
{
    public class Task
    {
        private string name; //имя пункта
        private string description; //полное описание пункта
        private string image; //фото для пункта

        public Task(string Name, string Description, string Image)
        {
            this.Name = Name;
            this.Description = Description;
            this.Image = Image;
        }

        public Task(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            this.Image = null;
        }

        public Task()
        {
            this.Name = "Unknown";
            this.Description = "Unknown";
            this.Image = null;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }
    }
}
