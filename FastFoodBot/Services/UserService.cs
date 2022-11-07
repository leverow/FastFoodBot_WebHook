using FastFoodBot.Data;
using FastFoodBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFoodBot.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _context;

    public UserService(
        ILogger<UserService> logger,
        AppDbContext context
    )
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }    
    public async Task<AppUser?> GetUserByUserId(long? userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(_context.Set<AppUser>());

        return await _context.Set<AppUser>().FirstOrDefaultAsync(u => u.UserId == userId);
    }
    
    public async Task<(bool IsSuccess, string? ErrorMessage)> AddUserAsync(AppUser user)
    {
        if(await Exists(user.UserId))
            return (false, "User exists");
        try
        {
            var result = await _context.Set<AppUser>().AddAsync(user);

            await _context.SaveChangesAsync();
            
            return (true, null);
        }
        catch(Exception e)
        {
            return (false, e.Message);
        }
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateLanguageCodeAsync(long? userId, string? languageCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);

        var user = await GetUserByUserId(userId);

        if(user is null)
            return (false, "User not found");

        user.LanguageCode = languageCode;
        _context?.Users?.Update(user);
        await _context?.SaveChangesAsync()!;

        return (true, null);
    }

    public async Task<bool> Exists(long? userId)
        => await _context.Set<AppUser>().AnyAsync(u => u.UserId == userId);
    public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateUserStepAsync(long? userId, ushort step)
    {
        var user = await GetUserByUserId(userId);

        if (user is null)
            return (false, "User not found");

        user.Step = step;
        _context?.Users?.Update(user);
        await _context?.SaveChangesAsync()!;

        return (true, null);
    }

    public async Task<string?> GetLanguageCodeAsync(long? userId)
    {
        var user = await GetUserByUserId(userId);

        return user?.LanguageCode;
    }
    public async Task<AppUser> FilterUser(long? userId, string? username)
    {
        var user = await GetUserByUserId(userId);
        if (user is null)
        {
            user = new AppUser
            {
                UserId = userId,
                Username = username,
            };

            await AddUserAsync(user);
        }
        return user;
    }
}