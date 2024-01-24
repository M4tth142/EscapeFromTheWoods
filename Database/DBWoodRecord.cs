using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EscapeFromTheWoods
{
    /// <summary>
    /// class where the data of a wood and its trees are stored
    /// </summary>
    public class DBWoodRecord
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int WoodID { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        // Constructor with parameters
        public DBWoodRecord(int woodID, int treeID, int x, int y)
        {
            WoodID = woodID;
            TreeID = treeID;
            X = x;
            Y = y;
        }
    }
}
