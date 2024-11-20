using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class RenewalTempHistory
    {
        public int Id { get; set; }
        public int EstId { get; set; }
        public string? Est_Name { get; set; }
        [StringLength(20)]
        public string? PermitNumber { get; set; }
        [StringLength(100)]
        public string? Owner { get; set; }
        [StringLength(250)]
        public string? Email { get; set; }
		
		public decimal FeesAmount { get; set; }

        public string? Title { get; set; }

        public int PaymentId { get; set; }

        public byte PermitStatus { get; set; }

        public string? Address { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? State { get; set; }
        [StringLength(10)]
        public string? Zip { get; set; }
    }
}
