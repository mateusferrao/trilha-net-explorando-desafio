using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

// Listas para armazenar dados
List<Pessoa> hospedes = new();
List<Suite> suites = new();
List<Reserva> reservas = new();

bool continuar = true;

while (continuar)
{
    Console.Clear();
    Console.WriteLine("=== SISTEMA DE HOSPEDAGEM ===");
    Console.WriteLine("1. Cadastrar Hóspede");
    Console.WriteLine("2. Cadastrar Suíte");
    Console.WriteLine("3. Criar Reserva");
    Console.WriteLine("4. Listar Hóspedes");
    Console.WriteLine("5. Listar Suítes");
    Console.WriteLine("6. Listar Reservas");
    Console.WriteLine("7. Calcular Valor de Reserva");
    Console.WriteLine("0. Sair");
    Console.Write("\nEscolha uma opção: ");

    string opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            CadastrarHospede();
            break;
        case "2":
            CadastrarSuite();
            break;
        case "3":
            CriarReserva();
            break;
        case "4":
            ListarHospedes();
            break;
        case "5":
            ListarSuites();
            break;
        case "6":
            ListarReservas();
            break;
        case "7":
            CalcularValorReserva();
            break;
        case "0":
            continuar = false;
            Console.WriteLine("Saindo do sistema...");
            break;
        default:
            Console.WriteLine("Opção inválida!");
            break;
    }

    if (continuar)
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}

void CadastrarHospede()
{
    Console.Clear();
    Console.WriteLine("=== CADASTRAR HÓSPEDE ===");
    
    Console.Write("Nome: ");
    string nome = Console.ReadLine();
    
    Console.Write("Sobrenome: ");
    string sobrenome = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(nome))
    {
        Pessoa hospede = new(nome, sobrenome);
        hospedes.Add(hospede);
        Console.WriteLine($"\nHóspede {hospede.NomeCompleto} cadastrado com sucesso!");
    }
    else
    {
        Console.WriteLine("\nNome é obrigatório!");
    }
}

void CadastrarSuite()
{
    Console.Clear();
    Console.WriteLine("=== CADASTRAR SUÍTE ===");
    
    Console.Write("Tipo da Suíte: ");
    string tipoSuite = Console.ReadLine();
    
    Console.Write("Capacidade: ");
    if (int.TryParse(Console.ReadLine(), out int capacidade))
    {
        Console.Write("Valor da Diária: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valorDiaria))
        {
            Suite suite = new(tipoSuite, capacidade, valorDiaria);
            suites.Add(suite);
            Console.WriteLine($"\nSuíte {suite.TipoSuite} cadastrada com sucesso!");
        }
        else
        {
            Console.WriteLine("\nValor da diária inválido!");
        }
    }
    else
    {
        Console.WriteLine("\nCapacidade inválida!");
    }
}

void CriarReserva()
{
    Console.Clear();
    Console.WriteLine("=== CRIAR RESERVA ===");

    if (hospedes.Count == 0)
    {
        Console.WriteLine("Não há hóspedes cadastrados!");
        return;
    }

    if (suites.Count == 0)
    {
        Console.WriteLine("Não há suítes cadastradas!");
        return;
    }

    // Selecionar hóspedes
    Console.WriteLine("\nHóspedes disponíveis:");
    for (int i = 0; i < hospedes.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {hospedes[i].NomeCompleto}");
    }

    Console.Write("\nDigite os números dos hóspedes (separados por vírgula): ");
    string[] indicesHospedes = Console.ReadLine().Split(',');
    List<Pessoa> hospedesSelecionados = new();

    foreach (string indice in indicesHospedes)
    {
        if (int.TryParse(indice.Trim(), out int idx) && idx > 0 && idx <= hospedes.Count)
        {
            hospedesSelecionados.Add(hospedes[idx - 1]);
        }
    }

    if (hospedesSelecionados.Count == 0)
    {
        Console.WriteLine("Nenhum hóspede válido selecionado!");
        return;
    }

    // Selecionar suíte
    Console.WriteLine("\nSuítes disponíveis:");
    for (int i = 0; i < suites.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {suites[i].TipoSuite} - Capacidade: {suites[i].Capacidade} - R$ {suites[i].ValorDiaria}");
    }

    Console.Write("\nEscolha o número da suíte: ");
    if (int.TryParse(Console.ReadLine(), out int indiceSuite) && indiceSuite > 0 && indiceSuite <= suites.Count)
    {
        Suite suiteSelecionada = suites[indiceSuite - 1];

        // Definir dias reservados
        Console.Write("Quantidade de dias reservados: ");
        if (int.TryParse(Console.ReadLine(), out int diasReservados) && diasReservados > 0)
        {
            try
            {
                Reserva reserva = new(diasReservados);
                reserva.CadastrarSuite(suiteSelecionada);
                reserva.CadastrarHospedes(hospedesSelecionados);
                reservas.Add(reserva);

                Console.WriteLine($"\nReserva criada com sucesso!");
                Console.WriteLine($"Valor total: R$ {reserva.CalcularValorDiaria():F2}");
                if (diasReservados >= 10)
                {
                    Console.WriteLine("Desconto de 10% aplicado para reservas de 10 dias ou mais!");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nErro: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Quantidade de dias inválida!");
        }
    }
    else
    {
        Console.WriteLine("Suíte inválida!");
    }
}

void ListarHospedes()
{
    Console.Clear();
    Console.WriteLine("=== LISTAR HÓSPEDES ===");
    
    if (hospedes.Count == 0)
    {
        Console.WriteLine("Nenhum hóspede cadastrado.");
    }
    else
    {
        for (int i = 0; i < hospedes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {hospedes[i].NomeCompleto}");
        }
    }
}

void ListarSuites()
{
    Console.Clear();
    Console.WriteLine("=== LISTAR SUÍTES ===");
    
    if (suites.Count == 0)
    {
        Console.WriteLine("Nenhuma suíte cadastrada.");
    }
    else
    {
        for (int i = 0; i < suites.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {suites[i].TipoSuite} - Capacidade: {suites[i].Capacidade} - R$ {suites[i].ValorDiaria}");
        }
    }
}

void ListarReservas()
{
    Console.Clear();
    Console.WriteLine("=== LISTAR RESERVAS ===");
    
    if (reservas.Count == 0)
    {
        Console.WriteLine("Nenhuma reserva cadastrada.");
    }
    else
    {
        for (int i = 0; i < reservas.Count; i++)
        {
            Reserva reserva = reservas[i];
            Console.WriteLine($"\nReserva {i + 1}:");
            Console.WriteLine($"Suíte: {reserva.Suite.TipoSuite}");
            Console.WriteLine($"Hóspedes: {reserva.ObterQuantidadeHospedes()}");
            Console.WriteLine($"Dias: {reserva.DiasReservados}");
            Console.WriteLine($"Valor: R$ {reserva.CalcularValorDiaria():F2}");
        }
    }
}

void CalcularValorReserva()
{
    Console.Clear();
    Console.WriteLine("=== CALCULAR VALOR DE RESERVA ===");
    
    if (reservas.Count == 0)
    {
        Console.WriteLine("Não há reservas cadastradas!");
        return;
    }

    Console.WriteLine("Reservas disponíveis:");
    for (int i = 0; i < reservas.Count; i++)
    {
        Reserva reserva = reservas[i];
        Console.WriteLine($"{i + 1}. Suíte {reserva.Suite.TipoSuite} - {reserva.DiasReservados} dias");
    }

    Console.Write("\nEscolha o número da reserva: ");
    if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= reservas.Count)
    {
        Reserva reserva = reservas[indice - 1];
        Console.WriteLine($"\nValor da reserva: R$ {reserva.CalcularValorDiaria():F2}");
        if (reserva.DiasReservados >= 10)
        {
            Console.WriteLine("Desconto de 10% aplicado!");
        }
    }
    else
    {
        Console.WriteLine("Reserva inválida!");
    }
}