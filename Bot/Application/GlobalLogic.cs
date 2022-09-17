using System.Text.Json;
using System.Text.Json.Serialization;

public static class GlobalLogic
{
    public static async Task UpdateHandler(ITelegramBotClient arg1, Update update, CancellationToken arg3)
    {
        Log.Warning("{@arst}", JsonSerializer.Serialize(update, new JsonSerializerOptions() {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true}));

        if (update.Type == UpdateType.Message)
        {
            // If it is a reply
            if (update.Message!.ReplyToMessage is not null)
            {
                if (update.Message!.Text == "/report")
                {
                    Log.Warning("Passing to report handler.");
                    await Components.ReportHandler(arg1, update, arg3);
                }
            }
        }
    }

    public static Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        // TODO: Handle errors
        Log.Fatal("An exception occurred {@exception}", exception);

        throw new Exception(exception.Message);
    }
}
