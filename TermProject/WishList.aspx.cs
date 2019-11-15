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
    public partial class WishList : System.Web.UI.Page
    {
        Cart cart = new Cart();
        Customer cust = new Customer();
        Wishlist wish = new Wishlist();
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
                wish = GetWish(result);
                cart = GetCart(result);



            }//end if request
            if (!IsPostBack)
            {

                if (success)
                {
                    
                    rptProducts.DataSource = wish;
                    rptProducts.DataBind();
                    
                    if (wish.Count == 0)
                    {
                        IsEmpty();
                    }//end if count

                }//end if success
            }//end if not postback

        }//end page load

        public Wishlist GetWish(int CustomerId)
        {


            Wishlist newwish = new Wishlist();
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetWishList";
            SqlParameter inputParameter = new SqlParameter("@CustomerId", CustomerId);

            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.Int;
            inputParameter.Size = 50;

            objCommand.Parameters.Add(inputParameter);


            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            foreach (DataRow dr in myDS.Tables[0].Rows)
            {
                if (dr["Wish_List"] != DBNull.Value)
                {
                    Byte[] byteArray = (Byte[])dr["Wish_List"];
                    BinaryFormatter deSerializer = new BinaryFormatter();

                    MemoryStream memStream = new MemoryStream(byteArray);



                    newwish = (Wishlist)deSerializer.Deserialize(memStream);
                }






            }
            return newwish;
        }//end get wish list

        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            EmptyWish();
        }//end btn empty

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Wishlist newwish = new Wishlist();
            DBConnect dbconn = new DBConnect();
            Product newproduct = new Product();
            string ProductAdded = "";
            bool NotAdded = false;
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            string empty = " ";
            Byte[] byteArray;
            int rowIndex = e.Item.ItemIndex;
            Label myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblTitle");
            newproduct.Title = myLabel.Text;
            TextBox myTextBox = (TextBox)rptProducts.Items[rowIndex].FindControl("txtQty");
            newproduct.Quantity = Convert.ToInt32(myTextBox.Text);
            
           
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblDesc");
            newproduct.Description = myLabel.Text;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblPrice");
            newproduct.Price = Convert.ToDouble(myLabel.Text);
            
            
            
            Image myImage = (Image)rptProducts.Items[rowIndex].FindControl("imgProduct");
            newproduct.ImageUrl = myImage.ImageUrl;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblDeptId");
            newproduct.DepartmentID = myLabel.Text;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblProductId");
            newproduct.ProductID = myLabel.Text;
            string command = e.CommandName.ToString();

            if (command == "Update")
            {
                foreach (Product p in wish)
                {
                    if (p.Title.Equals(newproduct.Title))
                    {
                        p.Quantity = GetInt(myTextBox.Text);

                        if (p.Quantity <= 0)
                        {
                            wish.Remove(p);
                        }

                        serializer.Serialize(memStream, wish);
                        byteArray = memStream.ToArray();

                        SqlCommand sqlcomm = new SqlCommand("TP_UpdateWishList");
                        sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
                        sqlcomm.Parameters.Add(new SqlParameter("@WishList", byteArray));


                        try
                        {
                            if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                            {

                                rptProducts.DataSource = wish;
                                rptProducts.DataBind();
                                if (wish.Count == 0)
                                {
                                    IsEmpty();
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

                foreach (Product p in wish)
                {
                    if (p.Title.Equals(newproduct.Title))
                    {
                        wish.Remove(p);


                        serializer.Serialize(memStream, wish);
                        byteArray = memStream.ToArray();

                        SqlCommand sqlcomm = new SqlCommand("TP_UpdateWishList");
                        sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
                        sqlcomm.Parameters.Add(new SqlParameter("@WishList", byteArray));


                        try
                        {
                            if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                            {

                                rptProducts.DataSource = wish;
                                rptProducts.DataBind();
                                if (wish.Count == 0)
                                {
                                    IsEmpty();
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

            if (command == "Add")
            {
                if (IsItemInCart(cart, newproduct.ProductID))
                {
                    for (int i = 0; i < cart.Count; i++)
                    {
                        if (cart[i].ProductID.Equals(newproduct.ProductID))
                        {
                            
                            if (cart[i].Quantity > 0)
                            {
                                cart[i].Quantity += newproduct.Quantity;
                                DeleteWish(newproduct.Title);
                            }
                            else
                            {
                                NotAdded = true;
                            }


                        }
                    }
                }//end if
                else
                {
                    if (newproduct.Quantity > 0)
                    {
                        cart.Add(newproduct);
                        
                        DeleteWish(newproduct.Title);
                        

                    }
                    else
                    {
                        NotAdded = true;
                    }
                }//end else


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
                        if (NotAdded == false)
                        {
                            ProductAdded = "Added to Cart";
                            

                        }
                        else
                        {
                            ProductAdded = "Not Added to Cart";
                        }


                    }//end if
                    else
                    {
                        ProductAdded = "Not Added to Cart";
                    }//end else
                }
                catch (SqlException sqlex)
                {
                    ProductAdded = "SQL Error";
                }//end catch sql ex
                catch (Exception ex)
                {
                    ProductAdded = ex.Message.ToString();
                }//end catch
            }//end if
            lblCartEmpty.Text = newproduct.Title + " " + ProductAdded;

            }//end rpt products


        public void EmptyWish()
        {
            Wishlist newwish = new Wishlist();
            DBConnect dbconn = new DBConnect();
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            string empty = " ";
            Byte[] byteArray;

            serializer.Serialize(memStream, newwish);
            byteArray = memStream.ToArray();

            SqlCommand sqlcomm = new SqlCommand("TP_UpdateWishList");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
            sqlcomm.Parameters.Add(new SqlParameter("@WishList", byteArray));


            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    empty = "WishList Emptied";
                    rptProducts.DataSource = newwish;
                    rptProducts.DataBind();
                    btnEmpty.Visible = false;
                    


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
        }

        public void IsEmpty()
        {
            btnEmpty.Visible = false;
            lblCartEmpty.Text = "Nothing in WishList";
            
        }

        protected bool IsItemInCart(Cart cart, string productId)
        {
            foreach (Product p in cart)
            {
                if (p.ProductID.Equals(productId))
                {
                    return true;
                }//end if

                
            }//end foreach

            return false;
        }//end IsItemInCart

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

        public void DeleteWish(string title)
        {
            DBConnect dbconn = new DBConnect();
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            string empty = " ";
            Byte[] byteArray;
            foreach (Product p in wish)
            {
                if (p.Title.Equals(title))
                {
                    wish.Remove(p);


                    serializer.Serialize(memStream, wish);
                    byteArray = memStream.ToArray();

                    SqlCommand sqlcomm = new SqlCommand("TP_UpdateWishList");
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
                    sqlcomm.Parameters.Add(new SqlParameter("@WishList", byteArray));


                    try
                    {
                        if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                        {

                            rptProducts.DataSource = wish;
                            rptProducts.DataBind();
                            if (wish.Count == 0)
                            {
                                IsEmpty();
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
        }


    }//end wishlist
} //end namesapce