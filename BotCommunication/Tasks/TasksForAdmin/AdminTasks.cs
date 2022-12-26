using BotCommunication.EntryOptionClass;

namespace BotCommunication.Tasks.TasksForAdmin
{
    public class AdminTasks
    {
        private Dictionary<long, Admin> adminDictionary = new();
        public List<long> authorizationAdminCheckList = new();

        public async Task<bool> AdminChoiceHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Admin admin = new(botClient, update, cancellationToken);

            ExamAdminDictionary(admin.id);
            adminDictionary.Add(update.Message.Chat.Id, admin);

            await admin.AdminChoiceAsync();

            return true;
        }
        private void ExamAdminDictionary(long id)
        {
            if (adminDictionary.ContainsKey(id))
                adminDictionary.Remove(id);
        }

        public async Task AdminPasswordHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, List<string> admins)
        {
            admins.Add(update.Message.Chat.Id.ToString());

            authorizationAdminCheckList.Add(update.Message.Chat.Id);

            await adminDictionary[update.Message.Chat.Id].AdminPasswordAsync(update);
        }

        public async Task AdminPasswordErrorHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) => 
            await adminDictionary[update.Message.Chat.Id].AdminPasswordErrorAsync(update);


        public async Task AdminMessageIdErrorHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) =>
            await adminDictionary[update.Message.Chat.Id].AdminMessageIdErrorAsync(update);

        public async Task AdminMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ConsoleMessage(update);

            await SendingMessage(botClient, update.Message.Chat.Id, cancellationToken, "Отправте сообщение пользователю.");

            AdminUser.Add(update.Message.Chat.Id.ToString(), update.Message.Text);

            exam = true;
        }

    }
}
