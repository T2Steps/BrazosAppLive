using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Document
    {
        public Document()
        {
            ReceivedDate = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }

        [StringLength(20)]
        public string? DocFileName { get; set; }
        [NotMapped]
        public IFormFile? DocFile { get; set; }

        [StringLength(50)]
        public string? Description { get; set; }

        public string? AssociatedNote { get; set; }

        public int UploadedBy { get; set; }

        public DateTime ReceivedDate { get; set; }

        [NotMapped]
        public string? EncryptedId { get; set; }

        public Document GetClone()
        {
            return (Document)this.MemberwiseClone();
        }
    }
}
