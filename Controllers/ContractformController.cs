using System;
using Microsoft.AspNetCore.Mvc;
using DC_CONTRACTFORM.Models;
using DC_CONTRACTFORM.Services;

namespace DC_CONTRACTFORM.Controllers;

[Controller]
[Route("api/[controller]")]
public class ContractformController: Controller {

    private readonly MongoDBService _mongoDBService;

    public ContractformController(MongoDBService mongoDBService) {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<Contractform>> Get() {
        return await _mongoDBService.GetAsync();
    }

    [HttpPost]
    //                                     accept data playload from body
    public async Task<IActionResult> Post([FromBody] Contractform contractform) {
        await _mongoDBService.CreateAsync(contractform);
        return CreatedAtAction(nameof(Get), new { id = contractform.Id}, contractform);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AddToContractform(string id, [FromBody] string contractformId) {
        await _mongoDBService.UpdateFirstNameAsync(id, contractformId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }



}