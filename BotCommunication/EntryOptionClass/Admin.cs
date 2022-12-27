namespace BotCommunication.EntryOptionClass
{
    public class Admin
    {
        private Update update;
        private SendMessage sendMessage;
        private ITelegramBotClient botClient;
        private ConsoleMessage consoleMessage;
        private CancellationToken cancellationToken;
        private string text { get; set; }
        public long id { get; set; }

        public Admin(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            this.id = update.Message.Chat.Id;
            this.text = update.Message.Text;
            this.botClient = botClient;
            this.update = update;
            this.cancellationToken = cancellationToken;

            sendMessage = new();
            consoleMessage = new();
        }

        public async Task AdminChoiceAsync()
        {
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Введите пароль.");
        }

        public async Task AdminPasswordAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Вы зашли как администратор. Для отправки сообщения обычному пользователю, введите его id.");
        }

        public async Task AdminPasswordErrorAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Неправильный пароль, ведите заново. Если вам нужно будет выйти из бота введите \"/stop\".");
        }

        public async Task AdminMessageIdErrorAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Несуществующий пользователь! Введите id заново.");
        }

        public async Task AdminMessageIdAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Отправте сообщение пользователю.");
        }

        public async Task AdminTransferToUserAsync(Update update, Dictionary<long, long> AdminTransferUser)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(AdminTransferUser[update.Message.Chat.Id]), $" Администратор отправил вам сообщение. \n {update.Message.Text}");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Вы отправили сообщение. Введите id для следующего сообщения.");
        }

        public async Task EnterChangePasswordAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Введите новый пароль.");
        }

        public async Task ResultChangePasswordAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Пароль успешно изменен.");
        }

        private void NewText(Update update) => text = update.Message.Text;
    }
}
