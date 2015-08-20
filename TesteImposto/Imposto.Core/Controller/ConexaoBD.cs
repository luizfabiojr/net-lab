using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Imposto.Core.Controller
{
    public class ConexaoBD
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Data\\Teste.mdf;Integrated Security=True";

        public SqlConnection cnn;

        public ConexaoBD ()
        {
            cnn = new SqlConnection(connectionString);
        }
        public bool ConectarBD() {
            try
            {
                cnn.Open();                
                return true;                
            }
            catch 
            {

                return false;
            }
        }
        public bool DesconectarBD()
        {
            try
            {
                cnn.Close();
                return true;
            }
            catch
            {

                return false;
            }
        }
        public bool ExecutarComando(SqlCommand cmd)
        {
            if (cnn == null) 
                throw new Exception("Conexão não criada");
            else
            {                
                return true;
            }
        }
    }
}
