using System.Net;
using System.Net.Http.Json;
using AdminService.Models;
using Moq;

namespace AdminService.Tests.Integration;

public class CustomerControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CustomerControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    #region GET /api/customer Tests

    [Fact]
    public async Task GetAllCustomers_ReturnsOk_WhenCustomersExist()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "John", lastName = "Doe", dogName = "Buddy" }
        };
        _factory.SetupGetAllCustomers(customers);

        // Act
        var response = await _client.GetAsync("/api/customer");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var returnedCustomers = await response.Content.ReadFromJsonAsync<List<Customer>>();
        Assert.NotNull(returnedCustomers);
        Assert.Single(returnedCustomers);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsNotFound_WhenNoCustomers()
    {
        // Arrange
        _factory.SetupGetAllCustomers(new List<Customer>());

        // Act
        var response = await _client.GetAsync("/api/customer");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GET /api/customer/search Tests

    [Fact]
    public async Task SearchCustomers_ReturnsOk_WhenMatchFound()
    {
        // Arrange
        var searchTerm = "Buddy";
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "John", dogName = "Buddy" }
        };
        _factory.SetupSearchCustomers(searchTerm, customers);

        // Act
        var response = await _client.GetAsync($"/api/customer/search?search={searchTerm}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchCustomers_ReturnsNotFound_WhenNoMatch()
    {
        // Arrange
        var searchTerm = "NonExistent";
        _factory.SetupSearchCustomers(searchTerm, new List<Customer>());

        // Act
        var response = await _client.GetAsync($"/api/customer/search?search={searchTerm}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region POST /api/customer Tests

    [Fact]
    public async Task CreateCustomer_ReturnsCreated_WhenValid()
    {
        // Arrange
        _factory.SetupCreateAsync();
        var customer = new Customer
        {
            firstName = "Jane",
            lastName = "Smith",
            dogName = "Max",
            email = "jane@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/customer", customer);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_CallsService()
    {
        // Arrange
        _factory.SetupCreateAsync();
        var customer = new Customer
        {
            firstName = "Test",
            lastName = "User",
            dogName = "Fido"
        };

        // Act
        await _client.PostAsJsonAsync("/api/customer", customer);

        // Assert
        _factory.MockCustomerService.Verify(s => s.CreateAsync(It.IsAny<Customer>()), Times.AtLeastOnce);
    }

    #endregion

    #region PUT /api/customer/{id} Tests

    [Fact]
    public async Task UpdateFirstName_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        _factory.SetupUpdateFirstName();
        var id = "123";
        var newName = "UpdatedName";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/customer/{id}", newName);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    #endregion

    #region DELETE /api/customer/{id} Tests

    [Fact]
    public async Task DeleteCustomer_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var id = "123";
        var customer = new Customer { Id = id, firstName = "John" };
        _factory.SetupSearchCustomerById(id, customer);
        _factory.SetupDeleteCustomer(true);

        // Act
        var response = await _client.DeleteAsync($"/api/customer/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCustomer_ReturnsOk_WhenCustomerDoesNotExist()
    {
        // Arrange
        var id = "nonexistent";
        _factory.SetupSearchCustomerById(id, null);

        // Act
        var response = await _client.DeleteAsync($"/api/customer/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion
}
