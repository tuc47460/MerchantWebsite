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
using System.Text;
using Newtonsoft.Json;

namespace TermProject
{
    public partial class Home : System.Web.UI.Page
    {
        String webApiUrl = "http://cis-iis2.temple.edu/Spring2019/CIS3342_tug91514/TermProjectWS/api/service/Merchants";
        Label myLabel = new Label();
        Cart cart = new Cart() { };
        Customer cust = new Customer();
        Wishlist wish = new Wishlist();
        List<Department> FinalList = new List<Department>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            if (!IsPostBack)
            {

                //create request and get response
                WebRequest request = WebRequest.Create(webApiUrl);
                WebResponse response = request.GetResponse();

                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream);
                string data = reader.ReadToEnd();
                reader.Close();
                response.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                List<Department> DeptList = js.Deserialize<List<Department>>(data);
                foreach(Department d in DeptList)
                {
                    Department temp = new Department();
                    temp.Department_id = d.Department_id;
                    temp.Name = d.Name;
                    temp.img_url = d.img_url;
                    temp.Merchant_Id = 0;
                    FinalList.Add(temp);
                }
                DBConnect dbconn = new DBConnect();

                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetMerchants";

                DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);
                List<Department> DepList = new List<Department>();

                foreach (DataRow dr in myDS.Tables[0].Rows)
                {

                    int id = GetInt(dr["MerchantId"].ToString());
                    string url = dr["APIurl"].ToString();
                    GetMerchantProducts(id, url);

                }//end foreach

                foreach (Department d in DepList)
                {
                    FinalList.Add(d);
                }//end foreach


                rptDept.DataSource = FinalList;
                rptDept.DataBind();
            }
            if (Request.Cookies["Customer_ID"] != null)

            {

                HttpCookie cookie = Request.Cookies["Customer_ID"];

                cust.CustomerID = cookie.Values["Customer_ID"].ToString();
                bool success = int.TryParse(cust.CustomerID, out int result);
                if (success)
                {
                    wish = GetWish(result);
                    cart = GetCart(result);

                }

            }
        }



        protected void rptDept_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int rowIndex = e.Item.ItemIndex;
            myLabel = (Label)rptDept.Items[rowIndex].FindControl("lblID");

            Label myLabel2 = (Label)rptDept.Items[rowIndex].FindControl("lblMercID");
            string command = e.CommandName.ToString();

            if (command == "Products")
            {
                
                rptDept.Visible = false;
                if (!string.IsNullOrEmpty(myLabel.Text))
                {
                    if (myLabel2.Text == "0") {
                        WebRequest request = WebRequest.Create(webApiUrl + "/Products?DepartmentNumber=" + myLabel.Text);
                        WebResponse response = request.GetResponse();

                        Stream DataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(DataStream);
                        string data = reader.ReadToEnd();
                        reader.Close();
                        response.Close();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        List<Product> ProductList = js.Deserialize<List<Product>>(data);
                        rptProducts.DataSource = ProductList;
                        rptProducts.DataBind();
                    }
                    else
                    {
                        AddMercIdToProduct(myLabel2.Text, GetInt(myLabel.Text));
                    }
                }
            }
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Label myLabel = new Label();
            Product newproduct = new Product();
            
            
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            string ProductAdded = "";
            bool NotAdded = false;
            int rowIndex = e.Item.ItemIndex;

            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblTitle");
            newproduct.Title = myLabel.Text;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblDesc");
            newproduct.Description = myLabel.Text;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblPrice");
            newproduct.Price = Convert.ToDouble(myLabel.Text);
            TextBox myTextBox = (TextBox)rptProducts.Items[rowIndex].FindControl("txtQuantity");
            newproduct.Quantity = Convert.ToInt32(myTextBox.Text);
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblTitle");
            newproduct.Title = myLabel.Text;
            Image myImage = (Image)rptProducts.Items[rowIndex].FindControl("imgProduct");
            newproduct.ImageUrl = myImage.ImageUrl;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblDeptId");
            newproduct.DepartmentID = myLabel.Text;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblProductId");
            newproduct.ProductID = myLabel.Text;
            myLabel = (Label)rptProducts.Items[rowIndex].FindControl("lblMerchantId");
            newproduct.MerchantID = GetInt(myLabel.Text);

            string command = e.CommandName.ToString();
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
                    }
                    else
                    {
                        NotAdded = true;
                    }
                }//end else
                

                serializer.Serialize(memStream, cart);
                byteArray = memStream.ToArray();


                DBConnect dbconn = new DBConnect();

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
                            ProductAdded = "Added";
                        }
                        else
                        {
                            ProductAdded = "Not Added";
                        }


                    }//end if
                    else
                    {
                        ProductAdded = "Not Added";
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



            if (command == "AddWish")
            {
                if (IsItemInWishList(wish, newproduct.ProductID))
                {
                    for (int i = 0; i < wish.Count; i++)
                    {
                        if (wish[i].ProductID.Equals(newproduct.ProductID))
                        {
                            wish[i].Quantity += newproduct.Quantity;
                            if (wish[i].Quantity > 0)
                            {
                                wish[i].Quantity += newproduct.Quantity;
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
                        wish.Add(newproduct);
                    }
                    else
                    {
                        NotAdded = true;
                    }
                }//end else


                serializer.Serialize(memStream, wish);
                byteArray = memStream.ToArray();


                DBConnect dbconn = new DBConnect();

                SqlCommand sqlcomm = new SqlCommand("TP_UpdateWishList");
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", GetInt(cust.CustomerID)));
                sqlcomm.Parameters.Add(new SqlParameter("@WishList", byteArray));


                try
                {
                    if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                    {
                        if (NotAdded == false)
                        {
                            ProductAdded = "Added";
                        }
                        else
                        {
                            ProductAdded = "Not Added";
                        }


                    }//end if
                    else
                    {
                        ProductAdded = "Not Added";
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


            lblAdded.Text = newproduct.Title + " " + ProductAdded;

        }//end rptproducts

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

        protected bool IsItemInWishList(Wishlist wish, string productId)
        {
            foreach (Product p in wish)
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



        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt

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

        public void GetMerchantProducts(int id,string url)
        {
            WebRequest request = WebRequest.Create(url+ "GetDepartments/");
            WebResponse response = request.GetResponse();

            Stream DataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(DataStream);
            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            JavaScriptSerializer js = new JavaScriptSerializer();
            //string DeptList = js.Deserialize(data);
            var json = JsonConvert.DeserializeObject<dynamic>(data);
            List<Department> DList = new List<Department>();

            foreach (dynamic item in json)
            {
                //local var
                Department dep = new Department();
                int count = 0;

                foreach (dynamic jsonPair in item)
                {
                    
                    var pair = jsonPair;
                    var value = pair.Value.ToString();

                    switch (count)
                    {
                        case 0:
                            dep.Department_id = GetInt(value);
                            break;
                        case 1:
                            dep.Name = value;
                            break;
                        case 2:
                            dep.img_url = value;
                            break;
                    }//end switch

                    count++;
                }//end foreach inner

                //add Mercant id to object
                dep.Merchant_Id = id;
                //add to list
                //dlist.add(dep);
                FinalList.Add(dep);
            }//end foreach outter
            

            
        }//end get merchant products


        public void AddMercIdToProduct(string merchantId, int deptid)
        {
            string api = GetApi(GetInt(merchantId));

            WebRequest request = WebRequest.Create(api + "/GetProductCatalog/" + deptid);
            WebResponse response = request.GetResponse();

            Stream DataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(DataStream);
            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();


            JavaScriptSerializer js = new JavaScriptSerializer();
            //string DeptList = js.Deserialize(data);
            var json = JsonConvert.DeserializeObject<dynamic>(data);
            List<Product> PList = new List<Product>();

            foreach (dynamic item in json)
            {
                //local var
                Product prod = new Product();
                int count = 0;

                foreach (dynamic jsonPair in item)
                {

                    var pair = jsonPair;
                    var value = pair.Value.ToString();

                    switch (count)
                    {
                        case 0:
                            prod.ProductID = value;
                            break;
                        case 1:
                            prod.Title = value;
                            break;
                        case 2:
                            prod.Description = value;
                            break;
                        case 3:
                            prod.Price = Math.Round(double.Parse(value), 2);
                            break;
                        case 4:
                            prod.Quantity = GetInt(value);
                            break;
                        case 5:
                            prod.ImageUrl = value;
                            break;
                        case 6:
                            prod.DepartmentID = value;
                            break;

                    }//end switch
                    
                    count++;
                    
                }//end foreach inner

                //add Mercant id to object
                prod.MerchantID = GetInt(merchantId);
                PList.Add(prod);





            }
            rptProducts.DataSource = PList;
            rptProducts.DataBind();
        }

        public string GetApi(int Merchantid)
        {
            string api = "";
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetApiByMerchantId";
            SqlParameter inputParameter = new SqlParameter("@merchant_id", Merchantid);

            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.Int;
            inputParameter.Size = 50;

            objCommand.Parameters.Add(inputParameter);


            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            foreach (DataRow dr in myDS.Tables[0].Rows)
            {
                api = dr["APIurl"].ToString();
            }
            return api;
        }//end get api

    }
}
