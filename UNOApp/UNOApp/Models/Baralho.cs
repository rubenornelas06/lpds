using System;
using System.Collections.Generic;
using WpfApp1.Models.Enums;

namespace WpfApp1.Models
{
    public class Baralho
    {
        public List<Carta> Cartas { get; set; } = new List<Carta>();

        public Baralho()
        {
            InicializarBaralhoBase();
        }

        // Método para inicializar as 108 cartas oficiais do UNO
        private void InicializarBaralhoBase()
        {
            int idAtual = 1;

            // Percorre as 4 cores normais (Vermelho, Azul, Verde, Amarelo)
            foreach (CorCarta cor in Enum.GetValues(typeof(CorCarta)))
            {
                if (cor == CorCarta.Nenhuma) continue;

                // 1 carta de valor Zero por cor
                Cartas.Add(new Carta(idAtual++, SimboloCarta.Zero, cor));

                // 2 cartas de cada valor (1 a 9) e cartas de ação (Compra2, Inverter, Salta)
                SimboloCarta[] simbolosDuplicados = {
                    SimboloCarta.Um, SimboloCarta.Dois, SimboloCarta.Tres, SimboloCarta.Quatro,
                    SimboloCarta.Cinco, SimboloCarta.Seis, SimboloCarta.Sete, SimboloCarta.Oito,
                    SimboloCarta.Nove, SimboloCarta.Compra2, SimboloCarta.Inverter, SimboloCarta.Salta
                };

                foreach (var simbolo in simbolosDuplicados)
                {
                    Cartas.Add(new Carta(idAtual++, simbolo, cor));
                    Cartas.Add(new Carta(idAtual++, simbolo, cor));
                }
            }

            // 4 Coringas normais e 4 Coringas +4 (sem cor definida)
            for (int i = 0; i < 4; i++)
            {
                Cartas.Add(new Carta(idAtual++, SimboloCarta.Coringa, CorCarta.Nenhuma));
                Cartas.Add(new Carta(idAtual++, SimboloCarta.CoringaMais4, CorCarta.Nenhuma));
            }
        }

        // Algoritmo clássico para baralhar as cartas
        public void Baralhar()
        {
            Random rnd = new Random();
            for (int i = 0; i < Cartas.Count; i++)
            {
                int j = rnd.Next(i, Cartas.Count);

                // Troca as cartas de posição
                Carta temp = Cartas[i];
                Cartas[i] = Cartas[j];
                Cartas[j] = temp;
            }
        }
    }
}