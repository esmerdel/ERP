using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _produtoRepository.GetAllAsync();
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            return await _produtoRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Produto produto)
        {
            ValidarProduto(produto);
            await _produtoRepository.AddAsync(produto);
        }

        public async Task UpdateAsync(int id, Produto produtoAtualizado)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);

            if (produto == null)
                throw new Exception("Produto não encontrado.");

            ValidarProduto(produtoAtualizado);

            produto.Nome = produtoAtualizado.Nome;
            produto.Preco = produtoAtualizado.Preco;
            produto.Estoque = produtoAtualizado.Estoque;

            produto.Atualizar();

            await _produtoRepository.UpdateAsync(produto);

        }

        public async Task DeleteAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);

            if (produto == null)
                throw new Exception("Produto não encontrado.");

            produto.MarcarComoDeletado();
            await _produtoRepository.DeleteAsync(produto);


            await _produtoRepository.DeleteAsync(produto);
        }

        private void ValidarProduto(Produto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.Nome))
                throw new Exception("O nome do produto é obrigatório.");

            if (produto.Preco < 0)
                throw new Exception("O preço do produto não pode ser negativo.");

            if (produto.Estoque < 0)
                throw new Exception("O estoque do produto não pode ser negativo.");
        }
    }
}
