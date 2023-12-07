using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace EscapeFromTheWoods
{
    public class DBwriter
    {
        private readonly string connectionString = "mongodb+srv://matthiaspolen:<root>@cluster0.d3mfnve.mongodb.net/?retryWrites=true&w=majority";
        private readonly MongoClient client;

        public DBwriter(string connectionString)
        {
            this.connectionString = connectionString;
            this.client = new MongoClient(connectionString);
        }

        private IMongoDatabase GetDatabase()
        {
            string databaseName = "Cluster0"; // database name??
            return client.GetDatabase(databaseName);
        }

        public async Task WriteWoodRecords(List<DBWoodRecord> data)
        {
            IMongoDatabase database = GetDatabase();
            IMongoCollection<DBWoodRecord> collection = database.GetCollection<DBWoodRecord>("WoodRecords");

            // Insert the data into the MongoDB collection asynchronously
            await collection.InsertManyAsync(data);
        }

        public async Task WriteMonkeyRecords(List<DBMonkeyRecord> data)
        {
            IMongoDatabase database = GetDatabase();
            IMongoCollection<DBMonkeyRecord> collection = database.GetCollection<DBMonkeyRecord>("MonkeyRecords");

            // Insert the data into the MongoDB collection asynchronously
            await collection.InsertManyAsync(data);
        }
    }
}
