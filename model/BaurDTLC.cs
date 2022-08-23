using readfile.model;

public class BaurDTLC
{
    public string? Descricao { get; set; }
    public string? TipoMedicao { get; set; }
    public string? NumeroSerieEquipamento { get; set; }
    public string? DataRealizacao { get; set; }
    public string? TestadoPor { get; set; } 
    public InformacaoMedicaoDTLC? InformacaoMedicao { get; set; } = new InformacaoMedicaoDTLC();
    public ValoresMedicaoDTLC? ValoresMedidos { get; set; } = new ValoresMedicaoDTLC();
    public string? UmiRelTempAmbiente { get; set; } 
    public string? TipoLiquidoIsolante { get; set; }
    public bool? StatusAnalise { get; set; }
    public string? StatusAnaliseDescricao { get; set; }

    public void SetValueFromTxt(string baurFilePath) {
        
        string[] allLines = File.ReadAllLines(baurFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        
        var whiteListDictonary = new Dictionary<string, string>()
        {
            {"Nome do protocolo",                             "InformacaoMedicao.NomeProtocolo"},
            {"Número de amostra",                             "InformacaoMedicao.NumeroAmostra"},
            {"Medição normatizada",                           "InformacaoMedicao.MedicaoNormatizada"},
            {"Designação de célula",                          "InformacaoMedicao.CelulaTeste.DesignacaoCelula"},
            {"Tipo",                                          "InformacaoMedicao.CelulaTeste.Tipo"},
            {"Número de série",                               "InformacaoMedicao.CelulaTeste.NumeroSerie"},
            {"Última calibração",                             "InformacaoMedicao.UltimaCalibracao"},
            {"Épsilon",                                       "ValoresMedidos.Epsilon"},
            {"Tensão de teste",                               "ValoresMedidos.Tensao"},
            {"Freqüência de teste",                           "ValoresMedidos.Frequencia"},
            {"Temperatura de teste",                          "ValoresMedidos.Temperatura"},
            {"Umidade relativa do ar / Temperatura ambiente", "UmiRelTempAmbiente"},
            {"Tipo do líquido isolante",                      "TipoLiquidoIsolante"},
            {"Teste efetuado por",                            "TestadoPor"},
            {"a 50 Hz",                                       "ValoresMedidos.TanDelta.A50Hz"},
            {"a 60 Hz",                                       "ValoresMedidos.TanDelta.A60Hz"},
        };

        var exceptionListDictonary = new Dictionary<string, string>()
        {
            {"Teste finalizado com sucesso!", "StatusAnalise"},
            {"VAC/mm", "ValoresMedidos.Tensao"},
        };

        // Insere os valores default
        Descricao = allLines[1];
        TipoMedicao = allLines[2];
        NumeroSerieEquipamento = allLines[3].Split(":").Last().Trim();
        DataRealizacao = allLines[4];
        StatusAnalise = false;
        
        // Começa a leitura linha a linha do arquivo
        for(var i = 0; i < allLines.Length; i++) 
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
                    if (exceptionReflection.Key.Equals("VAC/mm"))
                        ValoresMedidos.Tensao += $" - {line.Trim()}";

                #endregion
            }

            
        }
    }
}

public class InformacaoMedicaoDTLC {
    public string? NomeProtocolo { get; set; }
    public string? NumeroAmostra { get; set; }  
    public string? MedicaoNormatizada { get; set; }
    public CelulaTeste? CelulaTeste { get; set; } = new CelulaTeste();
    public string? UltimaCalibracao { get; set; } 
}

public class ValoresMedicaoDTLC {
    public string? Epsilon { get; set; }    
    public string? Tensao { get; set; }   
    public string? Frequencia { get; set; }   
    public string? Temperatura { get; set; }
    public TanDelta? TanDelta { get; set; } = new TanDelta();
}

public class TanDelta {
    public string? A50Hz { get; set; }
    public string? A60Hz { get; set; }
}

public class CelulaTeste {
    public string? DesignacaoCelula { get; set; }
    public string? Tipo { get; set; }
    public string? NumeroSerie { get; set; }
}