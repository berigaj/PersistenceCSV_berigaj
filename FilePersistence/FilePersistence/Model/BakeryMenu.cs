using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePersistence.Model
{
    public class BakeryMenu
    {
        public string Flavor { get; set; }
        public double Price { get; set; }

        public BakeryMenu()
        {

        }

        public BakeryMenu(string flavor, double price)
            {
                this.Flavor = flavor;
                this.Price = price;
            }

    }
}
