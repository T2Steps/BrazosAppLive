using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.Jetpay
{
    public class RedirectResponseModel
    {
        public string transactionIdentifier { get; set; }
        public string status { get; set; }
        public Error[] errors { get; set; }

        public class Error
        {
            public string id { get; set; }
            public string message { get; set; }
        }
    }
}
