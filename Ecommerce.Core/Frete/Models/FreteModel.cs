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

        }
        public void CalcularFrete(FreteModel frete)
        {
            switch (frete.TipoFrete)
            {
                case ETipoFrete.Normal:
                    ValorFrete = 15;
                    break;
                case ETipoFrete.Rapido:
                    ValorFrete = 20;
                    break;
                default: throw new ArgumentException("Não foi escolhido nenhum frete");
            }
        }
    }
}
