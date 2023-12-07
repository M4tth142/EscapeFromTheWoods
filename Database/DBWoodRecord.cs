using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EscapeFromTheWoods
{
    public class DBWoodRecord
    {
        public DBWoodRecord(int woodID, int treeID, int x, int y)
        {
            this.WoodID = woodID;
            this.TreeID = treeID;
            this.X = x;
            this.Y = y;
        }

        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public int WoodID { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
