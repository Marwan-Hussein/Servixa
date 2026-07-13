using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Domain.Exceptions
{
    public class NotFoundExceptionCustome (string msg) : Exception(msg)
    {
    }
}
