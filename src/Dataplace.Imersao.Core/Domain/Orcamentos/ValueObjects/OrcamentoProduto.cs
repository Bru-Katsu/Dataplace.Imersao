using Dataplace.Imersao.Core.Domain.Excepions;
using Dataplace.Imersao.Core.Domain.Orcamentos.Enums;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoProduto
    {
        public OrcamentoProduto(TpRegistroEnum tpRegistro, string cdRegistro)
        {
            if (string.IsNullOrEmpty(cdRegistro))
                throw new DomainException("O Código da tabela não pode ser branco ou nulo!");

            if (cdRegistro.Length > 5)
                throw new DomainException("O Código da tabela não pode ser maior que 5 caracteres!");

            TpProduto = tpRegistro;
            CdProduto = cdRegistro;
        }

        public TpRegistroEnum TpProduto { get; }
        public string CdProduto { get; }
    }
}
