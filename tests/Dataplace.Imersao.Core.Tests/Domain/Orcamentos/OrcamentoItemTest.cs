using Dataplace.Imersao.Core.Domain.Excepions;
using Dataplace.Imersao.Core.Domain.Orcamentos;
using Dataplace.Imersao.Core.Domain.Orcamentos.Enums;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;
using Dataplace.Imersao.Core.Tests.Fixtures;
using System.Linq;
using Xunit;

namespace Dataplace.Imersao.Core.Tests.Domain.Orcamentos
{
    [Collection(nameof(OrcamentoCollection))]
    public class OrcamentoItemTest
    {
        private readonly OrcamentoFixture _fixture;
        private readonly OrcamentoItemFixture _itemFixture;
        public OrcamentoItemTest(OrcamentoFixture fixture, OrcamentoItemFixture itemFixture)
        {
            _fixture = fixture;
            _itemFixture = itemFixture;
        }

        [Fact, Trait("Orçamento Item", "Cancelar Item")]
        public void OrcamentoItemCancelarDeveAtualizarStatus()
        {
            //arrange
            var item = _itemFixture.NovoItemValido();

            //act
            item.CancelarItem();

            //assert
            Assert.Equal(OrcamentoItemEnum.Cancelado, item.Situacao);
        }

        [Fact, Trait("Orçamento Item", "Cancelar Item")]
        public void OrcamentoItemCancelarItemJaCanceladoDeveRetornarException()
        {
            //arrange
            var item = _itemFixture.NovoItemValido();
            item.CancelarItem();

            //act & assert
            Assert.Throws<DomainException>(() => item.CancelarItem());
        }

        [Fact, Trait("Orçamento Item", "Fechar Item")]
        public void OrcamentoItemFecharDeveAtualizarStatus()
        {
            //arrange
            var item = _itemFixture.NovoItemValido();

            //act
            item.FecharItem();

            //assert
            Assert.Equal(OrcamentoItemEnum.Fechado, item.Situacao);
        }

        [Fact, Trait("Orçamento Item", "Fechar Item")]
        public void OrcamentoItemFecharItemJaFechadoDeveRetornarException()
        {
            //arrange
            var item = _itemFixture.NovoItemValido();
            item.FecharItem();

            //act & assert
            Assert.Throws<DomainException>(() => item.FecharItem());
        }


        [Fact, Trait("Orçamento Item", "Reabrir Item")]
        public void OrcamentoItemReabrirDeveAtualizarStatus()
        {
            //arrange
            var item = _itemFixture.NovoItemValido();
            item.FecharItem();

            //act
            item.ReabrirItem();

            //assert
            Assert.Equal(OrcamentoItemEnum.Aberto, item.Situacao);
        }

        [Fact, Trait("Orçamento Item", "Reabrir Item")]
        public void OrcamentoItemReabrirItemJaAbertoDeveRetornarException()
        {
            //arrange
            var item = _itemFixture.NovoItemValido();

            //act & assert
            Assert.Throws<DomainException>(() => item.ReabrirItem());
        }

        [Fact, Trait("Orçamento Item", "Validação do Item")]
        public void OrcamentoItemValidoNaoDeveConterMensagensDeErro()
        {
            //arrange            
            var item = _itemFixture.NovoItemValido();

            //act 
            item.IsValid();

            //assert
            var itemMessages = new[] {
                    "Código da empresa é requirido!",
                    "O tamanho máximo do código da empresa é de 5 caracteres!",
                    "Código da filial é requirido!",
                    "O tamanho máximo do código da filial é de 2 caracteres!",
                    "Número do orçamento inválido!",
                    "A quantidade precisa ser maior que zero!"
            };

            Assert.DoesNotContain(item.ValidationResult.Errors.Select(e => e.ErrorMessage), (validation) => itemMessages.Any(x => x == validation));
        }

        [Fact, Trait("Orçamento Item", "Validação do Item")]
        public void OrcamentoItemInvalidoDeveConterMensagensDeErro()
        {
            //arrange            
            var item = _itemFixture.NovoItemInvalido();

            //act 
            item.IsValid();

            //assert
            var itemMessages = new[] {
                    "Código da empresa é requirido!",
                    "O tamanho máximo do código da empresa é de 5 caracteres!",
                    "Código da filial é requirido!",
                    "O tamanho máximo do código da filial é de 2 caracteres!",
                    "Número do orçamento inválido!",
                    "A quantidade precisa ser maior que zero!"
            };

            Assert.Contains(item.ValidationResult.Errors.Select(e => e.ErrorMessage), (validation) => itemMessages.Any(x => x == validation));
        }

        //value objects
        [Fact, Trait("Orçamento Item", "Produto")]
        public void OrcamentoItemProdutoCodigoMaiorQueCaracteresPermitidosDeveRetornarException()
        {
            //arrange
            string codigo = string.Empty;
            for (int i = 0; i < 6; i++)
                codigo += $"{i}";

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoProduto(Core.Domain.Orcamentos.Enums.TpRegistroEnum.ProdutoFinal, codigo));
        }

        [Fact, Trait("Orçamento Item", "Produto")]
        public void OrcamentoItemProdutoCodigoEmBrancoDeveRetornarException()
        {
            //arrange
            string codigo = string.Empty;

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoProduto(Core.Domain.Orcamentos.Enums.TpRegistroEnum.ProdutoFinal, codigo));
        }

        [Theory, Trait("Orçamento Item", "Preço total")]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public void OrcamentoItemPrecoTotalValorTabelaOuVendaZeradoDeveRetornarException(decimal precoTabela, decimal precoVenda)
        {
            //arrange & act & assert
            Assert.Throws<DomainException>(() => new OrcamentoItemPrecoTotal(precoTabela, precoVenda));
        }

        [Theory, Trait("Orçamento Item", "Preço total")]
        [InlineData(10, 15, 50)]
        [InlineData(15, 10, -33.33)]
        [InlineData(15, 7.50, -50)]
        [InlineData(99, 99.25, 0.25)]
        public void OrcamentoItemPrecoTotalCalculoPercentualDeveEstarCorreto(decimal precoTabela, decimal precoVenda, decimal valorEsperado)
        {
            //arrange & act
            var preco = new OrcamentoItemPrecoTotal(precoTabela, precoVenda);
            
            //assert
            Assert.Equal(valorEsperado, preco.PercAltPreco, 2);
        }

        [Fact, Trait("Orçamento Item", "Preço por percentual")]
        public void OrcamentoItemPrecoPercentualValorTabelaZeradoDeveRetornarException()
        {
            //arrange
            var percentual = 5;
            var precoTabela = 0;

            //act & assert
            Assert.Throws<DomainException>(() => new OrcamentoItemPrecoPercentual(precoTabela, percentual));
        }

        [Theory, Trait("Orçamento Item", "Preço por percentual")]
        [InlineData(10, 50, 15)]
        [InlineData(10, -50, 5)]
        [InlineData(99, 0.25, 99.25)]
        public void OrcamentoItemPrecoPercentualCalculoPrecoDeVendaDeveEstarCorreto(decimal precoTabela, decimal percentual, decimal valorEsperado)
        {
            //arrange & act
            var preco = new OrcamentoItemPrecoPercentual(precoTabela, percentual);

            //assert
            Assert.Equal(valorEsperado, preco.PrecoVenda, 2);
        }
    }
}
