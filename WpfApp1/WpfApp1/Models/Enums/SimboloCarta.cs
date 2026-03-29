using System.Xml.Serialization;

namespace WpfApp1.Models.Enums
{
    /// <summary>
    /// Representa todos os símbolos possíveis de uma carta de UNO.
    /// Valores numéricos (0-9) correspondem aos seus pontos.
    /// Cartas de ação valem 20 pontos. Coringas valem 50 pontos.
    /// </summary>
    public enum SimboloCarta
    {
        [XmlEnum("Zero")] Zero = 0,
        [XmlEnum("Um")] Um = 1,
        [XmlEnum("Dois")] Dois = 2,
        [XmlEnum("Tres")] Tres = 3,
        [XmlEnum("Quatro")] Quatro = 4,
        [XmlEnum("Cinco")] Cinco = 5,
        [XmlEnum("Seis")] Seis = 6,
        [XmlEnum("Sete")] Sete = 7,
        [XmlEnum("Oito")] Oito = 8,
        [XmlEnum("Nove")] Nove = 9,

        /// <summary>Obriga o próximo jogador a comprar 2 cartas e perder a vez.</summary>
        [XmlEnum("Compra2")] Compra2 = 10,

        /// <summary>Inverte o sentido do jogo.</summary>
        [XmlEnum("Inverter")] Inverter = 11,

        /// <summary>O próximo jogador perde a vez.</summary>
        [XmlEnum("Salta")] Salta = 12,

        /// <summary>Permite ao jogador escolher a cor ativa.</summary>
        [XmlEnum("Coringa")] Coringa = 13,

        /// <summary>Muda a cor ativa e obriga o próximo jogador a comprar 4 cartas.</summary>
        [XmlEnum("CoringaMais4")] CoringaMais4 = 14
    }
}