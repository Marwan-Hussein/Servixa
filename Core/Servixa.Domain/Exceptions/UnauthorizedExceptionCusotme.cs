using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Domain.Exceptions
{
    public class UnauthorizedExceptionCusotme (string msg = "Invalid Operation") : Exception(msg)
    {
    }
}
