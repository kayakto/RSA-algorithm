using System;

namespace AlgorithmLab2
{
    public class FileHandler
    {
        // Читает текста из файла и получает массив строк, представляющих отдельные слова
        public static string[] ReadTextFromFile(string filePath)
        {
            return File.ReadAllText(filePath).Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        // Записывает строки текста в файл
        public static void WriteTextToFile(string text, string filePath)
        {
            File.WriteAllText(filePath, text);
        }

        // Читает текст из файла, преобразует каждую строку в объект BigInt и получает список этих объектов
        public static List<BigInt> ReadBigIntsFromFile(string filePath)
        {
            var bigIntStrings = File.ReadAllText(filePath).Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return bigIntStrings.Select(bigIntStr => new BigInt(bigIntStr)).ToList();
        }

        // Разбивает исходного текст на слова
        public static string[] SplitTextIntoWords(string text)
        {
            return text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
