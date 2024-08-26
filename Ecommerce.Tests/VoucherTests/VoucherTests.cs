using Ecommerce.Core.Carrinho.Models;
using Ecommerce.Core.DomainObjects;
using Ecommerce.Core.Produto.Models;
using Ecommerce.Core.Voucher.Enum;
using Ecommerce.Core.Voucher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Tests.VoucherTests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Aplicar Voucher inativo")]
        public void AdicionarVoucher_AddVoucherInativo_DeverRetornarExcecao()
        {
            // Arrange + Act + Assert
            var voucher = new VoucherModel("JOAO10DESC",false,1,DateTime.Now,0,10,EVoucher.Valor, true);

            // Act
            var result = voucher.ValidarSeAplicavelVoucher();

            // Assert 
            Assert.False(result.IsValid);
            Assert.Contains(VoucherAplicavelValidation.VoucherAtivoMsg, result.Errors.Select(c => c.ErrorMessage));

        }

        [Fact(DisplayName = "Aplicar Voucher com ID inválido")]
        public void AdicionarVoucher_AddVoucherIdInvalido_DeverRetornarExcecao()
        {
            // Arrange + Act + Assert
            var voucher = new VoucherModel("", true, 1, DateTime.Now, 0, 10, EVoucher.Valor, true);

            // Act
            var result = voucher.ValidarSeAplicavelVoucher();

            // Assert 
            Assert.False(result.IsValid);
            Assert.Contains(VoucherAplicavelValidation.IdErroMsg, result.Errors.Select(c => c.ErrorMessage));


        }

        [Fact(DisplayName = "Aplicar Voucher expirado")]
        public void AdicionarVoucher_AddVoucherExpirado_DeverRetornarExcecao()
        {
            // Arrange + Act + Assert
            var voucher = new VoucherModel("", true, 1, DateTime.Now.AddDays(-1), 0, 10, EVoucher.Valor, false);

            // Act
            var result = voucher.ValidarSeAplicavelVoucher();

            // Assert 
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count());
            Assert.Contains(VoucherAplicavelValidation.DataValidadeMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.IdErroMsg, result.Errors.Select(c => c.ErrorMessage));


        }




        [Fact(DisplayName = "Aplicar Voucher Valor carrinho ")]
        public void AdicionarProduto_AddVoucherValor_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            var voucher = new VoucherModel("JOAO10DESC", true, 1, DateTime.Now, 0, 100, EVoucher.Valor, false);



            // Act
            carrinho.AdicionarProduto(produto);
            var result = carrinho.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(700, carrinho.Subtotal);
            Assert.Equal(0, result.Errors.Count());

        }

        [Fact(DisplayName = "Aplicar Voucher Porcentagem carrinho ")]
        public void AdicionarProduto_AddVoucherPorcentagem_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            var voucher = new VoucherModel("JOAO10DESC", true, 1, DateTime.Now, 0.50m,0, EVoucher.Porcentagem, false);


            // Act
            carrinho.AdicionarProduto(produto);
            var result = carrinho.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(400, carrinho.Subtotal);
            Assert.Equal(0, result.Errors.Count());
        }

        [Fact(DisplayName = "Remover Voucher Valor carrinho ")]
        public void AdicionarProduto_RemoverVoucherValor_SubtotalDeveSerCalculadoCorretamente()
        {
            // Arrange
            var carrinho = CarrinhoModel.NovoCarrinhoRascunho(Guid.NewGuid());
            var guid = Guid.NewGuid();
            var produto = new ProdutoCarrinho(guid, "Tenis Nike", "Tenis Nike AirForce", 2, 400);
            var voucher = new VoucherModel("", true, 1, DateTime.Now.AddDays(-1), 0, 10, EVoucher.Valor, false);



            // Act
            carrinho.AdicionarProduto(produto);
            carrinho.AplicarVoucher(voucher);
            carrinho.AplicarVoucher(voucher);

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
            var voucher = new VoucherModel("", true, 1, DateTime.Now.AddDays(-1), 0, 10, EVoucher.Valor, false);



            // Act + Assert
            Assert.Throws<DomainException>(() => carrinho.AplicarVoucher(voucher));
        }

    }
}
