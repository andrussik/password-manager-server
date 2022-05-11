using Core.Dtos;
using Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class SecretsController : BaseController
{
    private readonly ISecretService _secretService;

    public SecretsController(ISecretService secretService)
    {
        _secretService = secretService;
    }

    [HttpPost]
    public async Task<Secret> Create(SecretDto secret) => await _secretService.Create(secret);
    
    [HttpPut("{id:guid}")]
    public async Task<Secret> Update(Guid id, SecretDto secret) => await _secretService.Update(id, secret);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task Delete(Guid id) => await _secretService.Delete(id);
}