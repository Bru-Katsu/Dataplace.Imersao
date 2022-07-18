using FluentValidation;
using FluentValidation.Results;
using System;

namespace Dataplace.Imersao.Core.Shared
{
    public class Validable<T> : AbstractValidator<T>
    {
        public ValidationResult ValidationResult { get; protected set; }
        public virtual bool IsValid() => throw new NotImplementedException();
    }
}
