using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EscapeFromTheWoods
{
    public class DBMonkeyRecord
    {
        public DBMonkeyRecord(int monkeyID, string monkeyName, int woodID, int seqNr, int treeID, int x, int y)
        {
            this.MonkeyID = monkeyID;
            this.MonkeyName = monkeyName;
            this.WoodID = woodID;
            this.SeqNr = seqNr;
            this.TreeID = treeID;
            this.X = x;
            this.Y = y;
        }

        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public int MonkeyID { get; set; }
        public string MonkeyName { get; set; }
        public int WoodID { get; set; }
        public int SeqNr { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
