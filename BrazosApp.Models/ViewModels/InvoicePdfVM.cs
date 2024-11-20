using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class InvoicePdfVM
      {
            public Fees? Fees { get; set; }

            public Payment? Payment { get; set; }
            public Establishment? Establishment { get; set; }

            public string? PaymentMethod { get; set; }

            public IEnumerable<FeesDetailsTable>? FeesDetails { get; set; }
            public IEnumerable<PaymentDetailsTable>? PaymentDetails { get; set; }
            public IEnumerable<PaymentSplit>? PaymentSplits { get; set; }

            public string? Program { get; set; }
            public string? Jurisdiction { get; set; }

            public string? InvoiceDate { get; set; }

            public DateTime PaymentDate { get; set; }

            public string? InvoiceCreatedBy { get; set; }
      }
}
