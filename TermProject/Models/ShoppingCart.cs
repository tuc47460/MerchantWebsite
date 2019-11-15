using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TermProject.Models
{
    [Serializable]
    public class Cart: List<Product>
    {
        public Cart()
        {

        }

        public double SubTotal()
        {
            double subprice = 0;

            foreach (Product p in this)
            {
                subprice += (p.Price * p.Quantity);
            }
            return subprice;
        }

        
    }
}