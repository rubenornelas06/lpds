using System.Xml.Serialization;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    /// <summary>
    /// Representa uma carta do jogo de UNO.
    /// Cada carta tem um identificador único, um símbolo, uma cor e um valor em pontos.
    /// </summary>
    [XmlRoot("Carta")]
    public class Carta
    {
        // ─────────────────────────────────────────────────
        //  Propriedades
        // ─────────────────────────────────────────────────

        /// <summary>Identificador único da carta no baralho.</summary>
        [XmlAttribute("Id")]
        public int Id { get; set; }

        /// <summary>Símbolo da carta (número ou ação).</summary>
        [XmlElement("Simbolo")]
        public SimboloCarta Simbolo { get; set; }

        /// <summary>Cor da carta. Coringas têm cor = Nenhuma.</summary>
        [XmlElement("Cor")]
        public CorCarta Cor { get; set; }

        /// <summary>Valor em pontos da carta (para contagem no final da partida).</summary>
        [XmlElement("Pontos")]
        public int Pontos { get; set; }

        // ─────────────────────────────────────────────────
        //  Construtores
        // ─────────────────────────────────────────────────

        /// <summary>Construtor sem parâmetros necessário para serialização XML.</summary>
        public Carta() { }

        /// <summary>Cria uma carta com o símbolo e cor indicados, calculando automaticamente os pontos.</summary>
        public Carta(int id, SimboloCarta simbolo, CorCarta cor)
        {
            Id = id;
            Simbolo = simbolo;
            Cor = cor;
            Pontos = CalcularPontos(simbolo);
        }

        // ─────────────────────────────────────────────────
        //  Métodos
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Determina se esta carta pode ser jogada sobre a carta no topo da pilha,
        /// tendo em conta a cor ativa (que pode diferir da cor da carta topo no caso de coringas).
        /// </summary>
        /// <param name="cartaTopo">Carta no topo da pilha de descarte.</param>
        /// <param name="corAtiva">Cor ativa definida no jogo (pode ter sido alterada por um coringa).</param>
        /// <returns>True se a carta pode ser jogada, False caso contrário.</returns>
        public bool PodeJogarSobre(Carta cartaTopo, CorCarta corAtiva)
        {
            // Coringas e Coringa+4 podem sempre ser jogados
            if (Simbolo == SimboloCarta.Coringa || Simbolo == SimboloCarta.CoringaMais4)
                return true;

            // Mesma cor ativa (relevante quando o topo é um coringa com cor escolhida)
            if (Cor == corAtiva)
                return true;

            // Mesmo símbolo (ex: Compra2 sobre Compra2)
            if (Simbolo == cartaTopo.Simbolo)
                return true;

            return false;
        }

        /// <summary>Calcula o valor em pontos de acordo com o símbolo da carta.</summary>
        private static int CalcularPontos(SimboloCarta simbolo)
        {
            return simbolo switch
            {
                SimboloCarta.Zero => 0,
                SimboloCarta.Um => 1,
                SimboloCarta.Dois => 2,
                SimboloCarta.Tres => 3,
                SimboloCarta.Quatro => 4,
                SimboloCarta.Cinco => 5,
                SimboloCarta.Seis => 6,
                SimboloCarta.Sete => 7,
                SimboloCarta.Oito => 8,
                SimboloCarta.Nove => 9,
                SimboloCarta.Compra2 => 20,
                SimboloCarta.Inverter => 20,
                SimboloCarta.Salta => 20,
                SimboloCarta.Coringa => 50,
                SimboloCarta.CoringaMais4 => 50,
                _ => 0
            };
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Cor} {Simbolo} ({Pontos} pts)";
    }
}