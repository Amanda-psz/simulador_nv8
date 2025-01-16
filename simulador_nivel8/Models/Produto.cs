using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulador_nivel8.Data.Models
{
    public class Produto
    {
        public int id { get; set; }
        public string nome_da_peca { get; set; }
        public string fabricante { get; set; }
        public string tipo_veiculo { get; set; }
        public string categoria { get; set; } 
        public decimal preco_compra { get; set; } 
        public decimal preco_venda { get; set; } 
    }

}
