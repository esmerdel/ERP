using System;
using System.Collections.Generic;

namespace ERP.Domain.Entities
{
    public class Venda : BaseEntity
    {
        public int ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }

        public ICollection<VendaProduto> Itens { get; private set; } = new List<VendaProduto>();

        protected Venda() { }

        public Venda(int clienteId)
        {
            ClienteId = clienteId;
        }

        public void AdicionarItem(Produto produto, int quantidade)
        {
            if (quantidade <= 0)
                throw new Exception("Quantidade inválida.");

            var item = new VendaProduto(
                produto.Id,
                quantidade,
                produto.Preco
            );

            Itens.Add(item);
            RecalcularTotal();
        }

        private void RecalcularTotal()
        {
            ValorTotal = 0;

            foreach (var item in Itens)
                ValorTotal += item.Subtotal;
        }
    }
}
