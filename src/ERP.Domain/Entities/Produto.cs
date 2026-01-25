using System;

namespace ERP.Domain.Entities
{
    public class Produto : BaseEntity
    {
        public string Nome { get; set; } = null!;
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }
}
