using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Models
{
    public class DetalhesTorneioViewModel
    {
        public Torneio Torneio { get; set; }
        public int JogosConcluidos { get; set; }
        public Jogo ProximoJogo { get; set; }
        public IList<Ranking> Ranking { get; set; }
    }
}
