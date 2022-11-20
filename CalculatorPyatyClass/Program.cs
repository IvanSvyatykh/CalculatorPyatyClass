using System;
using System.Linq;
using System.Text;

namespace Calculator
{
    class Program
    {
        public static void Main()
        {

            while (true)
            {
                Console.WriteLine("Используйте ключевое слово для выбора операции которую хотите произвести.");
                Console.WriteLine("Список команд: FromRoman, ToRoman, number + number : (baze) ,number - number : (baze) ,number * number : (baze), Convert ... to ... (из десятичной в другую) , ToDec ... base ... ");
                Console.WriteLine("Используйте пробел между командой и числом");
                Console.ReadKey();
                Console.Clear();
                string str = Console.ReadLine();
                Console.Clear();
                char[] splitPoint = { ' ', ':' };
                str = NormalizationOfData(str);
                string[] splittedStr = str.Split(splitPoint);
                if(splittedStr.Length < 3)
                {
                    Console.WriteLine("Ошибка ввода");
                    Console.ReadKey();
                    Console.Clear();
                    Main();
                }
                try
                {
                    if (str.Contains("ToRoman")) ToRoman(int.Parse(splittedStr[1]));
                    else if (str.Contains("FromRoman")) FromRoman(splittedStr[1]);
                    else if (str.Contains("Convert")) DecToAny(int.Parse(splittedStr[1]), int.Parse(splittedStr[3]));
                    else if (str.Contains("ToDec")) FromAnyToDec(splittedStr[1], int.Parse(splittedStr[3]));
                    else if (str.Contains("+")) Sum(splittedStr[0], splittedStr[2], int.Parse(splittedStr[3]));
                    else if (str.Contains("-")) Subtraction(splittedStr[0], splittedStr[2], int.Parse(splittedStr[3]));
                    else if (str.Contains("*")) Multiplication(splittedStr[0], splittedStr[2], int.Parse(splittedStr[3]));
                    else Console.WriteLine("Введен не известный оператор, посмотрите список операторов еще раз");
                }
                catch
                {
                    Console.WriteLine("Ошибка ввода");
                    Main();
                }
               
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Напишите stop если хотите завершить программу");
                Console.ReadKey();
                Console.Clear();
                if (Console.ReadLine() == "stop") break;

            }
        }
        static string NormalizationOfData(string str)
        {
            StringBuilder result = new StringBuilder();
            char[] splitPoint = { ' ', ':' };
            string[] splittedStr = str.Split(splitPoint);
            for (int i = 0; i < splittedStr.Length; i++)
            {
                if (splittedStr[i] != "")
                {
                    result.Append(splittedStr[i]);
                    result.Append(" ");
                }
            }
           
            return result.ToString();
        }
        static string CreateZeroLine(int count)
        {
            string str = "";

            for (int i = 0; i < count; i++)
            {
                str += "0";
            }
            return str;
        }
        static void Multiplication(string number1, string number2, int baze)
        {
            if (number1.Length < number2.Length)
            {
                string s = number1;
                number1 = number2;
                number2 = s;
            }
            int[] mass1 = new int[number1.Length];
            int[] mass2 = new int[number2.Length];
            int[] mass3 = new int[Math.Max(mass1.Length, mass2.Length)];
            char[] result = new char[Math.Max(mass1.Length, mass2.Length)];
            string[] parts = new string[Math.Min(mass1.Length, mass2.Length)];
            for (int i = 0; i < number1.Length; i++)
            {
                mass1[i] = ConvertSymbolToNumber(number1[i], baze);
            }

            for (int i = 0; i < number2.Length; i++)
            {
                mass2[i] = ConvertSymbolToNumber(number2[i], baze);
            }

            int count = 0;
            int flag = 0;

            Console.WriteLine($"{number1}*{number2}");
            Console.WriteLine();
            Console.WriteLine("При умножении будем получать числа каждое из которых будет содержать на разряд больше чем предыдущее");
            Console.WriteLine();
            for (int i = 0; i < number2.Length; i++)
            {
                for (int j = 0; j < number1.Length; j++)
                {
                    mass3[mass3.Length - 1 - j] = mass1[mass1.Length - 1 - j] * mass2[mass2.Length - 1 - i];
                    Console.WriteLine($"Запишем в {mass3.Length - j} разряд числа результат {mass1[mass1.Length - 1 - j]}*{mass2[mass2.Length - 1 - i]} = {mass3[mass3.Length - 1 - j]}");
                }
                for (int j = mass3.Length - 1; j >= 0; j--)
                {
                    if (mass3[j] >= baze && j != 0)
                    {
                        Console.WriteLine($"На {j} позиции числа, число больше чем основание системы, тогда вычем из нее {baze * (mass3[j] / baze)} и прибавим на {j + 1} позицию {(mass3[j] / baze)} ");
                        mass3[j - 1] += (mass3[j] / baze);
                        mass3[j] -= baze * (mass3[j] / baze);

                    }
                    else if (mass3[j] >= baze && j == 0)
                    {
                        Console.WriteLine($"На {j} позиции числа, число больше чем основание системы, тогда вычем из нее {baze * (mass3[j] / baze)} и прибавим на {j + 1} позицию {(mass3[j] / baze)} ");
                        flag += (mass3[j] / baze);
                        mass3[j] -= baze * (mass3[j] / baze);
                    }
                }
                for (int j = 0; j < mass3.Length; j++)
                {
                    result[j] = ConvertToSymbol(mass3[j]);
                }
                if (count != 0 && flag != 0) parts[count] = flag.ToString() + new string(result) + CreateZeroLine(count);
                else if (count != 0 && flag == 0) parts[count] = new string(result) + CreateZeroLine(count);
                else if (count == 0 && flag != 0) parts[count] = flag.ToString() + new string(result);
                else if (count == 0 && flag == 0) parts[count] = new string(result);
                count++;
                flag = 0;

            }
            Console.WriteLine();
            Console.WriteLine($"В результате умнженияя получили следующие числа :");
            for (int i = 0; i < parts.Length; i++)
            {
                Console.WriteLine(parts[i]);
            }
            Console.WriteLine();
            Console.WriteLine("Сложим их поразярдно");
            string str = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                str = Sum(str, parts[i], baze);
            }
            Console.WriteLine();
            Console.WriteLine($"Итог: {str}");
        }
        static void Subtraction(string number1, string number2, int baze)
        {
            int[] mass1 = new int[number1.Length];
            int[] mass2 = new int[number2.Length];
            int[] mass3 = new int[Math.Max(mass1.Length, mass2.Length)];
            char[] result = new char[Math.Max(mass1.Length, mass2.Length)];

            for (int i = 0; i < number1.Length; i++)
            {
                mass1[i] = ConvertSymbolToNumber(number1[i], baze);
            }

            for (int i = 0; i < number2.Length; i++)
            {
                mass2[i] = ConvertSymbolToNumber(number2[i], baze);
            }

            Console.WriteLine($"{number1} - {number2}");

            if (number1.Length >= number2.Length)
            {
                for (int i = 0; i < number1.Length; i++)
                {
                    if (mass2.Length - 1 - i >= 0)
                    {
                        mass3[mass3.Length - 1 - i] = mass3[mass3.Length - 1 - i] + (mass1[mass1.Length - 1 - i] - mass2[mass2.Length - 1 - i]);
                        Console.WriteLine($"Разность {i + 1} элементов запишем в {i + 1} позицию нового числа, прибавляя число в этой позиции(может быть либо ноль либо -1 взависимсоти занимали ли мы до этого из предыдущи разрядов): {mass1[mass1.Length - 1 - i]} - {mass2[mass2.Length - 1 - i]} = {mass3[mass3.Length - 1 - i]} ");

                    }

                    if (mass2.Length - 1 - i < 0)
                    {
                        mass3[mass3.Length - 1 - i] += mass1[mass1.Length - 1 - i];
                        Console.WriteLine($"Так как на {i + 1} позиции числа {number2} стоят незначащие нули, то записываем просто занчение разряда на {i + 1} первого числа");
                    }

                    if (mass3[mass3.Length - 1 - i] < 0)
                    {
                        mass3[mass3.Length - 1 - i] += baze;
                        if (mass3.Length - i - 2 >= 0) mass3[mass3.Length - i - 2]--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < number2.Length; i++)
                {
                    if (mass1.Length - 1 - i >= 0) mass3[mass3.Length - 1 - i] = mass3[mass3.Length - 1 - i] + (mass2[mass2.Length - 1 - i] - mass1[mass1.Length - 1 - i]);
                    if (mass1.Length - 1 - i < 0) mass3[mass3.Length - 1 - i] += mass2[mass2.Length - 1 - i];

                    if (mass3[mass3.Length - 1 - i] < 0)
                    {
                        mass3[mass3.Length - 1 - i] += baze;
                        if (mass3.Length - i - 2 >= 0) mass3[mass3.Length - i - 2]--;
                    }
                }
            }

            for (int i = 0; i < mass3.Length; i++)
            {
                result[i] = ConvertToSymbol(mass3[i]);
            }

            Console.WriteLine($"Итог: {new string(result)}");
        }
        static string Sum(string number1, string number2, int baze)
        {
            int[] mass1 = new int[number1.Length];
            int[] mass2 = new int[number2.Length];
            int[] mass3 = new int[Math.Max(mass1.Length, mass2.Length)];
            char[] result = new char[Math.Max(mass1.Length, mass2.Length)];
            bool flag = true;
            int newDigit = 1;
            for (int i = 0; i < number1.Length; i++)
            {
                mass1[i] = ConvertSymbolToNumber(number1[i], baze);
            }

            for (int i = 0; i < number2.Length; i++)
            {
                mass2[i] = ConvertSymbolToNumber(number2[i], baze);
            }
            Console.WriteLine($"{number1} + {number2} ");
            for (int i = 0; i < Math.Max(mass1.Length, mass2.Length); i++)
            {
                if (mass1.Length - 1 - i >= 0 && mass2.Length - 1 - i >= 0)
                {
                    mass3[mass3.Length - 1 - i] = mass3[mass3.Length - 1 - i] + mass1[mass1.Length - 1 - i] + mass2[mass2.Length - 1 - i];
                    Console.WriteLine($"Складываем {ConvertToSymbol(mass1[mass1.Length - 1 - i])} + {ConvertToSymbol(mass2[mass2.Length - 1 - i])} и прибавляем число {mass3[mass3.Length - 1 - i] - mass1[mass1.Length - 1 - i] - mass2[mass2.Length - 1 - i]} лежащие в этом разряде появивщиеся из-за переносов");
                }
                if (mass1.Length - 1 - i < 0)
                {
                    mass3[mass3.Length - 1 - i] += mass2[mass2.Length - 1 - i];
                    Console.WriteLine($"записываем в {i} разряд занчение {ConvertToSymbol(mass2[mass2.Length - 1 - i])}, так как первое число имеет на этих позициях незначащие нули");
                }
                if (mass2.Length - 1 - i < 0)
                {
                    mass3[mass3.Length - 1 - i] += mass1[mass1.Length - 1 - i];
                    Console.WriteLine($"записываем в {i} разряд занчение {ConvertToSymbol(mass1[mass1.Length - 1 - i])}, так как второе число имеет на этих позициях незначащие нули");
                }
                if (mass3[mass3.Length - 1 - i] >= baze)
                {
                    Console.WriteLine($"Число {mass3[mass3.Length - 1 - i]} в позиции {i + 1} больше основания ситемы, то вычиатем из него {baze} и прибавляем на {i + 2} позицию еденицу");
                    mass3[mass3.Length - 1 - i] -= baze;
                    if (mass3.Length - i - 2 >= 0) mass3[mass3.Length - i - 2]++;
                    else flag = false;


                }

            }

            for (int i = 0; i < mass3.Length; i++)
            {
                result[i] = ConvertToSymbol(mass3[i]);
            }

            if (!flag)
            {
                Console.WriteLine($"Итог: {newDigit.ToString() + new string(result)}");
                return newDigit.ToString() + new string(result);
            }
            else
            {
                Console.WriteLine($"Итог: {new string(result)}");
                return new string(result);
            }
        }
        static int ConvertSymbolToNumber(char num, int baze)
        {
            int n = (int)num;



            if (n >= 48 && n <= 57) n = n - '0';
            if (n >= 65 && n <= 90) n = n - 'A' + 10;
            if (n >= 97 && n <= 122) n = n - 'a' + 36;

            if (n >= baze)
            {
                Console.WriteLine("Inncorrect format of number, digit is bigger than baze.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                Main();
            }
            return n;
        }
        static void FromAnyToDec(string number, int baze)
        {
            try
            {
                if (baze > 50) throw new ArgumentException("Base should be less than or equal 50");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Baze is more than 50 or equal 50");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                Main();
            }

            int result = 0;

            int num;

            for (int i = 0; i < number.Length; i++)
            {
                char c = number[i];

                if (c >= '0' && c <= '9')
                    num = c - '0';
                else if (c >= 'A' && c <= 'Z')
                    num = c - 'A' + 10;
                else if (c >= 'a' && c <= 'z')
                    num = c - 'a' + (('Z' - 'A') + 1) + 10;
                else throw new ArgumentException("Invalid number");

                if (num >= baze) throw new ArgumentException("The string contains illegal characters");

                Console.WriteLine($"Умножаем результат на основание ситемы: {baze},  прибавляем число: {num}, отвечающее {i + 1} элементу числа.");

                result *= baze;
                result += num;

                Console.WriteLine($"({result / baze} * {baze}) + {num} = {result}");
            }
            Console.WriteLine($"Число {number} в ситеме счисления {baze} равно {result}");
        }
        static void DecToAny(int number, int baze)
        {

            try
            {
                if (baze > 50) throw new ArgumentException("Base should be less than or equal 50");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Base should be less than or equal 50");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                Main();
            }
            StringBuilder builder = new StringBuilder();

            do
            {
                Console.WriteLine($"Делим с остатком {number} на {baze}, остаток приписываем к результату. ");

                int mod = number % baze;
                char c = ConvertToSymbol(mod);

                builder.Append(c);
                number /= baze;
            }
            while (number >= baze);

            if (number != 0)
            {
                builder.Append(ConvertToSymbol(number));
                Console.WriteLine($"Делим с остатком {number} на 10, остаток приписываем к результату.");
            }
            Console.WriteLine($"Полученное число {string.Join("", builder.ToString())} необходимо записать в обратном порядке.");
            Console.WriteLine($"Итог: {string.Join("", builder.ToString().Reverse())}");
        }
        static char ConvertToSymbol(int mod)
        {
            if (mod >= 0 && mod <= 9) return (char)('0' + mod);
            if (mod >= 10 && mod <= 36) return (char)('A' + mod - 10);
            if (mod >= 37 && mod <= 62) return (char)('a' + mod - 36);
            try
            {
                if (mod < 0 || mod > 62) throw new ArgumentException("Illegal statement");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Illegal statement");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                Main();
            }
            return '0';
        }
        static void ToRoman(int number)
        {
            int[] nums = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] romanNumber = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            try
            {
                if (number > 5000) throw new ArgumentException("Number should be less or equal 5000");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Number should be less or equal 5000");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                Main();
            }

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < nums.Length && number != 0; i++)
            {
                while (number >= nums[i])
                {
                    Console.WriteLine($"{number} больше чем {nums[i]} следует вычтем из первого втоотое и запишем римский вариант числа {nums[i]}=={romanNumber[i]}");
                    number -= nums[i];
                    result.Append(romanNumber[i]);
                }
            }
            Console.WriteLine($"Искомое чило в римской записи :" + result.ToString());
        }
        static void FromRoman(string number)
        {
            int result = 0;
            int[] nums = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] romanNumber = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            for (int i = 0; i < number.Length; i++)
            {
                try
                {
                    if (!romanNumber.Contains(number[i].ToString())) throw new ArgumentException("The string contains illegal characters");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("The string contains illegal characters");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    Main();
                }

                if (number[i] == 'C' && i + 1 < number.Length && number[i + 1] == 'D')
                {
                    Console.WriteLine("Символы СD вместе означают 400, прибавим к результату 400");
                    i++;
                    result += 400;
                }
                else if (number[i] == 'C' && i + 1 < number.Length && number[i + 1] == 'M')
                {
                    Console.WriteLine("Символы СM вместе означают 900, прибавим к результату 400");
                    i++;
                    result += 900;
                }
                else if (number[i] == 'X' && i + 1 < number.Length && number[i + 1] == 'L')
                {
                    Console.WriteLine("Символы XL вместе означают 400, прибавим к результату 40");
                    i++;
                    result += 40;
                }
                else if (number[i] == 'X' && i + 1 < number.Length && number[i + 1] == 'C')
                {
                    Console.WriteLine("Символы XC вместе означают 400, прибавим к результату 90");
                    i++;
                    result += 90;
                }
                else if (number[i] == 'I' && i + 1 < number.Length && number[i + 1] == 'X')
                {
                    Console.WriteLine("Символы IX вместе означают 400, прибавим к результату 9");
                    i++;
                    result += 9;
                }
                else if (number[i] == 'I' && i + 1 < number.Length && number[i + 1] == 'V')
                {
                    Console.WriteLine("Символы IV вместе означают 400, прибавим к результату 4");
                    i++;
                    result += 4;
                }
                else
                {
                    int index = Array.IndexOf(romanNumber, number[i].ToString());
                    Console.WriteLine($"Символ {number[i]} в означают {nums[index]}, прибавим eго к  результату");
                    result += nums[index];
                }
            }
            Console.WriteLine($"Число {number}  в арабской записи {result}");
        }
    }
}