using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Imposto.Core.Controller
{
    class SerializadorXML
    {        
        private string path_xml = "\\XML\\";  // path definido manualmente - exercício 01

        public bool SalvarEmXML(object algumObjeto, string filename)
        {
            try
            {
                string path_app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\";  // pega a pasta da aplicação
                string path_dir = new Uri(Path.GetDirectoryName(path_app)).LocalPath; // usado para transformar o path em um formato aceito pelo CreateDirectory

                if (!System.IO.Directory.Exists(path_dir))  // verifica se o dir existe, caso contrário deve criá-lo
                    System.IO.Directory.CreateDirectory(path_dir);

                string path_completo = path_dir + path_xml + filename; // concatena diretórios com nome de arquivo               

                FileStream fs = new FileStream(path_completo, FileMode.Create); // cria o XML
                XmlSerializer serializador = new XmlSerializer(algumObjeto.GetType());  // dá o seguinte erro: there was an error reflecting type 'Imposto.Core.Domain.NotaFiscal'
                serializador.Serialize(fs, algumObjeto);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
