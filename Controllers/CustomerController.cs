using System;
using dc_api.Models;
using dc_api.Services;
using Microsoft.AspNetCore.Mvc;


namespace dc_api.Controllers;

[Controller]
[Route("api/[controller]")]
public class CustomerController: Controller {

    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService) {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<List<Customer>> Get() {
        return await _customerService.GetAsync();
    }

    [HttpGet("search")]
    public async Task<List<Customer>> Search([FromQuery]string search) {
        return await _customerService.SearchAsync(search);
    }


    [HttpPost]
    //                                     accept data playload from body
    public async Task<IActionResult> Post([FromBody] Customer customer) {
        await _customerService.CreateAsync(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id}, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFirstName(string id, [FromBody] string name) {
        await _customerService.UpdateFirstNameAsync(id, name);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await _customerService.DeleteAsync(id);
        return NoContent();
    }



}