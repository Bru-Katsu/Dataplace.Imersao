using Dataplace.Imersao.Core.Domain.Excepions;
using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{

    public class OrcamentoTabelaPreco : IEquatable<OrcamentoTabelaPreco>
    {
        public OrcamentoTabelaPreco(string cdTabela, short sqTabela)
        {
            if (string.IsNullOrEmpty(cdTabela))
                throw new DomainException("O Código da tabela não pode ser branco ou nulo!");

            if (cdTabela.Length > 5)
                throw new DomainException("O Código da tabela não pode ser maior que 5 caracteres!");

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
