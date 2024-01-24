using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    public static class WoodBuilder
    {
        /// <summary>
        /// place the amount of trees in the wood on random locations
        /// </summary>
        /// <param name="size">amount of trees</param>
        /// <param name="map">wood dimensions</param>
        /// <param name="path">path where bitmaps will be stored</param>
        /// <param name="db">databse reference</param>
        /// <returns> returns a wood </returns>
        /// TODO: rename "size" to "amountOfTrees"
        public static Wood GetWood(int size, Map map, string path, DBwriter db)
        {
            Random random = new Random(100);
            List<Tree> trees = new List<Tree>();
            int n = 0;
            while (n < size)
            {
                Tree t = new Tree(IDgenerator.GetTreeID(), random.Next(map.xmin, map.xmax), random.Next(map.ymin, map.ymax));
                if (!trees.Contains(t))
                {
                    trees.Add(t);
                    n++;
                }
            }
            Wood w = new Wood(IDgenerator.GetWoodID(), trees, map, path, db);
            return w;
        }
    }
}
