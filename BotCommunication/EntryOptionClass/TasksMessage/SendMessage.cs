namespace BotCommunication.EntryOptionClass.TasksMessage
{
    public class SendMessage
    {
        public async Task SendingMessage(ITelegramBotClient botClient, CancellationToken cancellationToken, long id, string text)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: id,
                text: text,
                cancellationToken: cancellationToken);
        }
    }
}
