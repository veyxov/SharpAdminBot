using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program {
    static TelegramBotClient bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TOKEN")!);
    static void Main() {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = new UpdateType[]
            {
                UpdateType.Message,
                UpdateType.EditedMessage
            }
        };

        bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

        Console.ReadKey();
    }

    static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }

    static async Task UpdateHandler(ITelegramBotClient arg1, Update update, CancellationToken arg3)
    {
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
