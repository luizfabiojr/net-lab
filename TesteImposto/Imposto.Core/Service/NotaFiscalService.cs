using Imposto.Core.Domain;
using Imposto.Core.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {

        public NotaFiscal notaFiscal;  // retirada a variável para fora de GerarNotaFiscal para ser usada em outras funções


        public void GerarNotaFiscal(Pedido pedido)
        {
            if (notaFiscal == null)
                notaFiscal = new NotaFiscal();

            notaFiscal.EmitirNotaFiscal(pedido);
        }
        public bool SalvaNotaFiscal()
        {
            if (notaFiscal == null)
                return false;
            else
            {
                SerializadorXML serial = new SerializadorXML();                          
                if (serial.SalvarEmXML(notaFiscal, notaFiscal.Serie + ".xml"))
                {
                    ProceduresNF insereNF = new ProceduresNF();
                    return (insereNF.InsereNFnoBD(notaFiscal) > 0);
                }
                else return false;
            }
        }

    }
}
