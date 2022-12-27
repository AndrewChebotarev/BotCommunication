using System.Net;

namespace TelegramBot.Tasks
{
    public class HundleUpdate
    {
        private UserTasks userTasks = new();
        private AdminTasks adminTasks = new();
        private UnauthorizedUserTasks unauthorizedUserTasks = new();
        private HundleCheckingMessage hundleCheckingMessage = new();

        private List<long> UnauthorizedUserList = new();
        private List<long> UserList = new();
        private List<string> MessagesForCommand = new() { "Обычный пользователь", "обычный пользователь", "Администратор", "администратор", "/start", "/stop", "Смена пароля", "смена пароля" };

        private Dictionary<long, bool> AdminDictionary = new();
        private Dictionary<long, long> AdminTransferUser = new();
        private Dictionary<long, bool> AdminForChangePassword = new();

        private string password = "a12345z";

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var checkingForMessage = hundleCheckingMessage.CheckingForMessage(update);

                if (!checkingForMessage)
                {
                    return;
                }

                if (update.Message.Text == MessagesForCommand[4])
                {
                    UnauthorizedUserList.Add(update.Message.Chat.Id);
                    await unauthorizedUserTasks.StartHundleAsync(botClient, update, cancellationToken);
                    return;
                }

                if (!UnauthorizedUserList.Contains(update.Message.Chat.Id) && !UserList.Contains(update.Message.Chat.Id) && !AdminDictionary.ContainsKey(update.Message.Chat.Id))
                {
                    UnauthorizedUserList.Add(update.Message.Chat.Id);
                    await unauthorizedUserTasks.StartHundleAsync(botClient, update, cancellationToken);
                    return;
                }

                if (UnauthorizedUserList.Contains(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[0] || update.Message.Text == MessagesForCommand[1])
                    {
                        await userTasks.StandardUserChoiceHundleAsync(botClient, update, cancellationToken);
                        return;
                    }

                    if ((update.Message.Text.StartsWith('+') || update.Message.Text.StartsWith('8')) && (update.Message.Text.Length == 11 || update.Message.Text.Length == 12))
                    {
                        UnauthorizedUserList.Remove(update.Message.Chat.Id);
                        UserList.Add(update.Message.Chat.Id);
                        await userTasks.EnterPhoneNumberHundleAsync(update);
                        return;
                    }

                    if (update.Message.Text == MessagesForCommand[2] || update.Message.Text == MessagesForCommand[3])
                    {
                        UnauthorizedUserList.Remove(update.Message.Chat.Id);
                        AdminDictionary.Add(update.Message.Chat.Id, false);
                        await adminTasks.AdminChoiceHundleAsync(botClient, update, cancellationToken);
                        return;
                    }

                    if (!MessagesForCommand.Contains(update.Message.Text))
                    {
                        await unauthorizedUserTasks.RepeatHundleAsync(update);
                        return;
                    }

                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }
                }

                if (UserList.Contains(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }

                    foreach (var Admin in AdminDictionary)
                    {
                        await userTasks.UserTransferToAdministratorHundleAsync(update, Admin.Key);
                    }
                }

                if (AdminDictionary.ContainsKey(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == false && update.Message.Text == password)
                    {
                        AdminDictionary[update.Message.Chat.Id] = true;
                        await adminTasks.AdminPasswordHundleAsync(update);
                        return;
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == false && update.Message.Text != password)
                    {
                        await adminTasks.AdminPasswordErrorHundleAsync(update);
                        return;
                    }

                    if (AdminTransferUser.ContainsKey(update.Message.Chat.Id))
                    {
                        await adminTasks.AdminTransferToUserHundleAsync(update, AdminTransferUser);
                        AdminTransferUser.Remove(update.Message.Chat.Id);
                        return;
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == true)
                    {
                        try
                        {
                            if (update.Message.Text == MessagesForCommand[6] || update.Message.Text == MessagesForCommand[7])
                            {
                                await adminTasks.EnterChangePasswordHundleAsync(update);
                                AdminForChangePassword.Add(update.Message.Chat.Id, true);
                                return;
                            }

                            if (AdminForChangePassword[update.Message.Chat.Id] == true)
                            {
                                password = update.Message.Text;
                                AdminForChangePassword.Remove(update.Message.Chat.Id);
                                await adminTasks.ResultChangePasswordHundleAsync(update);
                                return;
                            }

                            if (UserList.Contains(Convert.ToInt64(update.Message.Text)))
                            {
                                AdminTransferUser.Add(update.Message.Chat.Id, Convert.ToInt64(update.Message.Text));
                                await adminTasks.AdminMessageIdHundleAsync(update);
                                return;
                            }

                            if (!UserList.Contains(Convert.ToInt64(update.Message.Text)))
                            {
                                await adminTasks.AdminMessageIdErrorHundleAsync(update);
                                return;
                            }
                        }
                        catch
                        {
                            await adminTasks.AdminMessageIdErrorHundleAsync(update);
                            return;
                        }
                    }
                }
            }

            catch
            {
                Console.WriteLine("Ошибка!");
                return;
            }
        }

        private async void StopCommandHundle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UnauthorizedUserList.Remove(update.Message.Chat.Id);
            UserList.Remove(update.Message.Chat.Id);
            AdminDictionary.Remove(update.Message.Chat.Id);
            await unauthorizedUserTasks.StopMessageHundleAsync(update);
            await unauthorizedUserTasks.StartHundleAsync(botClient, update, cancellationToken);
            UnauthorizedUserList.Add(update.Message.Chat.Id);
        }
    }
}

