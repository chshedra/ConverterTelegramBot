﻿using System.Threading.Tasks;
using Telegram.Bot;

namespace ConverterTelegramBot.Services;

/// <summary>
/// Get and send data from chat
/// </summary>
public interface IChatDataProvider
{
    /// <summary>
    /// Send text message to user
    /// </summary>
    void SendMessage(long chatId, string message);

    /// <summary>
    /// Send pdf file to user
    /// </summary>
    void SendPdfFile(long chatId, byte[] fileBytes);

    Task SaveFileFromChat(string fileId, long chatId);
}
