using Ecommerce.Core.Carrinho.Models;
using Ecommerce.Core.DomainObjects;
using Ecommerce.Core.Produto.Models;
using Ecommerce.Core.Voucher.Enum;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Voucher.Models
{
    public class VoucherModel
    {
        public Guid Id { get; private set; }
        public bool Ativo { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public decimal ValorPercentual { get; private set; }
        public decimal ValorTipoValor { get; private set; }
        public EVoucher TipoVoucher { get; private set; }
        public bool Utilizado { get; private set; }

        public VoucherModel(Guid id, bool ativo, int quantidade, DateTime dataValidade, decimal valorPercentual, decimal valorTipoValor, EVoucher tipoVoucher, bool utilizado)
        {
            Id = id;
            Ativo = ativo;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            ValorPercentual = valorPercentual;
            ValorTipoValor = valorTipoValor;
            TipoVoucher = tipoVoucher;
            Utilizado = utilizado;
            ValidarSeAplicavelVoucher();
        }

        public VoucherModel() { }
        public FluentValidation.Results.ValidationResult ValidarSeAplicavelVoucher()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }

        public static void AplicarVoucher(CarrinhoModel carrinho, EVoucher voucher, decimal valor)
        {
            if (carrinho.ProdutosCarrinho.Count == 0) throw new DomainException("É necessário adicionar um produto primeiro");
            carrinho.Voucher.TipoVoucher = voucher;
            carrinho.Voucher.ValorTipoValor = valor;
        }
    }

    public class VoucherAplicavelValidation : AbstractValidator<VoucherModel>
    {
        public static string IdErroMsg => "Voucher sem código válido.";
        public static string VoucherAtivoMsg => "Este voucher não é mais válido.";
        public static string DataValidadeMsg => "Voucher expirado.";
        public static string ValorPercentualMsg => "Valor da porcentagem de desconto precisa ser superior a 0.";
        public static string ValorTipoValorMsg => "Valor do desconto do Voucher precisa ser superior a 0.";
        public static string TipVoucherMsg => "Tipo voucher inválido.";
        public static string UtilizadoErroMsg => "Voucher não utilizado.";
        public static string QuantidadeErroMsg => "Quantidade voucher deve ser igual a 1.";


        public VoucherAplicavelValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage(IdErroMsg);

            RuleFor(c => c.DataValidade)
                .Must(DataVencimentoSuperiorAtual)
                .WithMessage(DataValidadeMsg);

            RuleFor(c => c.Ativo)
                .Equal(true)
                .WithMessage(VoucherAtivoMsg);

            RuleFor(c => c.Utilizado)
                .Equal(false)
                .WithMessage(UtilizadoErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage(QuantidadeErroMsg);

            When(f => f.TipoVoucher == EVoucher.Valor, () =>
            {
                RuleFor(f => f.ValorTipoValor)
                    .NotNull()
                    .WithMessage(ValorTipoValorMsg)
                    .GreaterThan(0)
                    .WithMessage(ValorTipoValorMsg);
            });

            When(f => f.TipoVoucher == EVoucher.Porcentagem, () =>
            {
                RuleFor(f => f.ValorPercentual)
                    .NotNull()
                    .WithMessage(ValorPercentualMsg)
                    .GreaterThan(0)
                    .WithMessage(ValorPercentualMsg);
            });
        }

        protected static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }

    }
}
