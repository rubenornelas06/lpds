using System.Collections.Generic;

namespace WpfProjectLpdsw.Models
{
    public class Jogador
    {
        public string Nome { get; set; }
        public string Fotografia { get; set; } // Caminho para a imagem do jogador
        public List<Carta> Cartas { get; set; } = new List<Carta>();

        // Estatísticas
        public int N_Partidas_Jogadas { get; set; }
        public int N_Partidas_Ganhos { get; set; }
        public int N_Jogos_Jogados { get; set; }
        public int N_Jogos_Ganhos { get; set; }
    }
}