using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class FoodRenewalHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }

        public DateTime ActivationDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsCurrentYear { get; set; }
    }
}
