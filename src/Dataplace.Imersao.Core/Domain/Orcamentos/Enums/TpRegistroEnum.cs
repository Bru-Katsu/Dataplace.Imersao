namespace Dataplace.Imersao.Core.Domain.Orcamentos.Enums
{
    public enum TpRegistroEnum
    {
        ProdutoFinal = 1,
        SubProduto = 2,
        MateriaPrima = 3,
        Componente = 4,
        Servico = 5,
        BemDeConsumo = 9
    }

    public static class TpRegistroEnumExtension
    {
        public static string ToDataValue(this TpRegistroEnum value)
        {
            return
                value == TpRegistroEnum.ProdutoFinal ? "1" :
                value == TpRegistroEnum.SubProduto ? "2" :
                value == TpRegistroEnum.MateriaPrima ? "3" :
                value == TpRegistroEnum.Componente ? "4" :
                value == TpRegistroEnum.Servico ? "5" :
                value == TpRegistroEnum.BemDeConsumo ? "9" : null;


        }

        public static TpRegistroEnum? ToTpRegistroEnum(this string value)
        {
            TpRegistroEnum? t =
                   value == "1" ? TpRegistroEnum.ProdutoFinal :
                   value == "2" ? TpRegistroEnum.SubProduto :
                   value == "3" ? TpRegistroEnum.MateriaPrima :
                   value == "4" ? TpRegistroEnum.Componente :
                   value == "5" ? TpRegistroEnum.Servico :
                   value == "9" ? TpRegistroEnum.BemDeConsumo : default;

            return t;
        }
    }
}
