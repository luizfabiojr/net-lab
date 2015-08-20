using System;
using System.Collections.Generic;




namespace Imposto.Core.Domain
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public int Serie { get; set; }
        public Cliente ClienteNF { get; set; }       

        public Estado EstadoOrigem { get; set; }
        public Estado EstadoDestino { get; set; }       

        public List<NotaFiscalItem> ItensDaNotaFiscal { get; set; }  // substituindo o IEnumerable pois dava erro ao serializar para XML
        //public IEnumerable<NotaFiscalItem> ItensDaNotaFiscal { get; set; }  // dava erro ao serializar o objeto para XML trocado pela LIST

        public NotaFiscal()
        {
            ClienteNF = new Cliente();
            EstadoOrigem = new Estado();
            EstadoDestino = new Estado();            
            ItensDaNotaFiscal = new List<NotaFiscalItem>();
            
        }        

        public void EmitirNotaFiscal(Pedido pedido)
        {
            this.NumeroNotaFiscal = 99999;  // número da NF deveria ser sequencial, teria que fazer uma pesquisa no BD para atribuir um novo nr
            this.Serie = new Random().Next(Int32.MaxValue);
            this.ClienteNF = pedido.clientePedido;            

            //this.EstadoDestino = pedido.EstadoOrigem; // Adicionado por Luiz - Referente ao item 06 do teste - Destino recebia Origem e Origem recebia Destino - Corrigido 
            //this.EstadoOrigem = pedido.EstadoDestino;

            this.EstadoOrigem = pedido.EstadoOrigem;
            this.EstadoDestino = pedido.EstadoDestino;  



            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                NotaFiscalItem notaFiscalItem = new NotaFiscalItem();

                // notaFiscalItem.IdNotaFiscal ??? teria de implementar uma futura função para atualizar este campo com o ID da NF

                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;

                CalculaCFOP(notaFiscalItem);
                CalculaICMS(notaFiscalItem, itemPedido);
                CalculaIPI(notaFiscalItem, itemPedido);
                CalculaDesconto(notaFiscalItem);

                ItensDaNotaFiscal.Add(notaFiscalItem); // Adicionado por Luiz - Referente ao item 01 do teste - necessário para resolver problema de serialização de XML com IEnumerable (convertido para LIST)


            }

        }

        private void CalculaDesconto(NotaFiscalItem notaFiscalItem)
        {
            if (EstadoFuncs.EstadoSudeste(EstadoDestino)) // Adicionado por Luiz - Referente ao item 07 do teste 
                notaFiscalItem.Desconto = 0.1;  // 10% para Sudeste
            else
                notaFiscalItem.Desconto = 0;        
        }

        private static void CalculaIPI(NotaFiscalItem notaFiscalItem, PedidoItem itemPedido)
        {
            if (itemPedido.Brinde) // Adicionado por Luiz - Referente ao item 03 do teste                               
                notaFiscalItem.AliquotaIpi = 0;  // se brinde alíquota é 0                
            else
                notaFiscalItem.AliquotaIpi = 0.1;  // se não brinde alíquota é 10%               

            notaFiscalItem.BaseIpi = itemPedido.ValorItemPedido;  // Base de cálculo do IPI igual ao valor total do produto
            notaFiscalItem.ValorIPI = notaFiscalItem.BaseIpi * notaFiscalItem.AliquotaIpi;
        }

        private void CalculaICMS(NotaFiscalItem notaFiscalItem, PedidoItem itemPedido)
        {
            if (EstadoDestino == EstadoOrigem)
            {
                notaFiscalItem.TipoIcms = "60";
                notaFiscalItem.AliquotaIcms = 0.18;
            }
            else
            {
                notaFiscalItem.TipoIcms = "10";
                notaFiscalItem.AliquotaIcms = 0.17;
            }
            if (notaFiscalItem.Cfop == "6.009")
            {
                notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90; //redução de base
            }
            else
            {
                notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;
            }
            notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;

            if (itemPedido.Brinde)
            {
                notaFiscalItem.TipoIcms = "60";
                notaFiscalItem.AliquotaIcms = 0.18;
                notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;             
            }
        }

        private void CalculaCFOP(NotaFiscalItem notaFiscalItem)
        {
            string defaultCfop = "0.000";
            switch (EstadoOrigem.siglaEstado)
            {
                case "SP":
                    switch (EstadoDestino.siglaEstado)
                    {
                        case "RJ":
                            notaFiscalItem.Cfop = "6.000";
                            break;
                        case "PE":
                            notaFiscalItem.Cfop = "6.001";
                            break;
                        case "MG":
                            notaFiscalItem.Cfop = "6.002";
                            break;
                        case "PB":
                            notaFiscalItem.Cfop = "6.003";
                            break;
                        case "PR":
                            notaFiscalItem.Cfop = "6.004";
                            break;
                        case "PI":
                            notaFiscalItem.Cfop = "6.005";
                            break;
                        case "RO":
                            notaFiscalItem.Cfop = "6.006";
                            break;
                        case "SE":
                            notaFiscalItem.Cfop = "6.007";
                            break;
                        case "TO":
                            notaFiscalItem.Cfop = "6.008";
                            break;
                        /*case "SE":                        
                            notaFiscalItem.Cfop = "6.009";  // no código existia duas vezes SP/SE, só foi possível identificar trocando o if pelo switch
                            break;*/
                        case "PA":
                            notaFiscalItem.Cfop = "6.010";
                            break;
                        default:
                            notaFiscalItem.Cfop = defaultCfop; // caso não se enquadre nas opções abaixo o CFOP não pode ficar vazio
                            break;
                    }
                    break;

                case "MG":
                    switch (EstadoDestino.siglaEstado)
                    {
                        case "RJ":
                            notaFiscalItem.Cfop = "6.000";
                            break;
                        case "PE":
                            notaFiscalItem.Cfop = "6.001";
                            break;
                        case "MG":
                            notaFiscalItem.Cfop = "6.002";
                            break;
                        case "PB":
                            notaFiscalItem.Cfop = "6.003";
                            break;
                        case "PR":
                            notaFiscalItem.Cfop = "6.004";
                            break;
                        case "PI":
                            notaFiscalItem.Cfop = "6.005";
                            break;
                        case "RO":
                            notaFiscalItem.Cfop = "6.006";
                            break;
                        case "SE":
                            notaFiscalItem.Cfop = "6.007";
                            break;
                        case "TO":
                            notaFiscalItem.Cfop = "6.008";
                            break;
                        /*case "SE":                        
                            notaFiscalItem.Cfop = "6.009";  // no código existia duas vezes SP/SE, só foi possível identificar trocando o if pelo switch
                            break;*/
                        case "PA":
                            notaFiscalItem.Cfop = "6.010";
                            break;
                        default:
                            notaFiscalItem.Cfop = defaultCfop; // caso não se enquadre nas opções abaixo o CFOP não pode ficar vazio
                            break;
                    }
                    break;
                default:
                    notaFiscalItem.Cfop = defaultCfop; // caso não se enquadre nas opções abaixo o CFOP não pode ficar vazio
                    break;
            }       
            
        }
    }
}
