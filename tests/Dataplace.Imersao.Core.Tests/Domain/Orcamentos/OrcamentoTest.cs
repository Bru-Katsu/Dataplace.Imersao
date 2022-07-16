using Dataplace.Imersao.Core.Domain.Excepions;
using Dataplace.Imersao.Core.Domain.Orcamentos;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;
using Dataplace.Imersao.Core.Tests.Fixtures;
using System;
using Xunit;

namespace Dataplace.Imersao.Core.Tests.Domain.Orcamentos
{
    [Collection(nameof(OrcamentoCollection))]
    public class OrcamentoTest
    {
        private readonly OrcamentoFixture _fixture;
        public OrcamentoTest(OrcamentoFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void NovoOrcamentoDevePossuirValoresValidos()
        {
            // Arrange Act
            var orcamento = _fixture.NovoOrcamento();

            // Assert
            Assert.Equal(_fixture.CdEmpresa, orcamento.CdEmpresa);
            Assert.Equal(_fixture.CdFilial, orcamento.CdFilial);
            
            Assert.Equal(_fixture.NumOrcaemtp, orcamento.NumOrcamento);

            Assert.Equal(_fixture.Vendedor, orcamento.Vendedor);
            Assert.Equal(_fixture.Cliente, orcamento.Cliente);
            Assert.Equal(_fixture.UserName, orcamento.Usuario);

            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Aberto, orcamento.Situacao);
            
            Assert.Null(orcamento.Validade);

            Assert.NotNull(orcamento.TabelaPreco);            
            Assert.Equal(_fixture.TabelaPreco, orcamento.TabelaPreco);
        }

        [Fact]
        public void FecharOrcamentoDeveRetornarStatusFechado()
        {
            // Arrange
            var orcamento = _fixture.NovoOrcamento();

            // Act
            orcamento.FecharOrcamento();

            // Assert
            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Fechado, orcamento.Situacao);
            Assert.NotNull(orcamento.DtFechamento);
        }


        [Fact]
        public void TentarFecharOrcamentoJaFechadoRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamento();
            orcamento.FecharOrcamento();

            // act & assert
            Assert.Throws<DomainException>(() => orcamento.FecharOrcamento());
        }


        [Fact]
        public void ReabrirOrcamentoDeveRetornarStatusAberto()
        {
            // Arrange
            var orcamento = _fixture.NovoOrcamento();
            orcamento.FecharOrcamento();
            // Act
            orcamento.ReabrirOrcamento();

            // Assert
            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Aberto, orcamento.Situacao);
            Assert.Null(orcamento.DtFechamento);
        }


        [Fact]
        public void TentarReabrirOrcamentoJaAbertoRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamento();

            // act & assert
            Assert.Throws<DomainException>(() => orcamento.ReabrirOrcamento());
        }

        [Fact]
        public void CancelarOrcamentoDeveRetornarStatusCancelado()
        {
            // Arrange
            var orcamento = _fixture.NovoOrcamento();
            orcamento.FecharOrcamento();
            // Act
            orcamento.CancelarOrcamento();

            // Assert
            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Cancelado, orcamento.Situacao);
            Assert.Null(orcamento.DtFechamento);
        }


        [Fact]
        public void TentarCancelarOrcamentoJaCanceladoRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamento();
            orcamento.CancelarOrcamento();

            // act & assert
            Assert.Throws<DomainException>(() => orcamento.CancelarOrcamento());
        }

        [Fact]
        public void DefinirDiasDeValidadeDataDeveEstarCorreta()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamento();
            var dataEsperada = orcamento.DtOrcamento.AddDays(30);

            //act
            orcamento.DefinirValidade(30);

            //assert
            Assert.Equal(dataEsperada, orcamento.Validade.Data);
            Assert.Equal(30, orcamento.Validade.Dias);
        }

        [Fact]
        public void TentarDefinirDiasDeValidadeMenorQueZeroDeveRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamento();

            //act & assert
            Assert.Throws<DomainException>(() => orcamento.DefinirValidade(-1));
        }

        [Fact]
        public void AdicionarItemAoOrcamento()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamento();
            var produto = new OrcamentoProduto(Core.Domain.Orcamentos.Enums.TpRegistroEnum.ProdutoFinal, "1000");
            var preco = new OrcamentoItemPrecoTotal(10, 15);

            //act
            orcamento.AdicionarItem(new OrcamentoItem("", "", orcamento.NumOrcamento, produto, 5, preco));
        }
    }
}
