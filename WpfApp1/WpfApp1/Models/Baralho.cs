using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    /// <summary>
    /// Representa o baralho de cartas do UNO (108 cartas no total).
    /// Responsável por criar, baralhar e distribuir cartas.
    /// </summary>
    [XmlRoot("Baralho")]
    public class Baralho
    {
        // ─────────────────────────────────────────────────
        //  Propriedades
        // ─────────────────────────────────────────────────

        /// <summary>Lista de cartas disponíveis no baralho.</summary>
        [XmlArray("Cartas")]
        [XmlArrayItem("Carta")]
        public List<Carta> Cartas { get; set; } = new List<Carta>();

        private static readonly Random _rng = new Random();

        // ─────────────────────────────────────────────────
        //  Construtores
        // ─────────────────────────────────────────────────

        /// <summary>Construtor sem parâmetros necessário para serialização XML.</summary>
        public Baralho() { }

        // ─────────────────────────────────────────────────
        //  Métodos de Fábrica
        // ─────────────────────────────────────────────────

        /// <summary>
        /// Cria um baralho completo de UNO oficial (108 cartas) já baralhado.
        /// Composição:
        ///   - 4 cores × 1 carta zero = 4 cartas
        ///   - 4 cores × 2 × (1–9, Compra2, Inverter, Salta) = 96 cartas
        ///   - 4 Coringas + 4 Coringa+4 = 8 cartas
        ///   Total: 108 cartas
        /// </summary>
        public static Baralho CriarBaralhoCompleto()
        {
            var baralho = new Baralho();
            int id = 1;

            var cores = new[] { CorCarta.Vermelho, CorCarta.Azul, CorCarta.Verde, CorCarta.Amarelo };

            foreach (var cor in cores)
            {
                // Apenas 1 carta Zero por cor
                baralho.Cartas.Add(new Carta(id++, SimboloCarta.Zero, cor));

                // 2 cartas de cada para Um–Nove, Compra2, Inverter, Salta
                for (int rep = 0; rep < 2; rep++)
                {
                    for (int num = 1; num <= 9; num++)
                        baralho.Cartas.Add(new Carta(id++, (SimboloCarta)num, cor));

                    baralho.Cartas.Add(new Carta(id++, SimboloCarta.Compra2, cor));
                    baralho.Cartas.Add(new Carta(id++, SimboloCarta.Inverter, cor));
                    baralho.Cartas.Add(new Carta(id++, SimboloCarta.Salta, cor));
                }
            }

            // 4 Coringas e 4 Coringa+4 (sem cor)
            for (int i = 0; i < 4; i++)
            {
                baralho.Cartas.Add(new Carta(id++, SimboloCarta.Coringa, CorCarta.Nenhuma));
                baralho.Cartas.Add(new Carta(id++, SimboloCarta.CoringaMais4, CorCarta.Nenhuma));
            }

            baralho.Baralhar();
            return baralho;
        }

        // ─────────────────────────────────────────────────
        //  Métodos de Operação
        // ─────────────────────────────────────────────────

        /// <summary>Baralha as cartas usando o algoritmo Fisher–Yates.</summary>
        public void Baralhar()
        {
            for (int i = Cartas.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (Cartas[i], Cartas[j]) = (Cartas[j], Cartas[i]);
            }
        }

        /// <summary>Remove e devolve a carta do topo do baralho.</summary>
        /// <exception cref="InvalidOperationException">Lançada se o baralho estiver vazio.</exception>
        public Carta ComprarCarta()
        {
            if (Cartas.Count == 0)
                throw new InvalidOperationException("O baralho está vazio.");

            var carta = Cartas[^1];
            Cartas.RemoveAt(Cartas.Count - 1);
            return carta;
        }

        /// <summary>Remove e devolve uma lista de N cartas do topo do baralho.</summary>
        public List<Carta> ComprarCartas(int quantidade)
        {
            var lista = new List<Carta>();
            for (int i = 0; i < quantidade; i++)
                lista.Add(ComprarCarta());
            return lista;
        }

        /// <summary>
        /// Reabastece o baralho com as cartas descartadas (mantendo a carta do topo na pilha)
        /// e baralha de novo.
        /// </summary>
        public void ReabastecarComDescartadas(List<Carta> cartasDescartadas)
        {
            Cartas.AddRange(cartasDescartadas);
            Baralhar();
        }

        /// <summary>Número de cartas restantes no baralho.</summary>
        public int Total => Cartas.Count;

        /// <summary>Indica se o baralho está sem cartas.</summary>
        public bool EstaVazio => Cartas.Count == 0;
    }
}