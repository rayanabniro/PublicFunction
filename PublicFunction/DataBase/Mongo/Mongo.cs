using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace PublicFunction.DataBase.Mongo
{
    public class TBaseDocumentModel
    {
    }
    interface IMongo
    {
        public bool Insert<T>(string CollectionName, T BaseCollection) where T : TBaseDocumentModel;
        public bool Insert(string CollectionName, List<Dictionary<string, object>> BaseCollection);
        public bool Insert(string CollectionName, Dictionary<string, object> BaseCollection);
    }
    public class Mongo : IMongo {
        
        private readonly IConfiguration Configuration;

        private string ConnectionString
        {
            get
            {
                return Configuration["PublicFunction:DataBase:Mongo:ConnectionString"].ToString();
            }
        }
        private string DatabaseName
        {
            get
            {
                return Configuration["PublicFunction:DataBase:Mongo:DatabaseName"].ToString();
            }
        }

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
                throw ex;
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
                throw ex;
            }
        }

        public bool Insert(string CollectionName, List<Dictionary<string, object>> BaseCollection)
        {
            try
            {
                _IMongoDatabase.GetCollection<List<Dictionary<string, object>>>(CollectionName).InsertOne(BaseCollection);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(string CollectionName, Dictionary<string, object> BaseCollection)
        {
            try
            {
                _IMongoDatabase.GetCollection<Dictionary<string, object>>(CollectionName).InsertOne(BaseCollection);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
