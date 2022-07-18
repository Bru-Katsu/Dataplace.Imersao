using System;

namespace Dataplace.Imersao.Core.Domain.Excepions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
