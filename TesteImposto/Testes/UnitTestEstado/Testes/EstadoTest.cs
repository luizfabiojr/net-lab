using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imposto.Core.Domain;

namespace UnitTestEstado
{
    [TestClass]
    public class EstadoTest
    {
        [TestMethod]
        public void ReadWriteEstado()  // verifica se o get e o set da estado.siglaEstado funciona
        {
            Estado estado = new Estado();
            string valor_antigo, valor_novo;

            // testar se na inicialização o valor é null
            valor_antigo = estado.siglaEstado;
            Assert.IsNull(valor_antigo);

            // testar GET e SET do siglaEstado com valor válido
            estado.siglaEstado = "SP";
            valor_novo = estado.siglaEstado;
            Assert.AreEqual("SP", valor_novo);

            // testar SET de estado inválido

            valor_antigo = estado.siglaEstado;

            try
            {
                estado.siglaEstado = "XX";
            }
            finally
            {
                valor_novo = estado.siglaEstado;
                Assert.AreEqual(valor_antigo, valor_novo);
            }
        }

        [TestMethod]
        public void VerificaSudeste()
        {
            Estado estado = new Estado();
            bool resultadoSudeste;

            // veficica SP
            estado.siglaEstado = "SP";
            resultadoSudeste = EstadoFuncs.EstadoSudeste(estado);
            Assert.IsTrue(resultadoSudeste);

            // veficica MG
            estado.siglaEstado = "MG";
            resultadoSudeste = EstadoFuncs.EstadoSudeste(estado);
            Assert.IsTrue(resultadoSudeste);

            // veficica RJ
            estado.siglaEstado = "RJ";
            resultadoSudeste = EstadoFuncs.EstadoSudeste(estado);
            Assert.IsTrue(resultadoSudeste);

            // veficica ES
            estado.siglaEstado = "ES";
            resultadoSudeste = EstadoFuncs.EstadoSudeste(estado);
            Assert.IsTrue(resultadoSudeste);

            // veficica BA
            estado.siglaEstado = "BA";
            resultadoSudeste = EstadoFuncs.EstadoSudeste(estado);
            Assert.IsFalse(resultadoSudeste);

        }

        [TestMethod]
        public void VerificaEstadoValido()
        {
            Estado estado = new Estado();
            bool resultadoValido;

            // veficica SP
            resultadoValido = EstadoFuncs.EstadoValido("SP");
            Assert.IsTrue(resultadoValido);

            // veficica XX
            resultadoValido = EstadoFuncs.EstadoValido("XX");
            Assert.IsFalse(resultadoValido);

            // veficica XYZ
            resultadoValido = EstadoFuncs.EstadoValido("XYZ");
            Assert.IsFalse(resultadoValido);

            // veficica 123
            resultadoValido = EstadoFuncs.EstadoValido("123");
            Assert.IsFalse(resultadoValido);
        }
    }
}
