﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConverterTelegramBot.Commands;
using ConverterTelegramBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Update = Telegram.Bot.Types.Update;

namespace ConverterTelegramBot.Services;

/// <inheritdoc/>
public class CommandExecutor : ICommandExecutor
{
    /// <summary>
    /// List of commands.
    /// </summary>
    private readonly List<ICommand> _commands;

    private readonly IChatDataProvider _chatDataProvider;

    /// <summary>
    /// Object of last executed command.
    /// </summary>
    private ICommand _lastCommand;

    /// <summary>
    /// Create instance of service.
    /// </summary>
    /// <param name="serviceProvider">Creating commands service</param>
    public CommandExecutor(
        IServiceProvider serviceProvider,
        IChatDataProvider chatDataProvider,
        BotDbContext context
    )
    {
        _commands = serviceProvider.GetServices<ICommand>().ToList();
        _chatDataProvider = chatDataProvider;
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
                case CommandMessage.ConvertMessage:
                {
                    await ExecuteCommand(CommandName.GetTextCommandName, update);
                    return;
                }
                case CommandMessage.SeparateMessage:
                {
                    await ExecuteCommand(CommandName.RequestFileCommandName, update);
                    return;
                }
                case CommandMessage.CompressMessage:
                {
                    await ExecuteCommand(CommandName.RequestFileCommandName, update);
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
            await ExecuteCommand(CommandName.StartCommandName, update);
            return;
        }

        switch (_lastCommand?.Name)
        {
            case CommandName.RequestFileCommandName:
            {
                await _chatDataProvider.SaveFileFromChat(
                    update.Message.Document.FileId,
                    update.Message.Chat.Id
                );

                await ExecuteCommand(CommandName.CompressPdfCommand, update);
                break;
            }
            case CommandName.RequestPagesCommandName:
            {
                await ExecuteCommand(CommandName.SeparateCommandName, update);
                break;
            }
        }
    }

    /// <summary>
    /// Executes definite command async.
    /// </summary>
    private async Task ExecuteCommand(string commandName, Update update)
    {
        _lastCommand = _commands.First(x => x.Name == commandName);
        await _lastCommand.ExecuteAsync(update);
    }
}
