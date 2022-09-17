class Program {
    static TelegramBotClient bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TOKEN")!);

    static void Main() {
        ConfigurationExtensions.ConfigureLogging();

        bot.StartReceiving(GlobalLogic.UpdateHandler, ErrorHandler, new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() });

        Console.ReadKey();
    }

    static Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        // TODO: Handle errors
        Log.Fatal("An exception occurred {@exception}", exception);

        throw new Exception(exception.Message);
    }

}
