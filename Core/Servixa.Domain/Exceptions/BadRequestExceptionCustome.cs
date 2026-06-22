using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Domain.Exceptions
{
    public class BadRequestExceptionCustome : Exception
    {
        public IEnumerable<string>? _errors { get; set; }

        public BadRequestExceptionCustome(string msg , IEnumerable<string>? errors = null): base(msg)
        {
            _errors = errors;
        }
    }
}
