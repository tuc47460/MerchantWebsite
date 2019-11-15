using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TermProject.Models
{
    public class Credit
    {
        public int CreditID { get; set; }
        public CreditCard CreditCard { get; set; }
        public int CustomerID { get; set; }
        public bool Default {get;set;}

        public Credit() { }//default constructor

    }//end Credit
}