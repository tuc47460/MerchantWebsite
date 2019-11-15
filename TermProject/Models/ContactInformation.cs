using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermProject.Models
{
    public class ContactInformation
    {
        private String name;
        private String address;
        private String city;
        private String state;
        private int zipCode;
        private String email;
        private String phoneNumber;
        private String Shippingaddress;
        private String Shippingcity;
        private String Shippingstate;
        private int ShippingzipCode;
        private int usertype;

        public ContactInformation()
        {

        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        public String City
        {
            get { return city; }
            set { city = value; }
        }

        public String State
        {
            get { return state; }
            set { state = value; }
        }

        public int ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String Phone
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public String ShippingAddress
        {
            get { return Shippingaddress; }
            set { Shippingaddress = value; }
        }

        public String ShippingCity
        {
            get { return Shippingcity; }
            set { Shippingcity = value; }
        }

        public String ShippingState
        {
            get { return Shippingstate; }
            set { Shippingstate = value; }
        }

        public int ShippingZipCode
        {
            get { return ShippingzipCode; }
            set { ShippingzipCode = value; }
        }

        public int UserType
        {
            get { return usertype; }
            set { usertype = value; }
        }
    }
}
