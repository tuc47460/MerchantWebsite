using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utilities;
using System.ComponentModel;
using TermProject.Models;

namespace TermProject
{
    public partial class CustomerInfo : System.Web.UI.UserControl
    {
        //var
        string id;
        string name;
        string email;
        string phone;
        string address;
        string city;
        string state;
        string zip;
        string saddress;
        string scity;
        string sstate;
        string szip;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Category("Misc")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }//end id

        [Category("Misc")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }//end name

        [Category("Misc")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }//end email

        [Category("Misc")]
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }//end phone

        [Category("Misc")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }//end address

        [Category("Misc")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }//end city

        [Category("Misc")]
        public string State
        {
            get { return state; }
            set { state = value; }
        }//end state

        [Category("Misc")]
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }//end zip

        [Category("Misc")]
        public string ShippingAddress
        {
            get { return saddress; }
            set { saddress = value; }
        }//end address

        [Category("Misc")]
        public string ShippingCity
        {
            get { return scity; }
            set { scity = value; }
        }//end city

        [Category("Misc")]
        public string ShippingState
        {
            get { return sstate; }
            set { sstate = value; }
        }//end state

        [Category("Misc")]
        public string ShippingZip
        {
            get { return szip; }
            set { szip = value; }
        }//end zip

        public void FillCustomerInfo(int customerID)
        {
            //local var
            Customer cust = Account.GetCustomerInfo(customerID);

            lblName.Text = "Name: " + cust.Name;
            lblEmail.Text = "Email: " + cust.Email;
            lblPhone.Text = "Phone: " + cust.Phone;
            lblAddress.Text = "Address: " + cust.Address;
            lblCity.Text = "City: " + cust.City;
            lblState.Text = "State: " + cust.State;
            lblZip.Text = "ZipCode: " + cust.ZipCode;
            lblSAddress.Text = "Shipping Address: " + cust.ShippingAddress;
            lblSCity.Text = "Shipping City: " + cust.ShippingCity;
            lblSState.Text = "Shipping State: " + cust.ShippingState;
            lblSZip.Text = "Shipping ZipCode: " + cust.ShippingZipCode;
        }//end GetCustomerInfo

    }
}