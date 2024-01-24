using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EscapeFromTheWoods
{
    /// <summary>
    /// class where the data of a monkey is stored
    /// </summary>
    public class DBMonkeyRecord
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int MonkeyID { get; set; }
        public string MonkeyName { get; set; }
        public int WoodID { get; set; }
        public int SeqNr { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        // Constructor with parameters
        public DBMonkeyRecord(int monkeyID, string monkeyName, int woodID, int seqNr, int treeID, int x, int y)
        {
            MonkeyID = monkeyID;
            MonkeyName = monkeyName;
            WoodID = woodID;
            SeqNr = seqNr;
            TreeID = treeID;
            X = x;
            Y = y;
        }
    }
}
