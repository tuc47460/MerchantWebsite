using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TermProject.Models;
using Utilities;

namespace TermProject
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }//end page_load

        protected void ShowCustomerForm()
        {
            HideMerchantInfo();
            ShowShippingInfo();
        }//end ShowCustomerForm

        protected void ShowMerchantForm()
        {
            HideShippingInfo();
            ShowMerchantInfo();

            //generate apikey
            txtApiKey.Text = GenerateKey();
        }//end ShowCustomerForm

        private void ShowShippingInfo()
        {
            lblBillingInfo.Visible = true;
            lblShippingInfo.Visible = true;
            lblSAddress.Visible = true;
            txtSAddress.Visible = true;
            lblSCity.Visible = true;
            txtSCity.Visible = true;
            lblSState.Visible = true;
            txtSState.Visible = true;
            lblSZip.Visible = true;
            txtSZip.Visible = true;
            btnSubmit.Visible = true;
        }

        private void HideShippingInfo()
        {
            lblBillingInfo.Visible = false;
            lblShippingInfo.Visible = false;
            lblSAddress.Visible = false;
            txtSAddress.Visible = false;
            lblSCity.Visible = false;
            txtSCity.Visible = false;
            lblSState.Visible = false;
            txtSState.Visible = false;
            lblSZip.Visible = false;
            txtSZip.Visible = false;
            btnSubmit.Visible = false;
        }

        private void ShowMerchantInfo()
        {
            lblMerchantInfo.Visible = true;
            lblApiKey.Visible = true;
            txtApiKey.Visible = true;
            lblGenerateKey.Visible = true;
            btnGenerateKey.Visible = true;
            lblDescription.Visible = true;
            txtDesc.Visible = true;
            lblAPIURL.Visible = true;
            txtAPIURL.Visible = true;
            btnSubmit.Visible = true;
        }

        private void HideMerchantInfo()
        {
            lblMerchantInfo.Visible = false;
            lblApiKey.Visible = false;
            txtApiKey.Visible = false;
            lblGenerateKey.Visible = false;
            btnGenerateKey.Visible = false;
            lblDescription.Visible = false;
            txtDesc.Visible = false;
            lblAPIURL.Visible = false;
            txtAPIURL.Visible = false;
            btnSubmit.Visible = false;
        }

        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUserType.SelectedValue.Equals("1"))
            {
                ShowCustomerForm();
            }//end if
            else if (ddlUserType.SelectedValue.Equals("2"))
            {
                ShowMerchantForm();
            }//end else
            else
            {
                HideMerchantInfo();
                HideShippingInfo();
            }//end else
        }//end ddlUserType SelectedIndexChanged

        protected void btnGenerateKey_Click(object sender, EventArgs e)
        {
            txtApiKey.Text = GenerateKey();
        }//end generate APIKey

        protected string GenerateKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            string apiKey = Convert.ToBase64String(key);

            return apiKey;
        }//end GenerateKey

        protected bool RegisterUser()
        {
            //local var
            int userid = -1;
            int usertype = GetInt(ddlUserType.SelectedValue);

            //insert User
            InsertUser(txtUsername.Text, txtPassword1.Text, usertype);

            //find userid to insert into type of user
            userid = GetUserid(txtUsername.Text);

            //if userid is not -1 add user type to db
            if (userid != -1)
            {
                switch (usertype)
                {
                    case 1:
                        return InsertCustomer(txtName.Text, txtAddress.Text,
                            txtCity.Text, txtState.Text, GetInt(txtZip.Text),
                            txtEmail.Text, txtPhone.Text, txtSAddress.Text,
                            txtSCity.Text, txtSState.Text, txtSZip.Text,
                            userid);
                    case 2:
                        return InsertMerchant(txtName.Text, txtAddress.Text,
                            txtCity.Text, txtState.Text, GetInt(txtZip.Text),
                            txtEmail.Text, txtPhone.Text, txtDesc.Text,
                            txtApiKey.Text, userid, txtAPIURL.Text);
                }//end switch
                return false;
            }//end if
            else
            {
                return false;
            }//end else

        }//end RegisterUser

        protected bool RegisterOnMerchantAPI()
        {
            string webApiUrl = txtAPIURL.Text;
            ContactInfo contactInfo = new ContactInfo();

            contactInfo.Address = txtAddress.Text;
            contactInfo.City = txtCity.Text;
            contactInfo.State = txtState.Text;
            contactInfo.ZipCode = GetInt(txtZip.Text);
            contactInfo.Name = txtName.Text;
            contactInfo.Phone = txtPhone.Text;
            contactInfo.Email = txtEmail.Text;

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonContactInfo = js.Serialize(contactInfo);
            try
            {
                WebRequest request = WebRequest.Create(webApiUrl + "/RegisterSite/+" + "Bornes&Jobbles" +"/"+ "Books & Electronics" + "/" + txtApiKey.Text +"/" + "tuc47460@temple.edu");
                request.Method = "POST";
                request.ContentLength = jsonContactInfo.Length;
                request.ContentType = "application/json";

                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonContactInfo);
                writer.Flush();
                writer.Close();

                WebResponse response = request.GetResponse();
                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream);
                string data = reader.ReadToEnd();
                reader.Close();
                response.Close();

                if (data == "true")
                {
                    return true; ;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }//end RegisterOnMerchantAPI

        protected bool InsertUser(string username, string password, int usertype)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_AddUser");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@username", username));
            sqlcomm.Parameters.Add(new SqlParameter("@password", password));
            sqlcomm.Parameters.Add(new SqlParameter("@usertype", usertype));

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
        }//end InsertUser

        protected bool InsertCustomer(string name, string address, string city, string state,
            int zip, string email, string phone, string saddress,
            string scity, string sstate, string szip, int userid)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_AddCustomer");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@name", name));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_address", address));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_city", city));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_state", state));
            sqlcomm.Parameters.Add(new SqlParameter("@billing_zip", zip));
            sqlcomm.Parameters.Add(new SqlParameter("@email", email));
            sqlcomm.Parameters.Add(new SqlParameter("@phone", phone));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_address", saddress));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_city", scity));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_state", sstate));
            sqlcomm.Parameters.Add(new SqlParameter("@shipping_zip", szip));
            sqlcomm.Parameters.Add(new SqlParameter("@userid", userid));

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
        }//end InsertUser

        protected bool InsertMerchant(string name, string address, string city, string state,
            int zip, string email, string phone, string description, string apikey, int userid,
            string apiurl)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_AddMerchant");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@name", name));
            sqlcomm.Parameters.Add(new SqlParameter("@address", address));
            sqlcomm.Parameters.Add(new SqlParameter("@city", city));
            sqlcomm.Parameters.Add(new SqlParameter("@state", state));
            sqlcomm.Parameters.Add(new SqlParameter("@zip", zip));
            sqlcomm.Parameters.Add(new SqlParameter("@email", email));
            sqlcomm.Parameters.Add(new SqlParameter("@phone", phone));
            sqlcomm.Parameters.Add(new SqlParameter("@description", description));
            sqlcomm.Parameters.Add(new SqlParameter("@apikey", apikey));
            sqlcomm.Parameters.Add(new SqlParameter("@userid", userid));
            sqlcomm.Parameters.Add(new SqlParameter("@apiurl", apiurl));

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
        }//end InsertMerchant

        protected bool UsernameExists(string username)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_UsernameExists");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@username", username));
            DataSet TheDataSet = new DataSet();

            try
            {
                //get data set
                TheDataSet = dbconn.GetDataSetUsingCmdObj(sqlcomm);

                if (TheDataSet.Tables[0].Rows.Count > 0)
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
        }//end UsernameExists

        protected int GetUserid(string username)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_UsernameExists");
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
                    int userid = -1;

                    foreach (DataRow dr in TheDataSet.Tables[0].Rows)
                    {
                        userid = (int)dr["UserId"];
                    }//end foreach
                    return userid;
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
        }//end UsernameExists

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



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!UsernameExists(txtUsername.Text))
            {
                if (PasswordsMatch(txtPassword1.Text, txtPassword2.Text))
                {
                    if (RegisterUser())
                    {
                        lblWarning.Visible = true;
                        lblWarning.Text = "User Inserted";

                        if (ddlUserType.SelectedValue == "1")
                        {
                            Response.Redirect("LogIn.aspx");
                        }//end if
                        else
                        {
                            HideMerchantInfo();
                            HideShippingInfo();

                            lblWarning.Text = "User was inserted. Site" +
                                "Contact info: Email: ko2n777@yahoo.com " +
                                "Phone: 2675286572 Address: 1132 Krewstown rd. " +
                                "City: Philadelphia State: PA Zip: 19115";
                            //show api key
                            lblApiKey.Visible = true;
                            txtApiKey.Visible = true;
                        }//end else
                    }//end if
                    else
                    {
                        lblWarning.Visible = true;
                        lblWarning.Text = "User not Inserted";
                    }//end else
                }//end if
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Passwords must match.";
                }//end else
            }//end if
            else
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Choose another username. This one already exists.";
            }//end else

        }//end btnSubmit

        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt

    }//end Registration.aspx
}