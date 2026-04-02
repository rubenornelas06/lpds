using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class Jogo
    {
        public List<Jogador> Jogadores { get; set; } = new List<Jogador>();
        public List<int> Pontuacoes { get; set; } = new List<int>();

        // Representa o jogador que tem a vez de jogar
        public Jogador JogadorAtivo { get; set; }

        public Mesa Mesa { get; set; } = new Mesa();

        public Jogo() { }
    }
}