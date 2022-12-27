using BotCommunication.EntryOptionClass;

namespace BotCommunication.Tasks.TasksForUser
{
    public class UserTasks
    {
        public List<long> authorizationUserCheckList = new();
        private Dictionary<long, SimpleUser> UserDictionary = new();
        public async Task StandardUserChoiceHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            SimpleUser simpleUser = new(botClient, update, cancellationToken);

            ExamSimpleUserDictionary(simpleUser.id);
            UserDictionary.Add(update.Message.Chat.Id, simpleUser);

            await simpleUser.StandardUserChoiceAsync();
        }
        private void ExamSimpleUserDictionary(long id)
        {
            if (UserDictionary.ContainsKey(id))
                UserDictionary.Remove(id);
        }

        public async Task EnterPhoneNumberHundleAsync(Update update)
        {
            if (ExamSimpleUserNotDictionary(update.Message.Chat.Id))
                return;

            authorizationUserCheckList.Add(update.Message.Chat.Id);

            await UserDictionary[update.Message.Chat.Id].EnterPhoneNumberAsync(update);
        }
        private bool ExamSimpleUserNotDictionary(long id)
        {
            if (UserDictionary.ContainsKey(id))
                return false;

            else
                return true;
        }

        public async Task UserTransferToAdministratorHundleAsync(Update update, long Admin)
        {
            await UserDictionary[update.Message.Chat.Id].UserTransferToAdministratorAsync(update, Admin);
        }
    }
}
