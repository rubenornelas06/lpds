using System.Collections.Generic;

namespace WpfProjectLpdsw.Models
{
    public class Mesa
    {
        public List<Carta> CartasJogadas { get; set; } = new List<Carta>();
        public Baralho Baralho { get; set; } = new Baralho();
    }
}