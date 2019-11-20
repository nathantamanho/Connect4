using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Models
{
    public class JogadorPessoa: Jogador
    {
        public IList<Jogo> Jogos { get; set; } = new List<Jogo>();
    }
}
