using System;
using System.Collections.Generic;

namespace ISAT5
{
    class Program
    {
        static readonly string[] relatives = new string[] //Массив возможных родственников
        {
            "мама", "папа", "мать", "отец", "сестра", "брат", "дедушка", "бабушка", "дядя", "тетя"
        };

        static List<string> buffer = new List<string>(); //Список упомянутых родственников
        static void Main(string[] args)
        {
            Console.WriteLine("Привет, незнакомец!");
        Dialog:
            string input = Console.ReadLine();
            if (Contains(input, "привет") || Contains(input, "здравствуйте"))
            {
                string[] vars = new string[] { "Как дела?", "Как поживаете?" };
                var random = new Random();
                int index = random.Next(vars.Length);
                Console.WriteLine(vars[index]);
            }
            else if (Contains(input, "любовь") || Contains(input, "чувство")) Console.WriteLine("Не боитесь ли вы эмоций?");
            else if (Contains(input, "секс")) Console.WriteLine("Это сообщение очень важное");
            else if (Contains(input, "синхрофазотрон")) Console.WriteLine("не могу выговорить");
            else if (Contains(input, "купи слона")) Console.WriteLine("все так говорят");
            else if (Contains(input, "бешенство") || Contains(input, "гнев") || Contains(input, "ярость"))
                Console.WriteLine("Что вы испытываете в данный момент?");
            else if (Contains(input, "фиксация") || Contains(input, "комплекс")) Console.WriteLine("Вы слишком много играете!");
            else if (Contains(input, "всегда")) Console.WriteLine("Можете ли вы привести пример?");
            else if (Contains(input, relatives)) Console.WriteLine("Расскажите подробнее о своей семье");
            else if (Contains(input, "пока") || Contains(input, "свидания")) Environment.Exit(0);
            else if (Contains(input, "да") || Contains(input, "нет")) Console.WriteLine("Расскажите об этом поподробнее");
            else if (buffer.Count > 0)
            {
                var random = new Random();
                int index = random.Next(buffer.Count);
                //Выводим случайного родственника из буфера
                Console.WriteLine("Ранее вы упоминали о " + buffer[index]);
            }
            else Console.WriteLine("Продолжайте рассказ");
            goto Dialog;
        }

        /// <summary>
        /// Проверяет есть ли подстрока в строке без учета регистра
        /// </summary>
        /// <param name="input">Строка, в которой производится поиск</param>
        /// <param name="target">Строка, которую надо найти</param>
        /// <returns></returns>
        static bool Contains(string input, string target) 
        {
            return input.IndexOf(target, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Проверяет есть ли любая из подстрок в строке без учета регистра
        /// </summary>
        /// <param name="input">Строка, в которой производится поиск</param>
        /// <param name="relatives">Строки, которые надо найти</param>
        /// <returns></returns>
        static bool Contains(string input, string[] relatives)
        {
            foreach (var rel in relatives)
            {
                if (input.IndexOf(rel, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    buffer.Add(rel);
                    return true;
                } 
            }
            return false;
        }
    }
}
