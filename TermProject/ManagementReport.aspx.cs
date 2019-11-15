using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Data;
using TermProject.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlClient;
using Utilities;
using System.Web.UI.HtmlControls;

namespace TermProject
{
    public partial class ManagementReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<User> Userlist = new List<User>() ;
            double total = 0;

            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            if (Request.Cookies["Customer_ID"] != null)

            {

                HttpCookie cookie = Request.Cookies["Customer_ID"];

                string user = cookie.Values["Username"].ToString();
                if (user != "Admin")
                {
                    Response.Redirect("Home.aspx");
                }
                
                    DBConnect objDB = new DBConnect();
                    SqlCommand objCommand = new SqlCommand();

                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.CommandText = "TP_GetUsers";





                    DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

                    foreach (DataRow dr in myDS.Tables[0].Rows)
                    {
                        User newuser = new User();
                        Customer newcust = new Customer();
                        newuser.username =   dr["Username"].ToString();
                        int id = GetInt(dr["UserId"].ToString());
                        newcust = GetCust(id);
                        newuser.email = newcust.Email;
                        newuser.name = newcust.Name;
                        List<Purchase> PList = GetAllPurchases(GetInt(newcust.CustomerID));
                        if (PList != null)
                        {
                            foreach (Purchase p in PList)
                            {
                                total += p.PurchasedItems.SubTotal();
                            }
                            newuser.totalsales = total;
                            Userlist.Add(newuser);
                        }
                    }//end foreach
                    gvCustomerSales.DataSource = Userlist;
                    gvCustomerSales.DataBind();
                    CreditCards.Visible = false;
                    GetInvetory();
                    DisplayAllMerchantPurchases();
                
            }//end cookie request
        }//endpage load

        public Customer GetCust(int Userid)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetCustomerByID";
            SqlParameter inputParameter = new SqlParameter("@UserId", Userid);

            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.Int;
            inputParameter.Size = 50;

            objCommand.Parameters.Add(inputParameter);


            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);
            Customer temp = new Customer();
            foreach (DataRow dr in myDS.Tables[0].Rows)
            {
                
                temp.Name= dr["name"].ToString();
                temp.Email = dr["email"].ToString();
               temp.CustomerID = dr["Customer_ID"].ToString();

            }
            return temp;
        }//end getcust

        protected List<Purchase> GetAllPurchases(int CustomerId)
        {
            //local var
            DBConnect dbconn = new DBConnect();
            List<Purchase> PList = new List<Purchase>();

            SqlCommand sqlcomm = new SqlCommand("TP_GetPurchasesByCustomerID");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", CustomerId));
            DataSet TheDataSet = new DataSet();
            try
            {
                //get data set
                TheDataSet = dbconn.GetDataSetUsingCmdObj(sqlcomm);

                if (TheDataSet.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in TheDataSet.Tables[0].Rows)
                    {
                        Purchase P = new Purchase();

                        if (dr["Cart"] != DBNull.Value)
                        {
                            Byte[] byteArray = (Byte[])dr["Cart"];
                            BinaryFormatter deSerializer = new BinaryFormatter();

                            MemoryStream memStream = new MemoryStream(byteArray);

                            P.PurchasedItems = (Cart)deSerializer.Deserialize(memStream);

                            memStream.Flush();
                        }

                        P.PurchaseID = (int)dr["Purchase_ID"];
                        P.PurchaseDate = (DateTime)dr["Date"];

                        PList.Add(P);
                    }//end foreach
                    return PList;
                }//end if
                else
                {
                    return null;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return null;
            }//end catch sql ex
            catch (Exception ex)
            {
                return null;
            }//end catch
        }//end GetAllPurchases

        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt

        public void GetInvetory()
        {
            List<Product> ProductList = new List<Product>();
            String webApiUrl = "http://cis-iis2.temple.edu/Spring2019/CIS3342_tug91514/TermProjectWS/api/service/Merchants";
            for (int i = 1; i < 4; i++)
            {
                WebRequest request = WebRequest.Create(webApiUrl + "/Products?DepartmentNumber=" + i);
                WebResponse response = request.GetResponse();

                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream);
                string data = reader.ReadToEnd();
                reader.Close();
                response.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                List<Product>  ProductList2 = js.Deserialize<List<Product>>(data);
                foreach(Product p in ProductList2)
                {
                    ProductList.Add(p);
                }
               
            }
            gvInvetory.DataSource = ProductList;
            gvInvetory.DataBind();
        }//end get invetory

        protected void DisplayAllMerchantPurchases()
        {
            int id = 0;
            Purchases.Controls.Add(GetAllProductsByMerchant(id, GetAllPurchases()));
        }//end DisplayAllMerchantPurchase


        protected HtmlGenericControl GetAllProductsByMerchant(int merchantID,
           List<Purchase> PList)
        {
            //local var
            HtmlGenericControl MerchPurchase = new HtmlGenericControl();

            foreach (Purchase p in PList)
            {
                //local var
                HtmlGenericControl Prods = new HtmlGenericControl();
                string ProdList = "";
                double total = 0;

                if (HasItemInPurchase(p, merchantID))
                {
                    //get customer info
                    Customer cust = GetCustomerInfo(p.CustomerID);

                    ProdList += "</br>";
                    ProdList += "Purchase ID: " + p.PurchaseID;
                    ProdList += "</br>";
                    ProdList += "Purchase Date: " + p.PurchaseDate.ToShortDateString()
                        + " " + p.PurchaseDate.ToShortTimeString();
                    ProdList += "</br>";
                    ProdList += "Customer ID: " + p.CustomerID;
                    ProdList += "</br>";
                    ProdList += "Name: " + cust.Name;
                    ProdList += "</br>";
                    ProdList += "Email: " + cust.Email;
                    ProdList += "</br>";
                    ProdList += "Phone: " + cust.Phone;
                    ProdList += "</br>";
                    ProdList += "</br>";

                }//end 
                foreach (Product prod in p.PurchasedItems)
                {
                    if (prod.MerchantID.Equals(merchantID))
                    {
                        total += prod.Quantity * prod.Price;


                        ProdList += "Product ID : " + prod.ProductID;
                        ProdList += "</br>";
                        ProdList += "Product Name: " + prod.Title;
                        ProdList += "</br>";
                        ProdList += "Product Description: " + prod.Description;
                        ProdList += "</br>";
                        ProdList += "Quantity: " + prod.Quantity;
                        ProdList += "</br>";
                        ProdList += "Price: $" + prod.Price;
                        ProdList += "</br>";
                        ProdList += "Sub Total: $" + prod.Price * prod.Quantity;
                        ProdList += "</br>";
                        ProdList += "</br>";

                    }//end if
                }//end foreach
                if (HasItemInPurchase(p, merchantID))
                {
                    ProdList += "Total: $" + total;
                    ProdList += "</br>";
                    ProdList += "______________________________";
                    ProdList += "</br>";
                    ProdList += "</br>";

                    Button btnCustomerInfo = new Button();
                    btnCustomerInfo.Text = "Customer Info";
                    btnCustomerInfo.ToolTip = p.CustomerID.ToString();
                    btnCustomerInfo.Click += new EventHandler(this.btnGetCustomerInfo_Click);
                    Prods.InnerHtml = ProdList;
                    MerchPurchase.Controls.Add(btnCustomerInfo);
                    MerchPurchase.Controls.Add(Prods);

                }//end if
            }//end foreach

            return MerchPurchase;
        }//end GetAllProductsByMerchant

        protected List<Purchase> GetAllPurchases()
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_GetAllPurchases");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            DataSet TheDataSet = new DataSet();

            try
            {
                //get data set
                TheDataSet = dbconn.GetDataSetUsingCmdObj(sqlcomm);
                List<Purchase> PList = new List<Purchase>();

                if (TheDataSet.Tables[0].Rows.Count > 0)
                {
                    //local var
                    Purchase p = new Purchase();

                    foreach (DataRow dr in TheDataSet.Tables[0].Rows)
                    {
                        Purchase P = new Purchase();

                        if (dr["Cart"] != DBNull.Value)
                        {
                            Byte[] byteArray = (Byte[])dr["Cart"];
                            BinaryFormatter deSerializer = new BinaryFormatter();

                            MemoryStream memStream = new MemoryStream(byteArray);

                            P.PurchasedItems = (Cart)deSerializer.Deserialize(memStream);

                            memStream.Flush();
                        }

                        P.PurchaseID = (int)dr["Purchase_ID"];
                        P.PurchaseDate = (DateTime)dr["Date"];
                        P.CustomerID = (int)dr["Customer_ID"];

                        PList.Add(P);
                    }//end foreach
                    return PList;
                }//end if
                else
                {
                    return null;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return null;
            }//end catch sql ex
            catch (Exception ex)
            {
                return null;
            }//end catch
        }//end GetAllMerhcantPurchases

        protected bool HasItemInPurchase(Purchase P, int merchantID)
        {
            foreach (Product prod in P.PurchasedItems)
            {
                if (prod.MerchantID.Equals(merchantID))
                {
                    return true;
                }//end if
            }//end foreach
            return false;
        }//end HasITemInPurchase

        protected Customer GetCustomerInfo(int customerID)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_CustomerByCustomerID");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", customerID));
            DataSet TheDataSet = new DataSet();

            try
            {
                //get data set
                TheDataSet = dbconn.GetDataSetUsingCmdObj(sqlcomm);

                if (TheDataSet.Tables[0].Rows.Count == 1)
                {
                    //local var
                    Customer cust = new Customer();

                    foreach (DataRow dr in TheDataSet.Tables[0].Rows)
                    {


                        cust.Name = (string)dr["name"];
                        cust.Address = (string)dr["billing_address"];
                        cust.City = (string)dr["billing_city"];
                        cust.State = (string)dr["billing_state"];
                        cust.ZipCode = (int)dr["billing_zip"];
                        cust.Phone = (string)dr["phone"];
                        cust.Email = (string)dr["email"];
                        cust.ShippingAddress = (string)dr["shipping_address"];
                        cust.ShippingCity = (string)dr["shipping_city"];
                        cust.ShippingState = (string)dr["shipping_state"];
                        cust.ShippingZipCode = (int)dr["shipping_zip"];
                    }//end foreach

                    return cust;
                }//end if
                else
                {
                    return null;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return null;
            }//end catch sql ex
            catch (Exception ex)
            {
                return null;
            }//end catch
        }//end GetCustomerInfo

        protected void btnGetCustomerInfo_Click(object sender, EventArgs e)
        {
            //local var
            Customer cust = new Customer();
            string s = "";

            //get button
            Button btn = (Button)sender;

            //get customer info
            cust = GetCustomerInfo(GetInt(btn.ToolTip));

            CreditHeader.Visible = true;
            CreditHeader.InnerText = "Customer Info";

            //show div
            CreditCards.Visible = true;

            //insert customer info
            s += "Name: " + cust.Name;
            s += "</br>";
            s += "Email: " + cust.Email;
            s += "</br>";
            s += "Phone: " + cust.Phone;
            s += "</br>";
            s += "Billing Address: " + cust.Address;
            s += "</br>";
            s += "Billing City: " + cust.City;
            s += "</br>";
            s += "Billing State: " + cust.State;
            s += "</br>";
            s += "Billing ZipCode: " + cust.ZipCode;
            s += "</br>";
            s += "Shipping Address: " + cust.ShippingAddress;
            s += "</br>";
            s += "Shipping City: " + cust.ShippingCity;
            s += "</br>";
            s += "Shipping State: " + cust.ShippingState;
            s += "</br>";
            s += "Shipping ZipCode: " + cust.ShippingZipCode;

            CreditCards.InnerHtml = s;
        }//end btnEditCard

    }//end class
}//end namespace