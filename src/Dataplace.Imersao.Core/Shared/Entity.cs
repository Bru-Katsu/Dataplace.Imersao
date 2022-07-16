using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataplace.Imersao.Core.Shared
{
    public abstract class Entity<T> : AbstractValidator<T> where T : Entity<T>
    {
        public ValidationResult ValidationResult { get; protected set; }
        public virtual bool IsValid() => throw new NotImplementedException();
    }
}
