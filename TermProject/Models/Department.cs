using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TermProject.Models
{
    public class Department
    {
        public int Department_id { get; set; }
        public string Name { get; set; }
        public string img_url { get; set; }
        public int Merchant_Id { get; set; }

        public Department() { }//default constructor
    }
}