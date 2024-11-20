using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class PaymentOnlineInfo
    {
        public int Id { get; set; }

        [Required]
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }


        // In the event of a fully successful call the string "ok" will be present. If only partially successful, the string "warning" will be present. Otherwise "error". 
        [StringLength(20)]
        public string? RedirectApiCallStatus { get; set; }    // Ok, Warning, Error
        [StringLength(250)]
        public string? RedirectApiMessage { get; set; }
        public DateTime? RedirectApiCallOn { get; set; }

        public byte? RedirectUrlCallApiStatus { get; set; }   // 0 = Cancelled, 1 = Declined, 2 = Approved
        public DateTime? RedirectUrlCallOn { get; set; }

        // Transaction's approval status. 
        public byte? ApprovalStatus { get; set; }
        // Payment Receipt Confirmation (PRC): the reference number for this particular transaction. 
        public int? PaymentReceiptConfirmation { get; set; }
        // Date payment was effective 
        public DateTime? EffectiveDate { get; set; }

        // The card's issuer if the payment method is a card. 
        public byte? CardType { get; set; }           // 3 = AMEX, 4 = VISA, 5 = MasterCard, 6 = Discover, 7 = Debit 	
        // Name on card used to make the payment.
        [StringLength(100)]
        public string? NameOnCard { get; set; }
        // Masked value of card if credit/debit was used 
        [StringLength(20)]
        public string? CardNumber { get; set; }
        // Bank name if check was used 
        [StringLength(100)]
        public string? BankName { get; set; }
        // Routing number if check was used 
        [StringLength(20)]
        public string? RoutingNumber { get; set; }
        // Account number if check was used 
        [StringLength(20)]
        public string? AccountNumber { get; set; }

        // Collection mode through which transaction was received
        public byte? CollectionMode { get; set; }    // 1 = Web

        // This 18-character value matches the TransactionID submitted in the corresponding JetPay request transaction message.This value can be used to correlate the transaction's acknowledgment information with its submitted request data.
        [StringLength(50)]
        public string? HostTransactionId { get; set; }
        // Six (6) alphanumeric characters long returned by the issuer bank. 
        [StringLength(20)]
        public string? HostAuthorizationCode { get; set; }
        // This will indicate the current payment method credit status. 
        public byte? VoidCredit { get; set; }

        // In the event of a fully successful call the string "ok" will be present. If only partially successful, the string "warning" will be present. Otherwise "error".
        [StringLength(20)]
        public string? GetPaymentApiCallStatus { get; set; }   // Ok, Warning, Error
        [StringLength(250)]
        public string? GetPaymentApiMessage { get; set; }
        public DateTime? GetPaymentApiCallOn { get; set; }

    }
}
