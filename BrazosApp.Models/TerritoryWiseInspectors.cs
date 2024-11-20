using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class TerritoryWiseInspectors
    {
        public TerritoryWiseInspectors()
        {
            CreatedOn = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "ⓘ Required Field ")]
        public int TerritoryId { get; set; }

        [ForeignKey("TerritoryId")]
        public Territory? Territory { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        public int AssignedUserId { get; set; }
        [ForeignKey("AssignedUserId")]
        public Users? AssignedUser { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public TerritoryAssignedType? Type { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; }

        [NotMapped]
        public string? EncryptedId { get; set; }

    }
}
