using AdminService.Models;
using AdminService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AdminService.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ICustomerService> MockCustomerService { get; } = new Mock<ICustomerService>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing ICustomerService registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ICustomerService));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add our mock service
            services.AddSingleton(MockCustomerService.Object);
        });

        builder.UseEnvironment("Development");
    }

    public void SetupGetAllCustomers(List<Customer> customers)
    {
        MockCustomerService.Setup(s => s.GetAllCustomers()).ReturnsAsync(customers);
    }

    public void SetupSearchCustomers(string search, List<Customer> customers)
    {
        MockCustomerService.Setup(s => s.SearchCustomers(search)).ReturnsAsync(customers);
    }

    public void SetupSearchCustomerById(string id, Customer? customer)
    {
        MockCustomerService.Setup(s => s.SearchCustomerById(id)).ReturnsAsync(customer);
    }

    public void SetupCreateAsync()
    {
        MockCustomerService.Setup(s => s.CreateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
    }

    public void SetupUpdateFirstName()
    {
        MockCustomerService.Setup(s => s.UpdateFirstName(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
    }

    public void SetupDeleteCustomer(bool result)
    {
        MockCustomerService.Setup(s => s.DeleteCustomer(It.IsAny<string>())).ReturnsAsync(result);
    }
}
