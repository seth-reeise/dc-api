using DC_CONTRACTFORM.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DC_CONTRACTFORM.Services;

public class MongoDBService {

    private readonly IMongoCollection<Contractform> _contractformCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _contractformCollection = database.GetCollection<Contractform>(mongoDBSettings.Value.CollectionName);
    }

    // can call whatever
    public async Task CreateAsync(Contractform contractform) {
        await _contractformCollection.InsertOneAsync(contractform);
        return;
    }

    public async Task<List<Contractform>> GetAsync() {
        return await _contractformCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task UpdateFirstNameAsync(string id, string firstName) {
        FilterDefinition<Contractform> filter = Builders<Contractform>.Filter.Eq("Id", id);
        // UpdateDefinition<Contractform> update = Builders<Contractform>.Update.AddToSet<string>("firstName", contractformId);
        UpdateDefinition<Contractform> update1 = Builders<Contractform>.Update.Set<string>("firstName", firstName);
        await _contractformCollection.UpdateOneAsync(filter, update1);
        return;        
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<Contractform> filter = Builders<Contractform>.Filter.Eq("Id", id);
        await _contractformCollection.DeleteOneAsync(filter);
        return;
    } 

}

