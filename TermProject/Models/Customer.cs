using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TermProject.Models
{
    public class Customer : ContactInformation
    {
        private String customerID;
        
        public String CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
    }
}
