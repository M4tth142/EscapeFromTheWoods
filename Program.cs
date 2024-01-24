using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Hello World!");
            string connectionString = @"mongodb+srv://matthiaspolen:root@cluster0.tbnscpt.mongodb.net/?retryWrites=true&w=majority";
            DBwriter db = new DBwriter(connectionString);
            string path = @"C:\NET\monkeys";


            Map map1 = new Map(0, 500, 0, 500);
            Wood wood1 = WoodBuilder.GetWood(500, map1, path, db);

            Map map2 = new Map(0, 200, 0, 400);
            Wood wood2 = WoodBuilder.GetWood(2500, map2, path, db);

            Map map3 = new Map(0, 400, 0, 400);
            Wood wood3 = WoodBuilder.GetWood(2000, map3, path, db);


            // Create tasks for each wood
            Task task1 = Task.Run(() => ProcessWood(wood1, new[] { "Alice", "Janice", "Toby", "Mindy", "Jos" }));
            Task task2 = Task.Run(() => ProcessWood(wood2, new[] { "Tom", "Jerry", "Tiffany", "Mozes", "Jebus" }));
            Task task3 = Task.Run(() => ProcessWood(wood3, new[] { "Kelly", "Kenji", "Kobe", "Kendra" }));

            // Wait for all tasks to finish
            Task.WaitAll(task1, task2, task3);

            stopwatch.Stop();
            // Write result.
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.WriteLine("end");

            static void ProcessWood(Wood wood, string[] monkeyNames)
            {
                foreach (var name in monkeyNames)
                {
                    wood.PlaceMonkey(name, IDgenerator.GetMonkeyID());
                }

                wood.WriteWoodToDB();
                wood.Escape();
            }

        }
    }
}
