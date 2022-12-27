namespace BotCommunication.Tasks.TasksForUnauthorizedUser
{
    public class UnauthorizedUserTasks
    {
        private Dictionary<long, UnauthorizedUser> unauthorizedUserDictionary = new();
        public async Task StartHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UnauthorizedUser unauthorizedUser = new(botClient, update, cancellationToken);

            ExamAdminDictionary(unauthorizedUser.id);
            unauthorizedUserDictionary.Add(update.Message.Chat.Id, unauthorizedUser);

            await unauthorizedUser.StartAsync();
        }
        private void ExamAdminDictionary(long id)
        {
            if (unauthorizedUserDictionary.ContainsKey(id))
                unauthorizedUserDictionary.Remove(id);
        }

        public async Task RepeatHundleAsync(Update update)
        {
            if (ExamUnauthorizedUserNotDictionary(update.Message.Chat.Id)) 
                    return;

            await unauthorizedUserDictionary[update.Message.Chat.Id].RepeatAsync(update);
        }
        private bool ExamUnauthorizedUserNotDictionary(long id)
        {
            if (unauthorizedUserDictionary.ContainsKey(id))
                return false;

            else
                return true;
        }

        public async Task StopMessageHundleAsync(Update update) =>
            await unauthorizedUserDictionary[update.Message.Chat.Id].StopAsync(update);
    }
}
