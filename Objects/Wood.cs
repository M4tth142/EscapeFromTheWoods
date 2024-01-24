using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace EscapeFromTheWoods
{
    public class Wood
    {
        private const int drawingFactor = 8;
        private string path;
        private DBwriter db;
        private Random r = new Random(1);
        public int woodID { get; set; }
        public List<Tree> trees { get; set; }
        public List<Monkey> monkeys { get; private set; }
        private Map map;
        public Wood(int woodID, List<Tree> trees, Map map, string path, DBwriter db)
        {
            this.woodID = woodID;
            this.trees = trees;
            this.monkeys = new List<Monkey>();
            this.map = map;
            this.path = path;
            this.db = db;
        }
        /// <summary>
        /// Place a monkey on a random tree in the wood and add it to the list of monkeys in the wood object 
        /// </summary>
        /// <param name="monkeyName">name of the monkey</param>
        /// <param name="monkeyID">id of the monkey</param>
        public void PlaceMonkey(string monkeyName, int monkeyID)
        {
            int treeNr;
            do
            {
                treeNr = r.Next(0, trees.Count - 1);
            }
            while (trees[treeNr].hasMonkey);
            Monkey m = new Monkey(monkeyID, monkeyName, trees[treeNr]);
            monkeys.Add(m);
            trees[treeNr].hasMonkey = true;
        }
        /// <summary>
        /// saves the escape routes of all monkeys in the wood to a bitmap and calls the method escapemonkeys with a current monkey
        /// </summary>
        public void Escape()
        {
            List<List<Tree>> routes = new List<List<Tree>>();
            foreach (Monkey monkey in monkeys)
            {
                routes.Add(EscapeMonkey(monkey));
            }
            WriteEscaperoutesToBitmap(routes);
        }
        /// <summary>
        /// Writes the route information of a monkey to the database.
        /// </summary>
        /// <param name="monkey"> a Monkey object </param>
        /// <param name="route">The List of Tree objects representing the escape route of the monkey.</param>
        private async void writeRouteToDB(Monkey monkey, List<Tree> route)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} start");
            List<DBMonkeyRecord> records = new List<DBMonkeyRecord>();
            for (int j = 0; j < route.Count; j++)
            {
                records.Add(new DBMonkeyRecord(monkey.monkeyID, monkey.name, woodID, j, route[j].treeID, route[j].x, route[j].y));
            }
            await db.WriteMonkeyRecords(records);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} end");
        }
        /// <summary>
        /// Generates and saves a bitmap image illustrating escape routes for monkeys in a wooded area.
        /// </summary>
        /// <param name="routes">A List of Lists of Tree objects representing escape routes for monkeys.</param>
        public void WriteEscaperoutesToBitmap(List<List<Tree>> routes)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{woodID}:write bitmap routes {woodID} start");

            Color[] cvalues = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Cyan, Color.GreenYellow };

            // Create a new bitmap with dimensions based on the map and drawing factor
            Bitmap bitMap = new Bitmap((map.xmax - map.xmin) * drawingFactor, (map.ymax - map.ymin) * drawingFactor);
            Graphics graph = Graphics.FromImage(bitMap);
            int delta = drawingFactor / 2;
            Pen p = new Pen(Color.Green, 1);

            // Draw trees on the bitmap
            foreach (Tree t in trees)
            {
                graph.DrawEllipse(p, t.x * drawingFactor, t.y * drawingFactor, drawingFactor, drawingFactor);
            }

            // Loop through each escape route and draw it on the bitmap
            int colorN = 0;
            foreach (List<Tree> route in routes)
            {
                //starting point
                int p1x = route[0].x * drawingFactor + delta;
                int p1y = route[0].y * drawingFactor + delta;

                Color color = cvalues[colorN % cvalues.Length];
                Pen pen = new Pen(color, 1);

                graph.DrawEllipse(pen, p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                graph.FillEllipse(new SolidBrush(color), p1x - delta, p1y - delta, drawingFactor, drawingFactor);

                // Draw lines connecting consecutive trees in the route
                for (int i = 1; i < route.Count; i++)
                {
                    // Update the starting point coordinates for the next segment of the route
                    graph.DrawLine(pen, p1x, p1y, route[i].x * drawingFactor + delta, route[i].y * drawingFactor + delta);
                    p1x = route[i].x * drawingFactor + delta;
                    p1y = route[i].y * drawingFactor + delta;
                }
                colorN++;
            }

            // Save the bitmap image to a file
            bitMap.Save(Path.Combine(path, woodID.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{woodID}:write bitmap routes {woodID} end");
        }
        /// <summary>
        /// all trees in the wood are written to the database
        /// </summary>
        public async void WriteWoodToDB()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{woodID}:write db wood {woodID} start");
            List<DBWoodRecord> records = new List<DBWoodRecord>();
            foreach (Tree t in trees)
            {
                records.Add(new DBWoodRecord(woodID, t.treeID, t.x, t.y));
            }
            await db.WriteWoodRecords(records);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{woodID}:write db wood {woodID} end");
        }
        /// <summary>
        /// Calculates and records the escape route for a given monkey in the wood
        /// </summary>
        /// <param name="monkey">The Monkey object for the calculated escape route.</param>
        /// <returns>A List of Tree objects representing the escape route for the monkey.</returns>
        public List<Tree> EscapeMonkey(Monkey monkey)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{woodID}:start {woodID},{monkey.name}");

            //a dictionary of all trees that are already visited during the escape
            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            trees.ForEach(x => visited.Add(x.treeID, false));
            List<Tree> route = new List<Tree>() { monkey.tree };

            do
            {
                //mark tree as visited
                visited[monkey.tree.treeID] = true;

                SortedList<double, List<Tree>> distanceToMonkey = new SortedList<double, List<Tree>>();

                // Find the closest trees that are not visited and do not have a monkey
                foreach (Tree t in trees)
                {
                    if ((!visited[t.treeID]) && (!t.hasMonkey))
                    {
                        double d = Math.Sqrt(Math.Pow(t.x - monkey.tree.x, 2) + Math.Pow(t.y - monkey.tree.y, 2));

                        if (distanceToMonkey.ContainsKey(d)) distanceToMonkey[d].Add(t);
                        else distanceToMonkey.Add(d, new List<Tree>() { t });
                    }
                }
                //calulate the distance to the nearest border of the wood
                double distanceToBorder = (new List<double>(){ map.ymax - monkey.tree.y,
                map.xmax - monkey.tree.x,monkey.tree.y-map.ymin,monkey.tree.x-map.xmin }).Min();

                // Check if there are no available trees or the nearest border is closer than the closest tree
                if (distanceToMonkey.Count == 0)
                {
                    //log and write to db
                    writeRouteToDB(monkey, route);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{woodID}:end {woodID},{monkey.name}");
                    return route;
                }

                // Check if the nearest border is closer than the closest available tree
                if (distanceToBorder < distanceToMonkey.First().Key)
                {
                    //log and write to db
                    writeRouteToDB(monkey, route);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{woodID}:end {woodID},{monkey.name}");
                    return route;
                }

                // Add the closest available tree to the escape route and update the monkey's current tree
                route.Add(distanceToMonkey.First().Value.First());
                monkey.tree = distanceToMonkey.First().Value.First();
            }
            while (true);
        }
    }
}
