using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TermProject.Models;
using Utilities;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TermProject
{
    public partial class Account : System.Web.UI.Page
    {
        //global var
        string username = "";
        int customerId = -1;
        int merchantId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            if (Request.Cookies["Customer_ID"] != null)
            {
                HttpCookie userCookie = Request.Cookies["Customer_ID"];
                username = userCookie.Values["Username"].ToString();

                if (GetUserType(username) == 1)
                {
                    customerId = GetInt(userCookie.Values["Customer_ID"].ToString());
                }//end if
                else if (GetUserType(username) == 2)
                {
                    merchantId = GetInt(userCookie.Values["Customer_ID"].ToString());
                }//end else


            }//end if

            if (!IsPostBack)
            {
                ClearTextBoxes();
                FillAccountInfo();

                if (txtAccountType.Text.Equals("Customer"))
                {
                    List<Credit> CList = GetCreditCards(customerId);

                    if (CList != null)
                    {
                        foreach (Credit c in CList)
                        {
                            ddlCredit.Items.Add(c.CreditCard.CardNumber);
                            ddlCredit.Items.FindByText(c.CreditCard.CardNumber).Value = c.CreditID.ToString();
                        }//end foreach
                    }//end if
                }//end if
            }//end if


            if (txtAccountType.Text.Equals("Customer"))
            {
                //local var
                List<Purchase> PList = GetAllPurchases(customerId);


                if (PList != null)
                {
                    PList = PList.OrderByDescending(x => x.PurchaseDate).ToList();
                    DisplayAllPurchases(PList);
                }//end if



                ShowCustomerAccount();
            }//end if
            else if (merchantId != -1)
            {
                DisplayAllMerchantPurchases();
                ShowMerchantAccount();
            }//end else


        }//end pageload

        protected void DisplayAllMerchantPurchases()
        {
            Purchases.Controls.Add(GetAllProductsByMerchant(merchantId, GetAllPurchases()));
        }//end DisplayAllMerchantPurchases

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
                    ProdList += "_______________________________________________";
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

        protected void btnGetCustomerInfo_Click(object sender, EventArgs e)
        {
            //local var
            CreditCards.Controls.Clear();
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

            CustomerInfo CustInfo = (CustomerInfo)LoadControl("CustomerInfo.ascx");
            CustInfo.Id = btn.ToolTip;
            CustInfo.FillCustomerInfo(GetInt(btn.ToolTip));

            ////insert customer info
            //s += "Name: " + cust.Name;
            //s += "</br>";
            //s += "Email: " + cust.Email;
            //s += "</br>";
            //s += "Phone: " + cust.Phone;
            //s += "</br>";
            //s += "Billing Address: " + cust.Address;
            //s += "</br>";
            //s += "Billing City: " + cust.City;
            //s += "</br>";
            //s += "Billing State: " + cust.State;
            //s += "</br>";
            //s += "Billing ZipCode: " + cust.ZipCode;
            //s += "</br>";
            //s += "Shipping Address: " + cust.ShippingAddress;
            //s += "</br>";
            //s += "Shipping City: " + cust.ShippingCity;
            //s += "</br>";
            //s += "Shipping State: " + cust.ShippingState;
            //s += "</br>";
            //s += "Shipping ZipCode: " + cust.ShippingZipCode;

            //CreditCards.InnerHtml = s;
            CreditCards.Controls.Add(CustInfo);
        }//end btnEditCard

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

        private void DisplayAllPurchases(List<Purchase> PList)
        {
            foreach (Purchase p in PList)
            {
                DisplayPurchases(p);
            }//end foreach
        }//end display all purchases

        protected void ShowCustomerAccount()
        {
            ShippingHeader.Visible = true;
            BillingHeader.Visible = true;
            CreditHeader.Visible = true;
            lblSAddress.Visible = true;
            txtSAddress.Visible = true;
            lblSCity.Visible = true;
            txtSCity.Visible = true;
            lblSState.Visible = true;
            txtSState.Visible = true;
            lblSZip.Visible = true;
            txtSZip.Visible = true;
            lblSelectCard.Visible = true;
            ddlCredit.Visible = true;
        }//end ShowCustomerAccount

        protected void ShowMerchantAccount()
        {
            CreditCards.Visible = false;
            lblDesc.Visible = true;
            txtDesc.Visible = true;
            lblApiURL.Visible = true;
            txtApiURL.Visible = true;
            lblApiKey.Visible = true;
            txtAPIKey.Visible = true;

        }//end ShowMerchantAccount

        protected bool UpdateCustomerInfo(Customer cust)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_UpdateCustomer");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", customerId));
            sqlcomm.Parameters.Add(new SqlParameter("@name", cust.Name));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_address", cust.Address));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_city", cust.City));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_state", cust.State));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_zip", cust.ZipCode));
            sqlcomm.Parameters.Add(new SqlParameter("@email", cust.Email));
            sqlcomm.Parameters.Add(new SqlParameter("@phone", cust.Phone));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_address", cust.ShippingAddress));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_city", cust.ShippingCity));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_state", cust.ShippingState));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_zip", cust.ShippingZipCode));

            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    return true;
                }//end if
                else
                {
                    return false;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return false;
            }//end catch sql ex
            catch (Exception ex)
            {
                return false;
            }//end catch
        }//end UpdateCustomerInfo

        public static Customer GetCustomerInfo(int customerID)
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

        protected void ddlCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCredit.SelectedValue != "")
            {
                ShowCardDetails();

                List<Credit> cc = GetCreditCards(customerId);
                Credit Cred = new Credit();

                foreach (Credit c in cc)
                {
                    if (c.CreditID.Equals(GetInt(ddlCredit.SelectedValue)))
                    {
                        Cred = c;
                    }//end if
                }//end foreach

                txtCardNum.Text = Cred.CreditCard.CardNumber;
                txtCardType.Text = Cred.CreditCard.CardType;
                txtExpMonth.Text = Cred.CreditCard.ExpirationMonth.ToString();
                txtExpYear.Text = Cred.CreditCard.ExpirationYear.ToString();
                chxDefault.Checked = Cred.Default;
            }//end if
            else
            {
                HideCardDetails();
            }//end else
        }//end selectedindexchanged

        private void ShowCardDetails()
        {
            btnEditCard.Visible = true;
            lblCardNum.Visible = true;
            txtCardNum.Visible = true;
            lblCardType.Visible = true;
            txtCardType.Visible = true;
            lblExpMonth.Visible = true;
            txtExpMonth.Visible = true;
            lblExpYear.Visible = true;
            txtExpYear.Visible = true;
            chxDefault.Visible = true;
        }

        private void HideCardDetails()
        {
            btnEditCard.Visible = false;
            lblCardNum.Visible = false;
            txtCardNum.Visible = false;
            lblCardType.Visible = false;
            txtCardType.Visible = false;
            lblExpMonth.Visible = false;
            txtExpMonth.Visible = false;
            lblExpYear.Visible = false;
            txtExpYear.Visible = false;
            chxDefault.Visible = false;
        }

        protected void btnEditCard_Click(object sender, EventArgs e)
        {
            CardEditMode();
        }//end btnEditCard

        protected void btnSubmitCard_Click(object sender, EventArgs e)
        {
            CreditCard cc = new CreditCard();

            cc.CardNumber = txtCardNum.Text;
            cc.CardType = txtCardType.Text;
            cc.ExpirationMonth = GetInt(txtExpMonth.Text);
            cc.ExpirationYear = GetInt(txtExpYear.Text);

            Byte[] ccByteArray = SerializeCreditCard(cc);

            if (chxDefault.Checked == true)
            {
                List<Credit> CList = GetCreditCards(customerId);

                if (!HasDefaultCard(CList) || GetDefaultCard(CList) == GetInt(ddlCredit.SelectedValue))
                {
                    if (UpdateCard(GetInt(ddlCredit.SelectedValue), ccByteArray, chxDefault.Checked))
                    {
                        lblWarning.Visible = true;
                        lblWarning.Text = "Credit Card Updated";

                        CardNormalMode();
                    }//end if
                    else
                    {
                        lblWarning.Visible = true;
                        lblWarning.Text = "Credit Card not Updated";

                        CardNormalMode();
                    }//end else
                }//end if
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Default Credit Card Already Exists. Remove default and then select a new default.";

                    CardNormalMode();
                }//end else
            }//end if
            else
            {
                if (UpdateCard(GetInt(ddlCredit.SelectedValue), ccByteArray, chxDefault.Checked))
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Credit Card Updated";

                    CardNormalMode();
                }//end if
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Credit Card not Updated";

                    CardNormalMode();
                }//end else
            }//end else
        }//end btnSubmitCard

        protected bool HasDefaultCard(List<Credit> CList)
        {
            foreach (Credit cc in CList)
            {
                if (cc.Default)
                {
                    return true;
                }//end if
            }//end foreach
            return false;
        }//end HasDefaultCard

        protected int GetDefaultCard(List<Credit> CList)
        {
            foreach (Credit cc in CList)
            {
                if (cc.Default)
                {
                    return cc.CreditID;
                }//end if
            }//end foreach
            return -1;
        }//end HasDefaultCard

        protected Byte[] SerializeCreditCard(CreditCard cc)
        {
            // Serialize the CreditCard object
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            serializer.Serialize(memStream, cc);
            byteArray = memStream.ToArray();

            return byteArray;
        }//end Serialize Data

        protected bool UpdateMerchant(Merchant merch)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_UpdateMerchant");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@MerchantId", merchantId));
            sqlcomm.Parameters.Add(new SqlParameter("@name", merch.Name));
            sqlcomm.Parameters.Add(new SqlParameter("@address", merch.Address));
            sqlcomm.Parameters.Add(new SqlParameter("@state", merch.State));
            sqlcomm.Parameters.Add(new SqlParameter("@city", merch.City));
            sqlcomm.Parameters.Add(new SqlParameter("@zip", merch.ZipCode));
            sqlcomm.Parameters.Add(new SqlParameter("@email", merch.Email));
            sqlcomm.Parameters.Add(new SqlParameter("@phone", merch.Phone));
            sqlcomm.Parameters.Add(new SqlParameter("@description", merch.Desc));
            sqlcomm.Parameters.Add(new SqlParameter("@APIurl", merch.APIURL));

            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    return true;
                }//end if
                else
                {
                    return false;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return false;
            }//end catch sql ex
            catch (Exception ex)
            {
                return false;
            }//end catch
        }//end UpdateMerchant

        protected bool UpdateCard(int creditID, Byte[] ccByteArray, bool defaultValue)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_UpdateCredit");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CreditId", creditID));
            sqlcomm.Parameters.Add(new SqlParameter("@CreditCard", ccByteArray));
            sqlcomm.Parameters.Add(new SqlParameter("@Default", defaultValue));

            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    return true;
                }//end if
                else
                {
                    return false;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return false;
            }//end catch sql ex
            catch (Exception ex)
            {
                return false;
            }//end catch
        }//end UpdateCard

        private void CardNormalMode()
        {
            btnEdit.Visible = true;
            btnChangePassword.Visible = true;
            btnEditCard.Visible = true;
            txtCardNum.ReadOnly = true;
            txtCardType.ReadOnly = true;
            txtExpMonth.ReadOnly = true;
            txtExpYear.ReadOnly = true;
            btnSubmitCard.Visible = false;
        }

        private void CardEditMode()
        {
            btnEdit.Visible = false;
            btnChangePassword.Visible = false;
            btnEditCard.Visible = false;
            txtCardNum.ReadOnly = false;
            txtCardType.ReadOnly = false;
            txtExpMonth.ReadOnly = false;
            txtExpYear.ReadOnly = false;
            btnSubmitCard.Visible = true; ;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //refresh text boxes
            ClearTextBoxes();

            FillAccountInfo();

            if (customerId != -1)
            {
                EditInfoModeCustomer();
            }//end customer
            else if (merchantId != -1)
            {
                EditInfoModeMerchant();
            }//end else if


        }//end btnEdit

        protected void EditInfoModeMerchant()
        {
            btnSubmitInfo.Visible = true;
            btnChangePassword.Visible = false;
            btnEdit.Visible = false;
            txtName.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtCity.ReadOnly = false;
            txtState.ReadOnly = false;
            txtZip.ReadOnly = false;
            txtDesc.ReadOnly = false;
            txtApiURL.ReadOnly = false;
        }//end EditInfoModeMerchant

        protected void NormalInfoModeMerchant()
        {
            btnSubmitInfo.Visible = false;
            btnChangePassword.Visible = true;
            btnEdit.Visible = true;
            txtName.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtCity.ReadOnly = true;
            txtState.ReadOnly = true;
            txtZip.ReadOnly = true;
            txtDesc.ReadOnly = true;
            txtApiURL.ReadOnly = true;
        }//end normalinfomodemerchant

        private void ClearTextBoxes()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
            txtSAddress.Text = "";
            txtSCity.Text = "";
            txtSState.Text = "";
            txtSZip.Text = "";
        }

        private void EditInfoModeCustomer()
        {
            btnSubmitInfo.Visible = true;
            btnChangePassword.Visible = false;
            btnEditCard.Visible = false;
            btnEdit.Visible = false;
            txtName.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtCity.ReadOnly = false;
            txtState.ReadOnly = false;
            txtZip.ReadOnly = false;
            txtSAddress.ReadOnly = false;
            txtSCity.ReadOnly = false;
            txtSState.ReadOnly = false;
            txtSZip.ReadOnly = false;
        }

        private void NormalInfoModeCustomer()
        {
            btnSubmitInfo.Visible = false;
            btnChangePassword.Visible = true;
            btnEditCard.Visible = true;
            btnEdit.Visible = true;
            txtName.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtCity.ReadOnly = true;
            txtState.ReadOnly = true;
            txtZip.ReadOnly = true;
            txtSAddress.ReadOnly = true;
            txtSCity.ReadOnly = true;
            txtSState.ReadOnly = true;
            txtSZip.ReadOnly = true;
        }

        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt

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

        protected void FillAccountInfo()
        {
            int IntUserType = GetUserType(username);
            string StrUserType = UserTypeToString(IntUserType);

            if (merchantId == -1)
            {
                Customer cust = GetCustomerInfo(customerId);

                txtUsername.Text = username;
                txtAccountType.Text = StrUserType;
                txtName.Text = cust.Name;
                txtEmail.Text = cust.Email;
                txtPhone.Text = cust.Phone;
                txtAddress.Text = cust.Address;
                txtCity.Text = cust.City;
                txtState.Text = cust.State;
                txtZip.Text = cust.ZipCode.ToString();
                txtSAddress.Text = cust.ShippingAddress;
                txtSCity.Text = cust.ShippingCity;
                txtSState.Text = cust.ShippingState;
                txtSZip.Text = cust.ShippingZipCode.ToString();
            }//end if
            else if (customerId == -1)
            {
                Merchant merch = GetMerchantInfo(merchantId);

                if (merch != null)
                {
                    txtUsername.Text = username;
                    txtAccountType.Text = StrUserType;
                    txtName.Text = merch.Name;
                    txtEmail.Text = merch.Email;
                    txtPhone.Text = merch.Phone;
                    txtAddress.Text = merch.Address;
                    txtCity.Text = merch.City;
                    txtState.Text = merch.State;
                    txtZip.Text = merch.ZipCode.ToString();
                    txtDesc.Text = merch.Desc;
                    txtApiURL.Text = merch.APIURL;
                    txtAPIKey.Text = merch.APIKey;
                }//end if
            }//end else if

        }//end FillAccountInfo

        protected Merchant GetMerchantInfo(int merchantID)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_GetMerchantByMerchantID");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@MerchantId", merchantID));
            DataSet TheDataSet = new DataSet();

            try
            {
                //get data set
                TheDataSet = dbconn.GetDataSetUsingCmdObj(sqlcomm);

                if (TheDataSet.Tables[0].Rows.Count == 1)
                {
                    //local var
                    Merchant merch = new Merchant();

                    foreach (DataRow dr in TheDataSet.Tables[0].Rows)
                    {


                        merch.Name = (string)dr["name"];
                        merch.Address = (string)dr["address"];
                        merch.City = (string)dr["city"];
                        merch.State = (string)dr["state"];
                        merch.ZipCode = (int)dr["zip"];
                        merch.Email = (string)dr["email"];
                        merch.Phone = (string)dr["phone"];
                        merch.Desc = (string)dr["description"];
                        merch.APIKey = (string)dr["APIkey"];
                        merch.APIURL = (string)dr["APIurl"];
                    }//end foreach

                    return merch;
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
        }//end GetMerchantInfo

        protected string UserTypeToString(int theType)
        {
            switch (theType)
            {
                case 1:
                    return "Customer";
                case 2:
                    return "Merchant";
                default:
                    return "Error";
            }
        }//end UserTypeToString

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordMode();
        }

        private void ChangePasswordMode()
        {
            //clear password boxes
            txtPassword1.Text = "";
            txtPassword2.Text = "";

            btnChangePassword.Visible = false;
            btnSubmitNewPassword.Visible = true;
            lblPassword1.Visible = true;
            txtPassword1.Visible = true;
            lblPassword2.Visible = true;
            txtPassword2.Visible = true;
        }

        protected List<Credit> GetCreditCards(int CustomerId)
        {
            //local var
            DBConnect dbconn = new DBConnect();
            List<Credit> CList = new List<Credit>();

            SqlCommand sqlcomm = new SqlCommand("TP_GetCredit");
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
                        Credit C = new Credit();

                        if (dr["Credit_Card"] != DBNull.Value)
                        {
                            Byte[] byteArray = (Byte[])dr["Credit_Card"];
                            BinaryFormatter deSerializer = new BinaryFormatter();

                            MemoryStream memStream = new MemoryStream(byteArray);



                            C.CreditCard = (CreditCard)deSerializer.Deserialize(memStream);

                            memStream.Flush();
                        }

                        C.CreditID = (int)dr["Credit_ID"];
                        C.Default = (bool)dr["Default_Card"];

                        CList.Add(C);
                    }//end foreach
                    return CList;
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
        }//end GetCreditCards

        protected void btnSubmitNewPassword_Click(object sender, EventArgs e)
        {
            if (PasswordsMatch(txtPassword1.Text, txtPassword2.Text))
            {
                if (UpdatePassword(username, txtPassword1.Text))
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Password Successfully Updated";
                }//end if 
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Password not Updated";
                }//end else
            }//end if
            else
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Password not changed. Passwords must Match";
            }//end else

            NormalMode();
        }

        protected void btnSubmitInfo_Click(object sender, EventArgs e)
        {
            if (customerId != -1)
            {
                //local var
                Customer cust = new Customer();
                //pass variables into cust
                cust.Address = txtAddress.Text;
                cust.City = txtCity.Text;
                cust.Email = txtEmail.Text;
                cust.Name = txtName.Text;
                cust.Phone = txtPhone.Text;
                cust.ShippingAddress = txtSAddress.Text;
                cust.ShippingCity = txtSCity.Text;
                cust.ShippingState = txtSState.Text;
                cust.ShippingZipCode = GetInt(txtSZip.Text);
                cust.State = txtState.Text;
                cust.ZipCode = GetInt(txtZip.Text);

                if (UpdateCustomerInfo(cust))
                {
                    NormalInfoModeCustomer();
                    lblWarning.Visible = true;
                    lblWarning.Text = "Account Updated";
                    FillAccountInfo();
                }//end if
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Account not Updated";
                }//end else
            }//end if
            else if (merchantId != -1)
            {
                //local var
                Merchant merch = new Merchant();
                //pass variables into cust
                merch.Address = txtAddress.Text;
                merch.City = txtCity.Text;
                merch.Email = txtEmail.Text;
                merch.Name = txtName.Text;
                merch.Phone = txtPhone.Text;
                merch.State = txtState.Text;
                merch.ZipCode = GetInt(txtZip.Text);
                merch.APIURL = txtApiURL.Text;
                merch.Desc = txtDesc.Text;

                if (UpdateMerchant(merch))
                {
                    NormalInfoModeMerchant();
                    lblWarning.Visible = true;
                    lblWarning.Text = "Account Updated";
                    FillAccountInfo();
                }//end if
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Account not Updated";
                }//end else
            }//end if
        }//end submit info

        private void NormalMode()
        {
            btnChangePassword.Visible = true;
            btnSubmitNewPassword.Visible = false;
            lblPassword1.Visible = false;
            txtPassword1.Visible = false;
            lblPassword2.Visible = false;
            txtPassword2.Visible = false;
        }

        protected bool UpdatePassword(string username, string password)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_UpdatePasswordByUserName");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@username", username));
            sqlcomm.Parameters.Add(new SqlParameter("@password", password));

            try
            {
                if (dbconn.DoUpdateUsingCmdObj(sqlcomm) > 0)
                {
                    return true;
                }//end if
                else
                {
                    return false;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return false;
            }//end catch sql ex
            catch (Exception ex)
            {
                return false;
            }//end catch
        }//end UpdatePassword

        protected int GetUserType(string username)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_GetUserType");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@username", username));
            DataSet TheDataSet = new DataSet();
            try
            {
                //get data set
                TheDataSet = dbconn.GetDataSetUsingCmdObj(sqlcomm);

                if (TheDataSet.Tables[0].Rows.Count == 1)
                {
                    //local var
                    int usertype = -1;

                    foreach (DataRow dr in TheDataSet.Tables[0].Rows)
                    {
                        usertype = (int)dr["Usertype"];
                    }//end foreach
                    return usertype;
                }//end if
                else
                {
                    return -1;
                }//end else
            }
            catch (SqlException sqlex)
            {
                return -1;
            }//end catch sql ex
            catch (Exception ex)
            {
                return -1;
            }//end catch
        }//end GetUserInfo

        protected bool PasswordsMatch(string password1, string password2)
        {
            if (password1.Equals(password2))
            {
                return true;
            }//end if
            else
            {
                return false;
            }//end else

        }//end PasswordsMatch

        protected void DisplayPurchases(Purchase p)
        {
            HtmlGenericControl container = new HtmlGenericControl();
            HtmlGenericControl purchaseTitle = new HtmlGenericControl();
            HtmlGenericControl purchase = new HtmlGenericControl();
            container.Attributes.Add("class", "card");
            purchaseTitle.Attributes.Add("class", "card-title");
            purchaseTitle.Attributes.Add("style", "background-color: rgb(214,194,38);");
            purchase.Attributes.Add("class", "card-body");
            purchaseTitle.InnerHtml = "Purchase ID: <b>" + p.PurchaseID + "</b>" + "<br>";
            purchase.InnerHtml = p.ToString();
            container.Controls.Add(purchaseTitle);
            container.Controls.Add(purchase);

            Purchases.Controls.Add(container);
        }//end DispayPurchases
    }
}