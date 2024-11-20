using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class TFOperationDetails
      {
            public TFOperationDetails()
            {
                  CreatedOn= DateTime.Now;
                  IsActive = true;
            }
            [Key]
            public int Id { get; set; }

            public int EstablishmentId { get; set; }
            [ForeignKey("EstablishmentId")]
            public Establishment? Establishment { get; set; }

            public int EventId { get; set; }
            [ForeignKey("EventId")]
            public Events? Event { get; set; }

            public string? ListOfFoodToBePrepared { get; set; }

            public string? OtherPermitHolderAndSite { get; set; }

            public TimeSpan PreparingTime { get; set; }

            public TimeSpan ServingTime { get; set; }

            public bool IsActive { get; set; }
            public DateTime CreatedOn { get; set; }

            public int CreatedBy { get; set; }

            public DateTime? UpdatedOn { get; set; }

            public int? UpdatedBy { get; set; }

            public TFOperationDetails GetClone()
            {
                  return (TFOperationDetails)this.MemberwiseClone();
            }
      }
}
