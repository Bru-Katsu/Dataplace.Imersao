using Dataplace.Imersao.Core.Domain.Excepions;
using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoCliente : IEquatable<OrcamentoCliente>
    {
        public OrcamentoCliente(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                throw new DomainException("O código do cliente não pode ser nulo ou branco!");

            if (codigo.Length > 7)
                throw new DomainException("Tamanho do código do cliente deve conter no máximo 7 caracteres.");

            Codigo = codigo;
        }

        public string Codigo { get; private set; }

        public bool Equals(OrcamentoCliente other)
        {
            return Codigo == other.Codigo;
        }
    }
}
