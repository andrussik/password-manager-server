using Core.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class SecretsController : BaseController
{
    private readonly ISecretService _secretService;

    public SecretsController(ISecretService secretService)
    {
        _secretService = secretService;
    }

    [HttpPost]
    public async Task<Secret> Save(Secret secret) => await _secretService.Save(secret, GetUserId());
    
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task Delete(Guid id) => await _secretService.Delete(id, GetUserId());
}