namespace DesafioProjetoHospedagem.Models
{
    public class Reserva
    {
        public List<Pessoa> Hospedes { get; set; }
        public Suite Suite { get; set; }
        public int DiasReservados { get; set; }

        public Reserva() { }

        public Reserva(int diasReservados)
        {
            DiasReservados = diasReservados;
        }

        public void CadastrarHospedes(List<Pessoa> hospedes)
        {
            int numeroHospedes = hospedes.Count;
            bool reservaPodeSerRealizada = ReservaPossuiCapacidadeDisponivel(numeroHospedes);
            if (reservaPodeSerRealizada)
            {
                Hospedes = hospedes;
            }
            else
            {
                throw new ArgumentException($"Número de hospedes não pode ser maior do que a capacidade da suíte ({Suite.Capacidade}).");
            }
        }

        private bool ReservaPossuiCapacidadeDisponivel(int quantidadeHospedes)
        {
            return Suite.Capacidade >= quantidadeHospedes + ObterQuantidadeHospedes();
        }

        public void CadastrarSuite(Suite suite)
        {
            Suite = suite;
        }

        public int ObterQuantidadeHospedes()
        {
            return Hospedes?.Count ?? 0;
        }

        public decimal CalcularValorDiaria()
        {

            decimal valor = DiasReservados * Suite.ValorDiaria;

            if (ReservaEstaAptaParaDesconto())
            {
                valor *= 0.9M;
            }

            return valor;
        }

        private bool ReservaEstaAptaParaDesconto() {
            return DiasReservados >= 10;
        }
    }
}