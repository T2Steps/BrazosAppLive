using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class TemperatureObservation
    {
        public int Id { get; set; }
        public int InspectionId { get; set; }
        public string? ItemName { get; set; }
        [StringLength(10, ErrorMessage = "ⓘ Invalid ")]
        public string? Temperature { get; set; }
    }
}
