using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Watering.Bot.Bot.Interfaces;
using Watering.Bot.Commands.Interfaces;
using Watering.Bot.Options;

namespace Watering.Bot.Bot;

internal class WateringBot(ICommandExecutor commandExecutor, BotOptions options, ILogger<WateringBot> logger)
    : IBot
{
    private readonly ITelegramBotClient _bot = new TelegramBotClient(options.Token);

    private CancellationTokenSource? _cts;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message],
        };

        _cts = new CancellationTokenSource();
        _bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, _cts.Token);
        
        var me = await _bot.GetMe(_cts.Token);
        Console.WriteLine($"{me.FirstName} запущен!");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }
    }
    
    private async Task UpdateHandler(
        ITelegramBotClient botClient, 
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            if (update.Type != UpdateType.Message)
                return;
            
            var message = update.Message!;
            var textUser = message.From;
            logger.LogInformation($"{textUser.FirstName} ({textUser.Id}) написал: {message.Text}");

            await commandExecutor.ExecuteAsync(_bot, message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
        }
    }
    
    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var errorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => "Unhandled exception"
        };

        logger.LogError(error, errorMessage);
        return Task.CompletedTask;
    }
}