using Microsoft.EntityFrameworkCore;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<Context>(o => o.UseSqlite("DataSource=app.db"));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();


        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger();

        await new TelegramBot(new Context(new DbContextOptionsBuilder().UseSqlite("DataSource=app.db").Options)).StartBotAsync();

        app.Run();
    }
}
