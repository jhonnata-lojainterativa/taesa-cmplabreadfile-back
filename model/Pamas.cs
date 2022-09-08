public class Pamas
{
    public string? ProfileName { get; set; }
    public string? MeasVolume { get; set; }
    public string? SampleID { get; set; }
    public string? ServiceHours { get; set; }
    public string? OilService { get; set; }
    public string? Comment { get; set; }
    public AnalyseResult FirstAnalyseResult { get; set; } = new AnalyseResult();
    public AnalyseResult SecoundAnalyseResult { get; set; } = new AnalyseResult();
    public AnalyseResult AverageAnalyseResult { get; set; } = new AnalyseResult();
    
    public void SetValueFromTxt(string pamasFilePath)
    {
        string[] allLines = File.ReadAllLines(pamasFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        
        // Obtém apenas o último bloco de relatório da análise
        string[] allLinesSlice = allLines.Skip((allLines.Length - 11)).Take(11).ToArray();


        for (int i = 0; i < 6; i++)
        {
            string[] lineSplit = allLinesSlice[i].Replace("\t", "").Split(":");

            string anchor = lineSplit.First().Trim();
            string anchorValue = lineSplit.Last().Trim();

            if (anchor.Contains("ProfileName"))
                ProfileName = anchorValue;

            if (anchor.Contains("MeasVolume"))
                MeasVolume = anchorValue;

            if (anchor.Contains("SampleID"))
                SampleID = anchorValue;

            if (anchor.Contains("ServiceHours"))
                ServiceHours = anchorValue;

            if (anchor.Contains("OilService"))
                OilService = anchorValue;

            if (anchor.Contains("Comment"))
                Comment = anchorValue;
        }

        int counter = 1;
        for (int i = (allLinesSlice.Length - 2); i < allLinesSlice.Length; i++)
        {
            string[] lineSplit = allLinesSlice[i].Split("->")[0].Split("\t");

            string[] valueSlice = lineSplit.Skip(2).Take((lineSplit.Length - 3)).ToArray();

            var valuesResult = new ValuesResult();

            if (counter == 1) 
            {
                FirstAnalyseResult.Date = lineSplit[0].Trim();
                FirstAnalyseResult.Time = lineSplit[1].Trim();
            }
            else if (counter == 2) 
            {
                SecoundAnalyseResult.Date = lineSplit[0].Trim();
                SecoundAnalyseResult.Time = lineSplit[1].Trim();
            }

            for (int indiceValue = 0; indiceValue < valueSlice.Length; indiceValue++)
            {
                string valueLine = valueSlice[indiceValue].Trim();

                if (counter == 1)
                {
                    SetValuesResultFromIndex(FirstAnalyseResult.ValuesResult, indiceValue, valueLine);
                }
                else if (counter == 2)
                {
                    SetValuesResultFromIndex(SecoundAnalyseResult.ValuesResult, indiceValue, valueLine);
                }
            }

            counter++;
        }
        
    }

    private void SetValuesResultFromIndex(ValuesResult valuesResult, int index, string valueLine)
    {
        switch (index)
        {
            case 0:
                valuesResult.V4 = valueLine;
                break;
            case 1:
                valuesResult.V6 = valueLine;
                break;
            case 2:
                valuesResult.V10 = valueLine;
                break;
            case 3:
                valuesResult.V14 = valueLine;
                break;
            case 4:
                valuesResult.V21 = valueLine;
                break;
            case 5:
                valuesResult.V25 = valueLine;
                break;
            case 6:
                valuesResult.V38 = valueLine;
                break;
            case 7:
                valuesResult.V70 = valueLine;
                break;
        }
    }
    
}

public class AnalyseResult {
    public string? Date { get; set; }
    public string? Time { get; set; }
    public ValuesResult ValuesResult { get; set; } = new ValuesResult();
}

public class ValuesResult {
    public string? V4 { get; set; }
    public string? V6 { get; set; }
    public string? V10 { get; set; }
    public string? V14 { get; set; }
    public string? V21 { get; set; }
    public string? V25 { get; set; }
    public string? V38 { get; set; }
    public string? V70 { get; set; }
}