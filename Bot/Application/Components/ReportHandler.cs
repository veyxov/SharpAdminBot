public static partial class Components {
    public static async Task ReportHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await client.DeleteMessageAsync(update.Message!.Chat.Id, update.Message.ReplyToMessage!.MessageId);
        Log.Error("Deleted");
    }
}
