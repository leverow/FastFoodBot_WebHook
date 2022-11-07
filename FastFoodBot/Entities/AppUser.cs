namespace FastFoodBot.Entities;

public class AppUser : EntityBase
{
    public long? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? LanguageCode { get; set; }
    public string? PhoneNumber { get; set; }
    public ushort Step { get; set; }
}