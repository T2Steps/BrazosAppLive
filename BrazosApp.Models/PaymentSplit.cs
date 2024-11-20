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
    public class PaymentSplit
    {
        [Key]
        public int Id { get; set; }
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }

        public byte? PaymentMethod { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [StringLength(20)]
        public string? ReferenceNumber { get; set; }
    }
}
