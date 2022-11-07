using System.Globalization;
using FastFoodBot.Constants;
using FastFoodBot.Entities;
using FastFoodBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FastFoodBot.Services;

public partial class BotUpdateHandler
{
    private async Task HandleTextMessageAsync(Message message)
    {
        var handler = message.Text switch
        {
            "/start" => HandleStartMessageAsync(message),
            "O'zbekcha 🇺🇿" or "Pусский 🇷🇺" => HandleLanguageAsync(message)
        };
        try
        {
            await handler;
        }
        catch(Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    private async Task HandleLanguageAsync(Message message)
    {
        var cultureString = StringConstants.LanguageNames.FirstOrDefault(v => v.Value == message.Text).Key;
        await _userService.UpdateLanguageCodeAsync(message.From?.Id, cultureString);

        CultureInfo.CurrentCulture = new CultureInfo(cultureString);
        CultureInfo.CurrentUICulture = new CultureInfo(cultureString);
        
        await _botClient.SendTextMessageAsync(message.Chat.Id,$"Siz {message.Text}ni tanladingiz.");
        await _userService.UpdateUserStepAsync(message.From?.Id, 2);
    }

    private async Task HandleStartMessageAsync(Message message)
    {
        _logger.LogError("HandleStartMessageAsync ishladi.");
        var user = await _userService.FilterUser(message.From?.Id, message.From?.Username);

        if(user.Step == 0)
        {
            var languages = StringConstants.LanguageNames
            .Select(l => l.Value)
            .ToArray();
            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                StringConstants.StartMessage(),
                replyMarkup: MarkupHelpers.GetReplyKeyboardMarkup(languages, 3)
            );
        }
        else
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithWebApp("Buyurtma berish",new WebAppInfo(){ Url = "https://leverow.uz"})
                    },
                });
            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Sizni «Briros» yetkazuv xizmati telegram-boti qutlaydi.",
                replyMarkup: inlineKeyboard
            );
        }
    }
}