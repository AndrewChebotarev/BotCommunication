namespace BotCommunication.EntryOptionClass.TasksMessage
{
    public class ConsoleMessage
    {
        public void SendingConsole(string text, long id) => Console.WriteLine($"Received a '{text}' message in chat {id}.");
    }
}
