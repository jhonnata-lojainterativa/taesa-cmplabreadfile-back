using System.Text.Json;

namespace readfile;

class Program
{
    private static string agilentFilePath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/agilent/1ZBR60470.csv";
    private static string baurDTA100CFilePath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/baur/DTA100C/98022.txt";
    
    private static string baurDTLCFileRoutinePath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/baur/DTLC/1196.22-routine.txt";
    private static string baurDTLCFileStandardPath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/baur/DTLC/11872201-standard.txt";

    private static void Main(string[] args)
    {
        // ReadAgilentFile();
        // ReadBaurDTLCRoutine();
        ReadBaurDTLCStandard();
        // ReadBaurDTA100C();
    }

    public static void ReadAgilentFile()
    {
        var _agilentFile = new AgilentFile();
        _agilentFile.SetValueFromCsv(agilentFilePath);
        Console.WriteLine(JsonSerializer.Serialize(_agilentFile, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }
    
    public static void ReadBaurDTLCRoutine()
    {
        var _baurDTLC = new BaurDTLC();
        _baurDTLC.SetValueFromTxt(baurDTLCFileRoutinePath);
        Console.WriteLine(JsonSerializer.Serialize(_baurDTLC, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }

    public static void ReadBaurDTLCStandard()
    {
        var _baurDTLC = new BaurDTLC();
        _baurDTLC.SetValueFromTxt(baurDTLCFileStandardPath);
        Console.WriteLine(JsonSerializer.Serialize(_baurDTLC, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }
    
    public static void ReadBaurDTA100C()
    {
        var _baurDta100C = new BaurDTA100C();
        _baurDta100C.SetValueFromTxt(baurDTA100CFilePath);
        Console.WriteLine(JsonSerializer.Serialize(_baurDta100C, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }
}