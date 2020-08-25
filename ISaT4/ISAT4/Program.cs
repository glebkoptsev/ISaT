using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ISAT4
{
    class Program
    {
        static void Main(string[] args)
        {
        Menu:  //Метка на меню
            Console.WriteLine();
            Console.WriteLine("Для нахождения факториала нажмите 0");
            Console.WriteLine("Для нахождения степени нажмите 1");
            Console.WriteLine("Для нахождения числа Фибоначчи нажмите 2");
            Console.WriteLine("Для выхода нажмите любую другую цифру");
            if (!InputCheck(Console.ReadLine(), out long key)) goto Menu;
            switch (key) //В зависимости от введенной цифры выполняется сценарий
            {
                case 0:
                    if (!File.Exists("..\\..\\Факториалы.json"))
                    {
                        Console.WriteLine("Факториалы.json не найден");
                        goto Menu;
                    }
                    Console.WriteLine("Введите число, факториал которого нужно найти");
                    if(!InputCheck(Console.ReadLine(), out long keyFact)) goto Menu;
                    if (keyFact > 27)
                    {
                        Console.WriteLine("Число не должно быть больше 27, ограничение типа decimal");
                        goto Menu;
                    }                     
                    Console.WriteLine("Поиск значения в базе...");
                    //Если в файле что-то есть, то данные считываются в список. Если нет, создается новый список
                    var values = Value.GetData("..\\..\\Факториалы.json") ?? new List<Value>();
                    //Создание нового пустого объекта (записи)  
                    Value val = null;
                    //Если в списке что-то есть, то происходит поиск результата и присваивание объекту Val
                    if (values.Count > 0) val = values.SingleOrDefault(v => v.Input == keyFact); 
                    if (val == null) //Если результат не найден
                    {
                        Console.WriteLine("Значение не найдено в базе, вычисление результата...");
                        Console.WriteLine("Результат равен: " + Fact(keyFact)); //Передача входного значения в метод
                        values.Add(new Value { Input = keyFact, Result = Fact(keyFact) }); //Создание нового объекта (записи) и добавление в список
                        Value.PostData(values, "..\\..\\Факториалы.json"); //Запись в файл обновленного списка
                        Console.WriteLine("Результат записан в базу");
                        goto Menu;
                    }
                    //Если результат найден
                    Console.WriteLine("Результат найден в базе и равен: " + val.Result);
                    goto Menu;
                case 1:
                    if (!File.Exists("..\\..\\Степени.json"))
                    {
                        Console.WriteLine("Степени.json не найден");
                        goto Menu;
                    }
                    Console.WriteLine("Введите основание числа, степень которого нужно найти");
                    if (!InputCheck(Console.ReadLine(), out long osnPow)) goto Menu;
                    Console.WriteLine("Введите степень числа");
                    if (!InputCheck(Console.ReadLine(), out long keyPow)) goto Menu;
                    Console.WriteLine("Поиск значения в базе...");
                    var valuesPow = ValuePow.GetDataPow("..\\..\\Степени.json") ?? new List<ValuePow>();
                    ValuePow valPow = null;
                    if (valuesPow.Count > 0) valPow = valuesPow.SingleOrDefault(v => v.Input == osnPow && v.Pow == keyPow);
                    if (valPow == null)
                    {
                        Console.WriteLine("Значение не найдено в базе, вычисление результата...");
                        Console.WriteLine("Результат равен: " + Math.Pow(osnPow, keyPow));
                        valuesPow.Add(new ValuePow { Input = osnPow, Pow = keyPow, Result = Math.Pow(osnPow, keyPow) });
                        ValuePow.PostDataPow(valuesPow, "..\\..\\Степени.json");
                        Console.WriteLine("Результат записан в базу");
                        goto Menu;
                    }
                    Console.WriteLine("Результат найден в базе и равен: " + valPow.Result);
                    goto Menu;
                case 2:
                    if (!File.Exists("..\\..\\Фибоначчи.json"))
                    {
                        Console.WriteLine("Фибоначчи.json не найден");
                        goto Menu;
                    }
                    Console.WriteLine("Введите n-ый член");
                    if (!InputCheck(Console.ReadLine(), out long keyFib)) goto Menu;
                    Console.WriteLine("Поиск значения в базе...");
                    var valuesFib = Value.GetData("..\\..\\Фибоначчи.json") ?? new List<Value>();
                    Value valFib = null;
                    if (valuesFib.Count > 0) valFib = valuesFib.SingleOrDefault(v => v.Input == keyFib);
                    if (valFib == null)
                    {
                        Console.WriteLine("Значение не найдено в базе, вычисление результата...");
                        Console.WriteLine("Результат равен: " + Fibonacсi(keyFib));
                        valuesFib.Add(new Value { Input = keyFib, Result = Fibonacсi(keyFib) });
                        Value.PostData(valuesFib, "..\\..\\Фибоначчи.json");
                        Console.WriteLine("Результат записан в базу");
                        goto Menu;
                    }
                    Console.WriteLine("Результат найден в базе и равен: " + valFib.Result);
                    goto Menu;
            }
        }

        static decimal Fact(long x) //Рекурсивное вычисление факториала
        {
            return (x == 0) ? 1 : x * Fact(x - 1);
        }

        static decimal Fibonacсi(long n) //Вычисление числа Фибоначчи
        {
            //return (n == 0 || n == 1) ? n : Fibonacсi(n - 1) + Fibonacсi(n - 2); //Рекурсивный вариант
            decimal a = 0;
            decimal b = 1;
            decimal tmp;
            for (int i = 0; i < n; i++)
            {
                tmp = a;
                a = b;
                b += tmp;
            }
            return a;
        }

        /// <summary>
        /// Если введена буква, то выводить ошибку
        /// </summary>
        static bool InputCheck(string input, out long key)
        {
            if (!long.TryParse(input, out key)) 
            {
                Console.WriteLine();
                Console.WriteLine("Можно вводить только цифры!");
                return false;
            }
            return true;
        }
    }

    [DataContract]
    public class Value //Класс, описывающий структуру записей в файлах "Факториалы" и "Фибоначчи"
    {
        [DataMember]
        public long Input { get; set; }  //входное значение
        [DataMember]
        public decimal Result { get; set; } //результат

        public static void PostData(List<Value> Ab, string path)
        {
            string json = JsonHelper.JsonSerializer(Ab);
            File.WriteAllText(path, json);
        }

        public static List<Value> GetData(string path)
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonHelper.JsonDeserialize<List<Value>>(json);
        }
    }

    [DataContract]
    public class ValuePow //Класс, описывающий структуру записей в файле "Степени"
    {
        [DataMember]
        public double Input { get; set; }  //основание
        [DataMember]
        public double Pow { get; set; }  //степень
        [DataMember]
        public double Result { get; set; } //результат

        public static List<ValuePow> GetDataPow(string path)
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonHelper.JsonDeserialize<List<ValuePow>>(json);
        }

        public static void PostDataPow(List<ValuePow> Ab, string path)
        {
            string json = JsonHelper.JsonSerializer(Ab);
            File.WriteAllText(path, json);
        }
    }

    public static class JsonHelper
    {
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}
