using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    /// <summary>
    /// Representa um jogo completo de UNO, composto por várias partidas.
    /// Um jogo termina quando um jogador atinge a pontuação máxima (500 pts).
    /// Responsável por gerir a lógica central, pontuações e persistência em XML.
    /// </summary>
    [XmlRoot("Jogo")]
    public class Jogo
    {
        // ─────────────────────────────────────────────────
        //  Constantes
        // ─────────────────────────────────────────────────

        /// <summary>Pontuação máxima para ganhar o jogo (regras oficiais de UNO).</summary>
        public const int PONTUACAO_MAXIMA = 500;

        /// <summary>Número de cartas distribuídas a cada jogador no início de cada partida.</summary>
        public const int CARTAS_INICIAIS = 7;

        // ─────────────────────────────────────────────────
        //  Propriedades
        // ─────────────────────────────────────────────────

        /// <summary>Lista de todos os jogadores (humano + bots).</summary>
        [XmlArray("Jogadores")]
        [XmlArrayItem("Jogador", typeof(Jogador))]
        [XmlArrayItem("JogadorBot", typeof(JogadorBot))]
        public List<Jogador> Jogadores { get; set; } = new List<Jogador>();

        /// <summary>
        /// Pontuação acumulada de cada jogador no jogo atual.
        /// O índice corresponde ao índice do jogador em <see cref="Jogadores"/>.
        /// </summary>
        [XmlArray("Pontuacoes")]
        [XmlArrayItem("Pontuacao")]
        public List<int> Pontuacoes { get; set; } = new List<int>();

        /// <summary>
        /// Índice do jogador ativo na lista <see cref="Jogadores"/>.
        /// Usado para serialização XML (XmlIgnore no objeto direto).
        /// </summary>
        [XmlElement("IndiceJogadorAtivo")]
        public int IndiceJogadorAtivo { get; set; } = 0;

        /// <summary>Jogador cuja vez é de jogar. Não serializado diretamente.</summary>
        [XmlIgnore]
        public Jogador JogadorAtivo =>
            IndiceJogadorAtivo >= 0 && IndiceJogadorAtivo < Jogadores.Count
                ? Jogadores[IndiceJogadorAtivo]
                : null;

        /// <summary>Mesa de jogo (pilha de descarte + baralho).</summary>
        [XmlElement("Mesa")]
        public Mesa Mesa { get; set; } = new Mesa();

        /// <summary>Sentido de rotação atual das jogadas.</summary>
        [XmlElement("SentidoJogo")]
        public SentidoJogo SentidoJogo { get; set; } = SentidoJogo.Horario;

        /// <summary>Indica se a partida está em curso.</summary>
        [XmlElement("PartidaAtiva")]
        public bool PartidaAtiva { get; set; }

        /// <summary>Indica se o jogador humano gritou UNO na penúltima carta desta partida.</summary>
        [XmlElement("HumanoGritouUno")]
        public bool HumanoGritouUno { get; set; }

        // ─────────────────────────────────────────────────
        //  Construtores
        // ─────────────────────────────────────────────────

        /// <summary>Construtor sem parâmetros necessário para serialização XML.</summary>
        public Jogo() { }

        // ─────────────────────────────────────────────────
        //  Inicialização
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Cria um novo jogo com o jogador humano (nome do perfil Windows) e N bots.
        /// </summary>
        /// <param name="nomeHumano">Nome do utilizador Windows.</param>
        /// <param name="numeroBots">Número de bots (1 a 3).</param>
        public static Jogo Criar(string nomeHumano, int numeroBots)
        {
            if (numeroBots < 1 || numeroBots > 3)
                throw new ArgumentOutOfRangeException(nameof(numeroBots), "O número de bots deve ser entre 1 e 3.");

            var jogo = new Jogo();
            jogo.Jogadores.Add(new Jogador(nomeHumano));

            for (int i = 0; i < numeroBots; i++)
                jogo.Jogadores.Add(JogadorBot.Criar(i));

            jogo.InicializarPontuacoes();

            foreach (var jogador in jogo.Jogadores)
                jogador.RegistarInicioJogo();

            return jogo;
        }

        /// <summary>Inicializa a lista de pontuações a zero para todos os jogadores.</summary>
        public void InicializarPontuacoes()
        {
            Pontuacoes.Clear();
            foreach (var _ in Jogadores)
                Pontuacoes.Add(0);
        }

        // ─────────────────────────────────────────────────
        //  Gestão de Partida
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Inicia uma nova partida: baralha e distribui as cartas, vira a primeira carta.
        /// </summary>
        public void IniciarPartida()
        {
            // Cria e baralha um novo baralho
            Mesa.Baralho = Baralho.CriarBaralhoCompleto();
            Mesa.CartasJogadas.Clear();

            // Distribui CARTAS_INICIAIS cartas a cada jogador
            foreach (var jogador in Jogadores)
            {
                jogador.LimparMao();
                jogador.AdicionarCartas(Mesa.Baralho.ComprarCartas(CARTAS_INICIAIS));
                jogador.RegistarInicioPartida();

                if (jogador is JogadorBot bot)
                    bot.ReiniciarGritoUno();
            }

            // Vira a primeira carta (não pode ser Coringa+4)
            Carta primeiraCartaValida;
            do
            {
                primeiraCartaValida = Mesa.Baralho.ComprarCarta();
            }
            while (primeiraCartaValida.Simbolo == SimboloCarta.CoringaMais4);

            Mesa.ColocarCarta(primeiraCartaValida);

            // Se a primeira carta for um Coringa normal, a cor ativa fica por definir
            // (na implementação real, pede ao jogador inicial para escolher a cor)

            // O humano começa sempre
            IndiceJogadorAtivo = 0;
            HumanoGritouUno = false;
            PartidaAtiva = true;
        }

        // ─────────────────────────────────────────────────
        //  Gestão de Turnos
        // ─────────────────────────────────────────────────

        /// <summary>Avança para o próximo jogador de acordo com o sentido do jogo.</summary>
        public void PassarParaProximoJogador()
        {
            if (SentidoJogo == SentidoJogo.Horario)
                IndiceJogadorAtivo = (IndiceJogadorAtivo + 1) % Jogadores.Count;
            else
                IndiceJogadorAtivo = (IndiceJogadorAtivo - 1 + Jogadores.Count) % Jogadores.Count;
        }

        /// <summary>Salta o próximo jogador (efeito da carta Salta).</summary>
        public void SaltarProximoJogador()
        {
            PassarParaProximoJogador();
            PassarParaProximoJogador();
        }

        /// <summary>Inverte o sentido do jogo (efeito da carta Inverter).</summary>
        public void InverterSentido() =>
            SentidoJogo = SentidoJogo == SentidoJogo.Horario
                ? SentidoJogo.AntiHorario
                : SentidoJogo.Horario;

        // ─────────────────────────────────────────────────
        //  Fim de Partida e Pontuação
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Calcula e atualiza as pontuações após a partida terminar.
        /// O vencedor recebe os pontos das cartas que restam nas mãos dos outros jogadores.
        /// </summary>
        /// <param name="vencedor">Jogador que ficou sem cartas.</param>
        public void TerminarPartida(Jogador vencedor)
        {
            int indiceVencedor = Jogadores.IndexOf(vencedor);
            int pontosGanhos = 0;

            foreach (var jogador in Jogadores)
            {
                if (jogador != vencedor)
                    pontosGanhos += jogador.ContarPontosMao();
            }

            Pontuacoes[indiceVencedor] += pontosGanhos;
            vencedor.RegistarVitoriaPartida();
            PartidaAtiva = false;
        }

        /// <summary>
        /// Verifica se algum jogador atingiu ou ultrapassou a pontuação máxima.
        /// </summary>
        /// <returns>True se o jogo chegou ao fim.</returns>
        public bool JogoTerminou()
        {
            for (int i = 0; i < Pontuacoes.Count; i++)
            {
                if (Pontuacoes[i] >= PONTUACAO_MAXIMA)
                {
                    Jogadores[i].RegistarVitoriaJogo();
                    return true;
                }
            }
            return false;
        }

        /// <summary>Devolve o jogador com maior pontuação acumulada no jogo.</summary>
        public Jogador ObterVencedorJogo()
        {
            int maxPts = -1;
            Jogador vencedor = null;
            for (int i = 0; i < Pontuacoes.Count; i++)
            {
                if (Pontuacoes[i] > maxPts)
                {
                    maxPts = Pontuacoes[i];
                    vencedor = Jogadores[i];
                }
            }
            return vencedor;
        }

        // ─────────────────────────────────────────────────
        //  Persistência XML
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Serializa o estado completo do jogo para um ficheiro XML.
        /// </summary>
        /// <param name="caminhoFicheiro">Caminho completo do ficheiro de destino.</param>
        public void Guardar(string caminhoFicheiro)
        {
            var serializer = new XmlSerializer(typeof(Jogo));
            using var stream = new FileStream(caminhoFicheiro, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, this);
        }

        /// <summary>
        /// Carrega um jogo previamente guardado a partir de um ficheiro XML.
        /// </summary>
        /// <param name="caminhoFicheiro">Caminho completo do ficheiro a carregar.</param>
        public static Jogo Carregar(string caminhoFicheiro)
        {
            var serializer = new XmlSerializer(typeof(Jogo));
            using var stream = new FileStream(caminhoFicheiro, FileMode.Open, FileAccess.Read);
            return (Jogo)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Devolve o caminho padrão para guardar o estado do jogo
        /// (pasta pessoal do utilizador Windows).
        /// </summary>
        public static string ObterCaminhoGuardado()
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "UnoGame");

            Directory.CreateDirectory(pasta);
            return Path.Combine(pasta, "jogo_guardado.xml");
        }

        /// <summary>
        /// Devolve o caminho padrão para guardar as estatísticas dos jogadores.
        /// </summary>
        public static string ObterCaminhoEstatisticas()
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "UnoGame");

            Directory.CreateDirectory(pasta);
            return Path.Combine(pasta, "estatisticas.xml");
        }
    }
}