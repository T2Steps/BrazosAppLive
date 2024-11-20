using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Fees
    {
        public Fees()
        {
            CreatedOn = DateTime.Now;
            Status = 1;
            IsVoidEnabled = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }

        [StringLength(50)]
        public string? InvoiceNo { get; set; }

        [Precision(18, 2)]
        public decimal BaseAmount { get; set; }

        [Precision(18, 2)]
        public decimal FeesCalculation { get; set; }

        [Precision(18,2)]
        public decimal Amount { get; set; }

        public byte Status { get; set; }  //1 = Pending, 2 = Paid, 3 = Cancelled

        public bool IsPermitFee { get; set; }

        public bool IsVoidEnabled { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
