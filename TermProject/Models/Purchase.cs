using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TermProject.Models;

namespace TermProject.Models
{
    public class Purchase
    {
        private int purchaseID;
        private int customerID;
        private Cart purchasedItems;
        private DateTime purchaseDate;

        public Purchase() { }//default constructor

        public int PurchaseID
        {
            get { return purchaseID; }
            set { purchaseID = value; }
        }

        public int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public Cart PurchasedItems
        {
            get { return purchasedItems; }
            set { purchasedItems = value; }
        }

        public DateTime PurchaseDate
        {
            get { return purchaseDate; }
            set { purchaseDate = value; }
        }

        public override string ToString()
        {

            //local var
            string purchaseString = "";
            double subTotal = 0;
            double purchaseTotal = 0;


            foreach (Product p in PurchasedItems)
            {
                purchaseTotal += p.Quantity * p.Price;
                subTotal = p.Quantity * p.Price;

                purchaseString += "Product ID: " + p.ProductID;
                purchaseString += "<br>";
                purchaseString += " Product Name: " + p.Title;
                purchaseString += "<br>";
                purchaseString += " Quantity: " + p.Quantity;
                purchaseString += "<br>";
                purchaseString += " Price: <b>$" + p.Price + "</b>";
                purchaseString += "<br>";
                purchaseString += " Sub Total: <b>$" + subTotal + "</b>";
                purchaseString += "<br>";
                purchaseString += "<br>";
            }//end foreach

            purchaseString += "<br>";
            purchaseString += "Total: <b>$" + purchaseTotal + "</b>";
            purchaseString += "<br>";
            purchaseString += "Purchase Date: " + this.PurchaseDate.ToShortDateString() +
                " " + this.PurchaseDate.ToShortTimeString();
            purchaseString += "<br><br>";

            return purchaseString;
        }
    }//end Purchase
}