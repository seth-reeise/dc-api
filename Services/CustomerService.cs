using dc_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace dc_api.Services;

public class CustomerService {

    private readonly IMongoCollection<Customer> _customerCollection;

    public CustomerService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _customerCollection = database.GetCollection<Customer>(mongoDBSettings.Value.CollectionName);
    }

    // can call whatever
    public async Task CreateAsync(Customer customer) {
        await _customerCollection.InsertOneAsync(customer);
        return;
    }

    public async Task<List<Customer>> GetAsync() {
        return await _customerCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<List<Customer>> SearchAsync(string search) {
        // "/^" = starts with, "/i" = ignore case
        var regexSearch = "/^" + search + "/i";
        // Console.WriteLine(regexSearch);

        FilterDefinition<Customer> filter = 
          Builders<Customer>.Filter.Regex("dogName", regexSearch)
        | Builders<Customer>.Filter.Regex("firstName", regexSearch)
        | Builders<Customer>.Filter.Regex("lastName", regexSearch);

        return await _customerCollection.Find(filter).ToListAsync();
    }

    public async Task UpdateFirstNameAsync(string id, string firstName) {
        FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq("Id", id);
        // UpdateDefinition<Customer> update = Builders<Customer>.Update.AddToSet<string>("firstName", firstName);
        UpdateDefinition<Customer> update1 = Builders<Customer>.Update.Set<string>("firstName", firstName);
        await _customerCollection.UpdateOneAsync(filter, update1);
        return;        
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq("Id", id);
        await _customerCollection.DeleteOneAsync(filter);
        return;
    } 

}

