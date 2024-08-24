using Ecommerce.Core.Frete.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Frete.Models
{
    public class FreteModel
    {
        public ETipoFrete TipoFrete { get; private set; }
        public decimal ValorFrete { get; private set; }

        public FreteModel()
        {
            TipoFrete = ETipoFrete.Normal;
            ValorFrete = 0;
        }
        public FreteModel(ETipoFrete tipoFrete)
        {
            TipoFrete = tipoFrete;
            ValorFrete =CalcularFrete(tipoFrete);
        }

        public decimal CalcularFrete(ETipoFrete frete)
        {
            decimal valorFrete = 0;
            switch (frete)
            {
                case ETipoFrete.Normal:
                    valorFrete = 15;
                    break;
                case ETipoFrete.Rapido:
                    valorFrete = 20;
                    break;
                default:
                    valorFrete = 0;
                    throw new ArgumentException("Não foi escolhido nenhum frete");
            }
            return valorFrete;
        }
    }
}
