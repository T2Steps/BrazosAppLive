using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Application
    {
        public Application()
        {
            ApplicationDate = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        public int ApplicationForId { get; set; }

        [ForeignKey("ApplicationForId")]
        public ApplicationFor? ApplicationFor { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        public string? OwnerName { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        [EmailAddress(ErrorMessage = "ⓘ Invalid Email")]
        public string? EmailId { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        public string? ContactNumber { get; set; }

        public DateTime ApplicationDate { get; set; }


        public byte Status { get; set; }

        public string? ApplicationNo { get; set; }

        [NotMapped]
        public string? EncryptedId { get; set; }
    }
}
