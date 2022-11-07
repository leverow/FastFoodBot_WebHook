using FastFoodBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace FastFoodBot.Controllers;
//[ApiController]
//[Route("api/bot")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger)
    {
        _logger = logger;
    }
    [HttpPost("api/bot")]
    public async Task<IActionResult> Post([FromServices] BotUpdateHandler handleUpdateService,
                                          [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }

    [HttpPost("good")]
    public IActionResult Good([FromQuery]string? str)
    {
        _logger.LogInformation($"--=-=-=-=[Request keldi] {str}");
        _logger.LogInformation($"--=-=-=-=[Request keldi] {str}");
        _logger.LogInformation($"--=-=-=-=[Request keldi] {str}");
        _logger.LogInformation($"--=-=-=-=[Request keldi] {str}");
        _logger.LogInformation($"--=-=-=-=[Request keldi] {str}");
        return Ok();
    }
}