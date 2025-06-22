using LogStandardizer.Core;
using LogStandardizer.Core.Models;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string rootPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
        string inputPath = args.Length > 0 ? args[0] : Path.Combine(rootPath, "input.txt");
        string outputPath = args.Length > 1 ? args[1] : Path.Combine(rootPath, "output.txt");
        string problemsPath = args.Length > 2 ? args[2] : Path.Combine(rootPath, "problems.txt");

        Console.WriteLine($"Входной файл: {inputPath}");
        Console.WriteLine($"Выходной файл: {outputPath}");
        Console.WriteLine($"Файл с ошибками: {problemsPath}");

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не найден: " + inputPath);
            return;
        }

        try
        {
            var processor = new LogProcessor();
            processor.ProcessLogs(inputPath, outputPath, problemsPath);
            Console.WriteLine("Обработка завершена успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка при обработке логов:");
            Console.WriteLine(ex.Message);
        }
    }
}
