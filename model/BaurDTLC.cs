using readfile.model;

public class BaurDTLC
{
    public string? Descricao { get; set; }
    public string? TipoMedicao { get; set; }
    public string? TipoMedicaoDescricao { get; set; }
    public string? NumeroSerieEquipamento { get; set; }
    public string? DataRealizacao { get; set; }
    public string? TestadoPor { get; set; } 
    public InformacaoMedicaoDTLC? InformacaoMedicao { get; set; } = new InformacaoMedicaoDTLC();
    public ValoresMedicaoDTLC? ValoresMedidos { get; set; } = new ValoresMedicaoDTLC();
    public List<ValoresMedicaoDTLC>? ValoresMedidosList { get; set; } = new List<ValoresMedicaoDTLC>();
    public ResultadoMedicao? ResultadoMedicao { get; set; } = new ResultadoMedicao();
    public string? UmiRelTempAmbiente { get; set; } 
    public string? TipoLiquidoIsolante { get; set; }
    public bool? StatusAnalise { get; set; }
    public string? StatusAnaliseDescricao { get; set; }
    public string? StatusAnaliseComentario { get; set; }

    public void SetValueFromTxt(string baurFilePath) {
        
        string[] allLines = File.ReadAllLines(baurFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        var typeBaurFile = allLines.First(line => (line.Contains("Routine") || line.Contains("Standard")));
        
        // Executado quando identifica que a estrutura do arquivo é do tipo ROUTINE
        if (typeBaurFile.Contains("Routine"))
            SetValueWhenIsRoutine(allLines);
        
        // Executado quando identifica que a estrutura do arquivo é do tipo STANDARD
        if (typeBaurFile.Contains("Standard"))
            SetValueWhenIsStandard(allLines);

    }

    private void SetValueWhenIsRoutine(string[] allLines)
    {
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
            // {"VAC/mm", "ValoresMedidos.Tensao"},
        };

        // Insere os valores default
        Descricao = allLines[1];
        TipoMedicao = "Routine";
        TipoMedicaoDescricao = allLines[2];
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
                    // if (exceptionReflection.Key.Equals("VAC/mm"))
                    //     ValoresMedidos.Tensao += $" - {line.Trim()}";

                #endregion
            }

            
        }
    }

    private void SetValueWhenIsStandard(string[] allLines)
    {
        var whiteListDictonary = new Dictionary<string, string>()
        {
            {"Nome do protocolo", "InformacaoMedicao.NomeProtocolo"},
            {"Número de amostra", "InformacaoMedicao.NumeroAmostra"},
            {"Medição normatizada", "InformacaoMedicao.MedicaoNormatizada"},
            {"Designação de célula", "InformacaoMedicao.CelulaTeste.DesignacaoCelula"},
            {"Tipo", "InformacaoMedicao.CelulaTeste.Tipo"},
            {"Número de série", "InformacaoMedicao.CelulaTeste.NumeroSerie"},
            {"Última calibração", "InformacaoMedicao.UltimaCalibracao"},
            {"Umidade relativa do ar / Temperatura ambiente", "UmiRelTempAmbiente"},
            {"Tipo do líquido isolante", "TipoLiquidoIsolante"},
            {"Teste efetuado por", "TestadoPor"},
        };

        var blocoEnchimento = 0;
        var valorMedidoTemp = new ValoresMedicaoDTLC();
        var exceptionListDictonary = new Dictionary<string, string>()
        {
            {"Enchimento 1", ""},
            {"Enchimento 2", ""},
            {"Resultado da medição", ""},
            {"Teste finalizado com sucesso!", "StatusAnalise"},
            {"Critério atendido conforme a norma!", "StatusAnaliseComentario"},
            {"VAC/mm", "ValoresMedidos.Tensao"},
            {"Épsilon", "Epsilon"},
            {"Tensão de teste", "Tensao"},
            {"Freqüência de teste", "Frequencia"},
            {"Temperatura de teste", "Temperatura"},
            {"a 50 Hz", "TanDelta.A50Hz"},                                 
            {"a 60 Hz", "TanDelta.A60Hz"}  
        };
        
        
        // Insere os valores default
        Descricao = allLines[1];
        TipoMedicao = "Standard";
        TipoMedicaoDescricao = allLines[2];
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
            else if (exceptionListDictonary.ContainsKey(line.Split(":", 2).First().Trim()))
            {
                #region excessões

                    var keyLine = line.Split(":", 2).First().Trim();
                    var valueLine = line.Split(":", 2).Last().Trim();
            
                    // Busca uma palavra chave que corresponda ao dicionário EXCEPTION
                    // var exceptionReflection = exceptionListDictonary.Where(x => x.Key == line.Trim()).First();
                    
                    // Busca uma palavra chave que corresponda ao dicionário EXCEPTION de Valores Medidos
                    // var valorMedidoReflection = exceptionWhiteListDictonary.Where(x => x.Key == keyLine);

                    // Seta o status da análise de óleo
                    if (keyLine.Equals("Teste finalizado com sucesso!"))
                    {
                        StatusAnalise = true;
                        StatusAnaliseDescricao = line.Trim();
                        continue;
                    }
                    else if (keyLine.Equals("Critério atendido conforme a norma!"))
                    {
                        StatusAnaliseComentario = valueLine;
                        continue;
                    }
                    
                    // Condicional para juntar o valor restante do atributo "Tensão de Teste" 
                    if (keyLine.Equals("VAC/mm"))
                    {
                        ValoresMedidos.Tensao += $" - {line.Trim()}";
                        continue;
                    }


                    if (keyLine.Equals("Enchimento 1"))
                    {
                        blocoEnchimento = 1;
                        continue;
                    }
                    else if (keyLine.Equals("Enchimento 2"))
                    {
                        blocoEnchimento = 2;
                        ValoresMedidosList?.Add(valorMedidoTemp);
                        valorMedidoTemp = new ValoresMedicaoDTLC();
                        continue;
                    }
                    else if (keyLine.Equals("Resultado da medição"))
                    {
                        blocoEnchimento = 3;
                        ValoresMedidosList?.Add(valorMedidoTemp);
                        continue;
                    }

                    ;

                    if (blocoEnchimento is 1 or 2 && exceptionListDictonary.ContainsKey(keyLine))
                    {
                        switch (keyLine)
                        {
                            case "Épsilon":
                                valorMedidoTemp.Epsilon = valueLine;
                                break;
                            case "Tensão de teste":
                                valorMedidoTemp.Tensao = valueLine;
                                break;
                            case "Freqüência de teste":
                                valorMedidoTemp.Frequencia = valueLine;
                                break;
                            case "Temperatura de teste":
                                valorMedidoTemp.Temperatura = valueLine;
                                break;
                            case "a 50 Hz":
                                valorMedidoTemp.TanDelta.A50Hz = valueLine;
                                break;
                            case "a 60 Hz":
                                valorMedidoTemp.TanDelta.A60Hz = valueLine;
                                break;
                        }
                        
                    }
                    else if (blocoEnchimento == 3)
                    {
                        switch (keyLine)
                        {
                            case "Épsilon":
                                ResultadoMedicao.Epsilon = valueLine;
                                break;
                            case "a 50 Hz":
                                ResultadoMedicao.TanDelta.A50Hz = valueLine;
                                break;
                            case "a 60 Hz":
                                ResultadoMedicao.TanDelta.A60Hz = valueLine;
                                break;
                        }
                        
                    }
                    

                    #endregion
            }

            
        }
    }
}

public class InformacaoMedicaoDTLC {
    public string? NomeProtocolo { get; set; }
    public string? NumeroAmostra { get; set; }  
    public string? MedicaoNormatizada { get; set; }
    public string? TipoMedicaoNormatizada { get; set; }
    public CelulaTeste? CelulaTeste { get; set; } = new CelulaTeste();
    public string? UltimaCalibracao { get; set; } 
}

public class ValoresMedicaoDTLC {
    // public string? Tipo { get; set; }    
    public string? Epsilon { get; set; }    
    public string? Tensao { get; set; }   
    public string? Frequencia { get; set; }   
    public string? Temperatura { get; set; }
    public TanDelta? TanDelta { get; set; } = new TanDelta();
}

public class ResultadoMedicao {
    public string? Epsilon { get; set; }
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