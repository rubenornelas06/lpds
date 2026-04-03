using System.Collections.Generic;
using WpfApp1.Models.Enums; // Necessário para ver o SentidoJogo

namespace WpfApp1.Models
{
    public class Jogo
    {
        // O jogo termina aos 500 pontos
        public int PontuacaoMaxima { get; set; } = 500;

        // Direção atual do jogo (Horário ou Anti-Horário)
        public SentidoJogo SentidoAtual { get; set; } = SentidoJogo.Horario;

        public List<Jogador> Jogadores { get; set; } = new List<Jogador>();
        public List<int> Pontuacoes { get; set; } = new List<int>();

        public Jogador JogadorAtivo { get; set; }

        public Mesa Mesa { get; set; } = new Mesa();

        public Jogo() { }
    }
}