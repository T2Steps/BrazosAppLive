using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class PaymentDetailsTable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }

        public string? Title { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        public byte PaymentStatus { get; set; } //1 = Pending, 2= Paid, 3 = Cancelled, 4 = Failure 

        public int? PaymentBy { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? RefundVoidBy { get; set; }
        public DateTime? RefundVoidDate { get; set; }
    }
}
