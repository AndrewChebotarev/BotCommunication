namespace KolkovBot
{
    class Program
    {
        private static TelegramBotClient botClient = new TelegramBotClient("5980742443:AAHF2vbzphLGA9PW2NU7VzS0ovAB9REHsoo");
        private static CancellationTokenSource cts = new CancellationTokenSource();
        public async static Task Main(string[] args)
        {
            HundleUpdate hundleUpdate = new();
            HandlePollingError hundlePollingError = new();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            Update update = new();

            botClient.StartReceiving(
                updateHandler: hundleUpdate.HandleUpdateAsync,
                pollingErrorHandler: hundlePollingError.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            await ConsoleHelper();
        }

        private async static Task ConsoleHelper()
        {
            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }
    }
}