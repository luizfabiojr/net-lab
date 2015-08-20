using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imposto.Core.Domain;

namespace UnitTestEstado.Testes
{
    [TestClass]
    public class NotaFiscalTest
    {
        [TestMethod]
        public void TestSudeste()
        {
            Pedido ped = new Pedido();
            ped.clientePedido.nomeCliente = "Maria";
            ped.EstadoOrigem.siglaEstado = "SP";
            ped.EstadoDestino.siglaEstado = "RJ";

            PedidoItem item1 = new PedidoItem();
            item1.NomeProduto = "Alicate";
            item1.CodigoProduto = "125-172";
            item1.ValorItemPedido = 18.05;
            item1.Brinde = false;
            ped.ItensDoPedido.Add(item1);

            PedidoItem item2 = new PedidoItem();
            item2.NomeProduto = "Chave de Fenda";
            item2.CodigoProduto = "125-185";
            item2.ValorItemPedido = 10.79;
            item2.Brinde = true;
            ped.ItensDoPedido.Add(item2);

            NotaFiscal nf = new NotaFiscal();
            nf.EmitirNotaFiscal(ped);

            Assert.AreEqual(nf.ClienteNF, ped.clientePedido); // mesmo cliente

            Assert.AreEqual(nf.EstadoOrigem, ped.EstadoOrigem); // estados iguais entre NF e Pedido
            Assert.AreEqual(nf.EstadoDestino, ped.EstadoDestino);

            Assert.IsTrue(nf.ItensDaNotaFiscal[0].Cfop == "6.000");
            Assert.IsTrue(nf.ItensDaNotaFiscal[0].TipoIcms == "10");
            
            Assert.IsTrue(nf.ItensDaNotaFiscal[0].BaseIcms == ped.ItensDoPedido[0].ValorItemPedido);
            Assert.IsTrue(nf.ItensDaNotaFiscal[0].AliquotaIcms == 0.17);
            Assert.IsTrue(nf.ItensDaNotaFiscal[0].ValorIcms == (nf.ItensDaNotaFiscal[0].BaseIcms * nf.ItensDaNotaFiscal[0].AliquotaIcms));

            Assert.IsTrue(nf.ItensDaNotaFiscal[0].BaseIpi == ped.ItensDoPedido[0].ValorItemPedido);
            Assert.IsTrue(nf.ItensDaNotaFiscal[0].AliquotaIpi == 0.1);
            Assert.IsTrue(nf.ItensDaNotaFiscal[0].ValorIPI == (nf.ItensDaNotaFiscal[0].BaseIpi * nf.ItensDaNotaFiscal[0].AliquotaIpi));

            Assert.IsTrue(nf.ItensDaNotaFiscal[0].Desconto == 0.1);

            Assert.IsTrue(nf.ItensDaNotaFiscal[1].Cfop == "6.000");
            Assert.IsTrue(nf.ItensDaNotaFiscal[1].TipoIcms == "60");

            Assert.IsTrue(nf.ItensDaNotaFiscal[1].BaseIcms == ped.ItensDoPedido[1].ValorItemPedido);
            Assert.IsTrue(nf.ItensDaNotaFiscal[1].AliquotaIcms == 0.18);
            Assert.IsTrue(nf.ItensDaNotaFiscal[1].ValorIcms == (nf.ItensDaNotaFiscal[1].BaseIcms * nf.ItensDaNotaFiscal[1].AliquotaIcms));

            Assert.IsTrue(nf.ItensDaNotaFiscal[1].BaseIpi == ped.ItensDoPedido[1].ValorItemPedido);
            Assert.IsTrue(nf.ItensDaNotaFiscal[1].AliquotaIpi == 0);
            Assert.IsTrue(nf.ItensDaNotaFiscal[1].ValorIPI == (nf.ItensDaNotaFiscal[1].BaseIpi * nf.ItensDaNotaFiscal[1].AliquotaIpi));

            Assert.IsTrue(nf.ItensDaNotaFiscal[1].Desconto == 0.1);
        }
    }
}
