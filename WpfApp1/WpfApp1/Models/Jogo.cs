using System.Collections.Generic;

namespace WpfProjectLpdsw.Models
{
    public class Jogo
    {
        public List<Jogador> Jogadores { get; set; } = new List<Jogador>();
        public List<int> Pontuacoes { get; set; } = new List<int>();
        public Jogador JogadorAtivo { get; set; }
        public Mesa Mesa { get; set; } = new Mesa();
    }
}