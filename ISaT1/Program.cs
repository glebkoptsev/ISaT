using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ISaT1
{
    class Program
    {
        static List<Car> cars = new List<Car>();
        static void Main(string[] args)
        {
            cars.Add(new Car { mark = "Skoda", model = "Octavia" });
        Menu:
            Console.WriteLine();
            Console.WriteLine("Для поиска марки машины нажмите 0");
            Console.WriteLine("Для поиска модели машины нажмите 1");
            Console.WriteLine("Для добавления машины нажмите 2");
            Console.WriteLine("Для удаления машины нажмите 3");
            Console.WriteLine("Для обновления машины нажмите 4");
            Console.WriteLine("Для отображения всех машин нажмите 5");
            Console.WriteLine("Для выхода нажмите любую другую цифру");
            if(!int.TryParse(Console.ReadLine(), out int key))
            {
                Console.WriteLine();
                Console.WriteLine("Можно вводить только цифры!");
                goto Menu;
            }
            switch (key)
            {
                case 0:
                    Console.WriteLine("Введите марку машины");
                    string mark = Console.ReadLine();
                    var findResult = cars.Where(c => c.mark == mark).ToList();
                    Console.WriteLine("Результаты: ");
                    if (findResult.Any()) findResult.ForEach(f => Console.WriteLine($"{f.mark} {f.model}"));
                    else Console.WriteLine("Машин такой модели не найдено");
                    Console.WriteLine();
                    goto Menu;
                case 1:
                    Console.WriteLine("Введите модель машины");
                    string model = Console.ReadLine();
                    var findResult2 = cars.Where(c => c.model == model).ToList();
                    Console.WriteLine("Результаты: ");
                    if (findResult2.Any()) findResult2.ForEach(f => Console.WriteLine($"{f.mark} {f.model}"));
                    else Console.WriteLine("Машин такой марки не найдено");
                    Console.WriteLine();
                    goto Menu;
                case 2:
                    var car = new Car();                   
                    Console.WriteLine("Введите марку машины");
                    car.mark = Console.ReadLine();
                    Console.WriteLine("Введите модель машины");
                    car.model = Console.ReadLine();
                    if (cars.Where(c => c.mark == car.mark && c.model == car.model).Any())
                    {
                        Console.WriteLine("Такая машина уже существует");
                        goto Menu;
                    }
                    cars.Add(car);
                    Console.WriteLine("Машина добавлена");
                    goto Menu;
                case 3:
                    var carDel = new Car();
                    Console.WriteLine("Введите марку машины");
                    carDel.mark = Console.ReadLine();
                    Console.WriteLine("Введите модель машины");
                    carDel.model = Console.ReadLine();
                    var carForDelete = cars.SingleOrDefault(c => c.mark == carDel.mark && c.model == carDel.model);
                    if (carForDelete != null)
                    {
                        cars.Remove(carForDelete);
                        Console.WriteLine("Машина удалена");
                        goto Menu;
                    }
                    Console.WriteLine("Машина отсутствует");
                    goto Menu;
                case 4:
                    var carOld = new Car();
                    Console.WriteLine("Введите марку машины");
                    carOld.mark = Console.ReadLine();
                    Console.WriteLine("Введите модель машины");
                    carOld.model = Console.ReadLine();
                    var carForUpdate = cars.SingleOrDefault(c => c.model == carOld.model && c.mark == carOld.mark);
                    if (carForUpdate == null)
                    {
                        Console.WriteLine("Машина не найдена");
                        goto Menu;
                    }
                    Console.WriteLine("Машина найдена");
                    cars.Remove(carForUpdate);
                    var carNew = new Car();
                    Console.WriteLine("Введите марку машины");
                    carNew.mark = Console.ReadLine();
                    Console.WriteLine("Введите модель машины");
                    carNew.model = Console.ReadLine();
                    cars.Add(carNew);
                    Console.WriteLine("Машина обновлена");
                    goto Menu;
                case 5:
                    Console.WriteLine();
                    cars.ForEach(c => Console.WriteLine($"{c.mark} {c.model}"));
                    if (cars.Count == 0) Console.WriteLine("База пуста");
                    goto Menu;
            }
        }

        public class Car
        {
            public string mark;
            public string model;
        }
    }
}
