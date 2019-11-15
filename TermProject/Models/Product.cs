using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermProject.Models
{
    [Serializable]
    public class Product
    {
        private String productID;
        private String title;
        private String description;
        private double price;
        private int quantity;
        private String imageUrl;
        private String departmentID;
        private int merchantId;
        
        public Product()
        {

        }

        public String ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public String ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public String DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }
        public int MerchantID
        {
            get { return merchantId; }
            set { merchantId = value; }
        }
    }
}
