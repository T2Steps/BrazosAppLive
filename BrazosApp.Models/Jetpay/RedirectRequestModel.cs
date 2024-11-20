using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.Jetpay
{
    public class RedirectRequestModel
    {
        public string clientKey { get; set; }
        public string transactionIdentifier { get; set; }
        public int collectionMode { get; set; }
        public decimal amount { get; set; }
        public Address billing { get; set; }
        public int csiUserId { get; set; }
        public string notes { get; set; }
        public LineItem[] lineItems { get; set; }
        public string urlSilentPost { get; set; }
        public string urlReturnPost { get; set; }
        public int allowedPaymentMethod { get; set; }

        public class Address
        {
            public string phone { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string addressLine2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string email { get; set; }
            public string county { get; set; }
        }

        public class LineItem
        {
            public string[] identifiers { get; set; }
            public decimal amount { get; set; }
            public string paymentType { get; set; }
        }
    }
}
