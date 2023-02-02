namespace BotCommunication.EntryOptionClass
{
    public class Assistant
    {
        private Update update;
        private SendMessage sendMessage;
        private ITelegramBotClient botClient;
        private ConsoleMessage consoleMessage;
        private CancellationToken cancellationToken;
        private string text { get; set; }
        public long id { get; set; }

        public Assistant(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            this.id = update.Message.Chat.Id;
            this.text = update.Message.Text;
            this.botClient = botClient;
            this.update = update;
            this.cancellationToken = cancellationToken;

            sendMessage = new();
            consoleMessage = new();
        }

        public async Task AssistantMessageIdErrorAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Несуществующий пользователь! Введите id заново.");
        }

        public async Task AssistantMessageIdAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Отправте сообщение пользователю.");
        }

        public async Task AssistantTransferToUserAsync(Update update, Dictionary<long, long> AssistantTransferUser)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(AssistantTransferUser[update.Message.Chat.Id]), $" Помощник отправил вам сообщение. \n {update.Message.Text}");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Вы отправили сообщение. Введите id для следующего сообщения.");
        }

        private void NewText(Update update) => text = update.Message.Text;
    }
}
