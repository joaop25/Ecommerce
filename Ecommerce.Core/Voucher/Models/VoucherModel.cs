using Ecommerce.Core.Carrinho.Models;
using Ecommerce.Core.Produto.Models;
using Ecommerce.Core.Voucher.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Voucher.Models
{
    public class VoucherModel
    {
        public EVoucher TipoVoucher { get; private set; }
        public decimal Valor { get; private set; }

        public VoucherModel()
        {
            TipoVoucher = EVoucher.Nenhum;
            Valor = 0;
        }

        public static void AplicarVoucher(CarrinhoModel carrinho, EVoucher voucher, decimal valor)
        {
            if (carrinho.ProdutosCarrinho.Count == 0) throw new Exception("É necessário adicionar um produto primeiro");
            carrinho.Voucher.TipoVoucher = voucher;
            carrinho.Voucher.Valor = valor;
        }
    }
}
