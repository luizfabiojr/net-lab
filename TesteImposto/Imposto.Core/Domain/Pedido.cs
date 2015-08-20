using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    public class Pedido
    {
        public Cliente clientePedido { get; set; }
        public Estado EstadoOrigem { get; set; }
        public Estado EstadoDestino { get; set; } 
        public List<PedidoItem> ItensDoPedido { get; set; }

        public Pedido()
        {
            clientePedido = new Cliente();
            EstadoOrigem = new Estado();
            EstadoDestino = new Estado();
            ItensDoPedido = new List<PedidoItem>();
        }
    }
}
