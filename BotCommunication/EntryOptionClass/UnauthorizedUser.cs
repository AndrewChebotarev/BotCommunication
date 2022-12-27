namespace BotCommunication.EntryOptionClass
{
    public class UnauthorizedUser
    {
        private string text { get; set; }

        private Update update;
        private SendMessage sendMessage;
        private ITelegramBotClient botClient;
        private ConsoleMessage consoleMessage;
        private CancellationToken cancellationToken;

        public long id { get; set; }

        public UnauthorizedUser(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            this.id = update.Message.Chat.Id;
            this.text = update.Message.Text;
            this.botClient = botClient;
            this.update = update;
            this.cancellationToken = cancellationToken;

            sendMessage = new();
            consoleMessage = new();
        }

        public async Task StartAsync()
        {
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, update.Message.Chat.Id, "Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\". Если вам нужно будет выйти из бота введите \"/stop\".");
        }

        public async Task RepeatAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, update.Message.Chat.Id, "Бот не понял вашего сообщения. Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\")");
        }

        public async Task StopAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, update.Message.Chat.Id, "Вы очистили бота. Сделайте вход.");
        }

        private void NewText(Update update) => text = update.Message.Text;
    }
}
