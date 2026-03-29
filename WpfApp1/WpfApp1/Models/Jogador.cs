using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    /// <summary>
    /// Representa um jogador do UNO (humano ou bot).
    /// Guarda as cartas na mão e as estatísticas acumuladas ao longo de todos os jogos.
    /// </summary>
    [XmlRoot("Jogador")]
    [XmlInclude(typeof(JogadorBot))]
    public class Jogador
    {
        // ─────────────────────────────────────────────────
        //  Propriedades de Identidade
        // ─────────────────────────────────────────────────

        /// <summary>Nome do jogador (para o humano, corresponde ao perfil do Windows).</summary>
        [XmlElement("Nome")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>Caminho para a fotografia/avatar do jogador.</summary>
        [XmlElement("Fotografia")]
        public string Fotografia { get; set; } = string.Empty;

        // ─────────────────────────────────────────────────
        //  Cartas na Mão
        // ─────────────────────────────────────────────────

        /// <summary>Cartas atualmente na mão do jogador.</summary>
        [XmlArray("Cartas")]
        [XmlArrayItem("Carta")]
        public List<Carta> Cartas { get; set; } = new List<Carta>();

        // ─────────────────────────────────────────────────
        //  Estatísticas
        // ─────────────────────────────────────────────────

        /// <summary>Número total de partidas jogadas pelo jogador.</summary>
        [XmlElement("N_Partidas_Jogadas")]
        public int N_Partidas_Jogadas { get; set; }

        /// <summary>Número de partidas ganhas pelo jogador.</summary>
        [XmlElement("N_Partidas_Ganhos")]
        public int N_Partidas_Ganhos { get; set; }

        /// <summary>Número total de jogos completos em que o jogador participou.</summary>
        [XmlElement("N_Jogos_Jogados")]
        public int N_Jogos_Jogados { get; set; }

        /// <summary>Número de jogos completos ganhos pelo jogador.</summary>
        [XmlElement("N_Jogos_Ganhos")]
        public int N_Jogos_Ganhos { get; set; }

        // ─────────────────────────────────────────────────
        //  Construtores
        // ─────────────────────────────────────────────────

        /// <summary>Construtor sem parâmetros necessário para serialização XML.</summary>
        public Jogador() { }

        /// <summary>Cria um jogador com o nome indicado.</summary>
        public Jogador(string nome)
        {
            Nome = nome;
        }

        // ─────────────────────────────────────────────────
        //  Propriedades Calculadas
        // ─────────────────────────────────────────────────

        /// <summary>Indica se o jogador tem exatamente uma carta na mão (deve gritar UNO).</summary>
        public bool DeveGritarUno => Cartas.Count == 1;

        /// <summary>Indica se o jogador ganhou a partida (ficou sem cartas).</summary>
        public bool GanhouPartida => Cartas.Count == 0;

        // ─────────────────────────────────────────────────
        //  Métodos de Gestão da Mão
        // ─────────────────────────────────────────────────

        /// <summary>Adiciona uma carta à mão do jogador.</summary>
        public void AdicionarCarta(Carta carta) => Cartas.Add(carta);

        /// <summary>Adiciona uma lista de cartas à mão do jogador.</summary>
        public void AdicionarCartas(List<Carta> cartas) => Cartas.AddRange(cartas);

        /// <summary>Remove uma carta específica da mão do jogador.</summary>
        /// <returns>True se a carta foi encontrada e removida.</returns>
        public bool RemoverCarta(Carta carta) => Cartas.Remove(carta);

        /// <summary>Limpa todas as cartas da mão (início de nova partida).</summary>
        public void LimparMao() => Cartas.Clear();

        // ─────────────────────────────────────────────────
        //  Métodos de Jogo
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Verifica se o jogador tem pelo menos uma carta jogável sobre a carta do topo.
        /// </summary>
        public bool TemCartasJogaveis(Carta cartaTopo, CorCarta corAtiva)
            => Cartas.Any(c => c.PodeJogarSobre(cartaTopo, corAtiva));

        /// <summary>Devolve a lista de cartas jogáveis na situação atual.</summary>
        public List<Carta> ObterCartasJogaveis(Carta cartaTopo, CorCarta corAtiva)
            => Cartas.Where(c => c.PodeJogarSobre(cartaTopo, corAtiva)).ToList();

        /// <summary>Soma os pontos de todas as cartas que ainda restam na mão.</summary>
        public int ContarPontosMao() => Cartas.Sum(c => c.Pontos);

        // ─────────────────────────────────────────────────
        //  Métodos de Estatísticas
        // ─────────────────────────────────────────────────

        /// <summary>Regista o início de uma nova partida nas estatísticas.</summary>
        public void RegistarInicioPartida() => N_Partidas_Jogadas++;

        /// <summary>Regista uma vitória de partida nas estatísticas.</summary>
        public void RegistarVitoriaPartida() => N_Partidas_Ganhos++;

        /// <summary>Regista o início de um novo jogo completo nas estatísticas.</summary>
        public void RegistarInicioJogo() => N_Jogos_Jogados++;

        /// <summary>Regista uma vitória de jogo completo nas estatísticas.</summary>
        public void RegistarVitoriaJogo() => N_Jogos_Ganhos++;

        /// <inheritdoc/>
        public override string ToString() => Nome;
    }
}