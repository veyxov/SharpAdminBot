using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class TelegramBot
{
    private readonly Context _context;

    public TelegramBot(Context context)
    {
        _context = context;
    }

    public async Task StartBotAsync()
    {
        var bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TOKEN") ?? throw new Exception("No token."));
        using var cts = new CancellationTokenSource();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        bot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
                );

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Log.Warning("@{arst}", JsonSerializer.Serialize(update, new JsonSerializerOptions(){ WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull}));
            Log.Fatal("\n");

            if (update.Message is not null)
            {
                if (update.Message.ReplyToMessage is not null)
                {
                    if (update.Message.Text! == "-")
                    {
                        var reportedMessageId = update.Message.ReplyToMessage.MessageId;
                        var reporterId = update.Message.ReplyToMessage.From!.Id;
                        var reportMessageId = update.Message.MessageId;

                        await _context.Reports.AddAsync(new Report()
                        {
                            ChatId = update.Message.Chat.Id,
                            MessageId = reportedMessageId,
                            ReporterId = reporterId,
                            ReportMessageId = reportMessageId
                        });
                        await _context.SaveChangesAsync();

                        var query = _context.Reports.Where(x => x.MessageId == reportedMessageId);
                        var reports = await query.ToListAsync();
                        var reportsCount = reports.Count;

                        Log.Error("{@asrt}", reports);

                        if (reportsCount >= 3)
                        {
                            foreach (var report in reports)
                            {
                                await botClient.DeleteMessageAsync(messageId: (int)report.MessageId, chatId: report.ChatId);
                                await botClient.DeleteMessageAsync(messageId: (int)report.ReportMessageId, chatId: report.ChatId);
                            }
                        }
                    }
                }
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
            };

            return Task.CompletedTask;
        }
    }
}
