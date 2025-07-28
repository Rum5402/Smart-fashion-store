using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            
            return Ok(new
            {
                Status = report.Status.ToString(),
                Timestamp = DateTime.UtcNow,
                Checks = report.Entries.Select(e => new
                {
                    Name = e.Key,
                    Status = e.Value.Status.ToString(),
                    Description = e.Value.Description
                })
            });
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { Message = "Pong", Timestamp = DateTime.UtcNow });
        }
    }
} 