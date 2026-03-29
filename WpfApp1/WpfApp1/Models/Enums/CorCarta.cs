using System.Xml.Serialization;

namespace WpfApp1.Models.Enums
{
    /// <summary>
    /// Representa as cores possíveis de uma carta de UNO.
    /// </summary>
    public enum CorCarta
    {
        [XmlEnum("Vermelho")]
        Vermelho,

        [XmlEnum("Azul")]
        Azul,

        [XmlEnum("Verde")]
        Verde,

        [XmlEnum("Amarelo")]
        Amarelo,

        /// <summary>
        /// Usado para cartas Coringa e Coringa +4 (sem cor definida).
        /// </summary>
        [XmlEnum("Nenhuma")]
        Nenhuma
    }
}