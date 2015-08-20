using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    public static class EstadoFuncs
    {
        public static bool EstadoValido(string estado)
        {
            EstadosBrasil estadosBrasileiros = new EstadosBrasil();                 
            return estadosBrasileiros.ListaDeEstados.Contains(estado);
        }

        public static bool EstadoSudeste(Estado estado)
        {
            bool resultado;
            switch (estado.siglaEstado)
            {
                case "SP":
                case "MG":
                case "RJ":
                case "ES":
                    resultado = true;
                    break;
                default:
                    resultado = false;
                    break;
            }
            return resultado;
        }
    }
}
