using System;
using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class Baralho
    {
        public List<Carta> Cartas { get; set; } = new List<Carta>();

        public Baralho() { }

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