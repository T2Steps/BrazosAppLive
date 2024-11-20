using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Notes
    {
        public Notes()
        {
            CreatedOn = DateTime.Now;
        }

        [Key]
        public int NoteId { get; set; }
        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }
        [StringLength(80)]
        public string? Title { get; set; }
        //[StringLength(250)]
        public string? Description { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public byte Status { get; set; }

        [NotMapped]
        public string? EncryptedId { get; set; }
    }
}
