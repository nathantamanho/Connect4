using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Models
{
    public class JogadorPessoa : Jogador
    {
        public IList<Jogo> Jogos { get; set; } = new List<Jogo>();

        [InverseProperty("Jogador")]
        public virtual ApplicationUser Usuario { get; set; }

        public override string Nome {
            get {
                if (Usuario == null)
                {
                    return "Nome não recuperado";
                }
                return Usuario.Nome;
            }
        }
    }
}
