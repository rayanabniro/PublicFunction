using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;


namespace PublicFunction.DataBase.Mongo
{
    public class TBaseDocumentModel
    {
        public ObjectId Id { get; set; } // Default field for MongoDB document ID
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    interface IMongo
    {
        bool Insert<T>(string CollectionName, T BaseCollection) where T : TBaseDocumentModel;
        bool Insert(string CollectionName, List<Dictionary<string, object>> BaseCollection);
        bool Insert(string CollectionName, Dictionary<string, object> BaseCollection);
        bool Update<T>(string CollectionName, FilterDefinition<T> filter, UpdateDefinition<T> update) where T : TBaseDocumentModel;
        bool Delete<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel;
        List<T> Query<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel;
        T QueryOne<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel;
    }

    public class Mongo : IMongo
    {
        private readonly IConfiguration Configuration;

        private string ConnectionString => Configuration["PublicFunction:DataBase:Mongo:ConnectionString"];

        private string DatabaseName => Configuration["PublicFunction:DataBase:Mongo:DatabaseName"];

        private static MongoClient _MongoClient;
        private static IMongoDatabase _IMongoDatabase;

        public Mongo(IConfiguration configuration)
        {
            try
            {
                Configuration = configuration;
                _MongoClient = new MongoClient(this.ConnectionString);
                _IMongoDatabase = _MongoClient.GetDatabase(this.DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing MongoDB", ex);
            }
        }

        public bool Insert<T>(string CollectionName, T BaseCollection) where T : TBaseDocumentModel
        {
            try
            {
                _IMongoDatabase.GetCollection<T>(CollectionName).InsertOne(BaseCollection);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert operation failed", ex);
            }
        }

        public bool Insert(string CollectionName, List<Dictionary<string, object>> BaseCollection)
        {
            try
            {
                var collection = _IMongoDatabase.GetCollection<BsonDocument>(CollectionName);
                var bsonList = BaseCollection.Select(dict => new BsonDocument(dict)).ToList();
                collection.InsertMany(bsonList);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert operation failed", ex);
            }
        }

        public bool Insert(string CollectionName, Dictionary<string, object> BaseCollection)
        {
            try
            {
                var bsonDocument = new BsonDocument(BaseCollection);
                _IMongoDatabase.GetCollection<BsonDocument>(CollectionName).InsertOne(bsonDocument);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert operation failed", ex);
            }
        }

        public bool Update<T>(string CollectionName, FilterDefinition<T> filter, UpdateDefinition<T> update) where T : TBaseDocumentModel
        {
            try
            {
                var result = _IMongoDatabase.GetCollection<T>(CollectionName).UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update operation failed", ex);
            }
        }

        public bool Delete<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel
        {
            try
            {
                var result = _IMongoDatabase.GetCollection<T>(CollectionName).DeleteOne(filter);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete operation failed", ex);
            }
        }

        public List<T> Query<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel
        {
            try
            {
                return _IMongoDatabase.GetCollection<T>(CollectionName).Find(filter).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Query operation failed", ex);
            }
        }

        public T QueryOne<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel
        {
            try
            {
                return _IMongoDatabase.GetCollection<T>(CollectionName).Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Query operation failed", ex);
            }
        }
    }
}

