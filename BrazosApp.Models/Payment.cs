using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Payment
    {
        public Payment()
        {
            //CreatedOn = DateTime.Now;
            PaymentStatus = 1;
            IsVoidEnabled = true;
        }


        [Key]
        public int Id { get; set; }

        public int FeesId { get; set; }
        [ForeignKey("FeesId")]
        public Fees? Fees { get; set; }
        //[Required]
        public int EstablishmentId { get; set; }
        //[ForeignKey("EstablishmentId")]
        //public Establishment? Establishment { get; set; }

        [Required]
        [StringLength(50)]
        public string? InvoiceNo { get; set; }

        [StringLength(50)]
        public string? ReceiptNo { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        //Payment Type Online/Offline
        public byte? PaymentType { get; set; }  // 1 = Online, 2 = Offline

        // Payment method used with transaction 
        public byte? PaymentMethod { get; set; }      // 0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card, 5 = Money Order

        [StringLength(100)]
        public string? PayMethodType { get; set; }
        public byte PaymentStatus { get; set; }       //1 = Pending, 2= Paid, 3 = Cancelled, 4 = Failure 

        public int InvoiceBy { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public int? PaymentBy { get; set; }
        public DateTime? PaymentOn { get; set; }

        [StringLength(20)]
        public string? ReferenceNumber { get; set; }

        [StringLength(40)]
        public string? VoidedTransactionNo { get; set; }

        public DateTime? RefundVoidDate { get; set; }

        public int? RefundVoidBy { get; set; }

        public string? ReasonForRefundVoid { get; set; }

        public int? OldPaymentId { get; set; }

        public bool IsPermitFee { get; set; }

        public bool IsVoidEnabled { get; set; }

        public int? RefundVoidPaymentId { get; set; }
    }
}
