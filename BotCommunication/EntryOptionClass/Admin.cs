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

        public async Task AppointAssistantAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Для назначения помощника, введите его id.");
        }

        public async Task ResultAppointAssistantAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(update.Message.Text), $" Администратор принял ваш запрос. Вы стали помощником. \n Для отправки сообщения обычному пользователю, введите его id.");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Данный пользователь стал помощником.");
        }

        public async Task ErrorAppointAssistantAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Несуществующий пользователь! Для добавления помощника введите команду \"/Назначить помощника\". Если хотите отправить сообщение пользователю, а не помощнику введите его id.");
        }

        public async Task DeleteAssistantAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Введите id помощника, которго хотите удалить.");
        }
        public async Task ResultDeleteAssistantAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(update.Message.Text), $" Администратор отнял прова помощника. \n Введите /start для начала работы.");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Данный помощник удален. Введите id для отправки сообщения.");
        }
        public async Task ErrorDeleteAssistantAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Несуществующий пользователь! Введите правильный id. Если хотите вернуться, введите \"Назад\".");
        }

        public async Task BlackListAddAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Введите id пользователя, для добавления в черный список.");
        }
        public async Task ResultBlackListAddAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(update.Message.Text), $" Администратор добавил вас в черный список.");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Данный пользователь в черном списке. Если хотите убрать его из черного списка введите команду \"Бедый список\". Введите id пользователя для передачи сообщения.");
        }
        public async Task ErrorBlackListAddAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Несуществующий пользователь! Для добавления в черный список введите верный id. Если хотите вернуться, введите \"Назад\".");
        }

        public async Task WhiteListAddAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Введите id пользователя, для удаления из черного списка.");
        }
        public async Task ResultWhiteListAddAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, Convert.ToInt64(update.Message.Text), $" Администратор убрал вас из черного списка. Введите \"/start\" для начала работы.");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Данный пользователь убран из черного списка. Введите id пользователя для передачи сообщения.");
        }
        public async Task ErrorWhiteListAddAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, " Несуществующий пользователь! Для удаления из черного списка введите верный id. Если хотите вернуться, введите \"Назад\".");
        }

        public async Task AllUnauthorizedUserListAddAsync(Update update, long UnauthorizedUser)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, $"Незарегистрированный пользователь - {UnauthorizedUser}.");
        }

        public async Task AllUserListAddAsync(Update update, long User)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, $"Зарегистрированный пользователь - {User}.");
        }

        public async Task AllAssistantListAddAsync(Update update, long Assistent)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, $"Помощник - {Assistent}.");
        }

        public async Task AllAdminListAddAsync(Update update, long Admin)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, $"Администратор - {Admin}.");
        }

        public async Task AllCommandListAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Все доступные комманды: \n" + "/stop - начат все заново. \n" + "/start - запустить бота. \n" +
                "/Смена пароля - сменить пароль. \n" + "/Добавить помощника - после запроса от пользователя, назначить его помощником. \n" + "/Удалить помощника - удаление помощника. \n" + 
                "/Черный список - добавить пользователя в черный список. \n" + "/Белый список - убрать пользователя из черного списка. \n" + "/All user list - просмотр всех пользователей. \n" +
                "/Help - просмотр команд. \n" + "/Назад -  вернуться на один шаг назад (работает не везде - если не получается введите /stop)");
        }

        public async Task AllUserListSendAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Отправьте сообщение всем пользователям. Если не нужно то введите \"/stop\".");
        }

        public async Task ResultAllUserListSendAsync(Update update, long User)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, User, $"Сообщение от администратор: {update.Message.Text}");
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Вы отправили сообщения всем пользователям. Введите id пользователя для передачи сообщения.");
        }

        public async Task ErrorAllUserListSendAsync(Update update)
        {
            NewText(update);
            consoleMessage.SendingConsole(text, id);
            await sendMessage.SendingMessage(botClient, cancellationToken, id, "Нет пользователей.");
        }

        private void NewText(Update update) => text = update.Message.Text;
    }
}
