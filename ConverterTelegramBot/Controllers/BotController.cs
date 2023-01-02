using System;
using System.Threading.Tasks;
using ConverterTelegramBot.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Controllers;

[ApiController]
[Route("api/message")]
public class BotController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;

    public BotController(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody] object update)
    {
        var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

        var chat = upd.Message?.Chat;

        if (chat == null)
        {
            return Ok();
        }

        try
        {
            await _commandExecutor.Execute(upd);
        }
        catch (Exception ex)
        {
            return Ok();
        }

        return Ok();
    }
}
