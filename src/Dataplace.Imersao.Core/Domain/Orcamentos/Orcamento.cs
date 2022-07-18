using Dataplace.Imersao.Core.Domain.Excepions;
using Dataplace.Imersao.Core.Domain.Orcamentos.Enums;
using Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects;
using Dataplace.Imersao.Core.Shared;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataplace.Imersao.Core.Domain.Orcamentos
{
    public class Orcamento : Entity<Orcamento>
    {
        private Orcamento(string cdEmpresa, string cdFilial, int numOrcamento, OrcamentoCliente cliente, OrcamentoUsuario usuario, OrcamentoVendedor vendedor, OrcamentoTabelaPreco tabelaPreco)
        {
            CdEmpresa = cdEmpresa;
            CdFilial = cdFilial;
            Cliente = cliente;
            NumOrcamento = numOrcamento;
            Usuario = usuario;
            Vendedor = vendedor;
            TabelaPreco = tabelaPreco;

            // default
            Situacao = OrcamentoStatusEnum.Aberto;
            DtOrcamento = DateTime.Now.Date;
            ValorTotal = decimal.Zero;

            _itens = new List<OrcamentoItem>();
        }

        public string CdEmpresa { get; private set; }
        public string CdFilial { get; private set; }
        public int NumOrcamento { get; private set; }

        public OrcamentoCliente Cliente { get; private set; }
        public OrcamentoVendedor Vendedor { get; private set; }
        public OrcamentoUsuario Usuario { get; private set; }

        public OrcamentoStatusEnum Situacao { get; private set; }

        #region Setters Situação
        public void FecharOrcamento()
        {
            if (Situacao == OrcamentoStatusEnum.Fechado)
                throw new DomainException("Orçamento já está fechado!");

            Situacao = OrcamentoStatusEnum.Fechado;
            DtFechamento = DateTime.Now.Date;
        }

        public void ReabrirOrcamento()
        {
            if (Situacao == OrcamentoStatusEnum.Aberto)
                throw new DomainException("Orçamento já está aberto!");

            Situacao = OrcamentoStatusEnum.Aberto;
            DtFechamento = null;
        }

        public void CancelarOrcamento()
        {
            if (Situacao == OrcamentoStatusEnum.Cancelado)
                throw new DomainException("Orçamento já está cancelado!");

            Situacao = OrcamentoStatusEnum.Cancelado;
            DtFechamento = null;
        }
        #endregion

        public OrcamentoValidade Validade { get; private set; }

        #region Setters Validade
        public void DefinirValidade(int diasValidade)
        {
            Validade = new OrcamentoValidade(this, diasValidade);
        }
        #endregion

        public OrcamentoTabelaPreco TabelaPreco { get; private set; }
        public decimal ValorTotal { get; private set; }
        
        public DateTime DtOrcamento { get; private set; }
        public DateTime? DtFechamento { get; private set; }

        #region Itens
        public IReadOnlyCollection<OrcamentoItem> Itens => _itens.ToList();
        
        public ICollection<OrcamentoItem> _itens;
        public void AdicionarItem(OrcamentoItem item)
        {
            //validar item
            if (item == null)
                throw new DomainException("O Item não pode ser nulo!");

            if (!item.IsValid())
                throw new DomainException("Item inválido!");

            _itens.Add(item);
            ValorTotal += item.Total;
        }
        #endregion

        #region Validations
        public override bool IsValid()
        {
            CreateValidations();
            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }

        public void CreateValidations()
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
        }
        #endregion

        #region Factory Methods
        public static class Factory
        {
            public static Orcamento Orcamento(string cdEmpresa, string cdFilial, int numOrcamento, OrcamentoCliente cliente , OrcamentoUsuario usuario, OrcamentoVendedor vendedor, OrcamentoTabelaPreco tabelaPreco)
            {
                return new Orcamento(cdEmpresa, cdFilial, numOrcamento, cliente, usuario, vendedor, tabelaPreco);
            }

            public static Orcamento OrcamentoRapido(string cdEmpresa, string cdFilial, int numOrcamento, OrcamentoUsuario usuario, OrcamentoVendedor vendedor, OrcamentoTabelaPreco tabelaPreco)
            {
                return new Orcamento(cdEmpresa, cdFilial, numOrcamento, null, usuario, vendedor, tabelaPreco);
            }
        }

        #endregion
    }
}
