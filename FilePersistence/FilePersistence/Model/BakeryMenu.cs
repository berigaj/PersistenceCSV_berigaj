using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePersistence.Model
{
    class BakeryMenu
    {
        public int ID { get; set; }
        public string Flavor { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public enum MenuType { food, drink }
        public MenuType type { get; set; }

        public BakeryMenu (int ID, string Flavor, string Category, double Price, MenuType type)
            {
                this.ID = ID;
                this.Flavor = Flavor;
                this.Category = Category;
                this.Price = Price;
            }

    }
}
