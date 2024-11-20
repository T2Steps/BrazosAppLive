using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Services
{
    public interface IJetPayService
    {
        Task<bool> PaymentProcess(string id);
    }
}
