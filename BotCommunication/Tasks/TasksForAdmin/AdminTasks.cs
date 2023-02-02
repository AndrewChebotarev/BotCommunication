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

        public async Task AppointAssistantHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].AppointAssistantAsync(update);

        public async Task ResultAppointAssistantHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ResultAppointAssistantAsync(update);

        public async Task ErrorAppointAssistantHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ErrorAppointAssistantAsync(update);

        public async Task DeleteAssistantHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].DeleteAssistantAsync(update);

        public async Task ResultDeleteAssistantHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ResultDeleteAssistantAsync(update);

        public async Task ErrorDeleteAssistantHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ErrorDeleteAssistantAsync(update);

        public async Task BlackListAddHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].BlackListAddAsync(update);

        public async Task ResultBlackListAddHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ResultBlackListAddAsync(update);

        public async Task ErrorBlackListAddHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ErrorBlackListAddAsync(update);

        public async Task WhiteListAddHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].WhiteListAddAsync(update);

        public async Task ResultWhiteListAddHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ResultWhiteListAddAsync(update);

        public async Task ErrorWhiteListAddHundleAsync(Update update) => await adminDictionary[update.Message.Chat.Id].ErrorWhiteListAddAsync(update);

        public async Task AllUnauthorizedUserListAddHundleAsync(Update update, long UnauthorizedUser) =>
            await adminDictionary[update.Message.Chat.Id].AllUnauthorizedUserListAddAsync(update, UnauthorizedUser);

        public async Task AllUserListAddHundleAsync(Update update, long User) =>
            await adminDictionary[update.Message.Chat.Id].AllUserListAddAsync(update, User);

        public async Task AllAssistantListAddHundleAsync(Update update, long Assistant) =>
            await adminDictionary[update.Message.Chat.Id].AllAssistantListAddAsync(update, Assistant);

        public async Task AllAdminListAddHundleAsync(Update update, long Admin) =>
            await adminDictionary[update.Message.Chat.Id].AllAdminListAddAsync(update, Admin);

        public async Task AllCommandHundleAsync(Update update) =>
            await adminDictionary[update.Message.Chat.Id].AllCommandListAsync(update);
        public async Task AllUserListSendHundleAsync(Update update) =>
            await adminDictionary[update.Message.Chat.Id].AllUserListSendAsync(update);
        public async Task ResultAllUserListSendHundleAsync(Update update, long user) =>
            await adminDictionary[update.Message.Chat.Id].ResultAllUserListSendAsync(update, user);

        public async Task ErrorAllUserListSendHundleAsync(Update update) =>
            await adminDictionary[update.Message.Chat.Id].ErrorAllUserListSendAsync(update);

    }
}
