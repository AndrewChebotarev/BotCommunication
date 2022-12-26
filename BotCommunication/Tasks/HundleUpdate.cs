using BotCommunication.EntryOptionClass;
using BotCommunication.Tasks.TasksForAdmin;
using BotCommunication.Tasks.TasksForUser;
using Microsoft.VisualBasic;
using System.Threading;
using System.Xml.Xsl;
using Telegram.Bot.Types;

namespace TelegramBot.Tasks
{
    public class HundleUpdate
    {
        public List<string> admins = new();
        private Dictionary<string, string> AdminUser = new();
        private Dictionary<long, SimpleUser> simpleUserDictionary = new();
        private Dictionary<long, Admin> adminDictionary = new();
        private UserTasks userTasks = new();
        private AdminTasks adminTasks = new();
        public List<long> authorizationAdminCheckList = new();

        private string password = "1234";
        private bool passwordMessageExam = false;
        private bool exam = false;
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var checkingForMessage = await CheckingForMessage(update);

            if (!checkingForMessage) 
            {
                return;
            }

            var chatId = update.Message.Chat.Id;

            var adminExam = admins.Contains(chatId.ToString());

            if (exam)
            {
                await AdminMessageToUserAsync(botClient, update, cancellationToken);
                exam = false;
                return;
            }

            if (userTasks.authorizationUserCheckList.Contains(update.Message.Chat.Id))
            {
                if (update.Message.Text == "/stop")
                {
                    await StopMessageAsync(botClient, update, cancellationToken);
                }
                await userTasks.UserTransferToAdministratorHundleAsync(botClient, update, cancellationToken, admins);
            }

            if (adminExam)
            {
                if (update.Message.Text == "/stop")
                {
                    await StopMessageAsync(botClient, update, cancellationToken);
                }

                else if (simpleUserDictionary.ContainsKey(update.Message.Chat.Id))
                {
                    await AdminMessageAsync(botClient, update, cancellationToken);
                }

                else
                {
                    await adminTasks.AdminMessageIdErrorHundleAsync(botClient, update, cancellationToken);
                }
            }

            if (userTasks.authorizationUserCheckList.Contains(update.Message.Chat.Id) == false && adminExam == false)
            {
                if (update.Message.Text == "/start")
                {
                    await StartMessageAsync(botClient, update, cancellationToken);
                }

                else if (update.Message.Text == "обычный пользователь")
                    await userTasks.StandardUserChoiceHundleAsync(botClient, update, cancellationToken);


                else if ((update.Message.Text.StartsWith('+') || update.Message.Text.StartsWith('8')) && (update.Message.Text.Length == 11 || update.Message.Text.Length == 12))
                    await userTasks.EnterPhoneNumberHundleAsync(botClient, update, cancellationToken);


                else if (update.Message.Text == "администратор")
                {
                    var t = await adminTasks.AdminChoiceHundleAsync(botClient, update, cancellationToken);
                    passwordMessageExam = t;
                }

                else if (passwordMessageExam)
                {
                    if (update.Message.Text == password)
                        await adminTasks.AdminPasswordHundleAsync(botClient, update, cancellationToken, admins);

                    else
                        await adminTasks.AdminPasswordErrorHundleAsync(botClient, update, cancellationToken);

                    passwordMessageExam = false;
                }

                else
                {
                    await StartMessageRepeatAsync(botClient, update, cancellationToken);
                }
            }
        }

        public async Task StartMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ConsoleMessage(update);

            await SendingMessage(botClient, update.Message.Chat.Id, cancellationToken, "Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\". Если вам нужно будет выйти из бота введите \"/stop\".");
        }

        public async Task StartMessageRepeatAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ConsoleMessage(update);

            await SendingMessage(botClient, update.Message.Chat.Id, cancellationToken, "Бот не понял вашего сообщения. Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\")");
        }

        public async Task StopMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ConsoleMessage(update);

            var userExam = simpleUserDictionary.ContainsKey(update.Message.Chat.Id);
            var adminExam = admins.Contains(update.Message.Chat.Id.ToString());

            if (userExam)
                simpleUserDictionary.Remove(update.Message.Chat.Id);

            if (adminExam)
                admins.Remove(update.Message.Chat.Id.ToString());

            await SendingMessage(botClient, update.Message.Chat.Id, cancellationToken, "Вы вышли из телеграмм бота.");
        }

        public async Task AdminMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ConsoleMessage(update);

            await SendingMessage(botClient, update.Message.Chat.Id, cancellationToken, "Отправте сообщение пользователю.");

            AdminUser.Add(update.Message.Chat.Id.ToString(), update.Message.Text);

            exam = true;
        }

        public async Task AdminMessageToUserAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ConsoleMessage(update);

            await SendingMessage(botClient, Convert.ToInt64(AdminUser[update.Message.Chat.Id.ToString()]), cancellationToken, $" Администратор отправил вам сообщение. \n {update.Message.Text}");

            await SendingMessage(botClient, update.Message.Chat.Id, cancellationToken, " Вы отправили сообщение. Введите id для следующего сообщения.");

            AdminUser.Remove(update.Message.Chat.Id.ToString());
        }

        private async Task<bool> CheckingForMessage(Update update)
        {
            if (update.Message is not { } message)
                return false;

            if (message.Text is not { } messageText)
                return false;

            return true;
        }
        private async Task SendingMessage(ITelegramBotClient botClient, long id, CancellationToken cancellationToken, string text)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: id,
                text: text,
                cancellationToken: cancellationToken);
        }

        private void ConsoleMessage(Update update) => Console.WriteLine($"Received a '{update.Message.Text}' message in chat {update.Message.Chat.Id}.");
    }
}

