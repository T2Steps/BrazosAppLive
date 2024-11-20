using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Events
    {
        public Events()
        {
            CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string? EncryptedId { get; set; }

        //public virtual TFOperationDetails Details { get; set; }
    }
}
