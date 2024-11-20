using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility.Helpers
{
    public interface IEncrypt
    {
        string Encrypt256(string text);

        string Decrypt256(string text);
    }
}
