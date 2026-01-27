using System;

namespace ERP.Domain.Entities
{
    public class VendaProduto
    {
        public int VendaId { get; private set; }
        public int ProdutoId { get; private set; }

        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal Subtotal { get; private set; }

        protected VendaProduto() { }

        public VendaProduto(int produtoId, int quantidade, decimal precoUnitario)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;

            CalcularSubtotal();
        }

        private void CalcularSubtotal()
        {
            Subtotal = Quantidade * PrecoUnitario;
        }
    }
}
