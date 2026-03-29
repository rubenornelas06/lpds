using System.Xml.Serialization;

namespace WpfApp1.Models.Enums
{
    /// <summary>
    /// Representa o sentido de rotação do jogo (quem joga a seguir).
    /// </summary>
    public enum SentidoJogo
    {
        /// <summary>Ordem natural dos jogadores (esquerda para a direita).</summary>
        [XmlEnum("Horario")]
        Horario,

        /// <summary>Ordem inversa dos jogadores (direita para a esquerda).</summary>
        [XmlEnum("AntiHorario")]
        AntiHorario
    }
}