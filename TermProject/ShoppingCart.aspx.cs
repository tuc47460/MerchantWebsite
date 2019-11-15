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
using System.Runtime.Serialization.Formatters.Binary;      
using System.IO;

namespace TermProject
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        
        Cart cart = new Cart();
        Customer cust = new Customer();
        bool success;
        int result;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            if (!IsPostBack)
            {

            }
            if (Request.Cookies["Customer_ID"] != null)

            {

                HttpCookie cookie = Request.Cookies["Customer_ID"];

                cust.CustomerID = cookie.Values["Customer_ID"].ToString();
                success = int.TryParse(cust.CustomerID, out result);
                cart = GetCart(result);



            }//end if request
            if (!IsPostBack)
            {
               
                if (success)
                {
                    cart = GetCart(result);
                    rptProducts.DataSource = cart;
                    rptProducts.DataBind();
                    lblTotal.Text = (cart.SubTotal()).ToString();
                    if (cart.Count == 0)
                    {
                        IsEmpty();
                    }//end if count

                }//end if success
            }
        }


        public Cart GetCart(int CustomerId)
        {


            Cart newcart = new Cart();
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetCart";
            SqlParameter inputParameter = new SqlParameter("@CustomerId", CustomerId);

            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.Int;
            inputParameter.Size = 50;

            objCommand.Parameters.Add(inputParameter);


            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            foreach (DataRow dr in myDS.Tables[0].Rows)
            {
                if (dr["Cart"] != DBNull.Value)
                {
                    Byte[] byteArray = (Byte[])dr["Cart"];
                    BinaryFormatter deSerializer = new BinaryFormatter();

                    MemoryStream memStream = new MemoryStream(byteArray);



                    newcart = (Cart)deSerializer.Deserialize(memStream);
                }
                
                




            }
            return newcart;
        }//end get cart

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            EmptyCart();
        }//end btnempty

        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            DBConnect dbconn = new DBConnect();
            serializer.Serialize(memStream, cart);
            byteArray = memStream.ToArray();
            SqlCommand sqlcomm = new SqlCommand("TP_AddPurchase");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
            sqlcomm.Parameters.Add(new SqlParameter("@Cart", byteArray));
            sqlcomm.Parameters.Add(new SqlParameter("@Date", DateTime.Now));
            string purchase = "";


            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    Cart newcart = new Cart();
                    rptProducts.DataSource = newcart;
                    rptProducts.DataBind();
                    btnEmpty.Visible = false;
                    btnCheckout.Visible = false;
                    lbltot.Visible = false;
                    lblTotal.Text = " ";
                    rptProducts.Visible = false;
                    lblCartEmpty.Text = " ";
                    rptCheckout.DataSource = cart;
                    rptCheckout.DataBind();
                    rptCheckout.Visible = true;
                    LblCheckoutTotal.Text = cart.SubTotal().ToString();
                    lblt.Visible = true;
                    cust = GetCustomer(GetInt(cust.CustomerID));
                    lblCustName.Text = cust.Name;
                    lblShippingHeader.Visible = true;
                    lblShippingAddress.Text = cust.ShippingAddress;
                    lblShippingCity.Text = cust.ShippingCity;
                    lblShippingState.Text = cust.ShippingState;
                    lblShippingZip.Text = cust.ShippingZipCode.ToString();
                    lblBillingHeader.Visible = true;
                    lblBillAdd.Text = cust.Address;
                    lblBillCity.Text = cust.City;
                    lblBillState.Text = cust.State;
                    lblBillZip.Text = cust.ZipCode.ToString();
                    
                    EmptyCart();
                    foreach (Product p in cart)
                    {
                       purchase += p.Title + " Quantity: " + p.Quantity + " Price: " + p.Price+"\r\n";
                    }
                    Email objEmail = new Email();

                    String strTO = cust.Email;

                    String strFROM = "Tug91514@Temple.edu";

                    String strSubject = "Bornes & Jobbles Purchase";

                    String strMessage = "This is a confirmation of your purchase of "+purchase+"\r\n"+"Total: " +cart.SubTotal().ToString();



                    try

                    {

                        objEmail.SendMail(strTO, strFROM, strSubject, strMessage);

                        lblCartEmpty.Text = "The email was sent.";

                    }

                    catch (Exception ex)

                    {

                        lblCartEmpty.Text = "The email wasn't sent because one of the required fields was missing.";

                    }

                }//end if
                else
                {
                    lblCartEmpty.Text = "Checkout Failed";
                }//end else
            }
            catch (SqlException sqlex)
            {
                lblCartEmpty.Text = "SQL Error";
            }//end catch sql ex
            catch (Exception ex)
            {
                lblCartEmpty.Text = ex.Message.ToString();
            }//end catch
        }

        public void IsEmpty()
        {
            btnEmpty.Visible = false;
            lblCartEmpty.Text = "Nothing in Cart";
            btnCheckout.Visible = false;
            lblTotal.Text = " ";
            lbltot.Visible = false;
        }

        public void EmptyCart()
        {
            Cart newcart = new Cart();
            DBConnect dbconn = new DBConnect();
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            string empty = " ";
            Byte[] byteArray;

            serializer.Serialize(memStream, newcart);
            byteArray = memStream.ToArray();

            SqlCommand sqlcomm = new SqlCommand("TP_UpdateCart");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
            sqlcomm.Parameters.Add(new SqlParameter("@Cart", byteArray));


            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    empty = "Cart Emptied";
                    rptProducts.DataSource = newcart;
                    rptProducts.DataBind();
                    btnEmpty.Visible = false;
                    btnCheckout.Visible = false;
                    lblTotal.Text = " ";
                    lbltot.Visible = false;


                }//end if
                else
                {
                    empty = "Not Emptied";
                }//end else
            }
            catch (SqlException sqlex)
            {
                empty = "SQL Error";
            }//end catch sql ex
            catch (Exception ex)
            {
                empty = ex.Message.ToString();
            }//end catch
            lblCartEmpty.Text = empty;
        }//end empty cart

        public Customer GetCustomer(int UserId )
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            Customer newCust = new Customer();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_CustomerByCustomerID";
            SqlParameter inputParameter = new SqlParameter("@CustomerId", UserId);

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
                
                newCust.CustomerID = dr["Customer_ID"].ToString();




            }
            return newCust;
        }//end get customer

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Cart newcart = new Cart();
            DBConnect dbconn = new DBConnect();
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            string empty = " ";
            Byte[] byteArray;
            int rowIndex = e.Item.ItemIndex;
            Label myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblTitle");
            TextBox myTextBox = (TextBox)rptProducts.Items[rowIndex].FindControl("txtQty");
            string command = e.CommandName.ToString();

            if (command == "Update")
            {
                foreach (Product p in cart)
                {
                    if (p.Title.Equals(myLabel.Text))
                    {
                        p.Quantity = GetInt(myTextBox.Text);

                        if(p.Quantity <= 0)
                        {
                            cart.Remove(p);
                        }

                        serializer.Serialize(memStream, cart);
                        byteArray = memStream.ToArray();

                        SqlCommand sqlcomm = new SqlCommand("TP_UpdateCart");
                        sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
                        sqlcomm.Parameters.Add(new SqlParameter("@Cart", byteArray));


                        try
                        {
                            if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                            {

                                rptProducts.DataSource = cart;
                                rptProducts.DataBind();
                                if (cart.Count == 0)
                                {
                                    IsEmpty();
                                }
                                else
                                {
                                    lblTotal.Text = cart.SubTotal().ToString();
                                }
                                break;



                            }//end if
                            else
                            {
                                empty = "Not Deleted";
                            }//end else
                        }
                        catch (SqlException sqlex)
                        {
                            empty = "SQL Error";
                        }//end catch sql ex
                        catch (Exception ex)
                        {
                            empty = ex.Message.ToString();
                        }//end catch

                    }
                }


                    }

            if (command == "Delete")
            {

                foreach (Product p in cart)
                {
                    if (p.Title.Equals(myLabel.Text))
                    {
                        cart.Remove(p);
                        

                        serializer.Serialize(memStream, cart);
                        byteArray = memStream.ToArray();

                        SqlCommand sqlcomm = new SqlCommand("TP_UpdateCart");
                        sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
                        sqlcomm.Parameters.Add(new SqlParameter("@Cart", byteArray));


                        try
                        {
                            if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                            {

                                rptProducts.DataSource = cart;
                                rptProducts.DataBind();
                                if(cart.Count== 0)
                                {
                                    IsEmpty();
                                }
                                else
                                {
                                    lblTotal.Text = cart.SubTotal().ToString();
                                }
                                break;



                            }//end if
                            else
                            {
                                empty = "Not Deleted";
                            }//end else
                        }
                        catch (SqlException sqlex)
                        {
                            empty = "SQL Error";
                        }//end catch sql ex
                        catch (Exception ex)
                        {
                            empty = ex.Message.ToString();
                        }//end catch
                        lblCartEmpty.Text = empty;
                    }//end if
                }//end for each
                }//end dellet commaND

          
            }//END REPEAT PRODUCTS







    }//end shoppingCart
}//end namespace


