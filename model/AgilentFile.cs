using System.Text.Json;

public class AgilentFileElement {
    public string? Name { get; set; }   
    public string? RtMin { get; set; }   
    public string? Area { get; set; }   
    public string? AmountPPM { get; set; }   
    public string? ConcentrationPPM { get; set; }   
}

public class AgilentFile
{
    public string? SampleName { get; set; }
    public string? SecondSampleName { get; set; }
    public string? Operator { get; set; }
    public string? DataFile { get; set; }
    public string? Instrument { get; set; }
    public string? InjectionDate { get; set; }
    public string? AcqMethod { get; set; }
    public string? Location { get; set; }
    public string? ProcessingMethod { get; set; }

    public List<AgilentFileElement>? AgilentFileElements { get; set; } = new List<AgilentFileElement>();
    
    public void SetValueFromCsv(string agilentFilePath)
    {
        string[] allLines = File.ReadAllLines(agilentFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        
        string[] whiteList = new[] {"H2", "CH4", "C2H2", "C2H4", "C2H6", "CO", "CO2", "O2", "N2"};
        
        // Looping do primeiro tipo de estrutura do arquivo CSV
        for(var i = 0; i < 6; i++)
        {
            string[] split;
            string line = allLines[i];

            // Primeira linha do CSV
            if (i == 0) 
            {
                split = line.Split(",");
                SampleName = split[1];
            }
            // Segunda linha do CSV
            else if (i == 1) 
            {
                split = line.Split(",");

                var operatorSplit = split[1].Split(",");
                var dataFileSplit = split[3].Split(",");

                Operator = operatorSplit[0];
                DataFile = dataFileSplit[0];

            }
            // Terceira linha do CSV
            else if (i == 2) 
            {
                split = line.Split(",");

                var instrumentSplit    = split[1].Split(",");
                var injectionDateSplit = split[3].Split(",");

                Instrument    = instrumentSplit[0];
                InjectionDate = injectionDateSplit[0];

            }
            // Quarta linha do CSV
            else if (i == 3) 
            {
                split = line.Split(",");

                var acqMethodSplit = split[1].Split(",");
                var locationSplit  = split[3].Split(",");

                AcqMethod = acqMethodSplit[0];
                Location  = locationSplit[0];
            }
            // Quinta linha do CSV
            else if (i == 4) 
            {
                var processingMethodSplit    = line.Split(",");
                ProcessingMethod = processingMethodSplit[1];
            }
            // Sexta linha do CSV
            else if (i == 5) 
            {
                var secondSampleNamesplit    = line.Split(",");
                SecondSampleName = secondSampleNamesplit[1];
            }
        }

        // Looping do segundo tipo de estrutura do arquivo CSV
        for(var i = 1; i < allLines.Length; i++) 
        {
            string[] agilentFileElementSplit;
            string line = allLines[i];
            
            agilentFileElementSplit = line.Split(",");

            if (!whiteList.Contains(agilentFileElementSplit[0].ToUpper()))
                continue;
            
            var agileFileElement = new AgilentFileElement()
            {
                Name = agilentFileElementSplit[0],
                RtMin = agilentFileElementSplit[1],
                Area = agilentFileElementSplit[2],
                AmountPPM = agilentFileElementSplit[3],
                ConcentrationPPM = agilentFileElementSplit[4],
            };

            AgilentFileElements.Add(agileFileElement);

        }
    }
    
}