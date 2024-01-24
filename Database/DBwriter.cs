using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EscapeFromTheWoods
{
    public class DBwriter
    {
        private readonly string connectionString;
        private readonly MongoClient client;
        public DBwriter(string connectionString)
        {
            this.connectionString = connectionString;
            this.client = new MongoClient(connectionString);
        }
        /// <summary>
        /// gets the database from the connection string
        /// </summary>
        /// <returns> database </returns>
        private IMongoDatabase GetDatabase()
        {
            string databaseName = "escapeTheWoods";
            return client.GetDatabase(databaseName);
        }
        /// <summary>
        /// gets the collection from the database and writes the wood records to the database in a new bson document
        /// </summary>
        /// <param name="data"> wood data that needs to go to the database</param>
        /// <returns></returns>
        public async Task WriteWoodRecords(List<DBWoodRecord> data)
        {
            IMongoDatabase database = GetDatabase();

            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("WoodRecords");

            var woodRecords = data.ConvertAll(record =>
                new BsonDocument
                {
                    { "woodID", record.WoodID },
                    { "treeID", record.TreeID },
                    { "x", record.X },
                    { "y", record.Y },
                }
            );

            try
            {
                // Insert the data into the MongoDB collection asynchronously
                await collection.InsertManyAsync(woodRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing wood records to the database: {ex.Message}");
                // Optionally rethrow the exception if needed
            }
        }
        /// <summary>
        /// gets the collection from the database and writes the monkey records to the database in a new bson document
        /// </summary>
        /// <param name="data">moneky data that needs to go to the database</param>
        /// <returns></returns>
        public async Task WriteMonkeyRecords(List<DBMonkeyRecord> data)
        {
            IMongoDatabase database = GetDatabase();
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("MonkeyRecords");

            var monkeyRecords = data.ConvertAll(record =>
                new BsonDocument
                {
                    { "monkeyID", record.MonkeyID },
                    { "monkeyName", record.MonkeyName },
                    { "woodID", record.WoodID },
                    { "seqNr", record.SeqNr },
                    { "treeID", record.TreeID },
                    { "x", record.X },
                    { "y", record.Y },
                    // Add more fields as needed
                }
            );

            try
            {
                // Insert the data into the MongoDB collection asynchronously
                await collection.InsertManyAsync(monkeyRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing monkey records to the database: {ex.Message}");
                // Optionally rethrow the exception if needed
                // throw;
            }
        }
    }
}
