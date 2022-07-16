using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{

    public class OrcamentoTabelaPreco : IEquatable<OrcamentoTabelaPreco>
    {
        public OrcamentoTabelaPreco(string cdTabela, short sqTabela)
        {
            CdTabela = cdTabela;
            SqTabela = sqTabela;
        }

        public string CdTabela { get; }
        public short SqTabela { get; }

        public bool Equals(OrcamentoTabelaPreco other)
        {
            return CdTabela == other.CdTabela && SqTabela == other.SqTabela;
        }
    }
}
