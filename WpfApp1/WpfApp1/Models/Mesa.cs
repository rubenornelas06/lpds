using System.Collections.Generic;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    public class Mesa
    {
        public List<Carta> CartasJogadas { get; set; } = new List<Carta>();
        public Baralho Baralho { get; set; } = new Baralho();

        public Mesa() { }
    }
}