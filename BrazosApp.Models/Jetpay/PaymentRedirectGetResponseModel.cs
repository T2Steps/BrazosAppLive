using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.Jetpay
{
    public class PaymentRedirectGetResponseModel
    {
        public string transactionidentifier { get; set; }
        public int count { get; set; }
        public int? approvalStatus { get; set; }
        public int? paymentReceiptConfirmation { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? dateNow { get; set; }
        public string timeNow { get; set; }
        public BillingInformation billing { get; set; }
        public string notes { get; set; }
        public int? paymentMethod { get; set; }
        public int? cardType { get; set; }
        public string nameOnCard { get; set; }
        public string cardNumber { get; set; }
        public string bankName { get; set; }
        public string routingNumber { get; set; }
        public string checkNumber { get; set; }
        public string accountNumber { get; set; }
        public int? csiUserId { get; set; }
        public int? collectionMode { get; set; }
        public decimal? amount { get; set; }
        public decimal? feeAmount { get; set; }
        public decimal? totalRemitted { get; set; }

        public string HostTransactionId { get; set; }
        public string HostAuthorizationCode { get; set; }
        public int VoidCredit { get; set; }

        public TransactionLineItem[] lineItems { get; set; }
        public string status { get; set; }
        public Error[] errors { get; set; }

        public class Error
        {
            public string id { get; set; }
            public string message { get; set; }
        }

        public class BillingInformation
        {
            public string name { get; set; }
            public string address { get; set; }
            public string addressLine2 { get; set; }
            public string city { get; set; }
            public string county { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
        }

        public class TransactionLineItem
        {
            public decimal amount { get; set; }
            public string paymentType { get; set; }
            public string[] identifiers { get; set; }
        }
    }
}
