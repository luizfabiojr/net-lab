using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    public class NotaFiscalItem
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoProduto { get; set; }
        public int IdNotaFiscal { get; set; }
        public string Cfop { get; set; }
        public string TipoIcms { get; set; }

        public double BaseIcms { get; set; }
        public double AliquotaIcms { get; set; }
        public double ValorIcms { get; set; }        

        public double BaseIpi { get; set; } // Adicionado por Luiz - Referente ao item 03 do teste Dados IPI          
        public double AliquotaIpi { get; set; }    
        public double ValorIPI { get; set; }       

        public double Desconto { get; set; } // Adicionado por Luiz - Referente ao item 07 do teste - Desconto
    }
}
