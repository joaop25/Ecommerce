using Ecommerce.Core.Carrinho.Models;
using Ecommerce.Core.DomainObjects;
using Ecommerce.Core.Produto.Models;
using Ecommerce.Core.Voucher.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Tests.VoucherTests
{
    public class VoucherTests
    {
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
            Assert.Equal(700, carrinho.Subtotal);
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
            Assert.Throws<DomainException>(() => carrinho.AplicarVoucher(carrinho, EVoucher.Porcentagem, 0.5m));
        }

    }
}
