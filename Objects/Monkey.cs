using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    /// <summary>
    /// A monkey is defined with an ID, a name and a tree
    /// </summary>
    public class Monkey
    {
        public int monkeyID { get; set; }
        public string name { get; set; }
        public Tree tree { get; set; }

        public Monkey(int monkeyID, string name, Tree tree)
        {
            this.monkeyID = monkeyID;
            this.tree = tree;
            this.name = name;
        }
    }
}
