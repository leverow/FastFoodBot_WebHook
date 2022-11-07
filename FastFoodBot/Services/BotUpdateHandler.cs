using System.Globalization;
using FastFoodBot.Entities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FastFoodBot.Services;

public partial class BotUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<BotUpdateHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private UserService _userService;

    public BotUpdateHandler(
        ITelegramBotClient botClient,
        ILogger<BotUpdateHandler> logger,
        IServiceScopeFactory scopeFactory)
    {
        _botClient = botClient;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task EchoAsync(Update update)
    {
        using var scope = _scopeFactory.CreateScope();

        _userService = scope.ServiceProvider.GetRequiredService<UserService>();

        var culture = await GetCultureForUser(update);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        var handler = update.Type switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            UpdateType.Message            => BotOnMessageReceived(update.Message!),
            UpdateType.EditedMessage      => BotOnMessageReceived(update.EditedMessage!),
            UpdateType.CallbackQuery      => BotOnCallbackQueryReceived(update.CallbackQuery!),
            UpdateType.InlineQuery        => BotOnInlineQueryReceived(update.InlineQuery!),
            UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult!),
            _                             => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    private async Task<CultureInfo> GetCultureForUser(Update update)
    {
        User? from = update.Type switch
        {
            UpdateType.Message => update.Message?.From,
            UpdateType.EditedMessage => update?.EditedMessage?.From,
            UpdateType.CallbackQuery => update?.CallbackQuery?.From,
            UpdateType.InlineQuery => update?.InlineQuery?.From,
            _ => update?.Message?.From
        };

        var result = await _userService.AddUserAsync(new AppUser()
        {
            FirstName = from?.FirstName,
            LastName = from?.LastName,
            UserId = from?.Id,
            Username = from?.Username,
            LanguageCode = from?.LanguageCode,
            CreatedAt = DateTime.UtcNow
        });

        if(result.IsSuccess)
        {
            _logger.LogInformation($"New user successfully added: {from?.Id}, Name: {from?.FirstName}");
        }
        else
        {
            _logger.LogInformation($"User not added: {from?.Id}, Error: {result.ErrorMessage}");
        }

        var language = await _userService.GetLanguageCodeAsync(from?.Id);

        return new CultureInfo(language ?? "uz-Uz");
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public Task HandleErrorAsync(Exception exception)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        return Task.CompletedTask;
    }
}