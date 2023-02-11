using dc_api.Models;

namespace dc_api.Services;

public interface ICustomerService
{
    Task CreateAsync(Customer customer);

    Task<List<Customer>> GetAllCustomers();

    Task<List<Customer>> SearchCustomers(string search);

    Task UpdateFirstName(string id, string firstName);

    Task<bool> DeleteCustomer(string id); 
    Task<Customer?> SearchCustomerById(string id);
}