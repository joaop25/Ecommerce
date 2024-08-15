using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Produto.Models
{
    public class ProdutoCarrinho
    {
        public Guid Id { get; private set; }
        public string nome { get; private set; }
        public string nomeDescricao { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Valor { get; private set; }
        public decimal ValorTotal { get; private set; }

        public ProdutoCarrinho(Guid id, string nome, string nomeDescricao, int quantidade, decimal valor)
        {
            Id = id;
            this.nome = nome;
            this.nomeDescricao = nomeDescricao;
            Quantidade = quantidade;
            Valor = valor;
            CalcularValorTotalProduto();
        }

        public void CalcularValorTotalProduto()
        {
            ValorTotal = Quantidade * Valor;
        }
    }
}
