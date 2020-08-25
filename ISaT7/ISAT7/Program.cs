using System;
using System.Collections.Generic;
using System.Linq;

namespace ISAT7
{
    class Road
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Distance { get; set; }

        public Road(string from, string to, int dist)
        {
            From = from;
            To = to;
            Distance = dist;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            var roads = new List<Road>();
            var listWays = new List<List<string>>();
            roads.Add(new Road("Москва", "Калуга", 188));
            roads.Add(new Road("Калуга", "Саранск", 754));
            roads.Add(new Road("Саранск", "Воронеж", 624));
            roads.Add(new Road("Тула", "Воронеж", 352));
            roads.Add(new Road("Калуга", "Тула", 118));
            roads.Add(new Road("Калуга", "Воронеж", 454));
            FindPathes("Калуга", "Саранск", listWays, roads);
            Console.WriteLine("===============================");
            FindPathes("Калуга", "Саранск", listWays, roads, 800);
            Console.WriteLine("===============================");
            FindPathes("Калуга", "Саранск", listWays, roads, null, 1);
            Console.ReadLine();
        }

        static void FindPathes(string start, string finish, List<List<string>> listWays, List<Road> roads, int? distOgr = null, int? cntOgr = null)
        {
            Ways(roads, "Калуга", new List<string>(), listWays);
            foreach (var way in listWays)
            {
                if (way.First() == start && way.Last() == finish)
                {
                    int dist = 0;                  
                    for (int i = 0; i < way.Count; i++)
                    {
                        if (i + 1 >= way.Count) continue;
                        dist += roads
                            .SingleOrDefault(r => (r.From == way[i] && r.To == way[i + 1]) || (r.To == way[i] && r.From == way[i + 1]))?
                            .Distance ?? 0;                      
                    }

                    if (distOgr.HasValue && dist <= distOgr)
                    {
                        Print(way, dist);
                        continue;
                    }
                    else if (cntOgr.HasValue && way.Count - 2 <= cntOgr)
                    {
                        Print(way, dist);
                        continue;
                    }
                    else if (!distOgr.HasValue && !cntOgr.HasValue) Print(way, dist);
                }              
            }
            Console.WriteLine("Поиск закончен");
        }

        static bool Find(List<string> list, string x)
        {
            if (list == null) return false;
            else
            {
                foreach (var i in list)
                {
                    if (i == x) return true;
                }
            }
            return false;
        }

        static bool Compare(List<string> L1, List<string> L2)
        {
            if (L1.Count == L2.Count)
            {
                for (var i = 0; i < L1.Count; i++)
                    if (L1[i] != L2[i]) return false;
            }
            else return false;
            return true;
        }

        static void AddWay(List<string> list, List<List<string>> listWays)
        {
            int i = 0;
            bool temp = true;
            while (i < listWays.Count && temp)
            {
                if (Compare(list, listWays[i])) temp = false;
                else i++;
            }
            if (temp || i == listWays.Count)
            {
                listWays.Add(new List<string>());
                foreach (var j in list)
                {
                    listWays[listWays.Count - 1].Add(j);
                }
            }
        }

        static void Ways(List<Road> roads, string startCity, List<string> list, List<List<string>> listWays)
        {
            foreach (var road in roads)
            {
                if (!Find(list, startCity))
                {
                    if (road.From == startCity)
                    {
                        list.Add(startCity);

                        if (!Find(list, road.To)) Ways(roads, road.To, list, listWays);
                        else AddWay(list, listWays);
                        list.Remove(startCity);
                    }
                    else if (road.To == startCity)
                    {
                        list.Add(startCity);

                        if (!Find(list, road.From)) Ways(roads, road.From, list, listWays);
                        else AddWay(list, listWays);
                        list.Remove(startCity);
                    }
                }
            }
        }

        static void Print(List<string> list, int dist = 0)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1) Console.Write($"{list[i]}->");
                else Console.Write(list[i]);
            }
            Console.Write($", Расстояние: {dist}");
            Console.WriteLine();
        }

    }
}
