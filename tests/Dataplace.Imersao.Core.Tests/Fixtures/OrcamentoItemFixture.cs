using Dataplace.Imersao.Core.Domain.Orcamentos;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataplace.Imersao.Core.Tests.Fixtures
{
    public class OrcamentoItemFixture
    {
        internal OrcamentoProduto produto = new OrcamentoProduto(Core.Domain.Orcamentos.Enums.TpRegistroEnum.ProdutoFinal, "1000");
        internal OrcamentoItemPrecoTotal preco = new OrcamentoItemPrecoTotal(10, 15);

        public OrcamentoItem NovoItemValido()
        {
            return new OrcamentoItem("DPI", "01", 5, produto, 5, preco);
        }

        public OrcamentoItem NovoItemInvalido()
        {
            return new OrcamentoItem("      ", "   ", 0, produto, 0, preco);
        }
    }
}
