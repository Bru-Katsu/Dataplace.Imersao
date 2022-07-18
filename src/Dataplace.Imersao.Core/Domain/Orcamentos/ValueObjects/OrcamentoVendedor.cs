using Dataplace.Imersao.Core.Domain.Excepions;
using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoVendedor : IEquatable<OrcamentoVendedor>
    {
        public OrcamentoVendedor(string codigo)
        {
            if(string.IsNullOrEmpty(codigo))
                throw new DomainException("O código do vendedor não pode ser branco ou nulo!");

            if(codigo.Length > 10)
                throw new DomainException("O código do vendedor deve conter no máximo 10 caracteres!");

            Codigo = codigo;
        }

        public string Codigo { get; }

        public bool Equals(OrcamentoVendedor other)
        {
            return Codigo == other.Codigo;
        }
    }
}
