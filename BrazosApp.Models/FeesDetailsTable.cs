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
    public class FeesDetailsTable
    {
        public FeesDetailsTable()
        {
            CreatedOn = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int FeesId { get; set; }
        [ForeignKey("FeesId")]
        public Fees? Fees { get; set; }

        
        public int? EstablishmentTypeId { get; set; }
        //[ForeignKey("EstablishmentTypeId")]
        //public EstablishmentTypes? EstablishmentTypes { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        //public bool IsActive { get; set; }

        public string? Title { get; set; }
        public byte Status { get; set; }  //1 = Pending, 2 = Paid, 3 = Cancelled

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
