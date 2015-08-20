using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    public class Estado
    {
        private string siglaAux;
        public string siglaEstado {
            get {
                return siglaAux;
            }
            set
            {                
                if (EstadoFuncs.EstadoValido(value))                
                    siglaAux = value;
                else
                    throw new Exception("estado inválido");
            }
        }
    }
}
