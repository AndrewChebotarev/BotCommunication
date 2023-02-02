namespace BotCommunication.Tasks.TasksForAssistant
{
    public class AssistantTasks
    {
        private Dictionary<long, Assistant> assistantDictionary = new();

        public async Task AssistantChoiceHundleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Assistant assistant = new(botClient, update, cancellationToken);

            ExamAssistantDictionary(assistant.id);
            assistantDictionary.Add(update.Message.Chat.Id, assistant);
        }
        private void ExamAssistantDictionary(long id)
        {
            if (assistantDictionary.ContainsKey(id))
                assistantDictionary.Remove(id);
        }

        public async Task AssistantMessageIdErrorHundleAsync(Update update) =>
            await assistantDictionary[update.Message.Chat.Id].AssistantMessageIdErrorAsync(update);

        public async Task AssistantMessageIdHundleAsync(Update update) =>
            await assistantDictionary[update.Message.Chat.Id].AssistantMessageIdAsync(update);

        public async Task AssistantTransferToUserHundleAsync(Update update, Dictionary<long, long> AdminTransferUser) =>
            await assistantDictionary[update.Message.Chat.Id].AssistantTransferToUserAsync(update, AdminTransferUser);
    }
}
