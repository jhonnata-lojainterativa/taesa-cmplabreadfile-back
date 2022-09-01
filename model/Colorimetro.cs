public class Colorimetro
{
    public string? SampleId { get; set; }
    public string? Observer { get; set; }
    public string? Illuminant { get; set; }
    public string? PathLength { get; set; }
    public string? DateAnalyse { get; set; }
    public string? TimeAnalyse { get; set; }
    public string? AnalysesResult { get; set; }
    
    public void SetValueFromTxt(string colorimetroFilePath)
    {
        string[] allLines = File.ReadAllLines(colorimetroFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        string[] whiteList = new[] {"1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", "1.10"};

        // Obtém os dados de data e hora da análise de forma estática
        var lineDateTimeAnalyse = allLines[6].Split("  ");
        DateAnalyse = lineDateTimeAnalyse[0].Trim();
        TimeAnalyse = lineDateTimeAnalyse[1].Replace("\\par", "").Trim();

        // Primeiro loop de obtenção dos dados de forma automática
        for(var i = 6; i < allLines.Length; i++)
        {
            string line = allLines[i];

            if (line.Contains("Sample ID"))
                SampleId = line.Split(":")[1].Replace("\\par", "").Trim();

            if (line.Contains("Observer"))
                Observer = line.Split(":")[1].Replace("\\par", "").Trim();

            if (line.Contains("Illuminant"))
                Illuminant = line.Split(":")[1].Replace("\\par", "").Trim();

            if (line.Contains("PathLength"))
                PathLength = line.Split(":")[1].Replace("\\par", "").Trim();

            if (whiteList.Contains(line.Split("\\tab")[0]))
                AnalysesResult = line.Split("\\tab")[0].Trim();

        }
    }
    
}