using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class TransactionSearchVM
    {
        public string? Name { get; set; }
        public string? Permit { get; set; }
        public string? Address { get; set; }

        public string? Amount { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
