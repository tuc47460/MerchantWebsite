using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TermProject.Models
{
    public class Merchant : ContactInformation
    {
        private String merchantID;
        private String apikey;
        private String apiurl;
        private String desc;

        public String MerchantID
        {
            get { return merchantID; }
            set { merchantID = value; }
        }

        public String APIKey
        {
            get { return apikey; }
            set { apikey = value; }
        }
        public String APIURL
        {
            get { return apiurl; }
            set { apiurl = value; }
        }

        public String Desc
        {
            get { return desc; }
            set { desc = value; }
        }
    }
}