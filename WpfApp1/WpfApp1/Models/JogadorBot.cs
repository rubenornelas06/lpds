using System.Linq;
using System.Xml.Serialization;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    /// <summary>
    /// Representa um jogador controlado pela IA (bot).
    /// Herda de Jogador e implementa a lógica de decisão automática.
    /// Todos os bots partilham o mesmo comportamento de jogo (estratégia simples).
    /// </summary>
    [XmlRoot("JogadorBot")]
    public class JogadorBot : Jogador
    {
        // ─────────────────────────────────────────────────
        //  Nomes Pré-definidos para os Bots
        // ─────────────────────────────────────────────────

        private static readonly string[] _nomesDisponiveis =
        {
            "Bot Alpha",
            "Bot Beta",
            "Bot Gamma"
        };

        // ─────────────────────────────────────────────────
        //  Propriedades
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Indica se o bot já gritou "UNO" nesta partida antes de jogar a penúltima carta.
        /// </summary>
        [XmlElement("GritouUno")]
        public bool GritouUno { get; set; }

        // ─────────────────────────────────────────────────
        //  Construtores
        // ─────────────────────────────────────────────────

        /// <summary>Construtor sem parâmetros necessário para serialização XML.</summary>
        public JogadorBot() : base() { }

        /// <summary>Cria um bot com o nome indicado.</summary>
        public JogadorBot(string nome) : base(nome) { }

        // ─────────────────────────────────────────────────
        //  Métodos de Fábrica
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Cria um bot com nome pré-definido baseado no índice (0, 1 ou 2).
        /// </summary>
        /// <param name="indice">Posição do bot (0 a 2).</param>
        public static JogadorBot Criar(int indice)
        {
            string nome = indice >= 0 && indice < _nomesDisponiveis.Length
                ? _nomesDisponiveis[indice]
                : $"Bot {indice + 1}";
            return new JogadorBot(nome);
        }

        // ─────────────────────────────────────────────────
        //  Lógica de Decisão (IA simples)
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Escolhe a melhor carta a jogar com base na situação atual.
        /// Estratégia: jogar a primeira carta válida encontrada.
        /// Prioridade: cartas numeradas > cartas de ação > coringas.
        /// </summary>
        /// <param name="cartaTopo">Carta no topo da pilha de descarte.</param>
        /// <param name="corAtiva">Cor ativa definida no jogo.</param>
        /// <returns>A carta escolhida, ou null se não tiver nenhuma jogável.</returns>
        public Carta EscolherCartaAJogar(Carta cartaTopo, CorCarta corAtiva)
        {
            var jogaveis = ObterCartasJogaveis(cartaTopo, corAtiva);

            if (jogaveis.Count == 0)
                return null;

            // Priorizar cartas com cor (não coringas) para guardar coringas para o final
            var comCor = jogaveis
                .Where(c => c.Cor != CorCarta.Nenhuma)
                .ToList();

            return comCor.Count > 0 ? comCor[0] : jogaveis[0];
        }

        /// <summary>
        /// Escolhe a cor a declarar quando joga um Coringa ou Coringa+4.
        /// Estratégia: escolhe a cor mais frequente na mão atual.
        /// </summary>
        public CorCarta EscolherCor()
        {
            var cartasComCor = Cartas
                .Where(c => c.Cor != CorCarta.Nenhuma)
                .ToList();

            if (cartasComCor.Count == 0)
                return CorCarta.Vermelho; // fallback

            return cartasComCor
                .GroupBy(c => c.Cor)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
        }

        /// <summary>
        /// Verifica se o bot deve gritar "UNO" antes de jogar.
        /// O bot deve gritar UNO quando tem 2 cartas (vai ficar com 1 após jogar).
        /// </summary>
        public bool DeveGritarUnoAgora() => Cartas.Count == 2;

        /// <summary>Reinicia o estado do grito de UNO para uma nova partida.</summary>
        public void ReiniciarGritoUno() => GritouUno = false;
    }
}