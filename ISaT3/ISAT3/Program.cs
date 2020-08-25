using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ISAT3
{
    class Program
    {
        static List<Animal> foundAnimals; //список найденных животных
        static List<int> bufferFacts; //список фактов, которые "да"
        const string factsPath = "..\\..\\Факты.json";
        const string animPath = "..\\..\\Животные.json";
        static void Main()
        {
        Dialog: //метка на начало диалога
            Console.WriteLine("Ответьте, какими качествами обладет животное, а я попытаюсь его отгадать");
            Console.WriteLine("Отвечать можно только 'да' или 'нет'");
            if (!File.Exists(factsPath) || !File.Exists(animPath))
            {
                Console.WriteLine("Один или оба файла не найдены");
                Console.ReadLine();
            }
            var animals = Animal.GetData(animPath); //получение животных из файла
            if (animals == null || animals.Count == 0) //если файл пуст, добавляем туда хоть что-то
            {
                var aniList = new List<Animal>
                {
                    new Animal { Name = "Кот", Facts = new List<int> { 0, 1 } },
                    new Animal { Name = "Тигр", Facts = new List<int> { 0, 2 } }
                };
                Animal.PostData(aniList, animPath);
                var factList = new List<Fact>();
                Fact.PostData(factList, factsPath);
            }
            var facts = Fact.GetData(factsPath);  //получение фактов из файла
            if (facts == null || facts.Count == 0) //если файл пуст, добавляем туда хоть что-то
            {
                var factList = new List<Fact>
                {
                    new Fact { Id = 0, Value = "носит усы" },
                    new Fact { Id = 1, Value = "орет в марте" },
                    new Fact { Id = 2, Value = "оранжевого цвета" }
                };
                Fact.PostData(factList, factsPath);
            }
            int i = 0;
            bufferFacts = new List<int>(); //обнуляем буфер одобренных фактов
            foreach (var fact in facts) //предлагаем каждый факт из базы по очереди
            {
                i++; //номер итерации нужен для сохранения фактов в буфер после "да"
                foundAnimals = new List<Animal>(); //обнуляем результат поиска животных
            InputKey: //метка на повторный вопрос, если входное значение некорректное
                Console.WriteLine($"Животное {fact.Value}?"); //пример: "Животное орет в марте?", где "орет в марте" - факт
                string answer = Console.ReadLine(); //ожидается да или нет
                if (answer.ToLowerInvariant() != "да" && answer.ToLowerInvariant() != "нет")
                {
                    Console.WriteLine("Отвечать можно только 'да' или 'нет'");
                    Console.WriteLine("Попробуйте еще раз!");
                    goto InputKey; //если входная страка не ожидалась, возвращаемся к метке
                }
                //блок "нет"
                if (answer.ToLowerInvariant() == "нет") //если факт не подходит, то из списка животных удаляются все объекты, которые имеют этот факт
                {
                    var animalForRemove = new List<Animal>();
                    foreach (var anim in animals)
                    {
                        if (anim.Facts.Contains(fact.Id)) animalForRemove.Add(anim);
                    }
                    animalForRemove.ForEach(a => animals.Remove(a));
                    continue; //переходим к следующей итерации цикла по фактам
                }
                //блок "да"
                bufferFacts.Add(i - 1); //запоминаем факт
                foreach (var anim in animals) //ищем животных, которым соответствуют сразу все факты из буфера
                {
                    if (anim.Facts.ContainsAllItems(bufferFacts)) //если текущий зверь имеет все факты из буфера,
                        foundAnimals.Add(anim);                  //то сохранять в результат
                }

                if (foundAnimals != null && foundAnimals.Count() == 1) //если в результате всего одно животное, значит мы его нашли
                {
                    Console.WriteLine($"Я думаю это {foundAnimals.First().Name}"); //пишем, что нашли
                    goto Dialog; //возвращаемся к началу диалога
                }
                if (foundAnimals == null || foundAnimals.Count() == 0) //если животное не нашлось
                {
                    Console.WriteLine("Такое животное мне неизвестно, как оно называется?");
                    string animalName = Console.ReadLine(); //запоминаем новое животное
                    var newAnimal = new Animal { Name = animalName, Facts = new List<int>() }; //создаем объект 
                    bufferFacts.ForEach(bf => newAnimal.Facts.Add(bf)); //каждый факт из буфера присваивается этому животному
                    string input = "да"; //заглушка для цикла
                    while (input.ToLowerInvariant() != "нет") //спрашиваем какие еще характеристики животное имеет
                    {
                        Console.WriteLine($"{animalName} имеет еще какие-то характеристики?");
                        input = Console.ReadLine();
                        if (input.ToLowerInvariant() == "да") //если нет то выходим из цикла и сохраняем животное
                        {
                            Console.WriteLine("Введите новое качество");
                            var inputValue = Console.ReadLine(); //запоминаем факт
                            var localFacts = Fact.GetData(factsPath); //получаем уже имеющиеся
                            var inputFact = new Fact { Id = localFacts.Count, Value = inputValue }; //создаем объект факта
                            localFacts.Add(inputFact); //добавляем новый объект к уже имеющимся
                            Fact.PostData(localFacts, factsPath); //записываем в файл обновленный список
                            newAnimal.Facts.Add(inputFact.Id); //новый факт добавляем новому животному
                            Console.WriteLine("Качество сохранено");
                        }
                    };
                    var localAnimals = Animal.GetData(animPath);
                    localAnimals.Add(newAnimal);
                    Animal.PostData(localAnimals, animPath); //записываем нового зверя в файл
                    Console.WriteLine("Животное сохранено");
                    goto Dialog; //начинаем диалог заново
                }
            }
            goto Dialog;
        }
    }

    public static class LinqExtras
    {
        public static bool ContainsAllItems<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return !b.Except(a).Any();
        }
    }

    [DataContract]
    public class Animal
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<int> Facts { get; set; }

        public static void PostData(List<Animal> Ab, string path)
        {
            string json = JsonHelper.JsonSerializer(Ab);
            File.WriteAllText(path, json);
        }

        public static List<Animal> GetData(string path)
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonHelper.JsonDeserialize<List<Animal>>(json);
        }
    }

    [DataContract]
    public class Fact
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Value { get; set; }

        public static void PostData(List<Fact> Ab, string path)
        {
            string json = JsonHelper.JsonSerializer(Ab);
            File.WriteAllText(path, json);
        }

        public static List<Fact> GetData(string path)
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonHelper.JsonDeserialize<List<Fact>>(json);
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
