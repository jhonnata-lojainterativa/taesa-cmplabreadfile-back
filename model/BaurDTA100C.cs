using readfile.model;

public class BaurDTA100C
{
    public string? Descricao { get; set; }
    public string? TipoMedicao { get; set; }
    public string? NumeroSerieEquipamento { get; set; }
    public string? DataRealizacao { get; set; }
    public string? TestadoPor { get; set; }
    public InformacaoMedicao? InformacaoMedicao { get; set; } = new InformacaoMedicao();
    public ValoresMedicao? ValoresMedidos { get; set; } = new ValoresMedicao();
    public bool? StatusAnalise { get; set; }
    public string? StatusAnaliseDescricao { get; set; }

    public void SetValueFromTxt(string baurFilePath)
    {
        var allLines = File.ReadAllLines(baurFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        var whiteListDictonary = new Dictionary<string, string>()
        {
            {"Nome do protocolo",          "InformacaoMedicao.NomeProtocolo"},
            {"Número de amostra",          "InformacaoMedicao.NumeroAmostra"},
            {"Medição normatizada",        "InformacaoMedicao.MedicaoNormatizada"},
            {"Forma de eletrodo",          "InformacaoMedicao.FormaEletrodo"},
            {"Distância dos eletrodos",    "InformacaoMedicao.DistanciaEletrodo"},
            {"Freqüência de teste",        "InformacaoMedicao.FrequenciaTeste"},
            {"Temperatura",                "ValoresMedidos.Temperatura"},
            {"Teste 1",                    "ValoresMedidos.Testes.Teste1"},
            {"Teste 2",                    "ValoresMedidos.Testes.Teste2"},
            {"Teste 3",                    "ValoresMedidos.Testes.Teste3"},
            {"Teste 4",                    "ValoresMedidos.Testes.Teste4"},
            {"Teste 5",                    "ValoresMedidos.Testes.Teste5"},
            {"Teste 6",                    "ValoresMedidos.Testes.Teste6"},
            {"Valor médio",                "ValoresMedidos.ValorMedio"},
            {"Desvio padrão",              "ValoresMedidos.DesvioPadrao"},
            {"Desvio padrão/Valor médio",  "ValoresMedidos.ValorMedioDesvioPadrao"},
            {"Teste efetuado por",         "TestadoPor"},
        };
        
        var exceptionListDictonary = new Dictionary<string, string>()
        {
            {"Teste finalizado com sucesso!", "StatusAnalise"},
            // {"VAC/mm", "ValoresMedidos.Tensao"},
        };

        // Insere os valores do inicio do arquivo
        Descricao = allLines[1];
        TipoMedicao = allLines[2];
        NumeroSerieEquipamento = allLines[3].Split(":").Last().Trim();
        DataRealizacao = allLines[4];

        // VERIFICAR SE TODOS OS ARQUIVO DO TIPO DTA100C VEM SEM STATUS
        // StatusAnalise = true;
        // StatusAnaliseDescricao = ""

        // Começa a leitura linha a linha do arquivo
        for (var i = 0; i < allLines.Length; i++)
        {
            var line = allLines[i];

            if (whiteListDictonary.ContainsKey(line.Trim().Split(":").First()))
            {
                // Busca uma palavra chave que corresponda ao dicionário WHITELIST
                var atributeReflection = whiteListDictonary
                    .Where(item => item.Key == line.Trim().Split(":").First()) 
                    .Select(item => item.Value).First();
            
                // Obtém o valor que será setado no atributo
                var valueLine = line.Split(":", 2).Last().Trim(); 
                    
                // Seta os valors no objeto 
                this.SetValueObjectReflection(atributeReflection, valueLine);
            }
            else if (exceptionListDictonary.ContainsKey(line.Trim()))
            {
                #region excessões
            
                // Busca uma palavra chave que corresponda ao dicionário EXCEPTION
                var exceptionReflection = exceptionListDictonary.Where(x => x.Key == line.Trim()).First();

                // Seta o status da análise de óleo
                if (exceptionReflection.Key.Equals("Teste finalizado com sucesso!"))
                    StatusAnalise = true;
                    StatusAnaliseDescricao = line.Trim();
                    
                // Condicional para juntar o valor restante do atributo "Tensão de Teste" 
                // if (exceptionReflection.Key.Equals("VAC/mm"))
                //     ValoresMedidos.Tensao += $" - {line.Trim()}";

                #endregion
            }
        }
    }
}

public class InformacaoMedicao {
    public string? NomeProtocolo { get; set; }
    public string? NumeroAmostra { get; set; }  
    public string? MedicaoNormatizada { get; set; }  
    public string? FormaEletrodo { get; set; }  
    public string? DistanciaEletrodo { get; set; }  
    public string? FrequenciaTeste { get; set; }  
}

public class ValoresMedicao {

    public string? Temperatura { get; set; }
    public TestesMedicao? Testes { get; set; } = new TestesMedicao();
    public string? ValorMedio { get; set; }   
    public string? DesvioPadrao { get; set; }   
    public string? ValorMedioDesvioPadrao { get; set; }   
  
}

public class TestesMedicao {
    public string? Teste1 { get; set; } 
    public string? Teste2 { get; set; } 
    public string? Teste3 { get; set; } 
    public string? Teste4 { get; set; } 
    public string? Teste5 { get; set; } 
    public string? Teste6 { get; set; } 
}