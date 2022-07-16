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
        private Orcamento(string cdEmpresa, string cdFilial, int numOrcamento, OrcamentoCliente cliente,
            OrcamentoUsuario usuario, OrcamentoVendedor vendedor, OrcamentoTabelaPreco tabelaPreco)
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
            ValorTotal = 0;

            _itens = new List<OrcamentoItem>();
        }

        public string CdEmpresa { get; private set; }
        public string CdFilial { get; private set; }
        public int NumOrcamento { get; private set; }

        public OrcamentoCliente Cliente { get; private set; }
        public OrcamentoVendedor Vendedor { get; private set; }
        public OrcamentoUsuario Usuario { get; private set; }

        public OrcamentoStatusEnum Situacao { get; private set; }
        public OrcamentoValidade Validade { get; private set; }

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
                throw new DomainException("Item inválido!");

            if (!item.IsValid())
                throw new DomainException("Item inválido!");

            _itens.Add(item);
            ValorTotal += item.Total;
        }
        #endregion

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

        public void DefinirValidade(int diasValidade)
        {
            this.Validade = new OrcamentoValidade(this, diasValidade);
        }

        #region validations

        public override bool IsValid()
        {
            Validate(this);
            CreateValidations();
            return ValidationResult.IsValid;
        }

        public void CreateValidations()
        {
            RuleFor(x => x.CdEmpresa)
                .NotEmpty()
                .WithMessage("Código da empresa é requirido!");

            RuleFor(x => x.CdEmpresa)
                .NotEmpty()
                .WithMessage("Código da empresa é requirido!");

            RuleFor(x => x.NumOrcamento)
                .GreaterThan(0)
                .WithMessage("Número do orçamento inválido!");

            RuleFor(x => x.DtFechamento)
                .NotNull()
                .WithMessage("Data de fechamento inválida!")
                .When(x => x.Situacao == OrcamentoStatusEnum.Fechado);
        }

        #endregion

        #region factory methods
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
