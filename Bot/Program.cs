using System.Text.Json;
using System.Text.Json.Serialization;

class Program {
    static TelegramBotClient bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TOKEN")!);

    static void Main() {
        ConfigurationExtensions.ConfigureLogging();

        bot.StartReceiving(UpdateHandler, ErrorHandler, new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() });

        Console.ReadKey();
    }

    static Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        // TODO: Handle errors
        Log.Fatal("An exception occurred {@exception}", exception);

        throw new Exception(exception.Message);
    }

    static async Task UpdateHandler(ITelegramBotClient arg1, Update update, CancellationToken arg3)
    {
        Log.Warning("{@arst}", JsonSerializer.Serialize(update, new JsonSerializerOptions() {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull}));

        if (update.Type == UpdateType.Message)
        {
            var text = update?.Message?.Text;
            var id = update?.Message?.Chat.Id;
            var username = update?.Message?.Chat.Username;

            Message sent = await bot.SendTextMessageAsync(id!, "You said: " + text, cancellationToken: CancellationToken.None);

            Console.WriteLine($"{text} {id} {username}");
        }
    }
}
