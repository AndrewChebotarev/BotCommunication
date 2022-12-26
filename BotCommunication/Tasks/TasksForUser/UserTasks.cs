using BotCommunication.EntryOptionClass;

namespace BotCommunication.Tasks.TasksForUser
{
    public class UserTasks
    {
        public List<long> authorizationUserCheckList = new();
        private Dictionary<long, SimpleUser> simpleUserDictionary = new();
        public async Task StandardUserChoiceHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            SimpleUser simpleUser = new(botClient, update, cancellationToken);

            ExamSimpleUserDictionary(simpleUser.id);
            simpleUserDictionary.Add(update.Message.Chat.Id, simpleUser);

            await simpleUser.StandardUserChoiceAsync();
        }
        private void ExamSimpleUserDictionary(long id)
        {
            if (simpleUserDictionary.ContainsKey(id))
                simpleUserDictionary.Remove(id);
        }

        public async Task EnterPhoneNumberHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (ExamSimpleUserNotDictionary(update.Message.Chat.Id))
                return;

            authorizationUserCheckList.Add(update.Message.Chat.Id);

            await simpleUserDictionary[update.Message.Chat.Id].EnterPhoneNumberAsync(update);
        }
        private bool ExamSimpleUserNotDictionary(long id)
        {
            if (simpleUserDictionary.ContainsKey(id))
                return false;

            else
                return true;
        }

        public async Task UserTransferToAdministratorHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, List<string> admins)
        {
            await simpleUserDictionary[update.Message.Chat.Id].UserTransferToAdministratorAsync(update, admins);
        }
    }
}
