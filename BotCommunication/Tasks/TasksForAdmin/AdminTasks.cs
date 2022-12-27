namespace BotCommunication.Tasks.TasksForAdmin
{
    public class AdminTasks
    {
        private Dictionary<long, Admin> adminDictionary = new();
        public List<long> authorizationAdminCheckList = new();

        public async Task AdminChoiceHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Admin admin = new(botClient, update, cancellationToken);

            ExamAdminDictionary(admin.id);
            adminDictionary.Add(update.Message.Chat.Id, admin);

            await admin.AdminChoiceAsync();
        }
        private void ExamAdminDictionary(long id)
        {
            if (adminDictionary.ContainsKey(id))
                adminDictionary.Remove(id);
        }

        public async Task AdminPasswordHundleAsync(Update update)
        {
            authorizationAdminCheckList.Add(update.Message.Chat.Id);

            await adminDictionary[update.Message.Chat.Id].AdminPasswordAsync(update);
        }

        public async Task AdminPasswordErrorHundleAsync(Update update) => 
            await adminDictionary[update.Message.Chat.Id].AdminPasswordErrorAsync(update);

        public async Task AdminMessageIdHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].AdminMessageIdAsync(update);

        public async Task AdminMessageIdErrorHundleAsync(Update update) =>
            await adminDictionary[update.Message.Chat.Id].AdminMessageIdErrorAsync(update);

        public async Task AdminTransferToUserHundleAsync(Update update, Dictionary<long, long> AdminTransferUser) =>
           await adminDictionary[update.Message.Chat.Id].AdminTransferToUserAsync(update, AdminTransferUser);

        public async Task EnterChangePasswordHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].EnterChangePasswordAsync(update);

        public async Task ResultChangePasswordHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ResultChangePasswordAsync(update);
    }
}
