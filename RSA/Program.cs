using AlgorithmLab2;

class Program
{
    static void Main(string[] args)
    {
        RSA rsa = new RSA(64); 
        string inputFilePath = "input.txt";
        string encryptedFilePath = "encrypted_file.txt";
        string decryptedFilePath = "decrypted_file.txt";

        // Чтение исходного текста из файла
        string[] words = FileHandler.ReadTextFromFile(inputFilePath);
        Console.WriteLine("Original Text:");
        Console.WriteLine(string.Join(" ", words));

        // Шифрование текста
        List<BigInt> encryptedWords = rsa.EncryptWords(words);
        string encryptedText = string.Join(" ", encryptedWords.Select(x => x.ToString()));
        FileHandler.WriteTextToFile(encryptedText, encryptedFilePath);
        Console.WriteLine("Encrypted Text:");
        Console.WriteLine(encryptedText);

        // Чтение зашифрованного текста из файла
        List<BigInt> encryptedWordList = FileHandler.ReadBigIntsFromFile(encryptedFilePath);

        // Расшифровка текста
        string decryptedText = rsa.DecryptWords(encryptedWordList);
        FileHandler.WriteTextToFile(decryptedText, decryptedFilePath);
        Console.WriteLine("Decrypted Text:");
        Console.WriteLine(decryptedText);
        Console.Read();
    }
}