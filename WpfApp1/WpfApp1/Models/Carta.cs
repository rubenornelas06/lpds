namespace WpfProjectLpdsw.Models
{
    public class Carta
    {
        public int Id { get; set; }
        public string Simbolo { get; set; } // Ex: "0" a "9", "Inverter", "Coringa"
        public string Cor { get; set; }     // Ex: "Vermelho", "Azul", "Verde", "Amarelo", "Preto" (Coringas)
        public int Pontos { get; set; }
    }
}