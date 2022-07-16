using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new ArgumentOutOfRangeException(nameof(precoTabela));

            if (precoVenda <= 0)
                throw new ArgumentOutOfRangeException(nameof(precoVenda));

            this.PrecoTabela = precoTabela;
            this.PrecoVenda = precoVenda;
            this.PercAltPreco = (precoVenda * 100 / precoTabela) - 100;
        }
    }   

    public class OrcamentoItemPrecoPercentual : OrcamentoItemPreco
    {
        public OrcamentoItemPrecoPercentual(decimal precoTabela, decimal perAltPreco) 
        {
            if (precoTabela <= 0)
                throw new ArgumentOutOfRangeException(nameof(precoTabela));

            this.PrecoTabela = precoTabela;
            this.PercAltPreco = perAltPreco;

            var decontoAcrescimo = precoTabela * Math.Abs(perAltPreco) / 100;

            if (perAltPreco < 0)
                this.PrecoVenda = this.PrecoTabela - decontoAcrescimo;
            else
                this.PrecoVenda = this.PrecoTabela + decontoAcrescimo;
        }
    }
}
