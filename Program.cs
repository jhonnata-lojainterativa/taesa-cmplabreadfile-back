using System.Text.Json;

namespace readfile;

class Program
{
    private static string agilentFilePath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/agilent/1ZBR60470.csv";
    private static string baurDTA100CFilePath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/baur/DTA100C/98022.txt";
    
    private static string baurDTLCFileRoutinePath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/baur/DTLC/1196.22-routine.txt";
    private static string baurDTLCFileStandardPath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/baur/DTLC/11872201-standard.txt";
    private static string karlsFisherPath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/metrohm/Karl Fischer/PC_LIMS_Report-117120-20220829-162747.txt";
    private static string titrinoPlus887Path = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/metrohm/Titrino Plus 887/PC_LIMS_Report-94-20220830-150840.txt";
    private static string ecoTritratoPath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/metrohm/EcoTitrator/PC_LIMS_Report-teste1-20220830-134828.txt";
    private static string colorimetroPath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/colorimetro/log00012.rtf";
    private static string pamasPath = "C:/Users/jhonn/Desktop/CMP-Lab/readfile/files/pamas/ResultApos.txt";

    private static void Main(string[] args)
    {
        // ReadAgilentFile();
        // ReadBaurDTLCRoutine();
        // ReadBaurDTLCStandard();
        // ReadBaurDTA100C();
        // ReadKarlsFisher();
        // ReadTitrinoPlus887();
        // ReadEcoTritrator();
        ReadColorimetro();
        // ReadPamas();
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

    public static void ReadKarlsFisher()
    {
        var _karlFisher = new KarlsFisherFile();
        _karlFisher.SetValueFromTxt(karlsFisherPath);
        Console.WriteLine(JsonSerializer.Serialize(_karlFisher, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }

    public static void ReadTitrinoPlus887()
    {
        var _titrinoPlus887 = new TitrinoPlus887();
        _titrinoPlus887.SetValueFromTxt(titrinoPlus887Path);
        Console.WriteLine(JsonSerializer.Serialize(_titrinoPlus887, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }

    public static void ReadEcoTritrator()
    {
        var _ecoTritrator = new EcoTitrator();
        _ecoTritrator.SetValueFromTxt(ecoTritratoPath);
        Console.WriteLine(JsonSerializer.Serialize(_ecoTritrator, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }

    public static void ReadColorimetro()
    {
        var _colorimetro = new Colorimetro();
        _colorimetro.SetValueFromTxt(colorimetroPath);
        Console.WriteLine(JsonSerializer.Serialize(_colorimetro, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }

    public static void ReadPamas()
    {
        var _pamas = new Pamas();
        _pamas.SetValueFromTxt(pamasPath);
        Console.WriteLine(JsonSerializer.Serialize(_pamas, new JsonSerializerOptions(){WriteIndented = true}).ToString());
    }

}