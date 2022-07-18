using Dataplace.Imersao.Core.Domain.Excepions;
using Dataplace.Imersao.Core.Domain.Orcamentos.Enums;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;
using Dataplace.Imersao.Core.Shared;
using FluentValidation;

namespace Dataplace.Imersao.Core.Domain.Orcamentos
{
    public class OrcamentoItem : Entity<OrcamentoItem>
    {
        public OrcamentoItem(string cdEmpresa, string cdFilial, int numOrcamento, OrcamentoProduto produto, decimal quantidade, OrcamentoItemPreco preco)
        {
            CdEmpresa = cdEmpresa;
            CdFilial = cdFilial;
            NumOrcamento = numOrcamento;
            Produto = produto;
            Quantidade = quantidade;

            AtribuirPreco(preco);
        }

        public int Seq { get; private set; }
        public string CdEmpresa { get; private set; }
        public string CdFilial { get; private set; }
        public int NumOrcamento { get; private set; }

        public OrcamentoProduto Produto { get; private set; }
        public OrcamentoItemEnum Situacao { get; private set; }

        #region Setters Situação
        public void FecharItem()
        {
            if (Situacao == OrcamentoItemEnum.Fechado)
                throw new DomainException("O item já está fechado!");

            Situacao = OrcamentoItemEnum.Fechado;
        }

        public void ReabrirItem()
        {
            if (Situacao == OrcamentoItemEnum.Aberto)
                throw new DomainException("O item já está aberto!");

            Situacao = OrcamentoItemEnum.Aberto;
        }

        public void CancelarItem()
        {
            if (Situacao == OrcamentoItemEnum.Cancelado)
                throw new DomainException("O item já está cancelado!");

            Situacao = OrcamentoItemEnum.Cancelado;
        }
        #endregion

        public decimal Quantidade { get; private set; }
        public OrcamentoItemPreco Preco { get; private set; }
        public decimal Total { get; private set; }

        #region Setters Preço
        private void AtribuirPreco(OrcamentoItemPreco preco)
        {
            Preco = preco;
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            if (Quantidade < 0)
                throw new DomainException("Quantidade de itens não pode ser menor que zero!");

            if (Preco.PrecoVenda < 0)
                new DomainException("O preço de venda não pode ser negativo!");

            Total = Quantidade * Preco.PrecoVenda;
        }
        #endregion

        #region Validations
        public override bool IsValid()
        {
            CreateValidations();
            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }

        private void CreateValidations()
        {
            RuleFor(x => x.CdEmpresa)
                .NotEmpty()
                .WithMessage("Código da empresa é requirido!")
                .MaximumLength(5)
                .WithMessage("O tamanho máximo do código da empresa é de 5 caracteres!");

            RuleFor(x => x.CdFilial)
                .NotEmpty()
                .WithMessage("Código da filial é requirido!")
                .MaximumLength(2)
                .WithMessage("O tamanho máximo do código da filial é de 2 caracteres!");

            RuleFor(x => x.NumOrcamento)
                .GreaterThan(0)
                .WithMessage("Número do orçamento inválido!");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0)
                .WithMessage("A quantidade precisa ser maior que zero!");
        }

        #endregion
    }
}
