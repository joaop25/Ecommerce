using Ecommerce.Core.Carrinho.Enuns;
using Ecommerce.Core.DomainObjects;
using Ecommerce.Core.Frete.Enuns;
using Ecommerce.Core.Frete.Models;
using Ecommerce.Core.Produto.Models;
using Ecommerce.Core.Voucher.Enum;
using Ecommerce.Core.Voucher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Carrinho.Models
{
    public class CarrinhoModel
    {
        public Guid ClienteId { get; private set; }
        public ECarrinho StatusCarrinho { get; private set; }
       
        public readonly List<ProdutoCarrinho> _produtoCarrinho; 
        public IReadOnlyCollection<ProdutoCarrinho> ProdutosCarrinho => _produtoCarrinho;
        public decimal  Subtotal { get; private set; }
        public FreteModel Frete { get; private set; }

        public VoucherModel Voucher { get; private set; }

        public readonly int MAX_QTD_ITENS = 15;
        public CarrinhoModel()
        {
            _produtoCarrinho = new List<ProdutoCarrinho>();
        }

        public  static CarrinhoModel NovoCarrinhoRascunho(Guid id)
        {
            CarrinhoModel carrinho = new CarrinhoModel();
            FreteModel freteModel = new FreteModel();
            VoucherModel voucher = new VoucherModel();

            carrinho.ClienteId = id;
            carrinho.StatusRascunhoCarrinho();
            carrinho.Frete = freteModel;
            carrinho.Voucher = voucher;

            carrinho.CalcularSubtotal();
            return carrinho;
        }

        public void AdicionarFrete(ref CarrinhoModel carrinhoModel, ETipoFrete tipoFrete)
        {
            FreteModel freteModel = new FreteModel(tipoFrete);
            carrinhoModel.Frete = freteModel;
            carrinhoModel.CalcularSubtotal();
        }

        public void CalcularSubtotal()
        {
            var valorSubtotal = _produtoCarrinho.Sum(x => x.ValorTotal);
            if (Voucher.TipoVoucher == EVoucher.Porcentagem)
            {
                valorSubtotal -= valorSubtotal * Voucher.ValorTipoValor;
            }
            else
            {
                valorSubtotal -= Voucher.ValorTipoValor;
            }

            valorSubtotal += Frete.ValorFrete;
            Subtotal = valorSubtotal;
        }
        public void StatusRascunhoCarrinho()
        {
            StatusCarrinho = ECarrinho.Rascunho;
        }

        public void AtualizarStatusCarrinho (ECarrinho statusCarrinho)
        {
            StatusCarrinho = statusCarrinho;
        }

        public void AdicionarProduto(ProdutoCarrinho item)
        {
            ValidarRestricoesParaAddProduto(item);
            _produtoCarrinho.Add(item);
            AtualizarStatusCarrinho(ECarrinho.Iniciado);
            CalcularSubtotal();
        }


        public void AtualizarQtdProduto(ProdutoCarrinho item)
        {
            var produto = ProdutosCarrinho.Any(x => x.Id == item.Id);
            if (!produto) throw new DomainException("Produto não existe");
            ValidarRestricoesParaAddProduto(item);

            _produtoCarrinho.RemoveAll(x => x.Id == item.Id);
            if (item.Quantidade != 0)
            {
                _produtoCarrinho.Add(item);
                AtualizarStatusCarrinho(ECarrinho.Iniciado);
                CalcularSubtotal();
            }
        }

        public void AplicarVoucher(CarrinhoModel carrinho, EVoucher voucher, decimal valorVoucher)
        {
            VoucherModel.AplicarVoucher(carrinho, voucher, valorVoucher);
            CalcularSubtotal();
        }

        public void ValidarRestricoesParaAddProduto(ProdutoCarrinho item)
        {
            if (item.Quantidade > MAX_QTD_ITENS)
            {
                throw new DomainException($"Somente até {MAX_QTD_ITENS} são permitidas adicionar por item");
            }
            if (item.Quantidade <= 0 )
            {
                throw new DomainException($"Não é permitido adicionar quantidade igual ou inferior a 0");
            }
        }
        

    }
}
