using LogStandardizer.Core.Parsers;

namespace LogStandardizer.Core.Models;

public class LogProcessor
{
    private readonly Format1Parser _format1Parser = new ();
    private readonly Format2Parser _format2Parser = new ();

    public void ProcessLogs(string inputFilePath, string outputFilePath, string problemsFilePath)
    {
        var outputLines = new List<string>();
        var problemLines = new List<string>();

        using var reader = new StreamReader(inputFilePath);
        string? line;

        var buffer = new List<string>();

        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (buffer.Count > 0)
                {
                    ProcessBuffer(buffer, outputLines, problemLines);
                    buffer.Clear();
                }
            }
            else
            {
                buffer.Add(line);
                if (IsFormat2Line(line))
                {
                    ProcessBuffer(buffer, outputLines, problemLines);
                    buffer.Clear();
                }
            }
        }

        // После окончания чтения файла, если остались данные в буфере — обработать их
        if (buffer.Count > 0)
        {
            ProcessBuffer(buffer, outputLines, problemLines);
            buffer.Clear();
        }


        // Обработать остаток буфера (если файл не заканчивается пустой строкой)
        if (buffer.Count > 0)
        {
            ProcessBuffer(buffer, outputLines, problemLines);
        }

        // Записать результаты в файлы
        File.WriteAllLines(outputFilePath, outputLines);
        if (problemLines.Count > 0)
            File.WriteAllLines(problemsFilePath, problemLines);
    }

    private void ProcessBuffer(List<string> buffer, List<string> outputLines, List<string> problemLines)
    {
        LogEntry? entry;
        if (buffer.Count == 1 && IsFormat2Line(buffer[0]))
        {
            // Формат 2
            entry = _format2Parser.TryParse([.. buffer]);
        }
        else
        {
            // Формат 1 (многострочный)
            entry = _format1Parser.TryParse([.. buffer]);
        }

        if (entry != null)
        {
            outputLines.Add(LogFormatter.Format(entry));
            outputLines.Add(string.Empty); // Добавляем пустую строку для разделения записей
        }
        else
        {
            // Некорректная запись — сохраняем в problems.txt в исходном виде
            problemLines.AddRange(buffer);
            problemLines.Add(string.Empty);
        }
    }

    private static bool IsFormat2Line(string line)
    {
        // Проверка формата 2
        return line.Contains('|');
    }
}
