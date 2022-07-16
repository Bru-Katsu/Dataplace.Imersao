using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoVendedor : IEquatable<OrcamentoVendedor>
    {
        public OrcamentoVendedor(string codigo)
        {
            Codigo = codigo;
        }

        public string Codigo { get; }

        public bool Equals(OrcamentoVendedor other)
        {
            return Codigo == other.Codigo;
        }
    }
}
