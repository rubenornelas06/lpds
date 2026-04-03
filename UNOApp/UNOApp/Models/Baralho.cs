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

        // Inicializar as cartas
        private void InicializarBaralhoBase()
        {
            // Criação básica de algumas cartas só para popular a lista inicialmente
            foreach (CorCarta cor in Enum.GetValues(typeof(CorCarta)))
            {
                if (cor != CorCarta.Nenhuma)
                {
                    Cartas.Add(new Carta(Cartas.Count + 1, SimboloCarta.Zero, cor));
                    Cartas.Add(new Carta(Cartas.Count + 1, SimboloCarta.Um, cor));
                    Cartas.Add(new Carta(Cartas.Count + 1, SimboloCarta.Compra2, cor));
                }
            }
            // Adiciona coringas
            Cartas.Add(new Carta(Cartas.Count + 1, SimboloCarta.Coringa, CorCarta.Nenhuma));
            Cartas.Add(new Carta(Cartas.Count + 1, SimboloCarta.CoringaMais4, CorCarta.Nenhuma));
        }

        public void Baralhar()
        {
            Random rnd = new Random();
            for (int i = 0; i < Cartas.Count; i++)
            {
                int j = rnd.Next(i, Cartas.Count);
                Carta temp = Cartas[i];
                Cartas[i] = Cartas[j];
                Cartas[j] = temp;
            }
        }
    }
}