using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TermProject.Models;
using Utilities;

namespace TermProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //global var
        Customer cust = new Customer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            if (Request.Cookies["Customer_ID"] !=null)
            {
                HttpCookie custCookie = Request.Cookies["Customer_ID"];
                cust.CustomerID = custCookie.Values["Customer_ID"].ToString();
            }//end if
        }//end page load

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ChxAgree.Checked)
            {
                //local var
                CreditCard cc = GenerateCreditCard();

                //Serialize Credit Card
                Byte[] ccByteArray = SerializeCreditCard(cc);

                if (InsertCreditCard(GetInt(cust.CustomerID), ccByteArray, false))
                {
                    //remove buttons
                    ChxAgree.Visible = false;
                    btnSubmit.Visible = false;
                    //show warning label
                    lblWarning.Visible = true;
                    lblWarning.Text = "Credit Card Added to Account";

                    //redirect
                    Response.AddHeader("REFRESH", "3;URL=Account.aspx");
                }//end if
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "Credit Card not Added to Account";
                }//end else
                
                
            }//end if
            else
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Agree to terms before hitting submit.";
            }//end else
        }//end btnSubmit_click

        protected bool InsertCreditCard(int customerID, Byte[] creditCard, bool defaultCard)
        {
            //local var
            DBConnect dbconn = new DBConnect();

            SqlCommand sqlcomm = new SqlCommand("TP_AddCredit");
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add(new SqlParameter("@CustomerId", customerID));
            sqlcomm.Parameters.Add(new SqlParameter("@CreditCard", creditCard));
            sqlcomm.Parameters.Add(new SqlParameter("@Default", defaultCard));

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
        }//end insertCreditCard

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

        protected string GenerateRandomCardNumber()
        {
            //local var
            string cardNumber = "";
            Random rdm = new Random();

            for (int i = 0; i < 13; i++)
            {
                cardNumber += rdm.Next(0, 9);
            }//end for

            return cardNumber;
        }//end GenerateRandomCardNumber

        protected int[] GenerateCardExpDate()
        {
            //local var
            DateTime today = DateTime.Now;
            DateTime nextYear = new DateTime();
            int[] expDate = new int[2];

            nextYear = today.AddYears(1);

            expDate[0] = nextYear.Year;
            expDate[1] = nextYear.Month;

            return expDate;

        }//end generateCardExpDate

        protected string GenerateCardType()
        {
            //local var
            Random rdm = new Random();
            string type = "";

            switch (rdm.Next(0, 2))
            {
                case 0:
                    type = "VISA";
                    break;
                case 1:
                    type = "Master Card";
                    break;
                case 2:
                    type = "AMEX";
                    break;
                default:
                    type = "VISA";
                    break;
            }//end switch

            return type;
        }//end GenerateCardType

        protected CreditCard GenerateCreditCard()
        {
            //local var
            CreditCard cc = new CreditCard();
            int[] expDate = GenerateCardExpDate();

            cc.CardNumber = GenerateRandomCardNumber();
            cc.CardType = GenerateCardType();
            cc.ExpirationYear = expDate[0];
            cc.ExpirationMonth = expDate[1];

            return cc;

        }//end GenerateCreditCard

        public int GetInt(string toParse)
        {
            //local var
            int num = -1;
            //try parse
            int.TryParse(toParse, out num);
            //return num
            return num;
        }//end getIDInt
    }
}