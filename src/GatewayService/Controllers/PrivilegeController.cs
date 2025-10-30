using GatewayService.Models;
using GatewayService.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GatewayService.Controllers;

[ApiController]
[Route("api/v1/privilege")]
public class PrivilegeController : ControllerBase
{
    private readonly IGatewayService _gatewayService;
    private readonly ILogger<PrivilegeController> _logger;

    public PrivilegeController(IGatewayService gatewayService, ILogger<PrivilegeController> logger)
    {
        _gatewayService = gatewayService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPrivilegeInfo([FromHeader(Name = "X-User-Name")][Required] string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest(new { message = "Username is required" });
        }

        var response = await _gatewayService.GetPrivilegeInfoAsync(username);
        var json = JsonSerializer.Serialize(response);
        _logger.LogInformation(json);
        
        return response.IsSuccess ? Ok(response.Response) : StatusCode(503, new { message = "Bonus Service unavailable" });
    }
}