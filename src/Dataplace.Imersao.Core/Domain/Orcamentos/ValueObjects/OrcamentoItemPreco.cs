using Dataplace.Imersao.Core.Domain.Excepions;
using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public abstract class OrcamentoItemPreco
    {
        public decimal PrecoTabela { get; protected set; }
        public decimal PrecoVenda { get; protected set; }
        public decimal PercAltPreco { get; protected set; }
    }

    public class OrcamentoItemPrecoTotal : OrcamentoItemPreco
    {
        public OrcamentoItemPrecoTotal(decimal precoTabela, decimal precoVenda) 
        {
            if(precoTabela <= 0)
                throw new DomainException("Preço de tabela não pode ser negativo!");

            if (precoVenda <= 0)
                throw new DomainException("Preço de venta não pode ser negativo!");

            PrecoTabela = precoTabela;
            PrecoVenda = precoVenda;
            PercAltPreco = (precoVenda * 100 / precoTabela) - 100;
        }
    }   

    public class OrcamentoItemPrecoPercentual : OrcamentoItemPreco
    {
        public OrcamentoItemPrecoPercentual(decimal precoTabela, decimal perAltPreco) 
        {
            if (precoTabela <= 0)
                throw new DomainException("Preço de tabela não pode ser negativo!");

            PrecoTabela = precoTabela;
            PercAltPreco = perAltPreco;

            var decontoAcrescimo = precoTabela * Math.Abs(perAltPreco) / 100;

            if (perAltPreco < 0)
                PrecoVenda = PrecoTabela - decontoAcrescimo;
            else
                PrecoVenda = PrecoTabela + decontoAcrescimo;
        }
    }
}
