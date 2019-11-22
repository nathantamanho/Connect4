using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Models
{
    public class JogadorComputador : Jogador
    {
        public String URLServico { get; set; }

        public String NomeComputador { get; set; }

        public override string Nome
        {
            get
            {
                return "(Computador)" + NomeComputador;
            }
        }
    }
}
