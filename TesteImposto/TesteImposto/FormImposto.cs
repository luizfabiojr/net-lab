using Imposto.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imposto.Core.Domain;
using Imposto.Core.Controller;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private Pedido pedido = new Pedido();

        public FormImposto()
        {
            InitializeComponent();
            dataGridViewPedidos.AutoGenerateColumns = true;                       
            dataGridViewPedidos.DataSource = GetTablePedidos();
            PreencheEstados(cbxEstadoOrigem);
            InicializaCombo(cbxEstadoOrigem);
            PreencheEstados(cbxEstadoDestino);
            InicializaCombo(cbxEstadoDestino);
            ResizeColumns();
        }

        private void PreencheEstados (ComboBox cbx)
        {
            EstadosBrasil estadosBrasileiros = new EstadosBrasil();
            cbx.Items.Clear();
            foreach (string estado in estadosBrasileiros.ListaDeEstados)
            {
                cbx.Items.Add(estado);
            }
        }

        private void InicializaCombo (ComboBox cbx)
        {
            cbx.SelectedItem = null;
        }


        private void LimpaCampos()
        {
            InicializaCombo(cbxEstadoOrigem);
            InicializaCombo(cbxEstadoDestino);
            txtBoxNomeCliente.Text = "";
            
            ((DataTable)dataGridViewPedidos.DataSource).Rows.Clear();
            txtBoxNomeCliente.Focus();
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            mediaWidth = mediaWidth - 17; // espaço para barra de rolagem

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }   
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            table.Columns.Add(new DataColumn("Brinde", typeof(bool)));
                     
            return table;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {            
            NotaFiscalService service = new NotaFiscalService();           

            if (txtBoxNomeCliente.Text == "")  // verifica nome do cliente em branco
            {
                txtBoxNomeCliente.Focus();
                MessageBox.Show("Nome de cliente em branco.");
                return;
            }
            else
                pedido.clientePedido.nomeCliente = txtBoxNomeCliente.Text;

            if (! EstadoFuncs.EstadoValido(cbxEstadoOrigem.Text)) // verifica se estado é válido
            {
                cbxEstadoOrigem.Focus();
                MessageBox.Show("Estado de Origem não existe ou em branco.");
                return;
            }
            else
                pedido.EstadoOrigem.siglaEstado = cbxEstadoOrigem.Text;


            if (! EstadoFuncs.EstadoValido(cbxEstadoDestino.Text)) // verifica se estado é válido
            {
                cbxEstadoDestino.Focus();
                MessageBox.Show("Estado de Destino não existe ou em branco.");
                return;
            }
            else
                pedido.EstadoDestino.siglaEstado = cbxEstadoDestino.Text;            
            

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;

            if (table.Rows.Count == 0)  // não deixa criar pedido sem itens
            {
                MessageBox.Show("Pedido sem Itens.");
                return;
            }

            
            pedido.ItensDoPedido.Clear();  // limpa os itens de pedido caso seja chamada novamente por causa de erros nos dados dos itens
            int linhaAtual = 0;

            foreach (DataRow row in table.Rows)
            {
                PedidoItem pedidoItem = new PedidoItem();

                if (row["Nome do produto"].ToString() == "")  // verifica se nome do produto está em branco
                {
                    dataGridViewPedidos.CurrentCell = dataGridViewPedidos.Rows[linhaAtual].Cells["Nome do produto"];
                    MessageBox.Show("Nome do Produto em branco.");
                    return;
                }
                else
                    pedidoItem.NomeProduto = row["Nome do produto"].ToString();


                if (row["Codigo do produto"].ToString() == "")  // verifica se código do produto está em branco
                {
                    dataGridViewPedidos.CurrentCell = dataGridViewPedidos.Rows[linhaAtual].Cells["Codigo do produto"];
                    MessageBox.Show("Código do Produto em branco.");
                    return;
                }
                else
                    pedidoItem.CodigoProduto = row["Codigo do produto"].ToString();

                if (row["Valor"].ToString() == "")  // verifica se valor do produto está em branco
                {
                    dataGridViewPedidos.CurrentCell = dataGridViewPedidos.Rows[linhaAtual].Cells["Valor"];
                    MessageBox.Show("Valor em branco.");
                    return;
                }
                else
                {
                    double number;
                    if (Double.TryParse(row["Valor"].ToString(), out number))
                        pedidoItem.ValorItemPedido = number;
                    else
                    {
                        dataGridViewPedidos.CurrentCell = dataGridViewPedidos.Rows[linhaAtual].Cells["Valor"];
                        MessageBox.Show("Valor do produto é inválido.");
                        return;
                    }                   
                    
                }


                if (row["Brinde"] == DBNull.Value) // proteção contra o DBNull quando não é selecionado nada na coluna BRINDE
                    pedidoItem.Brinde = false;
                else
                    pedidoItem.Brinde = Convert.ToBoolean(row["Brinde"]);

                pedido.ItensDoPedido.Add(pedidoItem);

                linhaAtual++;  // passa para próxima linha

            }

            service.GerarNotaFiscal(pedido);
            if (service.SalvaNotaFiscal())
            {
                MessageBox.Show("NF salva com sucesso");
                LimpaCampos(); // item 06 - limpar campos
            }
            else
                MessageBox.Show("Erro ao Salvar NF");
        }

       
    }
}
