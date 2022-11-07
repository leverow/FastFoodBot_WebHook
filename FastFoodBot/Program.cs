using FastFoodBot;
using FastFoodBot.Data;
using FastFoodBot.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

builder.Services.AddRazorPages();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));

builder.Services.AddScoped<BotUpdateHandler>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();

builder.Services.AddDbContext<AppDbContext>(options
=> options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
});


app.UseRouting();
app.UseCors();

//app.UseEndpoints(endpoints =>
//{
//    var token = botConfig.BotToken;
//    endpoints.MapControllerRoute(name: "tgwebhook",
//                                 pattern: $"bot/{token}",
//                                 new { controller = "Webhook", action = "Post" });
//    endpoints.MapControllers();
//});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();