using System.Text;

namespace AlgorithmLab2
{
    public class BigInt
    {
        private List<byte> digits; // целое число со знаком в виде массива однобайтовых элементов
        private bool negative; // флаг, отвечающий за знак числа
        
        // Конструктор, принимающий большое число в виде строки
        public BigInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "Value cannot be null or empty.");
            
            // Определяем знак числа
            negative = value[0] == '-';
            digits = new List<byte>();
            
            // Парсим строку в обратном порядке и сохраняем цифры в массиве
            for (int i = value.Length - 1; i >= (negative ? 1 : 0); --i)
            {
                if (value[i] < '0' || value[i] > '9')
                    throw new ArgumentException("Invalid character in the decimal string");

                digits.Add((byte)(value[i] - '0'));
            }

            Normalize();
        }

        // Инициализирует объект BigInt из массива байт.
        // Если байты заданы в порядке старшего байта первым (big-endian), массив байт разворачивается
        public BigInt(byte[] bytes, bool isBigEndian = false)
        {
            if (bytes == null || bytes.Length == 0) // Проверяем, что массив байт не пустой
                throw new ArgumentNullException(nameof(bytes), "Byte array cannot be null or empty.");

            digits = new List<byte>();
            if (isBigEndian)
                Array.Reverse(bytes);
            // добавляем каждый байт в список digits
            foreach (var b in bytes)
            {
                digits.Add(b);
            }
            Normalize(); // нормализуем число
        }
        
        // Конструктор, принимающий массив байт
        public BigInt(byte[] bytes) 
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException(nameof(bytes), "Byte array cannot be null or empty.");

            digits = new List<byte>();
            foreach (var b in bytes)
            {
                // Преобразование каждого байта в строку и парсинг цифр
                string byteString = b.ToString("D3"); // Ensuring 3-digit representation for each byte
                foreach (var ch in byteString)
                {
                    if (ch < '0' || ch > '9')
                        throw new ArgumentException("Invalid character in the decimal string");

                    digits.Add((byte)(ch - '0'));
                }
            }
            Normalize();
        }
        
        // Приватный конструктор, используемый внутренне для операций
        private BigInt(List<byte> digits, bool negative)
        {
            this.digits = new List<byte>(digits);
            this.negative = negative;
            Normalize();
        }
        
        // Метод для нормализации числа (удаление ведущих нулей)
        private void Normalize()
        {
            while (digits.Count > 1 && digits[digits.Count - 1] == 0)
                digits.RemoveAt(digits.Count - 1);
            if (digits.Count == 1 && digits[0] == 0)
                negative = false; // корректируем знак, если число равно нулю
        }
        

        // Деструктор
        ~BigInt()
        {
            digits.Clear(); // Очищаем список цифр при уничтожении объекта
        }
        
        // Преобразует объект BigInt в массив байт.
        // Если указан порядок старшего байта первым (big-endian), разворачивает массив байт перед возвратом
        public byte[] ToByteArray(bool isBigEndian = false)
        {
            byte[] byteArray = digits.ToArray();
            if (isBigEndian)
                Array.Reverse(byteArray);

            return byteArray;
        }

        // Преобразует строку в объект BigInt
        public static BigInt FromString(string input)
        {
            if (string.IsNullOrEmpty(input)) // Проверяем входную строку на null или пустоту
                throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");
        
            BigInt result = new BigInt("0");
            BigInt baseValue = new BigInt("256"); 
        
            // Используем кодировку UTF-8, чтобы каждый символ строки представить как число, которое добавляется в BigInt
            foreach (char c in input)
            {
        
                BigInt charValue = new BigInt(((int)c).ToString());
        
                result = result * baseValue + charValue;
            }
        
            return result;
        }
        
        // Преобразует объект BigInt обратно в строку
        public string ToStringValue()
        {
            if (digits.Count == 0) // Проверяем, что digits не пуст
                return string.Empty;  
            // Создаем временный объект BigInt, затем преобразуем его в массив байт
            BigInt temp = new BigInt(this.digits, this.negative); 
            List<byte> bytes = new List<byte>();
            BigInt baseValue = new BigInt("256");  
            BigInt zero = new BigInt("0"); 
        
            while (temp > zero)  
            {
                BigInt remainder = temp % baseValue;  
                bytes.Add(byte.Parse(remainder.ToString())); 
                temp /= baseValue;  
            }
        
            // Если BigInt отрицательный, устанавливаем старший бит последнего байта
            if (this.negative && bytes.Count > 0)
                bytes[bytes.Count - 1] |= 0x80;  
            // Разворачиваем массив байт и преобразуем его в строку с использованием UTF-8
            bytes.Reverse(); 
            return Encoding.UTF8.GetString(bytes.ToArray()); 
        }
        
        // проверяет, равны ли два объекта BigInt
        public override bool Equals(object obj)
        {
            if (obj is BigInt b)
            {
                if (negative != b.negative || digits.Count != b.digits.Count)
                    return false; // если у чисел разный знак или разное количество значащих цифр
                
                // сравниваем каждый элемент списка цифр, начиная со старшего разряда
                for (int i = 0; i < digits.Count; ++i)
                    if (digits[i] != b.digits[i])
                        return false;
                // если все проверки прошли, то числа равны
                return true;
            }
            return false;
        }

        // вычисляет хэш-код для объекта BigInt
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (byte digit in digits)
                    hash = hash * 31 + digit;
                hash = hash * 31 + negative.GetHashCode();
                return hash;
            }
        }
        
        // перегрузка оператора равенства
        public static bool operator ==(BigInt a, BigInt b)
        {
            return a.Equals(b);
        }

        // перегрузка оператора неравенства
        public static bool operator !=(BigInt a, BigInt b)
        {
            return !a.Equals(b);
        }

        // перегрузка оператора сложения: складывает два объекта BigInt
        public static BigInt operator +(BigInt a, BigInt b)
        {
            if (a.negative == b.negative) // если числа имеют одинаковый знак
            {
                List<byte> result = new List<byte>();
                int carry = 0, sum; // 
                for (int i = 0; i < Math.Max(a.digits.Count, b.digits.Count) || carry > 0; ++i)
                {
                    sum = carry;
                    if (i < a.digits.Count) sum += a.digits[i];
                    if (i < b.digits.Count) sum += b.digits[i];
                    carry = sum / 10;
                    result.Add((byte)(sum % 10));
                }
                return new BigInt(result, a.negative);
            }
            else
            {
                if (a.negative) return b - (-a); // если  a - отрицательное, то вычитаем модуль a из b
                else return a - (-b); // если  b - отрицательное, то вычитаем модуль b из a
            }
        }

        //  вычитает одно число BigInt из другого
        public static BigInt operator -(BigInt a, BigInt b)
        {
            if (a == b) // Если числа равны, возвращаем 0
                return new BigInt("0");
            
            if (a.negative != b.negative)
                return a + (-b); // если знаки разные, то складываем a и b с другим знаком

            bool negResult = a < b; // Если первый операнд меньше второго, результат будет отрицательным
            if (negResult)
            {
                var temp = a;
                a = b;
                b = temp;
            }
            //
            List<byte> result = new List<byte>();
            int carry = 0;
            for (int i = 0; i < a.digits.Count; ++i)
            {
                int diff = a.digits[i] - carry - (i < b.digits.Count ? b.digits[i] : 0);
                carry = 0;
                if (diff < 0)
                {
                    diff += 10;
                    carry = 1;
                }
                result.Add((byte)diff);
            }

            return new BigInt(result, negResult);
        }

        // оператор унарного минуса
        public static BigInt operator -(BigInt a)
        {
            // возвращаем новое число BigInt, у которого знак противоположен исходному числу
            return new BigInt(a.digits, !a.negative);
        }

        // перегрузка оператора умножения
        public static BigInt operator *(BigInt a, BigInt b)
        {
            BigInt result = new BigInt("0");
            // инициализируем результат нулями
            result.digits = Enumerable.Repeat((byte)0, a.digits.Count + b.digits.Count).ToList();
            
            // проходим по каждому разряду чисел, суммируя произведения и добавляя переносы
            for (int i = 0; i < a.digits.Count; i++)
            {
                int carry = 0;
                for (int j = 0; j < b.digits.Count || carry > 0; j++)
                {
                    int current = result.digits[i + j] + carry + (j < b.digits.Count ? a.digits[i] * b.digits[j] : 0);
                    result.digits[i + j] = (byte)(current % 10);
                    carry = current / 10;
                }
            }
            
            // нормализуем результат и устанавливаем правильный знак
            result.negative = a.negative != b.negative;
            result.Normalize();
            return result;
        }

        // перегрузка оператора деления
        public static BigInt operator /(BigInt a, BigInt b)
        {
            if (b == new BigInt("0"))
                // Если делитель равен нулю, бросается исключение
                throw new DivideByZeroException("Cannot divide by zero."); 

            BigInt quotient = new BigInt("0"); // результат
            BigInt remainder = new BigInt("0"); // остаток
            // делим числа поразрядно, начиная с наиболее значимого разряда,
            // и накапливаем результат в quotient и остаток в remainder
            for (int i = a.digits.Count - 1; i >= 0; i--)
            {
                remainder *= new BigInt("10");
                remainder += new BigInt(a.digits[i].ToString());

                byte digit = 0;
                while (remainder >= b)
                {
                    remainder -= b;
                    digit++;
                }
                quotient.digits.Insert(0, digit);
            }
            quotient.negative = a.negative != b.negative;
            quotient.Normalize();
            return quotient;
        }
        
        // перегрузка оператора вычитания из BigInt числа a числа b типа int
        public static BigInt operator -(BigInt a, int b)
        {
            return a - new BigInt(b.ToString());
        }

        // перегрузка оператора вычитания из числа a типа int числа b типа BigInt
        public static BigInt operator -(int a, BigInt b)
        {
            return new BigInt(a.ToString()) - b;
        }

        // перегрузка оператора остатка от деления
        public static BigInt operator %(BigInt a, BigInt b)
        {
            // вычисляем частное, умножаем его на делитель и вычитаем из делимого
            return a - (a / b) * b; 
        }
        
        // перегрузка оператора a меньше b
        public static bool operator <(BigInt a, BigInt b)
        {
            // если первое число отрицательное - возвращаем истину
            if (a.negative && !b.negative) return true;
            // если второе число отрицательное - возвращаем ложь
            if (!a.negative && b.negative) return false;
            // если у чисел разное количество разрядов
            if (a.digits.Count != b.digits.Count)
                return a.digits.Count < b.digits.Count ? !a.negative : a.negative;
            
            // сравниваем разряды по порядку, если их одинаковое количество
            for (int i = a.digits.Count - 1; i >= 0; i--)
            {
                if (a.digits[i] != b.digits[i])
                    return a.digits[i] < b.digits[i] ? !a.negative : a.negative;
            }

            return false;
        }

        // перегрузка оператора a больше b
        public static bool operator >(BigInt a, BigInt b)
        {
            return b < a;
        }
        
        // перегрузка оператора a меньше или равно b
        public static bool operator <=(BigInt a, BigInt b)
        {
            return a == b || a < b;
        }

        // перегрузка оператора a больше или равно b
        public static bool operator >=(BigInt a, BigInt b)
        {
            return a == b || a > b;
        }

        // перегрузка оператора равенства большого числа a и числа b типа int
        public static bool operator ==(BigInt a, int b)
        {
            return a == new BigInt(b.ToString());
        }

        // перегрузка оператора неравенства большого числа a и числа b типа int
        public static bool operator !=(BigInt a, int b)
        {
            return !(a == b);
        }

        // перегрузка оператора большое число a меньше числа b типа int
        public static bool operator <(BigInt a, int b)
        {
            return a < new BigInt(b.ToString());
        }

        // перегрузка оператора большое число a больше числа b типа int
        public static bool operator >(BigInt a, int b)
        {
            return a > new BigInt(b.ToString());
        }

        // перегрузка оператора большое число a меньше или равно числа b типа int
        public static bool operator <=(BigInt a, int b)
        {
            return a <= new BigInt(b.ToString());
        }

        // перегрузка оператора большое число a больше или равно числа b типа int
        public static bool operator >=(BigInt a, int b)
        {
            return a >= new BigInt(b.ToString());
        }
        
        // Метод нахождения числа, обратного по модулю
        public BigInt ModInverse(BigInt modulus)
        {
            BigInt a = this, b = modulus;
            BigInt x0 = new BigInt("1"), x1 = new BigInt("0");
            BigInt q, temp;
            
            if (modulus == new BigInt("1"))
                return new BigInt("0");
            // реализуем обобщенный алгоритм Евклида для нахождения обратного по модулю числа
            while (a > new BigInt("1"))
            {
                q = a / b;
                temp = b;
                b = a % b;
                a = temp;

                temp = x1;
                x1 = x0 - q * x1;
                x0 = temp;
            }

            if (x0 < new BigInt("0"))
                x0 += modulus;

            return x0;
        }

        // проверяет, является ли число четным
        public bool IsEven()
        {
            // используем побитовую операцию & для проверки последнего бита младшего разряда
            return (digits[0] & 1) == 0;
        }
        
        // метод быстрого возведения в степень по модулю
        public static BigInt ModPow(BigInt baseValue, BigInt exponent, BigInt modulus)
        {
            BigInt result = new BigInt("1");
            BigInt b = baseValue % modulus;

            while (exponent > new BigInt("0"))
            {
                if (!exponent.IsEven())
                    result = (result * b) % modulus;

                exponent = exponent / new BigInt("2");
                b = (b * b) % modulus;
            }

            return result;
        }
        
        // Метод возведения в степень с использованием простого умножения в цикле
        public static BigInt Pow(BigInt a, BigInt b)
        {
            BigInt result = new BigInt("1");
            while (b > new BigInt("0"))
            {
                result *= a;
                b -= new BigInt("1");
            }
            return result;
        }
        
        // Метод генерации  случайного простого числа заданной длины в битах
        // рандом - полная хрень, нужен алгоритм для поиска больших чисел 
        public static BigInt GeneratePrime(int bitLength)
        {
            Random rand = new Random();
            BigInt candidate;

            do
            {
                candidate = BigInt.Random(bitLength, rand);
            } while (!IsPrime(candidate));

            return candidate;
        }
        
        //  проверяет, является ли число простым, используя метод пробного деления.
        // Он оптимизирован для быстрого отсеивания четных чисел и чисел, кратных 3, и затем проверяет делимость на числа вида 6k ± 1
        private static bool IsPrime(BigInt number)
        {
            if (number <= new BigInt("1")) return false;
            if (number <= new BigInt("3")) return true;

            if (number % new BigInt("2") == new BigInt("0") || number % new BigInt("3") == new BigInt("0"))
                return false;

            BigInt i = new BigInt("5");
            while (i * i <= number)
            {
                if (number % i == new BigInt("0") || number % (i + new BigInt("2")) == new BigInt("0"))
                    return false;
                i += new BigInt("6");
            }

            return true;
        }
        
        // Метод нахождения наибольшего общего делителя
        public static BigInt GCD(BigInt a, BigInt b)
        {
            // реализуем классический алгоритм Евклида для нахождения наибольшего
            // общего делителя двух чисел
            while (b != new BigInt("0"))
            {
                BigInt temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }


        // Метод генерации случайного большого числа
        public static BigInt Random(int bitLength, Random rand)
        {
            var bytes = new byte[(bitLength + 7) / 8];
            rand.NextBytes(bytes);
            return new BigInt(bytes);
        }
        
        // Парсинг числа в строку
        public override string ToString()
        {
            char[] charArray = digits.Select(d => (char)(d + '0')).ToArray();
            Array.Reverse(charArray);
            return (negative ? "-" : "") + new string(charArray);
        }
    }
}
