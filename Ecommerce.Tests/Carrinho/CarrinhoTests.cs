using Ecommerce.Core.Carrinho.Enuns;
using Ecommerce.Core.Carrinho.Models;
using Ecommerce.Core.Produto.Models;
using Ecommerce.Core.Voucher.Enum;
using Ecommerce.Core.Voucher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Tests.Carrinho
{
    public class CarrinhoTests
    {
        [Fact(DisplayName = "Incluir produto no carrinho")]
        public void AdicionarProduto_NovoProduto_DeveAdicionarProdutoCarrinho()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var produto = new ProdutoCarrinho(Guid.NewGuid(), "Tenis Nike", "Tenis Nike AirForce", 1,400);
            // Act
            carrinho.AdicionarProduto(produto);

            // Assert
            Assert.Single(carrinho._produtoCarrinho);
        }

        [Fact(DisplayName = "Validar Valor total do produto no carrinho")]
        public void AdicionarProduto_AddNovoProduto_DeveCalcularValorTotalProduto()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var produto = new ProdutoCarrinho(Guid.NewGuid(), "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            
            // Act
            carrinho.AdicionarProduto(produto);

            // Assert
            Assert.Equal(800, carrinho.ProdutosCarrinho.Sum(x => x.ValorTotal));
        }

        [Fact(DisplayName = "Atualizar quantidade do item no carrinho")]
        public void AdicionarProduto_Add2VezProduto_DeveAtualizarCarrinhoComQtdCorreta()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            carrinho.AdicionarProduto(produto);

            var produtoAtualizado = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 5, 400);

            // Act
            carrinho.AtualizarQtdProduto(produtoAtualizado);


            // Assert
            Assert.Equal(2000, carrinho.ProdutosCarrinho.Sum(x => x.ValorTotal));
            Assert.Equal(5, carrinho.ProdutosCarrinho.Sum(x => x.Quantidade));
        }

        [Fact(DisplayName = "Adicionar uma nova quantidade do item no carrinho")]
        public void AdicionarProduto_RemoverQtdProduto_DeveAtualizarCarrinhoComQtdCorreta()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            carrinho.AdicionarProduto(produto);

            var produtoAtualizado = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 1, 400);

            // Act
            carrinho.AtualizarQtdProduto(produtoAtualizado);


            // Assert
            Assert.Equal(400, carrinho.ProdutosCarrinho.Sum(x => x.ValorTotal));
            Assert.Equal(1, carrinho.ProdutosCarrinho.Sum(x => x.Quantidade));
        }

        [Fact(DisplayName = "Adicionar uma nova quantidade de um item que não existe no carrinho")]
        public void AdicionarProduto_AddQtdItemNaoExiste_DeveRetornarExcecao()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            carrinho.AdicionarProduto(produto);

            var produtoAtualizado = new ProdutoCarrinho(Guid.NewGuid(), "Tenis Nike", "Tenis Nike AirForce", 1, 400);

            // Act + Assert
            Assert.Throws<Exception>(() => carrinho.AtualizarQtdProduto(produtoAtualizado));
        }

        [Fact(DisplayName = "Adicionar dois produtos diferentes no carrinho")]
        public void AdicionarProduto_Add2ProdutosDiferentes_DeveAdicionarProdutosCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            var produto2 = new ProdutoCarrinho(Guid.NewGuid(), "Tenis Adidas", "Tenis Adidas", 5, 400);

            // Act
            carrinho.AdicionarProduto(produto);
            carrinho.AdicionarProduto(produto2);


            // Assert
            Assert.Equal(2800, carrinho.ProdutosCarrinho.Sum(x => x.ValorTotal));
            Assert.Equal(7, carrinho.ProdutosCarrinho.Sum(x => x.Quantidade));

            Assert.Equal(800, carrinho.ProdutosCarrinho.Where(x => x.Id == produto.Id).Sum(x => x.ValorTotal));
            Assert.Equal(2000, carrinho.ProdutosCarrinho.Where(x => x.Id == produto2.Id).Sum(x => x.ValorTotal));
        }

        [Fact(DisplayName = "Status carrinho Iniciado")]
        public void AdicionarProduto_AddNovoProduto_DeveMudarStatusCarrinhoIniciado()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var produto = new ProdutoCarrinho(Guid.NewGuid(), "Tenis Nike", "Tenis Nike AirForce", 1, 400);
            
            // Act
            carrinho.AdicionarProduto(produto);

            // Assert
            Assert.Equal(ECarrinho.Iniciado, carrinho.StatusCarrinho);
        }

        [Fact(DisplayName = "Validar Subtotal carrinho ")]
        public void AdicionarProduto_AddNovoProduto_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();  
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            var produto2 = new ProdutoCarrinho(Guid.NewGuid(), "Tenis Adidads", "Tenis Adidads", 3, 200);
            var produtoAtualizado = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 5, 400);

            // Act
            carrinho.AdicionarProduto(produto);
            carrinho.AdicionarProduto(produto2);
            carrinho.AtualizarQtdProduto(produtoAtualizado);


            // Assert
            Assert.Equal(carrinho.Subtotal, carrinho.ProdutosCarrinho.Sum(x => x.ValorTotal));
        }

        [Fact(DisplayName = "Aplicar Voucher Valor carrinho ")]
        public void AdicionarProduto_AddVoucherValor_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);

            // Act
            carrinho.AdicionarProduto(produto);
            carrinho.AplicarVoucher(carrinho, EVoucher.Valor, 100);

            // Assert
            Assert.Equal(700,carrinho.Subtotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Porcentagem carrinho ")]
        public void AdicionarProduto_AddVoucherPorcentagem_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);

            // Act
            carrinho.AdicionarProduto(produto);
            carrinho.AplicarVoucher(carrinho, EVoucher.Porcentagem, 0.5m);

            // Assert
            Assert.Equal(400, carrinho.Subtotal);
        }

        [Fact(DisplayName = "Remover Voucher Valor carrinho ")]
        public void AdicionarProduto_RemoverVoucherValor_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
           

            // Act
            carrinho.AdicionarProduto(produto);
            carrinho.AplicarVoucher(carrinho, EVoucher.Porcentagem, 0.5m);
            carrinho.AplicarVoucher(carrinho, EVoucher.Nenhum, 0);

            // Assert
            Assert.Equal(800, carrinho.Subtotal);
        }

        [Fact(DisplayName = "Adicionar Voucher sem ter adicionado item no carrinho ")]
        public void AdicionarProduto_AdicionarVoucherSemItem_DeveRetornarExcecao()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            
            // Act + Assert
            Assert.Throws<Exception>(() => carrinho.AplicarVoucher(carrinho, EVoucher.Porcentagem, 0.5m));
        }

        [Fact(DisplayName = "Adicionar produto com qtd negativa")]
        public void AdicionarProduto_AdicionarProdutoQtdNegativa_DeveRetornarExcecao()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", -2, 400);

            // Act + Assert
            Assert.Throws<Exception>(() => carrinho.AdicionarProduto(produto));
        }

        [Fact(DisplayName = "Atualizar produto com qtd negativa")]
        public void AdicionarProduto_AtualizarProdutoQtdNegativa_DeveRetornarExcecao()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            var produtoAtualizado = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", -1, 400);
            // Act 
            carrinho.AdicionarProduto(produto);

            // Act + Assert
            Assert.Throws<Exception>(() => carrinho.AtualizarQtdProduto(produtoAtualizado));
        }
    }
}
