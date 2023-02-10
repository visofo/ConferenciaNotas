using System.ComponentModel;

namespace Conferencia.ModelSerialization
{
    public class Produto
    {
        [DisplayName("Cod. Produto")]
        public string cProd { get; set; }

        public string cEAN { get; set; }

        [DisplayName("Produto")]
        public string xProd { get; set; }

        public string NCM { get; set; }

        public string CFOP { get; set; }

        [DisplayName("Unidade")]
        public string uCom { get; set; }

        [DisplayName("Qtd. Entrada")]
        public double qCom { get; set; }

        [DisplayName("Unid. Entrada")]
        public double vUnCom { get; set; }

        [DisplayName("Qtd. Recebida")]
        public double qtdRecebido { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }
    }
}