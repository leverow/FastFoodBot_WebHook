namespace FastFoodBot.Constants;

public class StringConstants
{
    public static string StartMessage()
    => $@"🇷🇺 Здравствуйте! Давайте для начала выберем язык обслуживания!

🇺🇿 Keling, avvaliga xizmat ko’rsatish tilini tanlab olaylik!";

    public static Dictionary<string, string> LanguageNames => new()
    {
        { "uz-Uz", "O'zbekcha 🇺🇿" },
        { "ru-Ru", "Pусский 🇷🇺" },
    };   
    
}