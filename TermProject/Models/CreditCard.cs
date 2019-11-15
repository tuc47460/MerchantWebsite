using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TermProject.Models
{
    [Serializable]

    public class CreditCard
    {

        private String cardNumber;
        private String cardType;
        private int expirationMonth;
        private int expirationYear;

        public CreditCard() { }//default constructor

        public String CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        public String CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        public int ExpirationMonth
        {
            get { return expirationMonth; }
            set { expirationMonth = value; }
        }

        public int ExpirationYear
        {
            get { return expirationYear; }
            set { expirationYear = value; }
        }

    }//end credit card class
}