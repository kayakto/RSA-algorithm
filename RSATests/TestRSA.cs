using AlgorithmLab2;

namespace TestProject1
{
    public class TestRsa
    {
        private RSA rsa;

        [SetUp]
        public void Setup()
        {
            rsa = new RSA(64); 
        }

        // проверяет, что метод шифрования и дешифрования RSA корректно работает для каждой строки из TestCase
        [TestCase("Hello, world!")]
        [TestCase("Big data and AI")]
        [TestCase("Check Test with longer text")]
        [TestCase("drop database")]
        public void EncryptAndDecryptWords_ShouldReturnOriginalWords(string originalText)
        {
            // Разбиение исходного текста на слова
            string[] words = FileHandler.SplitTextIntoWords(originalText);
            foreach (var word in words)
            {
                BigInt originalBigInt = BigInt.FromString(word);
                BigInt encryptedBigInt = rsa.Encrypt(originalBigInt);
                BigInt decryptedBigInt = rsa.Decrypt(encryptedBigInt);

                string decryptedWord = decryptedBigInt.ToStringValue();
                Assert.AreEqual(word, decryptedWord);
            }
        }

        // проверяет корректность генерации ключей RSA
        [Test]
        public void PublicKeyAndPrivateKey_ShouldBeValid()
        {
            Assert.AreNotEqual(rsa.PublicKey, rsa.PrivateKey);
            Assert.IsTrue(rsa.PublicKey > new BigInt("0"));
            Assert.IsTrue(rsa.PrivateKey > new BigInt("0"));
        }

        // проверяет, что метод шифрования и дешифрования RSA корректно работает для коротких строк
        [Test]
        public void EncryptAndDecrypt_WithEdgeCases()
        {
            string shortText = "Hi";
            BigInt shortBigInt = BigInt.FromString(shortText);
            BigInt shortEncrypted = rsa.Encrypt(shortBigInt);
            BigInt shortDecrypted = rsa.Decrypt(shortEncrypted);
            Assert.AreEqual(shortText, shortDecrypted.ToStringValue());
        }
    }
    
}
