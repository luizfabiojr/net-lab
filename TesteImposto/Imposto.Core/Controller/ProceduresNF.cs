using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imposto.Core.Domain;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.Core.Controller
{
    public class ProceduresNF
    {
        public ProceduresNF() { }   
        
        private void InsereItemNF(ConexaoBD conexao, NotaFiscalItem itemNF)
        {
            SqlCommand cmdItem = new SqlCommand("dbo.P_NOTA_FISCAL_ITEM", conexao.cnn);
            cmdItem.CommandType = CommandType.StoredProcedure;

            cmdItem.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
            cmdItem.Parameters.Add(new SqlParameter("@pIdNotaFiscal", SqlDbType.Int));
            cmdItem.Parameters.Add(new SqlParameter("@pCfop", SqlDbType.NChar, 5));
            cmdItem.Parameters.Add(new SqlParameter("@pTipoIcms", SqlDbType.NChar, 20));
            cmdItem.Parameters.Add(new SqlParameter("@pBaseIcms", SqlDbType.Decimal));
            cmdItem.Parameters.Add(new SqlParameter("@pAliquotaIcms", SqlDbType.Decimal));
            cmdItem.Parameters.Add(new SqlParameter("@pValorIcms", SqlDbType.Decimal));
            cmdItem.Parameters.Add(new SqlParameter("@pNomeProduto", SqlDbType.NChar, 50));
            cmdItem.Parameters.Add(new SqlParameter("@pCodigoProduto", SqlDbType.NChar, 20));
            cmdItem.Parameters.Add(new SqlParameter("@pBaseIpi", SqlDbType.Decimal));
            cmdItem.Parameters.Add(new SqlParameter("@pAliquotaIpi", SqlDbType.Decimal));
            cmdItem.Parameters.Add(new SqlParameter("@pValorIpi", SqlDbType.Decimal));
            cmdItem.Parameters.Add(new SqlParameter("@pDesconto", SqlDbType.Decimal));

            cmdItem.Parameters["@pId"].Value = 0;
            cmdItem.Parameters["@pIdNotaFiscal"].Value = itemNF.IdNotaFiscal;
            cmdItem.Parameters["@pCfop"].Value = itemNF.Cfop;
            cmdItem.Parameters["@pTipoIcms"].Value = itemNF.TipoIcms;
            cmdItem.Parameters["@pBaseIcms"].Value = itemNF.BaseIcms;
            cmdItem.Parameters["@pAliquotaIcms"].Value = itemNF.AliquotaIcms;
            cmdItem.Parameters["@pValorIcms"].Value = itemNF.ValorIcms;
            cmdItem.Parameters["@pNomeProduto"].Value = itemNF.NomeProduto;
            cmdItem.Parameters["@pCodigoProduto"].Value = itemNF.CodigoProduto;
            cmdItem.Parameters["@pBaseIpi"].Value = itemNF.BaseIpi;
            cmdItem.Parameters["@pAliquotaIpi"].Value = itemNF.AliquotaIpi;
            cmdItem.Parameters["@pValorIpi"].Value = itemNF.ValorIPI;
            cmdItem.Parameters["@pDesconto"].Value = itemNF.Desconto;

            cmdItem.ExecuteNonQuery();
        }

        public int InsereNFnoBD(NotaFiscal nf)
        {
            ConexaoBD conexao = new ConexaoBD();

            if (conexao.ConectarBD())
            {
                SqlCommand cmd = new SqlCommand("dbo.P_NOTA_FISCAL", conexao.cnn);
                cmd.CommandType = CommandType.StoredProcedure;                

                cmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int, 0, "pId"));
                cmd.Parameters.Add(new SqlParameter("@pNumeroNotaFiscal", SqlDbType.Int, 0, "pNumeroNotaFiscal"));
                cmd.Parameters.Add(new SqlParameter("@pSerie", SqlDbType.Int, 0, "pSerie"));
                cmd.Parameters.Add(new SqlParameter("@pNomeCliente", SqlDbType.NChar, 50, "pNomeCliente"));
                cmd.Parameters.Add(new SqlParameter("@pEstadoDestino", SqlDbType.NChar, 50, "pEstadoDestino"));
                cmd.Parameters.Add(new SqlParameter("@pEstadoOrigem", SqlDbType.NChar, 50, "pEstadoOrigem"));                

                cmd.Parameters["@pId"].Value = 0;                
                cmd.Parameters["@pNumeroNotaFiscal"].Value = nf.NumeroNotaFiscal;
                cmd.Parameters["@pSerie"].Value = nf.Serie;
                cmd.Parameters["@pNomeCliente"].Value = nf.ClienteNF.nomeCliente;
                cmd.Parameters["@pEstadoDestino"].Value = nf.EstadoDestino.siglaEstado;
                cmd.Parameters["@pEstadoOrigem"].Value = nf.EstadoOrigem.siglaEstado;               

                cmd.ExecuteNonQuery();

                foreach (NotaFiscalItem itemNF in nf.ItensDaNotaFiscal)
                    InsereItemNF(conexao, itemNF);                

                conexao.DesconectarBD();
                return 1;  // A idéia seria retornar o valor de ID de Output da procedure, mas tentei de várias maneiras e sempre retornava "0"

            }
            else
              return -1;
        }
    }
}
