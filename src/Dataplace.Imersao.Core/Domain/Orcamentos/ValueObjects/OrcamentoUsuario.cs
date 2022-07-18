using Dataplace.Imersao.Core.Domain.Excepions;
using System;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoUsuario : IEquatable<OrcamentoUsuario>
    {
        public OrcamentoUsuario(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
                throw new DomainException("O nome do usuário não pode ser branco ou nulo!");

            if (usuario.Length > 128)
                throw new DomainException("O tamanho máximo do nome de usuário é 128 caracteres");

            Usuario = usuario;
        }

        public string Usuario { get; }

        public bool Equals(OrcamentoUsuario other)
        {
            return Usuario == other.Usuario;
        }
    }
}
