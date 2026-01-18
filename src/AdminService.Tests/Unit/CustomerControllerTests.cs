using AdminService.Models;
using AdminService.Services;
using dc_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminService.Tests.Unit;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _mockCustomerService = new Mock<ICustomerService>();
        _controller = new CustomerController(_mockCustomerService.Object);
    }

    #region Get All Customers Tests

    [Fact]
    public async Task Get_ReturnsOkResult_WhenCustomersExist()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "John", lastName = "Doe", dogName = "Buddy" },
            new Customer { Id = "2", firstName = "Jane", lastName = "Smith", dogName = "Max" }
        };
        _mockCustomerService.Setup(s => s.GetAllCustomers()).ReturnsAsync(customers);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal(2, returnedCustomers.Count);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoCustomersExist()
    {
        // Arrange
        _mockCustomerService.Setup(s => s.GetAllCustomers()).ReturnsAsync(new List<Customer>());

        // Act
        var result = await _controller.Get();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region Search Tests

    [Fact]
    public async Task Search_ReturnsOkResult_WhenCustomersFound()
    {
        // Arrange
        var searchTerm = "Buddy";
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "John", lastName = "Doe", dogName = "Buddy" }
        };
        _mockCustomerService.Setup(s => s.SearchCustomers(searchTerm)).ReturnsAsync(customers);

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Single(returnedCustomers);
    }

    [Fact]
    public async Task Search_ReturnsNotFound_WhenNoCustomersFound()
    {
        // Arrange
        var searchTerm = "NonExistent";
        _mockCustomerService.Setup(s => s.SearchCustomers(searchTerm)).ReturnsAsync(new List<Customer>());

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Search_SearchesByDogName()
    {
        // Arrange
        var searchTerm = "Max";
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "Jane", lastName = "Smith", dogName = "Max" }
        };
        _mockCustomerService.Setup(s => s.SearchCustomers(searchTerm)).ReturnsAsync(customers);

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal("Max", returnedCustomers.First().dogName);
    }

    [Fact]
    public async Task Search_SearchesByFirstName()
    {
        // Arrange
        var searchTerm = "John";
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "John", lastName = "Doe", dogName = "Buddy" }
        };
        _mockCustomerService.Setup(s => s.SearchCustomers(searchTerm)).ReturnsAsync(customers);

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal("John", returnedCustomers.First().firstName);
    }

    [Fact]
    public async Task Search_SearchesByLastName()
    {
        // Arrange
        var searchTerm = "Smith";
        var customers = new List<Customer>
        {
            new Customer { Id = "1", firstName = "Jane", lastName = "Smith", dogName = "Max" }
        };
        _mockCustomerService.Setup(s => s.SearchCustomers(searchTerm)).ReturnsAsync(customers);

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal("Smith", returnedCustomers.First().lastName);
    }

    #endregion

    #region Create Customer Tests

    [Fact]
    public async Task Post_ReturnsCreatedAtAction_WhenCustomerCreated()
    {
        // Arrange
        var customer = new Customer
        {
            firstName = "John",
            lastName = "Doe",
            dogName = "Buddy",
            email = "john@example.com"
        };
        _mockCustomerService.Setup(s => s.CreateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Post(customer);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.Get), createdResult.ActionName);
        var returnedCustomer = Assert.IsType<Customer>(createdResult.Value);
        Assert.Equal("John", returnedCustomer.firstName);
    }

    [Fact]
    public async Task Post_CallsCreateAsync_WithCorrectCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            firstName = "Jane",
            lastName = "Smith",
            dogName = "Max"
        };

        // Act
        await _controller.Post(customer);

        // Assert
        _mockCustomerService.Verify(s => s.CreateAsync(It.Is<Customer>(c =>
            c.firstName == "Jane" &&
            c.lastName == "Smith" &&
            c.dogName == "Max"
        )), Times.Once);
    }

    #endregion

    #region Update Customer Tests

    [Fact]
    public async Task UpdateFirstName_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var id = "123";
        var newName = "UpdatedName";
        _mockCustomerService.Setup(s => s.UpdateFirstName(id, newName)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateFirstName(id, newName);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateFirstName_CallsService_WithCorrectParameters()
    {
        // Arrange
        var id = "456";
        var newName = "NewFirstName";

        // Act
        await _controller.UpdateFirstName(id, newName);

        // Assert
        _mockCustomerService.Verify(s => s.UpdateFirstName(id, newName), Times.Once);
    }

    #endregion

    #region Delete Customer Tests

    [Fact]
    public async Task Delete_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var id = "123";
        var customer = new Customer { Id = id, firstName = "John" };
        _mockCustomerService.Setup(s => s.SearchCustomerById(id)).ReturnsAsync(customer);
        _mockCustomerService.Setup(s => s.DeleteCustomer(id)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<OkResult>(result);
        _mockCustomerService.Verify(s => s.DeleteCustomer(id), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCustomerDoesNotExist()
    {
        // Arrange
        var id = "nonexistent";
        _mockCustomerService.Setup(s => s.SearchCustomerById(id)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<OkResult>(result);
        _mockCustomerService.Verify(s => s.DeleteCustomer(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DoesNotCallDeleteCustomer_WhenCustomerNotFound()
    {
        // Arrange
        var id = "missing";
        _mockCustomerService.Setup(s => s.SearchCustomerById(id)).ReturnsAsync((Customer?)null);

        // Act
        await _controller.Delete(id);

        // Assert
        _mockCustomerService.Verify(s => s.DeleteCustomer(It.IsAny<string>()), Times.Never);
    }

    #endregion
}
