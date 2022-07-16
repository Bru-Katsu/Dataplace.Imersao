using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoCliente : IEquatable<OrcamentoCliente>
    {
        public OrcamentoCliente(string codigo)
        {
            Codigo = codigo;
        }

        public string Codigo { get; private set; }

        public bool Equals(OrcamentoCliente other)
        {
            return Codigo == other.Codigo;
        }
    }
}
