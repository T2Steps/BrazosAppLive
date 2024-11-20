using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class EstablishmentOwner
    {
        public EstablishmentOwner()
        {
            CreatedOn = DateTime.Now;
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }

        public string? Name { get; set; }
        public string? MailingAddress { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }       
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public EstablishmentOwner GetClone()
        {
            return (EstablishmentOwner)this.MemberwiseClone();
        }

    }
}
