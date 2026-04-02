using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class Jogador
    {
        public string Nome { get; set; }
        public string Fotografia { get; set; }
        public List<Carta> Cartas { get; set; } = new List<Carta>();

        // Estatísticas do jogador
        public int N_Partidas_Jogadas { get; set; }
        public int N_Partidas_Ganhos { get; set; }
        public int N_Jogos_Jogados { get; set; }
        public int N_Jogos_Ganhos { get; set; }

        public Jogador() { }

        public Jogador(string nome)
        {
            Nome = nome;
        }
    }
}