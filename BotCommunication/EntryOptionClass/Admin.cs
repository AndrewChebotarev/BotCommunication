using static System.Net.Mime.MediaTypeNames;

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
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Введите пароль");
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
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Неправильный пароль, ведите заново. Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\". Если вам нужно будет выйти из бота введите \"/stop\".");
        }

        public async Task AdminMessageIdErrorAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Несуществующий пользователь! Введите id заново.");
        }

        public async Task AdminMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Отправте сообщение пользователю.");
        }

        private void NewText(Update update)
        {
            text = update.Message.Text;
        }
    }
}
