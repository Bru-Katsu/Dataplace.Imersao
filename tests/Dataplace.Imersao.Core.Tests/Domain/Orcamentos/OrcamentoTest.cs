using Dataplace.Imersao.Core.Domain.Excepions;
using Dataplace.Imersao.Core.Domain.Orcamentos;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;
using Dataplace.Imersao.Core.Tests.Fixtures;
using System.Linq;
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

        [Fact, Trait("Orçamento", "Fechar orçamento")]
        public void FecharOrcamentoDeveAtualizarParaStatusFechado()
        {
            // Arrange
            var orcamento = _fixture.NovoOrcamentoValido();

            // Act
            orcamento.FecharOrcamento();

            // Assert
            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Fechado, orcamento.Situacao);
            Assert.NotNull(orcamento.DtFechamento);
        }


        [Fact, Trait("Orçamento", "Fechar orçamento")]
        public void TentarFecharOrcamentoJaFechadoRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            orcamento.FecharOrcamento();

            // act & assert
            Assert.Throws<DomainException>(() => orcamento.FecharOrcamento());
        }


        [Fact, Trait("Orçamento", "Reabrir orçamento")]
        public void ReabrirOrcamentoDeveAtualizarParaStatusAberto()
        {
            // Arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            orcamento.FecharOrcamento();
            // Act
            orcamento.ReabrirOrcamento();

            // Assert
            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Aberto, orcamento.Situacao);
            Assert.Null(orcamento.DtFechamento);
        }


        [Fact, Trait("Orçamento", "Reabrir orçamento")]
        public void TentarReabrirOrcamentoJaAbertoRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();

            // act & assert
            Assert.Throws<DomainException>(() => orcamento.ReabrirOrcamento());
        }

        [Fact, Trait("Orçamento", "Cancelar orçamento")]
        public void CancelarOrcamentoDeveAtualizarParaStatusCancelado()
        {
            // Arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            orcamento.FecharOrcamento();
            // Act
            orcamento.CancelarOrcamento();

            // Assert
            Assert.Equal(Core.Domain.Orcamentos.Enums.OrcamentoStatusEnum.Cancelado, orcamento.Situacao);
            Assert.Null(orcamento.DtFechamento);
        }


        [Fact, Trait("Orçamento", "Cancelar orçamento")]
        public void TentarCancelarOrcamentoJaCanceladoRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            orcamento.CancelarOrcamento();

            // act & assert
            Assert.Throws<DomainException>(() => orcamento.CancelarOrcamento());
        }

        [Fact, Trait("Orçamento", "Adicionar validade")]
        public void DefinirDiasDeValidadeDataDeveEstarCorreta()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            var dataEsperada = orcamento.DtOrcamento.AddDays(30);

            //act
            orcamento.DefinirValidade(30);

            //assert
            Assert.Equal(dataEsperada, orcamento.Validade.Data);
            Assert.Equal(30, orcamento.Validade.Dias);
        }

        [Fact, Trait("Orçamento", "Adicionar validade")]
        public void TentarDefinirDiasDeValidadeMenorQueZeroDeveRetornarException()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();

            //act & assert
            Assert.Throws<DomainException>(() => orcamento.DefinirValidade(-1));
        }

        [Fact, Trait("Orçamento", "Adicionar item")]
        public void AdicionarItemComValorAoOrcamentoDeveIncrementarTotal()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            var produto = new OrcamentoProduto(Core.Domain.Orcamentos.Enums.TpRegistroEnum.ProdutoFinal, "1000");
            var preco = new OrcamentoItemPrecoTotal(10, 15);
            var item = new OrcamentoItem("DPI", "01", orcamento.NumOrcamento, produto, 5, preco);

            //act
            orcamento.AdicionarItem(item);

            //assert
            Assert.Equal(item.Total, orcamento.ValorTotal);
        }

        [Fact, Trait("Orçamento", "Adicionar item")]
        public void AdicionarItemNuloDeveRetornarException()
        {
            //arrange
            var orcamento = _fixture.NovoOrcamentoValido();
            OrcamentoItem item = null;

            //act & assert
            Assert.Throws<DomainException>(() => orcamento.AdicionarItem(item));
        }

        [Fact, Trait("Orçamento", "Validar orçamento")]
        public void OrcamentoComValoresInvalidosDeveConterMensagensDeErro()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoInvalido();

            //act
            orcamento.IsValid();

            //assert
            var orcamentoMessages = new[] {
                    "Código da empresa é requirido!",
                    "O tamanho máximo do código da empresa é de 5 caracteres!",
                    "Código da filial é requirido!",
                    "O tamanho máximo do código da filial é de 2 caracteres!",
                    "Número do orçamento inválido!",
            };

            Assert.Contains(orcamento.ValidationResult.Errors.Select(e => e.ErrorMessage), (validation) => orcamentoMessages.Any(x => x == validation));
        }

        [Fact, Trait("Orçamento", "Validar orçamento")]
        public void OrcamentoComValoresValidosNaoDeveConterMensagensDeErro()
        {
            // arrange
            var orcamento = _fixture.NovoOrcamentoValido();

            //act
            orcamento.IsValid();

            //assert
            var orcamentoMessages = new[] {
                    "Código da empresa é requirido!",
                    "O tamanho máximo do código da empresa é de 5 caracteres!",
                    "Código da filial é requirido!",
                    "O tamanho máximo do código da filial é de 2 caracteres!",
                    "Número do orçamento inválido!",
            };

            Assert.DoesNotContain(orcamento.ValidationResult.Errors.Select(e => e.ErrorMessage), (validation) => orcamentoMessages.Any(x => x == validation));
        }

        //value objects
        [Fact, Trait("Orçamento", "Usuario")]
        public void OrcamentoUsuarioMaiorQueCaracteresPermitidosDeveRetornarException()
        {
            //arrange
            string username = string.Empty;
            for (int i = 0; i < 129; i++)
                username += "a";

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoUsuario(username));
        }

        [Fact, Trait("Orçamento", "Usuario")]
        public void OrcamentoUsuarioComCodigoEmBrancoDeveRetornarException()
        {
            //arrange
            string username = string.Empty;

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoUsuario(username));
        }

        [Fact, Trait("Orçamento", "Vendedor")]
        public void OrcamentoVendedorMaiorQueCaracteresPermitidosDeveRetornarException()
        {
            //arrange
            string vendedor = string.Empty;
            for (int i = 0; i < 11; i++)
                vendedor += "a";

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoVendedor(vendedor));
        }

        [Fact, Trait("Orçamento", "Vendedor")]
        public void OrcamentoVendedorComCodigoEmBrancoDeveRetornarException()
        {
            //arrange
            string vendedor = string.Empty;

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoVendedor(vendedor));
        }

        [Fact, Trait("Orçamento", "Cliente")]
        public void OrcamentoClienteMaiorQueCaracteresPermitidosDeveRetornarException()
        {
            //arrange
            string cliente = string.Empty;
            for (int i = 0; i < 8; i++)
                cliente += "a";

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoCliente(cliente));
        }

        [Fact, Trait("Orçamento", "Cliente")]
        public void OrcamentoClienteCodigoEmBrancoDeveRetornarException()
        {
            //arrange
            string cliente = string.Empty;

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoCliente(cliente));
        }

        [Fact, Trait("Orçamento", "Tabela de preço")]
        public void OrcamentoTabelaPrecoCodigoMaiorQueCaracteresPermitidosDeveRetornarException()
        {
            //arrange
            string codigo = string.Empty;
            for (int i = 0; i < 6; i++)
                codigo += $"{i}";

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoTabelaPreco(codigo, 1));
        }

        [Fact, Trait("Orçamento", "Tabela de preço")]
        public void OrcamentoTabelaPrecoCodigoEmBrancoDeveRetornarException()
        {
            //arrange
            string codigo = string.Empty;

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoTabelaPreco(codigo, 1));
        }
    }
}
