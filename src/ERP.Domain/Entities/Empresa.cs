using System.Collections.Generic;

namespace ERP.Domain.Entities
{
    public class Empresa : BaseEntity
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }

        // Relação 1:N -> Empresa possui vários Usuários
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
