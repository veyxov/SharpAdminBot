using System.Text.Json;
using System.Text.Json.Serialization;

public static class GlobalLogic
{
    public static async Task UpdateHandler(ITelegramBotClient arg1, Update update, CancellationToken arg3)
    {
        Log.Warning("{@arst}", JsonSerializer.Serialize(update, new JsonSerializerOptions() {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true}));

    }
}
