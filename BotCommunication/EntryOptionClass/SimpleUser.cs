namespace BotCommunication.EntryOptionClass
{
    public class SimpleUser
    {
        private string text { get; set; }
        private string phone { get; set; }

        private Update update;
        private DateTime dateTime;
        private SendMessage sendMessage;
        private ITelegramBotClient botClient;
        private ConsoleMessage consoleMessage;
        private CancellationToken cancellationToken;

        public long id { get; set; }

        public SimpleUser(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            this.id = update.Message.Chat.Id;
            this.text = update.Message.Text;
            this.botClient = botClient;
            this.update = update;
            this.cancellationToken = cancellationToken;

            sendMessage = new();
            consoleMessage = new();

        }

        public async Task StandardUserChoiceAsync()
        {
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Введите номер телефона.");
        }

        public async Task EnterPhoneNumberAsync(Update update)
        {
            NewText(update);
            phone = update.Message.Text;
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Введите сообщение для передачи администратору");
        }

        public async Task UserTransferToAdministratorAsync(Update update, long Admin)
        {
            NewText(update);
            dateTime = DateTime.Now;
            consoleMessage.SendingConsole(text, id);

            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(Admin), $" Сообщение от {id}.\n Номер телефона {phone}. \n Дата и время отправки:{dateTime}. \n {update.Message.Text}");

            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Вы отправили сообщение администратору.");
        }

        private void NewText(Update update) => text = update.Message.Text;
    }
}
