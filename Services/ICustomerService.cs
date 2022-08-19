using dc_api.Models;

namespace dc_api.Services;

public interface ICustomerService
{
    Task CreateAsync(Customer customer);

    Task<List<Customer>> GetAsync();

    Task<List<Customer>> SearchAsync(string search);

    Task UpdateFirstNameAsync(string id, string firstName);

    Task DeleteAsync(string id);
}