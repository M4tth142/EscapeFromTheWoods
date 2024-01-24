using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    public class Tree
    {
        /// <summary>
        /// a tree is defined with an ID, an x and y coordinate and a boolean to check if a monkey is on it
        /// </summary>
        /// <param name="treeID"> id of the tree </param>
        /// <param name="x"> x value of the tree </param>
        /// <param name="y"> y value of the tree </param>
        public Tree(int treeID, int x, int y)
        {
            this.treeID = treeID;
            this.x = x;
            this.y = y;
            this.hasMonkey = false;
        }
        public int treeID { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool hasMonkey { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Tree tree &&
                   x == tree.x &&
                   y == tree.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
        public override string ToString()
        {
            return $"{treeID},{x},{y}";
        }
    }
}
