using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    /// <summary>
    /// Representa a mesa de jogo do UNO.
    /// Contém a pilha de descarte (cartas jogadas) e o baralho de compra.
    /// </summary>
    [XmlRoot("Mesa")]
    public class Mesa
    {
        // ─────────────────────────────────────────────────
        //  Propriedades
        // ─────────────────────────────────────────────────

        /// <summary>Pilha de cartas descartadas (jogadas). A última é o topo.</summary>
        [XmlArray("CartasJogadas")]
        [XmlArrayItem("Carta")]
        public List<Carta> CartasJogadas { get; set; } = new List<Carta>();

        /// <summary>Baralho de compra (cartas ainda disponíveis para distribuir).</summary>
        [XmlElement("Baralho")]
        public Baralho Baralho { get; set; } = new Baralho();

        /// <summary>
        /// Cor ativa no jogo. Normalmente é igual à cor da carta no topo,
        /// mas pode ser diferente quando um coringa é jogado.
        /// </summary>
        [XmlElement("CorAtiva")]
        public CorCarta CorAtiva { get; set; }

        // ─────────────────────────────────────────────────
        //  Propriedades Calculadas
        // ─────────────────────────────────────────────────

        /// <summary>Carta no topo da pilha de descarte (a última jogada). Null se a pilha estiver vazia.</summary>
        [XmlIgnore]
        public Carta CartaTopo =>
            CartasJogadas.Count > 0 ? CartasJogadas[^1] : null;

        // ─────────────────────────────────────────────────
        //  Construtores
        // ─────────────────────────────────────────────────

        /// <summary>Construtor sem parâmetros necessário para serialização XML.</summary>
        public Mesa() { }

        // ─────────────────────────────────────────────────
        //  Métodos
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Coloca uma carta no topo da pilha de descarte e atualiza a cor ativa.
        /// Para coringas, a cor ativa deve ser definida separadamente via <see cref="DefinirCorAtiva"/>.
        /// </summary>
        public void ColocarCarta(Carta carta)
        {
            CartasJogadas.Add(carta);

            // Só atualiza a cor se a carta tiver cor definida
            if (carta.Cor != CorCarta.Nenhuma)
                CorAtiva = carta.Cor;
        }

        /// <summary>
        /// Define a cor ativa manualmente (usado após jogar um Coringa ou Coringa+4).
        /// </summary>
        public void DefinirCorAtiva(CorCarta cor)
        {
            if (cor == CorCarta.Nenhuma)
                throw new ArgumentException("Não é possível definir 'Nenhuma' como cor ativa.");

            CorAtiva = cor;
        }

        /// <summary>
        /// Compra uma carta do baralho. Se o baralho estiver vazio,
        /// reabastece-o com as cartas descartadas (mantendo a carta do topo).
        /// </summary>
        public Carta ComprarCartaDoBaralho()
        {
            if (Baralho.EstaVazio)
                ReabastecarBaralho();

            return Baralho.ComprarCarta();
        }

        /// <summary>
        /// Compra N cartas do baralho, reabastecendo se necessário.
        /// </summary>
        public List<Carta> ComprarCartasDoBaralho(int quantidade)
        {
            var lista = new List<Carta>();
            for (int i = 0; i < quantidade; i++)
                lista.Add(ComprarCartaDoBaralho());
            return lista;
        }

        /// <summary>
        /// Reabastece o baralho usando todas as cartas descartadas (exceto a do topo).
        /// </summary>
        private void ReabastecarBaralho()
        {
            if (CartasJogadas.Count <= 1)
                throw new InvalidOperationException("Não há cartas suficientes para reabastecer o baralho.");

            var cartaTopo = CartaTopo;
            var descartadas = new List<Carta>(CartasJogadas);
            descartadas.RemoveAt(descartadas.Count - 1); // Remove o topo

            CartasJogadas.Clear();
            CartasJogadas.Add(cartaTopo);

            Baralho.ReabastecarComDescartadas(descartadas);
        }
    }
}