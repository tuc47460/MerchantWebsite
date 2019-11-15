using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Utilities;
using System.Data;
using System.Data.SqlClient;
using TermProject.Models;

namespace TermProject
{
    public partial class LogIn : System.Web.UI.Page
    {
        
        Customer cust = new Customer();
        Merchant merc = new Merchant();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
                
            }
        }
        protected void UserLogin(object sender, EventArgs e)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            int UserId = -1 ;
            int Usertype= 0;
            int PermissionId = 0;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetUserId";
            SqlParameter inputParameter = new SqlParameter("@username", Login1.UserName);
            SqlParameter inputParameter2 = new SqlParameter("@password", Login1.Password);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 50;
            inputParameter2.Direction = ParameterDirection.Input;
            inputParameter2.SqlDbType = SqlDbType.VarChar;
            inputParameter2.Size = 50;
            objCommand.Parameters.Add(inputParameter);
            objCommand.Parameters.Add(inputParameter2);

            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            if(myDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in myDS.Tables[0].Rows)
                {
                    UserId = (int)dr["UserId"];
                    Usertype = (int)dr["UserType"];
                }
                    PermissionId = 0;
                if (Usertype == 1)
                {
                    cust = GetCustomer(UserId, Usertype);
                    Session["Customer"] = cust;
                    HttpCookie myCookie = new HttpCookie("Customer_ID");

                    myCookie.Values["Customer_ID"] = cust.CustomerID;
                    myCookie.Values["Username"] = Login1.UserName;
                    myCookie.Expires = new DateTime(2025, 1, 1);
                    Response.Cookies.Add(myCookie);

                    

                }
                if (Usertype == 2)
                {
                    merc = GetMerchant(UserId, Usertype);
                    HttpCookie myCookie = new HttpCookie("Customer_ID");

                    myCookie.Values["Customer_ID"] = merc.MerchantID;
                    myCookie.Values["Username"] = Login1.UserName;
                    myCookie.Expires = new DateTime(2025, 1, 1);
                    Response.Cookies.Add(myCookie);
                }
                if (Usertype == 3)
                {
                    
                    HttpCookie myCookie = new HttpCookie("Customer_ID");

                    myCookie.Values["Customer_ID"] = UserId.ToString();
                    myCookie.Values["Username"] = Login1.UserName;
                    myCookie.Expires = new DateTime(2025, 1, 1);
                    Response.Cookies.Add(myCookie);
                }
            }
            else
            {
                PermissionId = -1;
            }

           
            
                


                switch (PermissionId)
                {
                    case -1:
                        Login1.FailureText = "Usename or Password is incorrect";
                        break;
                    default:
                        FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
                        break;
                }//end switvh
            

        }//end user login


        public Customer GetCustomer(int UserId, int Usertype)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            Customer newCust = new Customer();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetCustomerByID";
            SqlParameter inputParameter = new SqlParameter("@UserId", UserId);

            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.Int;
            inputParameter.Size = 50;

            objCommand.Parameters.Add(inputParameter);


            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            foreach (DataRow dr in myDS.Tables[0].Rows)
            {
                newCust.Name = dr["name"].ToString();
                newCust.Address = dr["billing_address"].ToString();
                newCust.City = dr["billing_city"].ToString();
                newCust.State = dr["billing_state"].ToString();
                newCust.ZipCode = (int)dr["billing_zip"];
                newCust.Email = dr["email"].ToString();
                newCust.Phone = dr["phone"].ToString();
                newCust.ShippingAddress = dr["shipping_address"].ToString();
                newCust.ShippingCity = dr["shipping_city"].ToString();
                newCust.ShippingState = dr["shipping_state"].ToString();
                newCust.ShippingZipCode = (int)dr["shipping_zip"];
                newCust.UserType = Usertype;
                newCust.CustomerID = dr["Customer_ID"].ToString();
                



            }
            return newCust;
        }//end get customer

        public Merchant GetMerchant(int UserId, int Usertype)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            Merchant newMerc = new Merchant();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetMerchantByID";
            SqlParameter inputParameter = new SqlParameter("@UserId", UserId);

            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.Int;
            inputParameter.Size = 50;

            objCommand.Parameters.Add(inputParameter);


            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            foreach (DataRow dr in myDS.Tables[0].Rows)
            {
                newMerc.Name = dr["name"].ToString();
                newMerc.Address = dr["address"].ToString();
                newMerc.City = dr["city"].ToString();
                newMerc.State = dr["state"].ToString();
                newMerc.ZipCode = (int)dr["zip"];
                newMerc.Email = dr["email"].ToString();
                newMerc.Phone = dr["phone"].ToString();
                newMerc.UserType = Usertype;
                newMerc.Desc = dr["description"].ToString();
                newMerc.APIKey = dr["APIkey"].ToString();
                newMerc.MerchantID = dr["MerchantId"].ToString();



            }
            return newMerc;
        }//end get merc



    }//end login 
}//endnamespace