using System;
using System.Net;
using dc_api.Models;
using dc_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dc_api.Controllers;

[Route("api/customer")]
public class CustomerController: ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService) {
        _customerService = customerService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get() {
        
        var customers = await _customerService.GetAllCustomers();

        if (!customers.Any()) return NotFound();
        
        return Ok(customers);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery]string search) {
        var searchResults = await _customerService.SearchCustomers(search);
        
        if (searchResults.Any()) return Ok(searchResults);

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Customer customer) {
        await _customerService.CreateAsync(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id}, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFirstName(string id, [FromBody] string name) {
        await _customerService.UpdateFirstName(id, name);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>Delete(string id)
    {
        var customer = await _customerService.SearchCustomerById(id);

        if (customer != null) await _customerService.DeleteCustomer(id);
        
        return Ok();
    }
}
