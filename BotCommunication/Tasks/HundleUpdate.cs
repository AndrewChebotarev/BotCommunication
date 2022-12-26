using Telegram.Bot.Types;

namespace TelegramBot.Tasks
{
    public class HundleUpdate
    {
        private List<string> users = new();
        private List<string> admins = new();
        private Dictionary<string, string> AdminUser = new();
        private Dictionary<string, string> User2 = new();

        private string password = "1234";
        private bool passwordMessageExam = false;
        private bool exam = false;
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            var userExam = users.Contains(chatId.ToString());

            var adminExam = admins.Contains(chatId.ToString());

            if (exam)
            {
                AdminMessageToUserAsync(botClient, update, cancellationToken);
                exam = false;
                return;
            }

            if (userExam)
            {
                if (message.Text == "/stop")
                {
                    await StopMessageAsync(botClient, update, cancellationToken);
                }

                await UserMessageAsync(botClient, update, cancellationToken);
            }

            if (adminExam)
            {
                if (message.Text == "/stop")
                {
                    await StopMessageAsync(botClient, update, cancellationToken);
                }

                else if (users.Contains(message.Text))
                {
                    await AdminMessageAsync(botClient, update, cancellationToken);
                }

                else
                {
                    await AdminErrorMessageAsync(botClient, update, cancellationToken);
                }
            }

            if (userExam == false && adminExam == false)
            {
                if (message.Text == "/start")
                {
                    await StartMessageAsync(botClient, update, cancellationToken);
                }

                else if (message.Text == "обычный пользователь")
                {
                    await ExamNumberPhoneAsync(botClient, update, cancellationToken);
                }

                else if ((messageText.StartsWith('+') || messageText.StartsWith('8')) && (messageText.Length == 11 || messageText.Length == 12))
                {
                    await RegularUserMessageAsync(botClient, update, cancellationToken);
                }

                else if (message.Text == "администратор")
                {
                    await RegularAdminMessageAsync(botClient, update, cancellationToken);
                }

                else if (passwordMessageExam)
                {
                    if (message.Text == password)
                    {
                        await AdminPasswordAsync(botClient, update, cancellationToken);
                    }

                    else
                    {
                        await AdminPasswordErrorAsync(botClient, update, cancellationToken);
                    }

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
            if (update.Message is not { } message)
                return;

            var chatId = message.Chat.Id;

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\". Если вам нужно будет выйти из бота введите \"/stop\".",
                cancellationToken: cancellationToken);
        }

        public async Task StartMessageRepeatAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            var chatId = message.Chat.Id;

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Бот не понял вашего сообщения. Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\")",
                cancellationToken: cancellationToken);
        }

        public async Task RegularUserMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            User2.Add(chatId.ToString(), messageText);

            users.Add(chatId.ToString());

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Введите сообщение для передачи администратору",
                cancellationToken: cancellationToken);
        }

        public async Task RegularAdminMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Введите пароль",
                cancellationToken: cancellationToken);

            passwordMessageExam = true;
        }

        public async Task AdminPasswordAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            admins.Add(chatId.ToString());

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Вы зашли как администратор. Для отправки сообщения обычному пользователю, введите его id.",
                cancellationToken: cancellationToken);
        }

        public async Task AdminPasswordErrorAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Неправильный пароль, ведите заново. Выберите, способ общения. Введите \"обычный пользователь\" или \"администратор\". Если вам нужно будет выйти из бота введите \"/stop\".",
                cancellationToken: cancellationToken);
        }

        public async Task StopMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            var userExam = users.Contains(chatId.ToString());

            if (userExam)
            {
                users.Remove(chatId.ToString());
            }

            var adminExam = admins.Contains(chatId.ToString());

            if (adminExam)
            {
                admins.Remove(chatId.ToString());
            }

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Вы вышли из телеграмм бота.",
                cancellationToken: cancellationToken);
        }

        public async Task UserMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            DateTime dateTime = DateTime.Now;

            var myValue = User2[chatId.ToString()];

            foreach (var admin in admins)
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: admin,
                    text: $" Сообщение от {chatId}.\n Номер телефона {myValue}. \n Дата и время отправки:{dateTime}. \n {message.Text}",
                    cancellationToken: cancellationToken); ;
            }

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Вы отправили сообщение администратору.",
                cancellationToken: cancellationToken);
        }

        public async Task AdminMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Отправте сообщение пользователю.",
                cancellationToken: cancellationToken);

            AdminUser.Add(chatId.ToString(), message.Text);

            exam = true;
        }

        public async Task AdminErrorMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Несуществующий пользователь! Введите id заново.",
                cancellationToken: cancellationToken);
        }

        public async Task AdminMessageToUserAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            var t = AdminUser[chatId.ToString()];

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: t,
                text: $" Администратор отправил вам сообщение. \n {messageText}",
                cancellationToken: cancellationToken);

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $" Вы отправили сообщение. Введите id для следующего сообщения.",
                cancellationToken: cancellationToken);

            AdminUser.Remove(chatId.ToString());
        }

        public async Task ExamNumberPhoneAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            {
                if (update.Message is not { } message)
                    return;

                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Введите номер телефона.",
                    cancellationToken: cancellationToken);
            }
        }
    }
}

