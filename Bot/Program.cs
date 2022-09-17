class Program {
    static TelegramBotClient bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TOKEN")!);

    static void Main() {
        ConfigurationExtensions.ConfigureLogging();

        bot.StartReceiving(GlobalLogic.UpdateHandler, GlobalLogic.ErrorHandler, new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() });

        Console.ReadKey();
    }
}
