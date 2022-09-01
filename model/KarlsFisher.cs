public class KarlsFisherFile
{
    public string? FirstID { get; set; }
    public string? SecoundID { get; set; }
    public string? SampleDataValue { get; set; }
    public string? SampleDataUnitValue { get; set; }
    public string? TypeAnalysesResult { get; set; }
    public string? AnalysesResult { get; set; }
    public string? AnalysesUnitResult { get; set; }
    
    public void SetValueFromTxt(string karlFisherFilePath)
    {
        string[] allLines = File.ReadAllLines(karlFisherFilePath)
            .Where(
                line => !string.IsNullOrWhiteSpace(line))
                .Select(
                    line => line.Replace("$S", "").Trim().Replace("$E", "").Trim()
            ).ToArray();
        
        // Looping do primeiro tipo de estrutura do arquivo CSV
        for(var i = 0; i < allLines.Length; i++)
        {
            string[] split;
            string line = allLines[i];

            // Estrutura de decisão para pegar os cabeçalho da análise
            if (line.Equals("Sample data V1")) 
            {
                split = allLines[i + 1].Split("\t");

                FirstID = split[0].Trim();
                SecoundID = split[1].Trim();
                SampleDataValue = split[2].Trim();
                SampleDataUnitValue = split[3].Trim();
            }

            // Estrutura de decisão para pegar os dados de análise
            if (line.Equals("Result 1C.R1 V1"))
            {
                split = allLines[i + 1].Split("\t");

                TypeAnalysesResult = split[0].Trim();
                AnalysesResult     = split[1].Trim();
                AnalysesUnitResult     = split[2].Trim();
                continue;
            }

        }
    }
    
}