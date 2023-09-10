using System.Text.Json;

namespace Gestor_de_Clientes;

class Cliente
{
    enum Opcao
    {
        Adicionar = 1,
        Remover,
        Historico,
        Sair
    }
    
    // Para salvar os dados em arquivos
    [Serializable] 
    struct Info
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
    }

    // Vincula a list ao struct Info
    static List<Info> infos = new();

    static void Main()
    {
        // Faz a leitura se já tem algum arquivo salvo
        LeituraDoArquivo();
        Console.Clear();

        Console.WriteLine("Menu\n");
        Console.WriteLine("1 - Adicionar\n2 - Remover\n3 - Histórico\n4 - Sair\n");
        Console.Write("Digite sua opção: ");
        int indice = int.Parse(Console.ReadLine()!);
        Opcao opcaoSelecionada = (Opcao)indice;

        switch (opcaoSelecionada)
        {
            case Opcao.Adicionar:
                AdicionarCliente();
                break;
            case Opcao.Remover:
                RemoverCliente();
                break;
            case Opcao.Historico:
                VerHistorico();
                break;
            case Opcao.Sair:
                Console.WriteLine("Fechando programa...");
                break;
            default:
                Console.WriteLine("Opção inválida!");
                Thread.Sleep(2000);
                Main();
                break;
        }
    }

    public static void AdicionarCliente()
    {
        Console.Clear();

        Info info = new();

        Console.WriteLine("Adicione o cliente\n");

        Console.Write("Nome: ");
        info.nome = Console.ReadLine()!;

        Console.Write("E-mail: ");
        info.email = Console.ReadLine()!;

        Console.Write($"Senha: ");
        info.senha = Console.ReadLine()!;

        // Salva os valores dentro da list
        infos.Add(info);
        SalvarNoArquivo();

        EscolhaUsuario();

        static void EscolhaUsuario()
        {
            Console.WriteLine("\nO que quer fazer agora?\n");
            Console.WriteLine("1 - Voltar ao menu\n2 - Adicionar outro registro\n");
            Console.Write("Digite sua opção: ");
            int escolha = int.Parse(Console.ReadLine()!);

            switch (escolha)
            {
                case 1:
                    Main();
                    break;
                case 2:
                    AdicionarCliente();
                    break;
                default:
                    Console.Write("\nOpção inválida!");
                    Thread.Sleep(2000);
                    Console.Clear();
                    EscolhaUsuario();
                    break;
            }
        }
    }

    public static void RemoverCliente()
    {
        Console.Clear();

        Console.WriteLine("Remover registro\n");

        Console.Write("Escreva o ID do usuário que deseja remover: ");

        /* A variável indice é inicializada com o valor 0, por isso,
        colocamos seu valor com -1 para não retornar nulo */
        int id = int.Parse(Console.ReadLine()!);

        // Verifica se ID é maior que 0 se o ID compreende o tamanho da lista
        if (id > 0 && id < infos.Count)
        {
            infos.RemoveAt(id);
            Console.WriteLine($"\nO ID {id} foi removido com sucesso!");
            SalvarNoArquivo();
        }
        else
        {
            Console.WriteLine("ID inválido!");
            Thread.Sleep(2000);
            RemoverCliente();
        }

        EscolhaUsuario();

        static void EscolhaUsuario()
        {
            Console.WriteLine("\nO que quer fazer agora?\n");
            Console.WriteLine("1 - Voltar ao menu\n2 - Remover outro registro\n");
            Console.Write("Digite sua opção: ");
            int escolha = int.Parse(Console.ReadLine()!);

            switch (escolha)
            {
                case 1:
                    Main();
                    break;
                case 2:
                    RemoverCliente();
                    break;
                default:
                    Console.Write("\nOpção inválida!");
                    Thread.Sleep(2000);
                    Console.Clear();
                    EscolhaUsuario();
                    break;
            }
        }
    }

    public static void VerHistorico()
    {
        Console.Clear();

        Console.WriteLine("Histórico de registros\n");
        int i = 1;

        // Verifica se tem pelo menos 1 cliente registrado
        if (infos.Count > 0)
        {
            // Verifica as informações na lista de dados
            foreach (Info info in infos)
            {
                Console.WriteLine($"ID: {i}");
                Console.WriteLine($"Nome: {info.nome}");
                Console.WriteLine($"E-mail: {info.email}");
                Console.WriteLine($"Senha: {info.senha}");
                Console.WriteLine();
                i++;
            }
        }
        else
        {
            Console.WriteLine("Não há nada registrado...");
        }

        EscolhaUsuario();

        static void EscolhaUsuario()
        {
            Console.WriteLine("O que quer fazer agora?\n");
            Console.WriteLine("1 - Voltar ao menu\n2 - Atualizar histórico\n");
            Console.Write("Digite sua opção: ");
            int escolha = int.Parse(Console.ReadLine()!);

            switch (escolha)
            {
                case 1:
                    Main();
                    break;
                case 2:
                    VerHistorico();
                    break;
                default:
                    Console.Write("\nOpção inválida!");
                    Thread.Sleep(2000);
                    Console.Clear();
                    EscolhaUsuario();
                    break;
            }
        }
    }

    public static void SalvarNoArquivo()
    {
        FileStream stream = new("dados.txt", FileMode.OpenOrCreate);

        // Torna o arquivo binário e salva os dados no arquivo
        JsonSerializer.Serialize(stream, infos);

        stream.Close();
    }

    public static void LeituraDoArquivo()
    {
        FileStream stream = new("dados.txt", FileMode.OpenOrCreate);

        // Tenta executar isso
        try
        {
            // Faz a leitura dos dados no arquivo
            infos = JsonSerializer.Deserialize<List<Info>>(stream)!;

            if (infos == null)
            {
                infos = new List<Info>();
            }
        }
        // Caso não funcione faça
        catch (Exception)
        {
            // Se qualquer erro acontecer cria uma nova list
            infos = new List<Info>();
        }

        // Fecha a stream quando o try ou catch for executado
        stream.Close();
    }
}
