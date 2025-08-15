using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

var sistemaHospedagem = new SistemaHospedagem();
sistemaHospedagem.Executar();

public class SistemaHospedagem
{
    private readonly List<Pessoa> _hospedes;
    private readonly List<Suite> _suites;
    private readonly List<Reserva> _reservas;
    private bool _executando;

    public SistemaHospedagem()
    {
        _hospedes = new List<Pessoa>();
        _suites = new List<Suite>();
        _reservas = new List<Reserva>();
        _executando = true;
    }

    public void Executar()
    {
        while (_executando)
        {
            ExibirMenuPrincipal();
            ProcessarOpcaoSelecionada();
        }
    }

    private void ExibirMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║         SISTEMA DE HOSPEDAGEM           ║");
        Console.WriteLine("╠══════════════════════════════════════════╣");
        Console.WriteLine("║ 1. Cadastrar Hóspede                    ║");
        Console.WriteLine("║ 2. Cadastrar Suíte                      ║");
        Console.WriteLine("║ 3. Criar Reserva                        ║");
        Console.WriteLine("║ 4. Listar Hóspedes                      ║");
        Console.WriteLine("║ 5. Listar Suítes                        ║");
        Console.WriteLine("║ 6. Listar Reservas                      ║");
        Console.WriteLine("║ 7. Calcular Valor de Reserva            ║");
        Console.WriteLine("║ 0. Sair                                 ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.Write("\nEscolha uma opção: ");
    }

    private void ProcessarOpcaoSelecionada()
    {
        var opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                GerenciarCadastroHospede();
                break;
            case "2":
                GerenciarCadastroSuite();
                break;
            case "3":
                GerenciarCriacaoReserva();
                break;
            case "4":
                GerenciarListagemHospedes();
                break;
            case "5":
                GerenciarListagemSuites();
                break;
            case "6":
                GerenciarListagemReservas();
                break;
            case "7":
                GerenciarCalculoReserva();
                break;
            case "0":
                FinalizarSistema();
                break;
            default:
                ExibirMensagemErro("Opção inválida!");
                break;
        }

        if (_executando)
        {
            AguardarContinuacao();
        }
    }

    private void GerenciarCadastroHospede()
    {
        Console.Clear();
        ExibirTituloSecao("CADASTRAR HÓSPEDE");
        
        var dadosHospede = ObterDadosHospede();
        if (dadosHospede != null)
        {
            CadastrarHospede(dadosHospede.Nome, dadosHospede.Sobrenome);
            ExibirMensagemSucesso($"Hóspede {dadosHospede.NomeCompleto} cadastrado com sucesso!");
        }
    }

    private void GerenciarCadastroSuite()
    {
        Console.Clear();
        ExibirTituloSecao("CADASTRAR SUÍTE");
        
        var dadosSuite = ObterDadosSuite();
        if (dadosSuite != null)
        {
            CadastrarSuite(dadosSuite.TipoSuite, dadosSuite.Capacidade, dadosSuite.ValorDiaria);
            ExibirMensagemSucesso($"Suíte {dadosSuite.TipoSuite} cadastrada com sucesso!");
        }
    }

    private void GerenciarCriacaoReserva()
    {
        Console.Clear();
        ExibirTituloSecao("CRIAR RESERVA");

        if (!ValidarPreRequisitosReserva())
            return;

        var dadosReserva = ObterDadosReserva();
        if (dadosReserva != null)
        {
            CriarReserva(dadosReserva);
        }
    }

    private void GerenciarListagemHospedes()
    {
        Console.Clear();
        ExibirTituloSecao("LISTAR HÓSPEDES");
        ListarHospedes();
    }

    private void GerenciarListagemSuites()
    {
        Console.Clear();
        ExibirTituloSecao("LISTAR SUÍTES");
        ListarSuites();
    }

    private void GerenciarListagemReservas()
    {
        Console.Clear();
        ExibirTituloSecao("LISTAR RESERVAS");
        ListarReservas();
    }

    private void GerenciarCalculoReserva()
    {
        Console.Clear();
        ExibirTituloSecao("CALCULAR VALOR DE RESERVA");
        
        if (_reservas.Count == 0)
        {
            ExibirMensagemErro("Não há reservas cadastradas!");
            return;
        }

        var indiceReserva = SelecionarReserva();
        if (indiceReserva.HasValue)
        {
            CalcularEExibirValorReserva(indiceReserva.Value);
        }
    }

    private DadosHospede? ObterDadosHospede()
    {
        Console.Write("Nome: ");
        var nome = Console.ReadLine()?.Trim();
        
        Console.Write("Sobrenome: ");
        var sobrenome = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(nome))
        {
            ExibirMensagemErro("Nome é obrigatório!");
            return null;
        }

        return new DadosHospede(nome, sobrenome ?? "");
    }

    private DadosSuite? ObterDadosSuite()
    {
        Console.Write("Tipo da Suíte: ");
        var tipoSuite = Console.ReadLine()?.Trim();
        
        Console.Write("Capacidade: ");
        if (!int.TryParse(Console.ReadLine(), out int capacidade) || capacidade <= 0)
        {
            ExibirMensagemErro("Capacidade deve ser um número maior que zero!");
            return null;
        }

        Console.Write("Valor da Diária: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal valorDiaria) || valorDiaria <= 0)
        {
            ExibirMensagemErro("Valor da diária deve ser um número maior que zero!");
            return null;
        }

        if (string.IsNullOrWhiteSpace(tipoSuite))
        {
            ExibirMensagemErro("Tipo da suíte é obrigatório!");
            return null;
        }

        return new DadosSuite(tipoSuite, capacidade, valorDiaria);
    }

    private DadosReserva? ObterDadosReserva()
    {
        var hospedesSelecionados = SelecionarHospedes();
        if (hospedesSelecionados == null || !hospedesSelecionados.Any())
            return null;

        var suiteSelecionada = SelecionarSuite();
        if (suiteSelecionada == null)
            return null;

        var diasReservados = ObterDiasReservados();
        if (!diasReservados.HasValue)
            return null;

        return new DadosReserva(hospedesSelecionados, suiteSelecionada.Value, diasReservados.Value);
    }

    private List<int>? SelecionarHospedes()
    {
        ExibirListaHospedes();
        Console.Write("\nDigite os números dos hóspedes (separados por vírgula): ");
        
        var entrada = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(entrada))
        {
            ExibirMensagemErro("Nenhum hóspede selecionado!");
            return null;
        }

        var indices = ParseIndices(entrada, _hospedes.Count);
        if (!indices.Any())
        {
            ExibirMensagemErro("Nenhum hóspede válido selecionado!");
            return null;
        }

        return indices;
    }

    private int? SelecionarSuite()
    {
        ExibirListaSuites();
        Console.Write("\nEscolha o número da suíte: ");
        
        if (!int.TryParse(Console.ReadLine(), out int indice) || indice <= 0 || indice > _suites.Count)
        {
            ExibirMensagemErro("Suíte inválida!");
            return null;
        }

        return indice;
    }

    private int? ObterDiasReservados()
    {
        Console.Write("Quantidade de dias reservados: ");
        
        if (!int.TryParse(Console.ReadLine(), out int dias) || dias <= 0)
        {
            ExibirMensagemErro("Quantidade de dias deve ser maior que zero!");
            return null;
        }

        return dias;
    }

    private int? SelecionarReserva()
    {
        ExibirListaReservas();
        Console.Write("\nEscolha o número da reserva: ");
        
        if (!int.TryParse(Console.ReadLine(), out int indice) || indice <= 0 || indice > _reservas.Count)
        {
            ExibirMensagemErro("Reserva inválida!");
            return null;
        }

        return indice;
    }

    private void CadastrarHospede(string nome, string sobrenome)
    {
        var hospede = new Pessoa(nome, sobrenome);
        _hospedes.Add(hospede);
    }

    private void CadastrarSuite(string tipoSuite, int capacidade, decimal valorDiaria)
    {
        var suite = new Suite(tipoSuite, capacidade, valorDiaria);
        _suites.Add(suite);
    }

    private void CriarReserva(DadosReserva dados)
    {
        try
        {
            var hospedes = ObterHospedesPorIndices(dados.IndicesHospedes);
            var suite = _suites[dados.IndiceSuite - 1];

            var reserva = new Reserva(dados.DiasReservados);
            reserva.CadastrarSuite(suite);
            reserva.CadastrarHospedes(hospedes);

            _reservas.Add(reserva);

            ExibirMensagemSucesso("Reserva criada com sucesso!");
            ExibirDetalhesReserva(reserva);
        }
        catch (ArgumentException ex)
        {
            ExibirMensagemErro($"Erro: {ex.Message}");
        }
    }

    private void CalcularEExibirValorReserva(int indiceReserva)
    {
        var reserva = _reservas[indiceReserva - 1];
        var valor = reserva.CalcularValorDiaria();

        Console.WriteLine($"\nValor da reserva: R$ {valor:F2}");
        
        if (reserva.DiasReservados >= 10)
        {
            Console.WriteLine("✓ Desconto de 10% aplicado para reservas de 10 dias ou mais!");
        }
    }

    // ==================== MÉTODOS DE EXIBIÇÃO ====================
    private void ListarHospedes()
    {
        if (_hospedes.Count == 0)
        {
            ExibirMensagemInfo("Nenhum hóspede cadastrado.");
            return;
        }

        for (int i = 0; i < _hospedes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_hospedes[i].NomeCompleto}");
        }
    }

    private void ListarSuites()
    {
        if (_suites.Count == 0)
        {
            ExibirMensagemInfo("Nenhuma suíte cadastrada.");
            return;
        }

        for (int i = 0; i < _suites.Count; i++)
        {
            var suite = _suites[i];
            Console.WriteLine($"{i + 1}. {suite.TipoSuite} - Capacidade: {suite.Capacidade} - R$ {suite.ValorDiaria:F2}");
        }
    }

    private void ListarReservas()
    {
        if (_reservas.Count == 0)
        {
            ExibirMensagemInfo("Nenhuma reserva cadastrada.");
            return;
        }

        for (int i = 0; i < _reservas.Count; i++)
        {
            ExibirDetalhesReserva(_reservas[i], i + 1);
        }
    }

    private void ExibirListaHospedes()
    {
        Console.WriteLine("\nHóspedes disponíveis:");
        for (int i = 0; i < _hospedes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_hospedes[i].NomeCompleto}");
        }
    }

    private void ExibirListaSuites()
    {
        Console.WriteLine("\nSuítes disponíveis:");
        for (int i = 0; i < _suites.Count; i++)
        {
            var suite = _suites[i];
            Console.WriteLine($"{i + 1}. {suite.TipoSuite} - Capacidade: {suite.Capacidade} - R$ {suite.ValorDiaria:F2}");
        }
    }

    private void ExibirListaReservas()
    {
        Console.WriteLine("Reservas disponíveis:");
        for (int i = 0; i < _reservas.Count; i++)
        {
            var reserva = _reservas[i];
            Console.WriteLine($"{i + 1}. Suíte {reserva.Suite.TipoSuite} - {reserva.DiasReservados} dias");
        }
    }

    private void ExibirDetalhesReserva(Reserva reserva, int? numeroReserva = null)
    {
        var prefixo = numeroReserva.HasValue ? $"Reserva {numeroReserva}:" : "Detalhes da Reserva:";
        
        Console.WriteLine($"\n{prefixo}");
        Console.WriteLine($"Suíte: {reserva.Suite.TipoSuite}");
        Console.WriteLine($"Hóspedes: {reserva.ObterQuantidadeHospedes()}");
        Console.WriteLine($"Dias: {reserva.DiasReservados}");
        Console.WriteLine($"Valor: R$ {reserva.CalcularValorDiaria():F2}");
        
        if (reserva.DiasReservados >= 10)
        {
            Console.WriteLine("✓ Desconto de 10% aplicado!");
        }
    }

    // ==================== MÉTODOS AUXILIARES ====================
    private bool ValidarPreRequisitosReserva()
    {
        if (_hospedes.Count == 0)
        {
            ExibirMensagemErro("Não há hóspedes cadastrados!");
            return false;
        }

        if (_suites.Count == 0)
        {
            ExibirMensagemErro("Não há suítes cadastradas!");
            return false;
        }

        return true;
    }

    private List<Pessoa> ObterHospedesPorIndices(List<int> indices)
    {
        var hospedes = new List<Pessoa>();
        foreach (var indice in indices)
        {
            hospedes.Add(_hospedes[indice - 1]);
        }
        return hospedes;
    }

    private List<int> ParseIndices(string entrada, int maximo)
    {
        var indices = new List<int>();
        var partes = entrada.Split(',');
        
        foreach (var parte in partes)
        {
            if (int.TryParse(parte.Trim(), out int indice) && indice > 0 && indice <= maximo)
            {
                indices.Add(indice);
            }
        }
        
        return indices;
    }

    private void FinalizarSistema()
    {
        _executando = false;
        Console.WriteLine("Saindo do sistema...");
    }

    private void AguardarContinuacao()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private void ExibirTituloSecao(string titulo)
    {
        Console.WriteLine($"=== {titulo} ===");
    }

    private void ExibirMensagemSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ {mensagem}");
        Console.ResetColor();
    }

    private void ExibirMensagemErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n✗ {mensagem}");
        Console.ResetColor();
    }

    private void ExibirMensagemInfo(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nℹ {mensagem}");
        Console.ResetColor();
    }
}

public record DadosHospede(string Nome, string Sobrenome)
{
    public string NomeCompleto => $"{Nome} {Sobrenome}".Trim().ToUpper();
}

public record DadosSuite(string TipoSuite, int Capacidade, decimal ValorDiaria);

public record DadosReserva(List<int> IndicesHospedes, int IndiceSuite, int DiasReservados);