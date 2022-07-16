using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataplace.Imersao.Core.Domain.Orcamentos.ValueObjects
{
    public class OrcamentoUsuario : IEquatable<OrcamentoUsuario>
    {
        public OrcamentoUsuario(string usuario)
        {
            Usuario = usuario;
        }

        public string Usuario { get; }

        public bool Equals(OrcamentoUsuario other)
        {
            return Usuario == other.Usuario;
        }
    }
}
