using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class PaymentIndexVM
      {
            public string? Name { get; set; }
            public string? Owner { get; set; }
            public string? Permit { get; set; }
            public string? Address { get; set; }

            public string? Amount { get; set; }

            public string? InvoiceNo { get; set; }

            //public DateTime? FromDate { get; set; }
            //public DateTime? ToDate { get; set; }

            public OfflinePaymentVM? OfflinePaymentVM { get; set; }
      }
}
