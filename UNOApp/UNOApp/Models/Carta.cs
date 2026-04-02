using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    public class Carta
    {
        public int Id { get; set; }
        public SimboloCarta Simbolo { get; set; }
        public CorCarta Cor { get; set; }
        public int Pontos { get; set; }

        public Carta() { }

        public Carta(int id, SimboloCarta simbolo, CorCarta cor)
        {
            Id = id;
            Simbolo = simbolo;
            Cor = cor;
            Pontos = CalcularPontos(simbolo);
        }

        // Calcula os pontos da carta usando um switch normal
        private int CalcularPontos(SimboloCarta simbolo)
        {
            switch (simbolo)
            {
                case SimboloCarta.Zero: return 0;
                case SimboloCarta.Um: return 1;
                case SimboloCarta.Dois: return 2;
                case SimboloCarta.Tres: return 3;
                case SimboloCarta.Quatro: return 4;
                case SimboloCarta.Cinco: return 5;
                case SimboloCarta.Seis: return 6;
                case SimboloCarta.Sete: return 7;
                case SimboloCarta.Oito: return 8;
                case SimboloCarta.Nove: return 9;
                case SimboloCarta.Compra2:
                case SimboloCarta.Inverter:
                case SimboloCarta.Salta:
                    return 20;
                case SimboloCarta.Coringa:
                case SimboloCarta.CoringaMais4:
                    return 50;
                default:
                    return 0;
            }
        }
    }
}