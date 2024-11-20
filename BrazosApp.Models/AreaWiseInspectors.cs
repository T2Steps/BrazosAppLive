using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class AreaWiseInspectors
      {
            public AreaWiseInspectors()
            {
                  CreatedOn = DateTime.Now;
            }
            [Key]
            public int Id { get; set; }
            [Required(ErrorMessage = "ⓘ Required Field ")]
            public int AreaId { get; set; }

            [ForeignKey("AreaId")]
            public Area? Area { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            public int AssignedUserId { get; set; }
            [ForeignKey("AssignedUserId")]
            public Users? AssignedUser { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int? UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }

            public bool IsDeleted { get; set; }

            [NotMapped]
            public string? EncryptedId { get; set; }
      }
}
