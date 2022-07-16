using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.Enums
{
    public enum OrcamentoStatusEnum
    {
        Aberto,
        Fechado,
        Cancelado
    }

    public static class OrcamentoStatusEnumExtensions
    {
        public static string ToDataValue(this OrcamentoStatusEnum value)
        {
            return value == OrcamentoStatusEnum.Fechado ? "f" : 
                   value == OrcamentoStatusEnum.Aberto ? "p" :
                   value == OrcamentoStatusEnum.Cancelado ? "c" : 
                   null;
        }

        public static OrcamentoStatusEnum ToOrcamentoStatusEnum(this string value)
        {
            return string.IsNullOrEmpty(value) ? OrcamentoStatusEnum.Aberto :
                   value == "p" ? OrcamentoStatusEnum.Aberto :
                   value == "f" ? OrcamentoStatusEnum.Fechado :
                   OrcamentoStatusEnum.Cancelado;
        }
    }
}
