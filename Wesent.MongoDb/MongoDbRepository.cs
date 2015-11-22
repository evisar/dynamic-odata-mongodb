using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Wesent.Common;

namespace Wesent.MongoDb
{
    public class MongoDbRepository<TKey, TEntity>: IRepository<TKey, TEntity>
        where TKey : IComparable<TKey>
        where TEntity : IEntity
    {

        readonly static Type _entityType = typeof(TEntity);
        static readonly object _sync = new object();
        readonly IMongoClient _mongo;
        readonly IMongoDatabase _database;
        readonly IMongoCollection<TEntity> _collection;

        public MongoDbRepository(string url)
        {
            lock (_sync)
            {
                if (_mongo == null)
                {
                    _mongo = new MongoClient(url);
                }
                if(_database==null)
                {
                    string databaseName = _entityType.Namespace.Replace('.', '_');
                    _database = _mongo.GetDatabase(databaseName);
                }
            }
            _collection = _database.GetCollection<TEntity>(_entityType.Name);
        }

        public IQueryable<TEntity> Query()
        {
            return _collection.AsQueryable();
        }

        public TEntity Get(TKey key)
        {
            FilterDefinition<TEntity> filter = new BsonDocument("_id", BsonValue.Create(key));
            return _collection.Find<TEntity>( filter).FirstOrDefaultAsync().Result;
        }

        public void Update(TEntity value)
        {
            _collection.InsertOneAsync(value);
        }

        public void Delete(TKey key)
        {
            FilterDefinition<TEntity> filter = new BsonDocument("_id", BsonValue.Create(key));
            _collection.DeleteOneAsync(filter);
        }
    }
}