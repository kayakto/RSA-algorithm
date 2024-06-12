using NUnit.Framework;
using AlgorithmLab2;
namespace TestProject1
{
    public class Tests
    {
        // Проверяет сложение двух BigInt чисел
        [Test]
        public void TestAddition()
        {
            BigInt num1 = new BigInt("123456789");
            BigInt num2 = new BigInt("987654321");
            BigInt sum = num1 + num2;
            Assert.AreEqual(new BigInt("1111111110"), sum);
        }

        // Проверяет вычитание двух BigInt чисел
        [Test]
        public void TestSubtraction()
        {
            BigInt num1 = new BigInt("987654321");
            BigInt num2 = new BigInt("123456789");
            BigInt result = num1 - num2;
            Assert.AreEqual(new BigInt("864197532"), result);
        }

        // Проверяет умножение двух BigInt чисел
        [Test]
        public void TestMultiplication()
        {
            BigInt num1 = new BigInt("12345");
            BigInt num2 = new BigInt("6789");
            BigInt result = num1 * num2;
            Assert.AreEqual(new BigInt("83810205"), result);
        }

        // Проверяет деление одного BigInt числа на другое
        [Test]
        public void TestDivision()
        {
            BigInt num1 = new BigInt("123456");
            BigInt num2 = new BigInt("123");
            BigInt result = num1 / num2;
            Assert.AreEqual(new BigInt("1003"), result);
        }

        // Проверяет операцию остатка от деления
        [Test]
        public void TestModulus()
        {
            BigInt num1 = new BigInt("123456");
            BigInt num2 = new BigInt("123");
            BigInt result = num1 % num2;
            Assert.AreEqual(new BigInt("87"), result);
        }

        // Проверяет, что одно BigInt число меньше другого
        [Test]
        public void TestLessThan()
        {
            BigInt num1 = new BigInt("12345");
            BigInt num2 = new BigInt("67890");
            Assert.IsTrue(num1 < num2);
        }

        // Проверяет, что одно BigInt число больше другого
        [Test]
        public void TestGreaterThan()
        {
            BigInt num1 = new BigInt("67890");
            BigInt num2 = new BigInt("12345");
            Assert.IsTrue(num1 > num2);
        }

        // Проверяет равенство двух BigInt чисел
        [Test]
        public void TestEquality()
        {
            BigInt num1 = new BigInt("12345");
            BigInt num2 = new BigInt("12345");
            Assert.IsTrue(num1 == num2);
        }

        // Проверяет неравенство двух BigInt чисел
        [Test]
        public void TestInequality()
        {
            BigInt num1 = new BigInt("12345");
            BigInt num2 = new BigInt("54321");
            Assert.IsTrue(num1 != num2);
        }

        // Проверяет вычисление обратного по модулю числа
        [Test]
        public void TestModularInverse()
        {
            BigInt num = new BigInt("3");
            BigInt modulus = new BigInt("11");
            BigInt result = num.ModInverse(modulus);
            Assert.AreEqual(new BigInt("4"), result);  // 3 * 4 % 11 = 1
        }

        // проверяет, что метод ToStringValue корректно преобразует объект BigInt, созданный из строки, обратно в исходную строку
        [Test]
        public void ToStringValue_ShouldConvertBigIntToStringCorrectly()
        {
            string input = "hello";
            BigInt bigInt = BigInt.FromString(input);
            string output = bigInt.ToStringValue();

            Assert.AreEqual(input, output);
        }

        // проверяет, что метод FromString корректно обрабатывает пустую строку
        [Test]
        public void FromString_ShouldHandleEmptyString()
        {
            BigInt bigInt = BigInt.FromString("");
            Assert.AreEqual("0", bigInt.ToString(), "Empty string should be treated as zero.");
        }

        // проверяет, что метод ToStringValue возвращает пустую строку для BigInt, представляющего ноль
        [Test]
        public void ToStringValue_ShouldReturnEmptyStringForZero()
        {
            BigInt bigInt = new BigInt("0");
            Assert.AreEqual("", bigInt.ToStringValue(), "BigInt representing zero should return an empty string.");
        }
    }
}