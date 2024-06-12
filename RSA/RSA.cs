using System;

namespace AlgorithmLab2
{
    public class RSA
    {
        public BigInt PublicKey { get; private set; }
        public BigInt PrivateKey { get; private set; }
        public BigInt Modulus { get; private set; }

        public RSA(int bitLength)
        {
            GenerateKeys(bitLength);
        }

        private void GenerateKeys(int bitLength)
        {
            Random rand = new Random();
            // Генерируем два случайных больших простых числа p и q длиной bitLength / 2 битов
            BigInt p = BigInt.GeneratePrime(bitLength / 2);
            BigInt q = BigInt.GeneratePrime(bitLength / 2);
            // Вычисляем Modulus как произведение p и q
            Modulus = p * q;
            // вычисляем функцию Эйлера для закрытого ключа
            BigInt phi = (p - 1) * (q - 1);

            PublicKey = new BigInt("65537"); // типичное значение для e
            // выбираем открытый ключ до того момента, пока он не станет взаимно простым с phi
            while (BigInt.GCD(PublicKey, phi) != new BigInt("1"))
            {
                PublicKey = BigInt.Random(bitLength / 4, rand);
            }
            // вычисление приватного ключа, который равен публичному ключу, обратному по модулю phi
            PrivateKey = PublicKey.ModInverse(phi);
        }

        // Шифрует сообщение с использованием публичного ключа и модуля
        public BigInt Encrypt(BigInt message)
        {
            // Вычисляем message^PublicKey % Modulus с помощью быстрого возведения в степень по модулю
            return BigInt.ModPow(message, PublicKey, Modulus);
        }

        // Дешифрует шифротекст с использованием приватного ключа и модуля
        public BigInt Decrypt(BigInt ciphertext)
        {
            // Вычисляет ciphertext^PrivateKey % Modulus с помощью быстрого возведения в степень по модулю
            return BigInt.ModPow(ciphertext, PrivateKey, Modulus);
        }
        
        // щифрует слова в список чисел BigInt
        public List<BigInt> EncryptWords(string[] words)
        {
            return words.Select(word => BigInt.FromString(word)) 
                .Select(bigInt => Encrypt(bigInt))
                .ToList();
        }

        // расшифровывает список чисел в строку
        public string DecryptWords(List<BigInt> encryptedWords)
        {
            return string.Join(" ", encryptedWords.Select(word => Decrypt(word))
                .Select(decryptedBigInt => decryptedBigInt.ToStringValue()));
        }
    }
}
  

