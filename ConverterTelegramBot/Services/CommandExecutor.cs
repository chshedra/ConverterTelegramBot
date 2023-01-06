using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConverterTelegramBot.Commands;
using ConverterTelegramBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Services;

/// <inheritdoc/>
public class CommandExecutor : ICommandExecutor
{
    /// <summary>
    /// List of commands
    /// </summary>
    private readonly List<ICommand> _commands;

    /// <summary>
    /// Object of last executed command
    /// </summary>
    private ICommand _lastCommand;

    /// <summary>
    /// Create instance of service
    /// </summary>
    /// <param name="serviceProvider">Creating commands service</param>
    public CommandExecutor(IServiceProvider serviceProvider)
    {
        _commands = serviceProvider.GetServices<ICommand>().ToList();
    }

    /// <inheritdoc/>
    public async Task Execute(Update update)
    {
        if (update?.Message?.Chat == null && update?.CallbackQuery == null)
        {
            return;
        }

        if (update.Type == UpdateType.Message)
        {
            switch (update.Message?.Text)
            {
                case CommandText.ConvertMessage:
                {
                    await ExecuteCommand("GetTextCommand", update);
                    return;
                }
                case CommandText.SeparateMessage:
                {
                    await ExecuteCommand("RequestFileCommand", update);
                    return;
                }
            }
        }

        if (
            update.Message != null
            && update.Message.Text != null
            && update.Message.Text.Contains("/start")
        )
        {
            await ExecuteCommand("StartCommand", update);
            return;
        }

        switch (_lastCommand?.Name)
        {
            case "RequestFileCommand":
            {
                await ExecuteCommand("RequestPagesRangeCommand", update);
                break;
            }
            case "RequestPagesRangeCommand":
            {
                await ExecuteCommand("SeparateCommand", update);
                break;
            }
        }
    }

    /// <summary>
    /// Execute definite command async
    /// </summary>
    private async Task ExecuteCommand(string commandName, Update update)
    {
        _lastCommand = _commands.First(x => x.Name == commandName);
        await _lastCommand.ExecuteAsync(update);
    }
}
