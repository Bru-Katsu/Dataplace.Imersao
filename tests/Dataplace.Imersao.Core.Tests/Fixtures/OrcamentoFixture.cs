using Dataplace.Imersao.Core.Domain.Orcamentos;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;

namespace Dataplace.Imersao.Core.Tests.Fixtures
{
    public class OrcamentoFixture
    {
        internal string CdEmpresa = "IMS";
        internal string CdFilial = "01";
        internal OrcamentoCliente Cliente = new OrcamentoCliente("CLI01");
        internal OrcamentoVendedor Vendedor = new OrcamentoVendedor("VDD01");
        internal OrcamentoUsuario UserName = new OrcamentoUsuario("sym_usuario");
        internal int NumOrcaemtp = 1000;
        internal OrcamentoTabelaPreco TabelaPreco = new OrcamentoTabelaPreco("2022", 1);


        public Orcamento NovoOrcamento()
        {
            return Orcamento.Factory.Orcamento(
                CdEmpresa, 
                CdFilial,
                NumOrcaemtp,
                Cliente, 
                UserName,
                Vendedor, 
                TabelaPreco);
        }

    }
}
