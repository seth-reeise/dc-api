using dc_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace dc_api.Services;

public class CustomerService: ICustomerService {

    private readonly IMongoCollection<Customer> _customerCollection;

    public CustomerService(IMongoDatabase database) {
        _customerCollection = database.GetCollection<Customer>("contract_form");
    }

    public async Task CreateAsync(Customer customer) {
        await _customerCollection.InsertOneAsync(customer);
    }

    public async Task<List<Customer>> GetAllCustomers() {
        return await _customerCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<List<Customer>> SearchCustomers(string search) {
        // "/^" = starts with, "/i" = ignore case
        var regexSearch = "/^" + search + "/i";

        FilterDefinition<Customer> filter = 
          Builders<Customer>.Filter.Regex("dogName", regexSearch)
        | Builders<Customer>.Filter.Regex("firstName", regexSearch)
        | Builders<Customer>.Filter.Regex("lastName", regexSearch);

        return await _customerCollection.Find(filter).ToListAsync();
    }

    public async Task<Customer?> SearchCustomerById(string id)
    {
        return await _customerCollection.Find(customer => customer.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateFirstName(string id, string firstName) {
        FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq("Id", id);
        UpdateDefinition<Customer> update = Builders<Customer>.Update.Set<string>("firstName", firstName);
        await _customerCollection.UpdateOneAsync(filter, update);
    }

    public async Task<bool> DeleteCustomer(string id) {
        var deleteResult = await _customerCollection.DeleteOneAsync(customer => customer.Id == id);
        return deleteResult.IsAcknowledged;
    }
}

